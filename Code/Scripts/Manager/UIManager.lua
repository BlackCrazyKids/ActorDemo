local pairs = pairs
local require = require
local format = string.format
local match = string.match
local insert = table.insert
local loaded = package.loaded
local clear = table.clear
local print = print

local Time = Time
local Local = Local
local ResMgr = ResMgr
local Util = Util
local List = List
local LuaHelper = LuaHelper
local GameObject = GameObject
local ConditionOp = ConditionOp
local GameEvent = GameEvent
local Define = Define
local LogError = LogError
local printt = printt
local printyellow = printyellow
local printmodule = printmodule
local IsNull = IsNull

local ViewUtil = require "Common.ViewUtil"

local LOAD_ING = 1              --正在加载中
local LOAD_SUCC = 2             --加载成功
local UI_LOAD_TYPE = Define.ResourceLoadType.LoadBundleFromFile
local MAX_HIDE_VIEW_NUM = 0
local HIDE_POSITION = Vector2(9999, 9999)

local _views = { }
local _callBackDestroyAllDlgs
local _layout = {
    [Define.UILayoutType.Backgroud] = nil,
    [Define.UILayoutType.Normal] = nil,
    [Define.UILayoutType.Pupop] = nil,
}
local _viewStack
local _secondsTimer
local _modalDlg
local _loadingTip

local Show                      --开始显示
local OnShow                    --页面显示回调
local Hide                      --开始隐藏
local OnHide                    --页面隐藏回调
local HideOther                 --隐藏其他界面
local ShowLoading               --显示加载中提示!
local HideLoading               --隐藏加载中提示!

local UIManager = {}
local this = UIManager


--[[
    1.模态窗口,遮罩界面通用.
        a.多模态叠加时,上层遮罩颜色浅,下层深时,只能考虑多个遮罩层?
        b.加载界面时的遮罩(提示加载中),禁止点击下层界面
--]]


---@class UIData
local Data = {
    gameObject = nil,
    fields = nil,
    fileName = nil, -- 界面名称/ab包资源名称
    viewName = nil, -- 脚本相对UI路径名,用于请求脚本
    status = nil,
    needRefresh = nil,
    params = nil, -- 用于Show,Refresh传参
    isShow = false,
    hideTime = 0, -- 隐藏时刻

    originPos = nil, -- 原始坐标,显示隐藏时调用
    showType = nil,
    layoutType = nil,
}

function UIManager.Init()
    local uiRoot = FindObj("/UIRoot").transform
    _layout[Define.UILayoutType.Backgroud] = uiRoot:Find("Backgroud")
    _layout[Define.UILayoutType.Normal] = uiRoot:Find("Normal")
    _layout[Define.UILayoutType.Pupop] = uiRoot:Find("Pupop")
    _viewStack = List:new()
    _modalDlg = uiRoot:Find("Popup/ModalDlg")
    _loadingTip = uiRoot:Find("Popup/LoadingTip").gameObject
    _modalDlg.parent = _layout[Define.UILayoutType.Normal]

    GameEvent.UpdateEvent:Add(this.Update)
    --GameEvent.LateUpdateEvent:Add(this.LateUpdate)
    GameEvent.DestroyEvent:Add(this.Destroy)
    _secondsTimer = Timer:new(this.UnloadExpireView, 1, -1)
    _secondsTimer:Start()
end

function UIManager.Destroy()
    for name, data in pairs(_views) do
        this.DestroyView(name)
    end
    secondTimer = nil
end

function UIManager.Update(dt)
    for viewName, info in pairs(_views) do
        if info.isShow then
            if this.HasMethod(viewName, "Update") then
                this.Call(viewName, "Update", dt)
            end
        end

        if info.needRefresh then
            info.needRefresh = false
            this.Call(viewName, "Refresh", info.params)
        end
    end
end

function UIManager.UnloadExpireView()
    local unshowViewNum = 0
    local toDestroyViewName
    local minHideTime = Time.time
    for name, data in pairs(_views) do
        if data ~= nil and data.status == LOAD_SUCC and not data.isShow
                and not this.IsPersistent(name) and not this.IsInStack(name) then
            unshowViewNum = unshowViewNum + 1
            if data.hideTime ~= nil and data.hideTime < minHideTime then
                toDestroyViewName = name
                minHideTime = data.hideTime
            end
        end
    end
    if toDestroyViewName and unshowViewNum > MAX_HIDE_VIEW_NUM then
        this.DestroyView(toDestroyViewName)
    end
end

function UIManager.GetViewData(viewName)
    ---@type UIData
    local data = _views[viewName]
    if not data then
        local _, fileName = match(viewName, "^(.*)%.(.*)$")
        data = {}
        data.isShow = false
        data.fileName = ConditionOp(fileName, fileName, viewName)
        data.viewName = viewName
        _views[viewName] = data
    end
    return data
end

function UIManager.GetModuleName(viewName)
    return "UI." .. viewName
end

function UIManager.GetViewModule(viewName)
    local view = require(this.GetModuleName(viewName))
    if not view then
        LogError("[UIManager]%s script file not find!", viewName)
    end
    return view
end

function UIManager.HasScript(viewName)
    return LuaHelper.HasScript(this.GetViewModule(viewName))
end

function UIManager.HasMethod(viewName, methodName)
    local view = this.GetViewModule(viewName)
    if not view then
        return false
    end
    local method = view[methodName]
    if not method then
        return false
    end
    return true
end

function UIManager.IsShow(viewName)
    local data = _views[viewName]
    if data and data.isShow then
        return data.isShow
    end
    return false
end

function UIManager.HasLoaded(viewName)
    local viewData = this.GetViewData(viewName)
    return viewData.status == LOAD_SUCC
end

function UIManager.Call(viewName, methodName, params)
    local view = this.GetViewModule(viewName)
    if not view then
        return
    end
    local method = view[methodName]
    if not method then
        print(format("[UIManager] %s.%s not find.", viewName, methodName))
        return
    end
    local viewData = this.GetViewData(viewName)
    if viewData.status ~= LOAD_SUCC and methodName ~= "Show" then
        LogError("[UIManager]%s not loaded! can't call method:%s", viewName, methodName)
        return
    end
    local succ = Util.Myxpcall(method, params)
    if succ then
        return true
    else
        LogError("[UIManager]call  %s.%s fail.", viewName, methodName)
        return false
    end
end

function UIManager.CallWithReturn(viewName, methodName, params)
    local view = this.GetViewModule(viewName)
    if not view then
        return nil
    end
    local method = view[methodName]
    if not method then
        print(format("[UIManager]%s.%s not find.", viewName, methodName))
        return nil
    end
    return method(params)
end

function UIManager.GetUIShowType(viewName)
    local uishowtype = UIShowType.Default
    if this.HasMethod(viewName, "UIShowType") then
        uishowtype = this.CallWithReturn(viewName, "UIShowType")
    end
    return uishowtype
end
---获取所有显示的窗口
function UIManager.GetViewsShow()
    local list = {}
    for name, data in pairs(_views) do
        if data.isShow then
            insert(list, name)
        end
    end
    return list
end
function UIManager.IsPersistent(viewName)
    return Local.UIPersistentMap[viewName] ~= nil
end
function UIManager.IsInStack(viewName)
    return _viewStack[viewName] ~= nil
end
function UIManager.GetUIShowType(viewName)
    local data = this.GetViewData(viewName)
    local showType = Define.UIShowType.Default
    if data.showType then
        return data.showType
    elseif this.HasMethod(viewName, "UIShowType") then
        showType = this.CallWithReturn(viewName, "UIShowType")
        data.showType = showType
    end
    return showType
end
function UIManager.IsUIShowType(viewName, showType)
    local viewuishowtype = this.GetUIShowType(viewName)
    return (showType & viewuishowtype) > 0
end
function UIManager.GetLayoutType(viewName)
    local data = this.GetViewData(viewName)
    local layoutType = Define.UILayoutType.Normal
    if data.layoutType then
        return data.layoutType
    elseif this.HasMethod(viewName, "UILayoutType") then
        layoutType = this.CallWithReturn(viewName, "UILayoutType")
        data.layoutType = layoutType
    end
    return layoutType
end

---------------------------------------------
------------普通窗口操作方法
---------------------------------------------
ShowLoading = function()
    printmodule(Local.Moduals.UIManager, "[UIManager]ShowLoading")
    _loadingTip:SetActive(true)
end
HideLoading = function()
    printmodule(Local.Moduals.UIManager, "[UIManager]HideLoading")
    _loadingTip:SetActive(false)
end
local HideViewPos = function(transform)
    transform.anchoredPosition = HIDE_POSITION
    transform:SetAsLastSibling()
    --transform.gameObject:SetActive(false)
end

--- 加载或者直接显示已加载界面
Show = function(viewName, params)
    if Local.Moduals.UIManager then
        if params then
            printt(params, '[UIManager]Show ' .. viewName)
        else
            print("[UIManager]Show", viewName)
        end
    end
    local data = this.GetViewData(viewName)
    if data.isShow then
        return
    end
    data.params = params
    if not data.status then
        --print(format("view: %s %s", viewName, "be going to be loaded!\n"))
        ShowLoading(viewName)-- 开启遮罩
        data.status = LOAD_ING
        ResMgr:AddTask(format("ui/%s.bundle", data.fileName), function(obj)
            if IsNull(obj) then
                HideLoading()-- 关闭遮罩:资源不存在
                return
            end
            local layoutType = this.GetLayoutType(viewName)
            local viewObj = GameObject.Instantiate(obj, _layout[layoutType])
            GameObject.DontDestroyOnLoad(viewObj)
            if not viewObj then
                data.status = nil
                LogError(format("View %s prefab load fail!", viewName))
                HideLoading()-- 关闭遮罩:无法实例化
                return
            end
            data.status = LOAD_SUCC
            data.gameObject = viewObj
            data.transform = viewObj.transform
            data.fields = ViewUtil.ExportFields(viewObj)
            data.originPos = viewObj.transform.anchoredPosition
            viewObj.transform:SetAsFirstSibling()
            --viewObj:SetActive(true)

            if this.Call(viewName, "Init", { viewName, viewObj, data.fields })
                    and this.Call(viewName, "Show", params) then
                printmodule(Local.Moduals.UIManager, '[UIManager]OnInit ' .. viewName)
                OnShow(data)
            else
                HideViewPos(viewObj.transform)
            end
            HideLoading()-- 关闭遮罩:界面加载和初始化完毕
        end, UI_LOAD_TYPE)
        return
    end

    data.transform.anchoredPosition = data.originPos
    data.transform:SetAsFirstSibling()
    --data.gameObject:SetActive(true)
    if this.Call(viewName, "Show", params) then
        OnShow(data)
    else
        HideViewPos(data.transform)
    end
end
---@param data UIData
OnShow = function(data)
    local viewName, params = data.viewName, data.params
    data.isShow = true
    printmodule(Local.Moduals.UIManager, '[UIManager]OnShow ' .. viewName)
    if this.HasScript(viewName) then
        this.Refresh(viewName, params)
    end
end
Hide = function(viewName)
    printmodule(Local.Moduals.UIManager, "[UIManager]Hide", viewName)
    local data = this.GetViewData(viewName)
    if not data.isShow then
        --printyellow(format("view:%s not show!", viewName))
        return
    end
    OnHide(data)
end
---@param data UIData
OnHide = function(data)
    local viewName, transform = data.viewName, data.transform
    printmodule(Local.Moduals.UIManager, '[UIManager]OnHide ' .. viewName)
    if this.Call(viewName, "Hide") then
        data.isShow = false
        data.hideTime = Time.time
        HideViewPos(transform)
        if this.IsUIShowType(viewName, UIShowType.DestroyWhenHide) then
            this.DestroyView(viewName)
        end
    end
end
HideOther = function(viewName)
    for name in pairs(_views) do
        if name ~= viewName then
            Hide(viewName)
        end
    end
end

function UIManager.Refresh(viewName, params)
    printmodule(Local.Moduals.UIManager, "[UIManager]Refresh", viewName)
    if this.IsShow(viewName) then
        local data = this.GetViewData(viewName)
        data.needRefresh = true
        data.params = params
    end
end
function UIManager.IfShowThenCall(viewName, methodName, params)
    printmodule(Local.Moduals.UIManager, "[UIManager]IfShowThenCall", viewName)
    if this.IsShow(viewName) then
        this.Call(viewName, methodName, params)
    end
end
function UIManager.HideAll()
    for viewName in pairs(_views) do
        Hide(viewName)
    end
end
function UIManager.DestroyView(viewName)
    local data = this.GetViewData(viewName)
    if data.isShow then
        Hide(viewName)
    end

    this.Call(viewName, "Destroy")
    GameObject.Destroy(data.gameObject)
    clear(data.fields)
    data.fields = nil
    _views[viewName] = nil
    --assert(loaded[this.GetModuleName(viewName)])
    loaded[this.GetModuleName(viewName)] = nil
end
function UIManager.RegistCallBackDestroyAllDlgs(callback)
    printt(_callBackDestroyAllDlgs, "RegistCallBackDestroyAllDlgs")
    _callBackDestroyAllDlgs = callback
end
function UIManager.DestroyAllDlgs()
    local list = this.GetViewsShow()
    local d = false
    for _, name in pairs(list) do
        d = false
        for _, persistentName in pairs(Local.UIPersistentMap) do
            if name == persistentName then
                Hide(name)
                d = true
                break
            end
        end
    end
    if _viewStack then
        _viewStack:Clear()
    end
    if _callBackDestroyAllDlgs then
        _callBackDestroyAllDlgs()
        _callBackDestroyAllDlgs = nil
    end
end

function UIManager.ShowPanel(viewName, params)
    local uishowType = this.GetUIShowType(viewName)
    if (uishowType & Define.UIShowType.Default) > 0 then
        Show(viewName, params)
    elseif (uishowType & Define.UIShowType.ReturnType) > 0 then
        if _viewStack.length > 0 then
            local lastViewName = _viewStack:Pop()
            if lastViewName == viewName then
                return
            end
            Show(viewName, params)
            Hide(lastViewName)
        else
            Show(viewName, params)
            HideOther(viewName)
        end
        _viewStack:Push(viewName)
    elseif (uishowType & Define.UIShowType.HideOther) > 0 then
        Show(viewName, params)
        HideOther(viewName)
    end
end
function UIManager.HidePanel(viewName, params)
    local uishowType = this.GetUIShowType(viewName)
    if (uishowType & Define.UIShowType.Default) > 0 then
        Hide(viewName)
    elseif (uishowType & Define.UIShowType.ReturnType) > 0 then
        if _viewStack.length == 0 then
            printyellow(format('[UIManager]Hide a view[%s] that is not on the stack.', viewName))
            this.ShowMainCityViews()
            return
        else
            _viewStack:Pop()
            this.ShowMainCityViews()
        end
    elseif (uishowType & Define.UIShowType.HideOther) > 0 then
        Hide(viewName) -- 不显示之前隐藏的多个界面
    end
end
function UIManager.ShowMainCityViews()
    for i = 1, #Local.MainCityViews do
        local viewName = Local.MainCityViews[i]
        if not this.IsShow(viewName) then
            Show(viewName)
        end
    end
end
function UIManager.HideMainCityViews()
    for i = 1, #Local.MainCityViews do
        local viewName = Local.MainCityViews[i]
        if this.IsShow(viewName) then
            Hide(viewName)
        end
    end
end
function UIManager.ShowPupop(viewName, params)
    _modalDlg.gameObject:SetActive(true)
    _modalDlg:SetAsFirstSibling()
    Show(viewName, params)
end
function UIManager.HidePupop(viewName)
    Hide(viewName)
    local ramianView = _layout[Define.UILayoutType.Pupop].childCount - 2
    if ramianView > 0 then
        _modalDlg:SetSiblingIndex(ramianView - 1)
    else
        _modalDlg.gameObject:SetActive(false)
    end
end


---------------------------------------------------------------
----------------------------弹窗窗口设计[固定几种样式]
---------------------------------------------------------------

return UIManager

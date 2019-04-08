local pairs = pairs
local require = require
local format = string.format
local match = string.match
local insert = table.insert
local loaded = package.loaded
local trim = string.trim
local copy = table.copy
local clear = table.clear

local Time = Time
local Local = Local
local ResMgr = ResMgr
local Vector3 = Vector3
local Util = Util
local LuaHelper = LuaHelper
local GameObject = GameObject
local ConditionOp = ConditionOp
local GameEvent = GameEvent
local XUtil = XUtil
local GRoot = GRoot
local UIPackage = UIPackage
local Stage = Stage

local LogError = LogError
local printt = printt
local print = print
local printyellow = printyellow
local printmodule = printmodule
local IsNull = IsNull

local ViewUtil = require "Common.ViewUtil"

local LOAD_ING = 1              --正在加载中
local LOAD_SUCC = 2             --加载成功
local LAYER_UI = 20             --UI Layer层
local UI_LOAD_TYPE = Define.ResourceLoadType.LoadBundleFromFile | Define.ResourceLoadType.ReturnAssetBundle -- www方式加载
--local DLG_LOADING_MASK = "DlgUILock"    -- 加载界面时锁UI,禁止一切操作
local MAX_HIDE_VIEW_NUM = 0

local _views = {}
local _stage = nil             -- 舞台对象
local _isLocked = false        -- UI界面被锁,禁止一切操作
local _callBackDestroyAllDlgs

local NeedRefresh               --是否需要刷新
local OnInit
local DoShow
local OnShow                    --页面显示回调
local DoHide
local OnHide                    --页面隐藏回调
local ShowLoadingLock           --显示加载中提示!
local HideLoadingLock           --隐藏加载中提示!

local UIManager = {}
local this = UIManager
local secondTimer
local pkgItems = {}

--[[
    注:问题
        1.LuaWindow类缓存重复利用,直接再C#层完成!   --- 待测试
        2.添加移除子对象后,子对象无法完整释放.[子对象应该已被缓存,后续有待集中释放]
        3.Lua UI API提示功能
        4.Timer 功能设计
--]]

---#注:未完成功能
---#遮罩界面,避免对当前窗口后面物体进行交互
---#界面返回,至上一次界面
---窗口定义UIShowType函数指明显示类型
UIShowType = {
    Default = 1 << 0, --默认策略显示
    Refresh = 1 << 1, --强制调用Show{执行Show,Refresh函数}
    DestroyWhenHide = 1 << 2, --Hide时释放资源,默认隐藏不销毁资源,仅在资源销毁时调用
    TabGroup = 1 << 3, --多界面切换组[树结构]..未完成
    ReturnType = 1 << 4, --界面可回退到上一个界面[栈结构]..未完成
}

---@class UIData
local Data = {
    base = nil,
    script = nil,
    fields = nil,
    params = nil, -- 用于Show,Refresh传参
    fileName = nil, -- 界面名称/ab包资源名称
    viewName = nil, -- 脚本相对UI路径名
    status = nil,
    loaded = nil,
    isShow = false,

    OnInit = OnInit, --  脚本对应事件 Init
    DoShow = DoShow, --  脚本对应事件 DoShow
    OnShow = OnShow, --  脚本对应事件 Show
    DoHide = DoHide, --  脚本对应事件 DoHide
    OnHide = OnHide, --  脚本对应事件 Hide
}

function UIManager.Init()
    _stage = FindObj("Stage")
    local list = { "Stage", "Stage Camera" }
    for i = 1, #list do
        local obj = FindObj(list[i])
        if obj then
            GameObject.DontDestroyOnLoad(obj)
        end
    end
    UIConfig.globalModalWaiting = 'ui://Atlas_BaseSprite/DlgUILock'

    GameEvent.UpdateEvent:Add(this.Update)
    GameEvent.LateUpdateEvent:Add(this.LateUpdate)
    GameEvent.DestroyEvent:Add(this.Destroy)
    secondTimer = Timer:new(this.SecondUpdate, 1, -1, false)
    secondTimer:Start()
end

function UIManager.Destroy()
    for name, data in pairs(_views) do
        this.DestroyView(name)
    end
    secondTimer = nil
    for _, v in pairs(pkgItems) do
        v:Dispose()
    end
end

function UIManager.Update(dt)
    for viewName, info in pairs(_views) do
        if info.isShow then
            if this.HasMethod(viewName, "Update") then
                this.Call(viewName, "Update", dt)
            end
        end
    end
end

function UIManager.LateUpdate()
    for viewName, info in pairs(_views) do
        if info.isShow then
            if this.HasMethod(viewName, "LateUpdate") then
                this.Call(viewName, "LateUpdate")
            end
        end

        if info.needRefresh and NeedRefresh(viewName) then
            if Local.Moduals.UIManager then
                if info.refreshParams then
                    printt(info.refreshParams, "[UIManager]LateUpdate Refresh " .. viewName)
                else
                    print("[UIManager]LateUpdate Refresh " .. viewName)
                end
            end
            info.needRefresh = false
            this.Call(viewName, "Refresh", info.params)
        end
    end
end

function UIManager.SecondUpdate(now)
    this.UnloadExpireView(now)
    for viewName, info in pairs(_views) do
        if info.isShow and this.HasMethod(viewName, "SecondUpdate") then
            this.Call(viewName, "SecondUpdate", now)
        end
    end
end

function UIManager.UnloadExpireView(now)
    local unshowViewNum = 0
    local toDestroyViewName
    local minHideTime = now
    for name, data in pairs(_views) do
        if data ~= nil and data.status == LOAD_SUCC and not data.isShow and not this.IsPersistent(name) then
            --and not this.IsInStack(name)
            unshowViewNum = unshowViewNum + 1
            if data.hideTime ~= nil and data.hideTime < minHideTime then
                toDestroyViewName = name
                minHideTime = data.hideTime
            end
        end
    end
    if toDestroyViewName and unshowViewNum > MAX_HIDE_VIEW_NUM then
        --this.Destroy(toDestroyViewName)
    end
end

function UIManager.GetViewData(viewName)
    ---@type UIData
    local data = _views[viewName]
    if not data then
        data = {
            isShow = false,
            OnInit = OnInit,
            DoShowTween = DoShow,
            OnShow = OnShow,
            DoHideTween = DoHide,
            OnHide = OnHide,
        }
        local pkgName, fileName = match(viewName, "^(.*)%.(.*)$")
        data.fileName = ConditionOp(fileName, fileName, viewName)
        data.pkgName = pkgName
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
---字段指变量和函数
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
function UIManager.GetDlgsShow()
    local list = {}
    for name, data in pairs(_views) do
        if data.isShow then
            insert(list, name)
        end
    end
    return list
end
function UIManager.IsPersistent(viewName)
    return Local.UIPersistentMap[viewName] == true
end
function UIManager.GetUIShowType(viewName)
    local showType = UIShowType.Default
    if (this.HasMethod(viewName, "UIShowType")) then
        showType = this.CallWithReturn(viewName, "UIShowType")
    end
    return showType
end
function UIManager.IsUIShowType(viewName, showType)
    local viewuishowtype = this.GetUIShowType(viewName)
    return (showType & viewuishowtype) > 0
end

---------------------------------------------
---普通窗口操作方法
---------------------------------------------
ShowLoadingLock = function(beginTime, endTime)
    if _isLocked == true then
        return
    end

    printmodule(Local.Moduals.UIManager, "[UIManager]ShowLoading Locked")
    --local params = { beginTime = ConditionOp(beginTime, beginTime, 0.5), endTime = ConditionOp(endTime, endTime, 3) }
    GRoot.inst:ShowModalWait()
    --if this.IsShow(DLG_LOADING_MASK) then
    --    this.Refresh(DLG_LOADING_MASK, params)
    --else
    --    this.Show(DLG_LOADING_MASK, params)
    --end
end
HideLoadingLock = function()
    if _isLocked == true then
        return
    end
    printmodule(Local.Moduals.UIManager, "[UIManager]HideLoading Locked")
    GRoot.inst:CloseModalWait()
    --this.Hide(DLG_LOADING_MASK)
end
---@param data UIData
OnInit = function(data)
    local viewName = data.viewName
    ---@type Game.LuaWindow
    local window = data.base
    window:Center()
    window.displayObject.gameObject.layer = LAYER_UI
    if this.Call(viewName, "Init", { viewName, window, data.fields }) then
        printmodule(Local.Moduals.UIManager, '[UIManager]OnInit ' .. viewName)
    else
        window:HideImmediately()
    end
end
---@param data UIData
DoShow = function(data)
    local viewName = data.viewName
    ---@type Game.LuaWindow
    local window = data.base
    data.isShow = true
    if this.Call(viewName, "DoShow", window.ShowImmediately) then
        ---界面显示动画 ! 不知道什么时候结束
        ---TODO
    else
        window:ShowImmediately()
    end
end
---@param data UIData
OnShow = function(data)
    local window, viewName, params = data.base, data.viewName, data.params
    printmodule(Local.Moduals.UIManager, '[UIManager]OnShow ' .. viewName)
    HideLoadingLock()

    if this.Call(viewName, "Show", data.params) then
        this.Refresh(viewName, params)
    else
        window:HideImmediately()
    end
end
---@param data UIData
DoHide = function(data)
    local viewName = data.viewName
    ---@type Game.LuaWindow
    local window = data.base
    if this.HasMethod(viewName, "DoHide", window.HideImmediately) then
        ---界面隐藏动画 ! 不知道什么时候结束
        ---TODO
    else
        window:HideImmediately()
    end
end
---@param data UIData
OnHide = function(data)
    local viewName = data.viewName
    printmodule(Local.Moduals.UIManager, '[UIManager]OnHide ' .. viewName)
    if this.Call(viewName, "Hide") then
        data.isShow = false
        if this.IsUIShowType(viewName, UIShowType.DestroyWhenHide) then
            this.DestroyView(viewName, "Destroy")
        end
    end
end

NeedRefresh = function(viewName)
    return this.IsShow(viewName) --or this.IsInStack(viewName)
end

function UIManager.Show(viewName, params)
    if Local.Moduals.UIManager then
        if params then
            printt(params, '[UIManager]Show ' .. viewName)
        else
            print("[UIManager]Show", viewName)
        end
    end
    local data = this.GetViewData(viewName)
    if data.isShow and not this.IsUIShowType(viewName, UIShowType.Refresh) then
        return
    end
    data.params = params
    ShowLoadingLock(viewName)
    local pkg = UIPackage.GetByName(data.pkgName)
    if IsNull(pkg) and data.status ~= LOAD_SUCC then
        if not data.loaded then
            --print(format("view: %s %s", viewName, "be going to be loaded!\n"))
            data.loaded = LOAD_ING
            --ShowLoading()--显示界面加载提示界面
            ResMgr:AddTask(format("ui/%s.bundle", data.pkgName), function(ab)
                ---@type UnityEngine.AssetBundle
                local asstbundle = ab
                local viewCom = nil
                if IsNull(asstbundle) then
                    return
                end
                UIPackage.AddPackage(asstbundle)
                viewCom = UIPackage.CreateObject(data.pkgName, data.fileName).asCom
                if not viewCom then
                    data.loaded = nil
                    LogError(format("view %s fgui load fail!", viewName))
                    return
                end
                ---@type Game.LuaWindow
                local window = LuaWindow.Get()
                window.contentPane = viewCom
                window:ConnectLua(data)
                data.base = window      -- LuaWindow
                data.hideTime = 0       -- 脚本对象
                data.isShow = true
                data.status = LOAD_SUCC
                data.fields = ViewUtil.ExportFields(viewCom)
                window:Show()
            end, UI_LOAD_TYPE)
        end
        return
    end

    local viewCom = pkg:CreateObject(data.fileName).asCom
    local window = LuaWindow.Get()
    window.contentPane = viewCom
    window:ConnectLua(data)
    data.base = window      -- LuaWindow
    data.hideTime = 0       -- 脚本对象
    data.isShow = true
    data.status = LOAD_SUCC
    data.fields = ViewUtil.ExportFields(viewCom)
    window:Show()
end
---所有类型的窗口均可关闭[默认]
function UIManager.ShowAndCloseOther(viewName, params)

end
function UIManager.Refresh(viewName, params)
    printmodule(Local.Moduals.UIManager, "[UIManager]Refresh", viewName)
    if NeedRefresh(viewName) then
        local data = this.GetViewData(viewName)
        data.needRefresh = true
        data.params = params
    end
end
function UIManager.IfShowThenCall(viewName, methodName, params)
    if this.IsShow(viewName) then
        this.Call(viewName, methodName, params)
    end
end
function UIManager.Hide(viewName)
    printmodule(Local.Moduals.UIManager, "[UIManager]Hide", viewName)
    local data = this.GetViewData(viewName)
    data.hideTime = Time.time
    if not data.isShow then
        print(format("view:%s not show!", viewName))
        return
    end

    ---@type Game.LuaWindow
    local window = data.base
    window:Hide()
end
function UIManager.HideImmediate(viewName)
    printmodule(Local.Moduals.UIManager, "[UIManager]HideImmediate", viewName)
    local data = this.GetViewData(viewName)
    data.hideTime = Time.time
    if not data.isShow then
        print(format("view:%s not show!", viewName))
        return
    end
    ---@type Game.LuaWindow
    local window = data.base
    window:HideImmediately()
end
function UIManager.DestroyView(viewName)
    local data = this.GetViewData(viewName)
    if data.isShow then
        this.HideImmediate(viewName)
    end

    ---@type Game.LuaWindow
    local window = data.base
    if not window then
        _views[viewName] = nil
        return
    end
    this.Call(viewName, "Destroy")
    data.base = nil
    --clear(data)
    _views[viewName] = nil
    assert(loaded[this.GetModuleName(viewName)])
    loaded[this.GetModuleName(viewName)] = nil
    if window then
        window:Dispose()
    end
end
function UIManager.HideAll()
    GRoot.inst:CloseAllWindows()
    for viewName, data in pairs(_views) do
        data.hideTime = Time.time
        if this.Call(viewName, "Hide") then
            data.isShow = false
            if this.IsUIShowType(viewName, UIShowType.DestroyWhenHide) then
                this.DestroyView(viewName, "Destroy")
            end
        end
    end
end
function UIManager.RegistCallBackDestroyAllDlgs(callback)
    printt(_callBackDestroyAllDlgs, "RegistCallBackDestroyAllDlgs")
    _callBackDestroyAllDlgs = callback
end
function UIManager.DestroyAllDlgs()
    local list = this.GetDlgsShow()
    local d = false
    for _, name in pairs(list) do
        d = false
        for _, persistentName in pairs(Local.UIPersistentMap) do
            if name == persistentName then
                this.Hide(name)
                d = true
                break
            end
        end
    end
    --_dialogStack:Clear()
    if _callBackDestroyAllDlgs then
        _callBackDestroyAllDlgs()
        _callBackDestroyAllDlgs = nil
    end
end
function UIManager.GetPkgItem(viewName, item)
    local data = this.GetViewData(viewName)
    local comp = UIPackage.CreateObject(data.pkgName, item)
    insert(pkgItems, comp)
    return comp
end


---------------------------------------------------------------
-------------------------Tab界面组[树]
---------------------------------------------------------------







---------------------------------------------------------------
-------------------------界面返回功能[栈]
---------------------------------------------------------------




---------------------------------------------------------------
-------------------------弹窗窗口设计[固定几种样式]
---------------------------------------------------------------
--[[
    警告提示
    普通提示
--]]
function UIManager.PopSystemTip(content)

end
function UIManager.PopFlyText(content)

end
function UIManager.ShowPopup(target, sender)
    GRoot.inst:ShowPopup(target, sender, false)
end

return UIManager

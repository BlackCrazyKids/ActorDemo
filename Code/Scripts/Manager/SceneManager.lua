local ipairs = ipairs

---@type Game.SceneManager
local Mgr = Game.Client.SceneMgr
local UIMgr = require("Manager.UIManager")
local GameEvent = require("Common.GameEvent")

---@class SceneManager
local SceneManager = {}

local _isLoading = false
local _mapId
local _sceneName

local EVENT_LOAD_SCENE_START = "LoadScene_Start"
local EVENT_LOAD_SCENE_END = "LoadScene_End"

----------------------------------------------------
-----------加载场景方法
----------------------------------------------------
local function LoadBySceneName(sceneName)
    if sceneName then
        _sceneName = sceneName
        Mgr:ChangeMap(sceneName)
    else
        --返回默认场景
        _sceneName = ""
        --Mgr:ChangeMap("maincity_01")
    end
end
local function OnLoad(sceneName, callback)
    --加载/配置场景相关参数
    --隐藏销毁进度条dlgloading
    GameEvent.NotifyEvent:Trigger(EVENT_LOAD_SCENE_END, { sceneName = sceneName })
    _isLoading = false

    if callback then
        callback()
    end
end
local function Transition2Scene(sceneName, views, callback)
    Mgr:RegisteOnSceneLoadFinish(function(result)
        if result then
            OnLoad(sceneName, callback)
        else
            LoadBySceneName(sceneName)
        end
    end)
    LoadBySceneName(sceneName)
    for _, v in ipairs(views) do
        UIMgr.Show(v)
    end
end
---@param views UI界面名称数组
---@param callback 回调函数
function SceneManager.LoadScene(sceneName, views, callback)
    --显示加载进度条dlgloading
    _isLoading = true
    UIMgr.DestroyAllDlgs()
    GameEvent.NotifyEvent:Trigger(EVENT_LOAD_SCENE_START, { sceneName = sceneName })
    Mgr:RegisteOnSceneLoadFinish(function(result)
        if result then
            LuaGC()
            Transition2Scene(sceneName, views, callback)
        else
            --登出游戏到选人界面[可设定条件]
            --LoadBySceneName(sceneName)
        end
    end)
    Mgr:ChangeMap("Transition")
end

function SceneManager.LoadLoginScene()
    Mgr:RegisteOnSceneLoadFinish(function(result)
        if result then
            printcolor('orange', 'Game Start,Ready Go!!')
        else
            --返回默认场景
            --Mgr:ChangeMap("Login")
        end
    end)
    SceneManager.LoadScene("Login", {})
end

----------------------------------------------------
-----------场景信息
----------------------------------------------------
function SceneManager.GetMapId()
    return _mapId
end
function SceneManager.GetSceneName()
    return _sceneName
end
function SceneManager.IsLoadingScene()
    return _isLoading
end

function SceneManager.Init()

end

return SceneManager
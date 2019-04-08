local require = require
require "Global"

local modules = require "Modules"
local GameEvent = GameEvent
local LogError = LogError

local Time = Time

local function InitModule()
    for _, name in ipairs(modules) do
        local module = require(name)
        if module then
            module.Init()
        else
            LogError("module %s init fail!", name)
        end
    end
end
local function Init()
    Util.Myxpcall(InitModule)
    printcolor("orange", 'lua framework init successful.')	 

    local sceneManager = require "Manager.SceneManager"
    sceneManager.LoadLoginScene()

    --注册登入完成事件
    --local UIMgr = require('Manager.UIManager')
    --Util.Myxpcall(UIMgr.Show, 'MJoystick.DlgJoystick')
	
    Util.Myxpcall(TEST)
    --local demo = require "UIExample.UIDemo"
    --Util.Myxpcall(demo.Init, demo)
end

--逻辑update
local function Update(deltaTime, unscaledDeltaTime)
    Time:SetDeltaTime(deltaTime, unscaledDeltaTime)
    GameEvent.UpdateEvent:Trigger()
end
--帧Update
local function LateUpdate()
    GameEvent.LateUpdateEvent:Trigger()
    Time:SetFrameCount()
end
--物理update
local function FixedUpdate(fixedDeltaTime)
    Time:SetFixedDelta(fixedDeltaTime)
    GameEvent.FixedUpdateEvent:Trigger()
end

local function OnDestroy()
    GameEvent.DestroyEvent:Trigger()
    --local util = require('xlua.util')
    --util.print_func_ref_by_csharp()
end

--------------------------------------------------------
---------------------- 测试 ----------------------------

function TEST()
    --local Filter = require('Core.Filter')
    --local f1 = Filter:new()
    --local f2 = Filter:new()
    --f1:All(2, 1, 3)
    --f1:None(4)
    --f1:Any(5)
    --f2:All(1, 2, 3, f2:None(4), f2:Any(5))
    --f2:Init()
    --f1:Init()
    --printyellow(f1.hashCode , f2.hashCode)
    --local func = function(timer)
    --    print('Time - ', Time.time, timer.loop)
    --end
    --local timer = Timer:new(func, 1, 5)
    --timer:Start()
end

Main = {
    Init = Init,
    Update = Update,
    LateUpdate = LateUpdate,
    FixedUpdate = FixedUpdate,
    OnDestroy = OnDestroy,
}




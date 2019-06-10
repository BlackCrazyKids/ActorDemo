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
    --local pb = require "pb"
    --local protoc = require "protoc"
    --assert(protoc:load([[
    --    message Phone {
    --      optional string name        = 1;
    --      optional int64  phonenumber = 2;
    --    }
    --    message Person {
    --      optional string name     = 1;
    --      optional int32  age      = 2;
    --      optional string address  = 3;
    --      repeated Phone  contacts = 4;
    --    }
    --    ]]))
    --for name, basename, type in pb.types() do
    --    print(name, basename, type)
    --end
end

Main = {
    Init = Init,
    Update = Update,
    LateUpdate = LateUpdate,
    FixedUpdate = FixedUpdate,
    OnDestroy = OnDestroy,
}




UnityEngine = CS.UnityEngine
Shader = UnityEngine.Shader
Animator = UnityEngine.Animator
Animation = UnityEngine.Animation
AnimationClip = UnityEngine.AnimationClip
AnimationEvent = UnityEngine.AnimationEvent
AnimationState = UnityEngine.AnimationState
AudioClip = UnityEngine.AudioClip
AudioSource = UnityEngine.AudioSource
Physics = UnityEngine.Physics
GameObject = UnityEngine.GameObject
Transform = UnityEngine.Transform
---@type UnityEngine.Application
Application = UnityEngine.Application
SystemInfo = UnityEngine.SystemInfo
Screen = UnityEngine.Screen
Camera = UnityEngine.Camera
Material = UnityEngine.Material
Renderer = UnityEngine.Renderer
WWW = UnityEngine.WWW
Input = UnityEngine.Input
KeyCode = UnityEngine.KeyCode
CharacterController = UnityEngine.CharacterController
SkinnedMeshRenderer = UnityEngine.SkinnedMeshRenderer
Rect = UnityEngine.Rect
---@type UnityEngine.RuntimePlatform
RuntimePlatform = UnityEngine.RuntimePlatform

Game = CS.Game
CSUtil = Game.Util
AppConst = Game.AppConst
LuaHelper = Game.LuaHelper
Interface = Game.Platform.Interface
ResMgr = Game.Client.ResMgr

local require = require
require 'xlua.extend'
require 'xlua.coroutine'            --?
XUtil = require 'xlua.util'
XProfiler = require 'xlua.profiler'

Time = require "UnityEngine.Time"
Mathf = require "UnityEngine.Mathf"
Vector3 = require "UnityEngine.Vector3"
Quaternion = require "UnityEngine.Quaternion"
Vector2 = require "UnityEngine.Vector2"
Vector4 = require "UnityEngine.Vector4"
Color = require "UnityEngine.Color"
Ray = require "UnityEngine.Ray"
Bounds = require "UnityEngine.Bounds"
RaycastHit = require "UnityEngine.RaycastHit"
Touch = require "UnityEngine.Touch"
LayerMask = require "UnityEngine.LayerMask"
Plane = require "UnityEngine.Plane"

require "Local"
-----------------------------------------------------------
require "Common.Function"

Define = require "Define"
---@type Class
Class = require "Common.Class"
Util = require "Common.Util"
List = require 'Common.List'
Event = require "Common.Event"
GameEvent = require "Common.GameEvent"
Protocol = require "Protocol.Protocol"
Message = require "Protocol.Message"
require "Common.Timer"

unpack = table.unpack

IsEditor = Application.isEditor
IsAndroid = Application.platform == RuntimePlatform.Android
IsIPhone = Application.platform == RuntimePlatform.IPhonePlayer

if IsEditor then
    --- 导出文件路径以Unity/为相对路径
    Memory = require "xlua.MemoryReferenceInfo"
    Memory.m_cConfig.m_bAllMemoryRefFileAddTime = false
    Profiler = require "xlua.profiler"
end

function LuaGC()
    local before = collectgarbage("count")
    collectgarbage("collect")
    local after = collectgarbage("count")
    print(string.format("GC Before:%.1fKB, After %.1fKB, -%.1fKB", before, after, before - after))
end

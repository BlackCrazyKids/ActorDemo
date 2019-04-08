---@field public events UnityEngine.AnimationEvent[]
---@field public length System.Single
---@field public frameRate System.Single
---@field public wrapMode UnityEngine.WrapMode
---@field public localBounds UnityEngine.Bounds
---@field public legacy System.Boolean
---@field public humanMotion System.Boolean
---@field public empty System.Boolean
---@class UnityEngine.AnimationClip : UnityEngine.Motion
local m = {}

---@param evt UnityEngine.AnimationEvent
---@return System.Void
function m:AddEvent(evt)end
---@param go UnityEngine.GameObject
---@param time System.Single
---@return System.Void
function m:SampleAnimation(go,time)end
---@param relativePath System.String
---@param type System.Type
---@param propertyName System.String
---@param curve UnityEngine.AnimationCurve
---@return System.Void
function m:SetCurve(relativePath,type,propertyName,curve)end
---@return System.Void
function m:EnsureQuaternionContinuity()end
---@return System.Void
function m:ClearCurves()end
UnityEngine = {}
UnityEngine.AnimationClip = m
return m

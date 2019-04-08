---@field public enabled System.Boolean
---@field public weight System.Single
---@field public wrapMode UnityEngine.WrapMode
---@field public time System.Single
---@field public normalizedTime System.Single
---@field public speed System.Single
---@field public normalizedSpeed System.Single
---@field public length System.Single
---@field public layer System.Int32
---@field public clip UnityEngine.AnimationClip
---@field public name System.String
---@field public blendMode UnityEngine.AnimationBlendMode
---@class UnityEngine.AnimationState : UnityEngine.TrackedReference
local m = {}

---@overload fun(mix : UnityEngine.Transform,recursive : System.Boolean) : System.Void
---@param mix UnityEngine.Transform
---@param recursive System.Boolean
---@return System.Void
function m:AddMixingTransform(mix,recursive)end
---@param mix UnityEngine.Transform
---@return System.Void
function m:RemoveMixingTransform(mix)end
UnityEngine = {}
UnityEngine.AnimationState = m
return m

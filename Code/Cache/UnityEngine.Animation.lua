---@field public clip UnityEngine.AnimationClip
---@field public playAutomatically System.Boolean
---@field public wrapMode UnityEngine.WrapMode
---@field public isPlaying System.Boolean
---@field public Item UnityEngine.AnimationState
---@field public animatePhysics System.Boolean
---@field public animateOnlyIfVisible System.Boolean
---@field public cullingType UnityEngine.AnimationCullingType
---@field public localBounds UnityEngine.Bounds
---@class UnityEngine.Animation : UnityEngine.Behaviour
local m = {}

---@overload fun() : System.Void
---@return System.Void
function m:Stop()end
---@overload fun(name : System.String) : System.Void
---@param name System.String
---@return System.Void
function m:Rewind(name)end
---@return System.Void
function m:Sample()end
---@param name System.String
---@return System.Boolean
function m:IsPlaying(name)end
---@overload fun() : System.Boolean
---@overload fun() : System.Boolean
---@overload fun() : System.Boolean
---@overload fun() : System.Boolean
---@overload fun() : System.Boolean
---@return System.Boolean
function m:Play()end
---@overload fun(animation : System.String,fadeLength : System.Single,mode : UnityEngine.PlayMode) : System.Void
---@overload fun(animation : System.String,fadeLength : System.Single,mode : UnityEngine.PlayMode) : System.Void
---@param animation System.String
---@param fadeLength System.Single
---@param mode UnityEngine.PlayMode
---@return System.Void
function m:CrossFade(animation,fadeLength,mode)end
---@overload fun(animation : System.String,targetWeight : System.Single,fadeLength : System.Single) : System.Void
---@overload fun(animation : System.String,targetWeight : System.Single,fadeLength : System.Single) : System.Void
---@param animation System.String
---@param targetWeight System.Single
---@param fadeLength System.Single
---@return System.Void
function m:Blend(animation,targetWeight,fadeLength)end
---@overload fun(animation : System.String,fadeLength : System.Single,queue : UnityEngine.QueueMode,mode : UnityEngine.PlayMode) : UnityEngine.AnimationState
---@overload fun(animation : System.String,fadeLength : System.Single,queue : UnityEngine.QueueMode,mode : UnityEngine.PlayMode) : UnityEngine.AnimationState
---@overload fun(animation : System.String,fadeLength : System.Single,queue : UnityEngine.QueueMode,mode : UnityEngine.PlayMode) : UnityEngine.AnimationState
---@param animation System.String
---@param fadeLength System.Single
---@param queue UnityEngine.QueueMode
---@param mode UnityEngine.PlayMode
---@return UnityEngine.AnimationState
function m:CrossFadeQueued(animation,fadeLength,queue,mode)end
---@overload fun(animation : System.String,queue : UnityEngine.QueueMode,mode : UnityEngine.PlayMode) : UnityEngine.AnimationState
---@overload fun(animation : System.String,queue : UnityEngine.QueueMode,mode : UnityEngine.PlayMode) : UnityEngine.AnimationState
---@param animation System.String
---@param queue UnityEngine.QueueMode
---@param mode UnityEngine.PlayMode
---@return UnityEngine.AnimationState
function m:PlayQueued(animation,queue,mode)end
---@overload fun(clip : UnityEngine.AnimationClip,newName : System.String) : System.Void
---@overload fun(clip : UnityEngine.AnimationClip,newName : System.String) : System.Void
---@param clip UnityEngine.AnimationClip
---@param newName System.String
---@return System.Void
function m:AddClip(clip,newName)end
---@overload fun(clip : UnityEngine.AnimationClip) : System.Void
---@param clip UnityEngine.AnimationClip
---@return System.Void
function m:RemoveClip(clip)end
---@return System.Int32
function m:GetClipCount()end
---@param layer System.Int32
---@return System.Void
function m:SyncLayer(layer)end
---@return System.Collections.IEnumerator
function m:GetEnumerator()end
---@param name System.String
---@return UnityEngine.AnimationClip
function m:GetClip(name)end
UnityEngine = {}
UnityEngine.Animation = m
return m

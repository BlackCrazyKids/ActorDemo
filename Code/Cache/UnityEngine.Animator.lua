---@field public isOptimizable System.Boolean
---@field public isHuman System.Boolean
---@field public hasRootMotion System.Boolean
---@field public humanScale System.Single
---@field public isInitialized System.Boolean
---@field public deltaPosition UnityEngine.Vector3
---@field public deltaRotation UnityEngine.Quaternion
---@field public velocity UnityEngine.Vector3
---@field public angularVelocity UnityEngine.Vector3
---@field public rootPosition UnityEngine.Vector3
---@field public rootRotation UnityEngine.Quaternion
---@field public applyRootMotion System.Boolean
---@field public linearVelocityBlending System.Boolean
---@field public animatePhysics System.Boolean
---@field public updateMode UnityEngine.AnimatorUpdateMode
---@field public hasTransformHierarchy System.Boolean
---@field public gravityWeight System.Single
---@field public bodyPosition UnityEngine.Vector3
---@field public bodyRotation UnityEngine.Quaternion
---@field public stabilizeFeet System.Boolean
---@field public layerCount System.Int32
---@field public parameters UnityEngine.AnimatorControllerParameter[]
---@field public parameterCount System.Int32
---@field public feetPivotActive System.Single
---@field public pivotWeight System.Single
---@field public pivotPosition UnityEngine.Vector3
---@field public isMatchingTarget System.Boolean
---@field public speed System.Single
---@field public targetPosition UnityEngine.Vector3
---@field public targetRotation UnityEngine.Quaternion
---@field public cullingMode UnityEngine.AnimatorCullingMode
---@field public playbackTime System.Single
---@field public recorderStartTime System.Single
---@field public recorderStopTime System.Single
---@field public recorderMode UnityEngine.AnimatorRecorderMode
---@field public runtimeAnimatorController UnityEngine.RuntimeAnimatorController
---@field public hasBoundPlayables System.Boolean
---@field public avatar UnityEngine.Avatar
---@field public playableGraph UnityEngine.Playables.PlayableGraph
---@field public layersAffectMassCenter System.Boolean
---@field public leftFeetBottomHeight System.Single
---@field public rightFeetBottomHeight System.Single
---@field public logWarnings System.Boolean
---@field public fireEvents System.Boolean
---@field public keepAnimatorControllerStateOnDisable System.Boolean
---@class UnityEngine.Animator : UnityEngine.Behaviour
local m = {}

---@param layerIndex System.Int32
---@return UnityEngine.AnimationInfo[]
function m:GetCurrentAnimationClipState(layerIndex)end
---@param layerIndex System.Int32
---@return UnityEngine.AnimationInfo[]
function m:GetNextAnimationClipState(layerIndex)end
---@return System.Void
function m:Stop()end
---@overload fun(name : System.String) : System.Single
---@param name System.String
---@return System.Single
function m:GetFloat(name)end
---@overload fun(name : System.String,value : System.Single) : System.Void
---@overload fun(name : System.String,value : System.Single) : System.Void
---@overload fun(name : System.String,value : System.Single) : System.Void
---@param name System.String
---@param value System.Single
---@return System.Void
function m:SetFloat(name,value)end
---@overload fun(name : System.String) : System.Boolean
---@param name System.String
---@return System.Boolean
function m:GetBool(name)end
---@overload fun(name : System.String,value : System.Boolean) : System.Void
---@param name System.String
---@param value System.Boolean
---@return System.Void
function m:SetBool(name,value)end
---@overload fun(name : System.String) : System.Int32
---@param name System.String
---@return System.Int32
function m:GetInteger(name)end
---@overload fun(name : System.String,value : System.Int32) : System.Void
---@param name System.String
---@param value System.Int32
---@return System.Void
function m:SetInteger(name,value)end
---@overload fun(name : System.String) : System.Void
---@param name System.String
---@return System.Void
function m:SetTrigger(name)end
---@overload fun(name : System.String) : System.Void
---@param name System.String
---@return System.Void
function m:ResetTrigger(name)end
---@overload fun(name : System.String) : System.Boolean
---@param name System.String
---@return System.Boolean
function m:IsParameterControlledByCurve(name)end
---@param goal UnityEngine.AvatarIKGoal
---@return UnityEngine.Vector3
function m:GetIKPosition(goal)end
---@param goal UnityEngine.AvatarIKGoal
---@param goalPosition UnityEngine.Vector3
---@return System.Void
function m:SetIKPosition(goal,goalPosition)end
---@param goal UnityEngine.AvatarIKGoal
---@return UnityEngine.Quaternion
function m:GetIKRotation(goal)end
---@param goal UnityEngine.AvatarIKGoal
---@param goalRotation UnityEngine.Quaternion
---@return System.Void
function m:SetIKRotation(goal,goalRotation)end
---@param goal UnityEngine.AvatarIKGoal
---@return System.Single
function m:GetIKPositionWeight(goal)end
---@param goal UnityEngine.AvatarIKGoal
---@param value System.Single
---@return System.Void
function m:SetIKPositionWeight(goal,value)end
---@param goal UnityEngine.AvatarIKGoal
---@return System.Single
function m:GetIKRotationWeight(goal)end
---@param goal UnityEngine.AvatarIKGoal
---@param value System.Single
---@return System.Void
function m:SetIKRotationWeight(goal,value)end
---@param hint UnityEngine.AvatarIKHint
---@return UnityEngine.Vector3
function m:GetIKHintPosition(hint)end
---@param hint UnityEngine.AvatarIKHint
---@param hintPosition UnityEngine.Vector3
---@return System.Void
function m:SetIKHintPosition(hint,hintPosition)end
---@param hint UnityEngine.AvatarIKHint
---@return System.Single
function m:GetIKHintPositionWeight(hint)end
---@param hint UnityEngine.AvatarIKHint
---@param value System.Single
---@return System.Void
function m:SetIKHintPositionWeight(hint,value)end
---@param lookAtPosition UnityEngine.Vector3
---@return System.Void
function m:SetLookAtPosition(lookAtPosition)end
---@overload fun(weight : System.Single) : System.Void
---@overload fun(weight : System.Single) : System.Void
---@overload fun(weight : System.Single) : System.Void
---@overload fun(weight : System.Single) : System.Void
---@param weight System.Single
---@return System.Void
function m:SetLookAtWeight(weight)end
---@param humanBoneId UnityEngine.HumanBodyBones
---@param rotation UnityEngine.Quaternion
---@return System.Void
function m:SetBoneLocalRotation(humanBoneId,rotation)end
function m:GetBehaviour()end
---@overload fun() : T[]
---@return T[]
function m:GetBehaviours()end
---@param layerIndex System.Int32
---@return System.String
function m:GetLayerName(layerIndex)end
---@param layerName System.String
---@return System.Int32
function m:GetLayerIndex(layerName)end
---@param layerIndex System.Int32
---@return System.Single
function m:GetLayerWeight(layerIndex)end
---@param layerIndex System.Int32
---@param weight System.Single
---@return System.Void
function m:SetLayerWeight(layerIndex,weight)end
---@param layerIndex System.Int32
---@return UnityEngine.AnimatorStateInfo
function m:GetCurrentAnimatorStateInfo(layerIndex)end
---@param layerIndex System.Int32
---@return UnityEngine.AnimatorStateInfo
function m:GetNextAnimatorStateInfo(layerIndex)end
---@param layerIndex System.Int32
---@return UnityEngine.AnimatorTransitionInfo
function m:GetAnimatorTransitionInfo(layerIndex)end
---@param layerIndex System.Int32
---@return System.Int32
function m:GetCurrentAnimatorClipInfoCount(layerIndex)end
---@param layerIndex System.Int32
---@return System.Int32
function m:GetNextAnimatorClipInfoCount(layerIndex)end
---@overload fun(layerIndex : System.Int32) : UnityEngine.AnimatorClipInfo[]
---@param layerIndex System.Int32
---@return UnityEngine.AnimatorClipInfo[]
function m:GetCurrentAnimatorClipInfo(layerIndex)end
---@overload fun(layerIndex : System.Int32) : UnityEngine.AnimatorClipInfo[]
---@param layerIndex System.Int32
---@return UnityEngine.AnimatorClipInfo[]
function m:GetNextAnimatorClipInfo(layerIndex)end
---@param layerIndex System.Int32
---@return System.Boolean
function m:IsInTransition(layerIndex)end
---@param index System.Int32
---@return UnityEngine.AnimatorControllerParameter
function m:GetParameter(index)end
---@overload fun(matchPosition : UnityEngine.Vector3,matchRotation : UnityEngine.Quaternion,targetBodyPart : UnityEngine.AvatarTarget,weightMask : UnityEngine.MatchTargetWeightMask,startNormalizedTime : System.Single) : System.Void
---@param matchPosition UnityEngine.Vector3
---@param matchRotation UnityEngine.Quaternion
---@param targetBodyPart UnityEngine.AvatarTarget
---@param weightMask UnityEngine.MatchTargetWeightMask
---@param startNormalizedTime System.Single
---@return System.Void
function m:MatchTarget(matchPosition,matchRotation,targetBodyPart,weightMask,startNormalizedTime)end
---@overload fun() : System.Void
---@return System.Void
function m:InterruptMatchTarget()end
---@param normalizedTime System.Single
---@return System.Void
function m:ForceStateNormalizedTime(normalizedTime)end
---@overload fun(stateName : System.String,fixedTransitionDuration : System.Single) : System.Void
---@overload fun(stateName : System.String,fixedTransitionDuration : System.Single) : System.Void
---@overload fun(stateName : System.String,fixedTransitionDuration : System.Single) : System.Void
---@overload fun(stateName : System.String,fixedTransitionDuration : System.Single) : System.Void
---@overload fun(stateName : System.String,fixedTransitionDuration : System.Single) : System.Void
---@overload fun(stateName : System.String,fixedTransitionDuration : System.Single) : System.Void
---@overload fun(stateName : System.String,fixedTransitionDuration : System.Single) : System.Void
---@param stateName System.String
---@param fixedTransitionDuration System.Single
---@return System.Void
function m:CrossFadeInFixedTime(stateName,fixedTransitionDuration)end
---@overload fun(stateName : System.String,normalizedTransitionDuration : System.Single,layer : System.Int32,normalizedTimeOffset : System.Single) : System.Void
---@overload fun(stateName : System.String,normalizedTransitionDuration : System.Single,layer : System.Int32,normalizedTimeOffset : System.Single) : System.Void
---@overload fun(stateName : System.String,normalizedTransitionDuration : System.Single,layer : System.Int32,normalizedTimeOffset : System.Single) : System.Void
---@overload fun(stateName : System.String,normalizedTransitionDuration : System.Single,layer : System.Int32,normalizedTimeOffset : System.Single) : System.Void
---@overload fun(stateName : System.String,normalizedTransitionDuration : System.Single,layer : System.Int32,normalizedTimeOffset : System.Single) : System.Void
---@overload fun(stateName : System.String,normalizedTransitionDuration : System.Single,layer : System.Int32,normalizedTimeOffset : System.Single) : System.Void
---@overload fun(stateName : System.String,normalizedTransitionDuration : System.Single,layer : System.Int32,normalizedTimeOffset : System.Single) : System.Void
---@param stateName System.String
---@param normalizedTransitionDuration System.Single
---@param layer System.Int32
---@param normalizedTimeOffset System.Single
---@return System.Void
function m:CrossFade(stateName,normalizedTransitionDuration,layer,normalizedTimeOffset)end
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@param stateName System.String
---@param layer System.Int32
---@return System.Void
function m:PlayInFixedTime(stateName,layer)end
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@overload fun(stateName : System.String,layer : System.Int32) : System.Void
---@param stateName System.String
---@param layer System.Int32
---@return System.Void
function m:Play(stateName,layer)end
---@param targetIndex UnityEngine.AvatarTarget
---@param targetNormalizedTime System.Single
---@return System.Void
function m:SetTarget(targetIndex,targetNormalizedTime)end
---@param transform UnityEngine.Transform
---@return System.Boolean
function m:IsControlled(transform)end
---@param humanBoneId UnityEngine.HumanBodyBones
---@return UnityEngine.Transform
function m:GetBoneTransform(humanBoneId)end
---@return System.Void
function m:StartPlayback()end
---@return System.Void
function m:StopPlayback()end
---@param frameCount System.Int32
---@return System.Void
function m:StartRecording(frameCount)end
---@return System.Void
function m:StopRecording()end
---@param layerIndex System.Int32
---@param stateID System.Int32
---@return System.Boolean
function m:HasState(layerIndex,stateID)end
---@param name System.String
---@return System.Int32
function m.StringToHash(name)end
---@param deltaTime System.Single
---@return System.Void
function m:Update(deltaTime)end
---@return System.Void
function m:Rebind()end
---@return System.Void
function m:ApplyBuiltinRootMotion()end
---@overload fun(name : System.String) : UnityEngine.Vector3
---@param name System.String
---@return UnityEngine.Vector3
function m:GetVector(name)end
---@overload fun(name : System.String,value : UnityEngine.Vector3) : System.Void
---@param name System.String
---@param value UnityEngine.Vector3
---@return System.Void
function m:SetVector(name,value)end
---@overload fun(name : System.String) : UnityEngine.Quaternion
---@param name System.String
---@return UnityEngine.Quaternion
function m:GetQuaternion(name)end
---@overload fun(name : System.String,value : UnityEngine.Quaternion) : System.Void
---@param name System.String
---@param value UnityEngine.Quaternion
---@return System.Void
function m:SetQuaternion(name,value)end
UnityEngine = {}
UnityEngine.Animator = m
return m

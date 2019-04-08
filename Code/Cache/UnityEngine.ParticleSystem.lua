---@field public isPlaying System.Boolean
---@field public isEmitting System.Boolean
---@field public isStopped System.Boolean
---@field public isPaused System.Boolean
---@field public time System.Single
---@field public particleCount System.Int32
---@field public randomSeed System.UInt32
---@field public useAutoRandomSeed System.Boolean
---@field public automaticCullingEnabled System.Boolean
---@field public main UnityEngine.ParticleSystem+MainModule
---@field public emission UnityEngine.ParticleSystem+EmissionModule
---@field public shape UnityEngine.ParticleSystem+ShapeModule
---@field public velocityOverLifetime UnityEngine.ParticleSystem+VelocityOverLifetimeModule
---@field public limitVelocityOverLifetime UnityEngine.ParticleSystem+LimitVelocityOverLifetimeModule
---@field public inheritVelocity UnityEngine.ParticleSystem+InheritVelocityModule
---@field public forceOverLifetime UnityEngine.ParticleSystem+ForceOverLifetimeModule
---@field public colorOverLifetime UnityEngine.ParticleSystem+ColorOverLifetimeModule
---@field public colorBySpeed UnityEngine.ParticleSystem+ColorBySpeedModule
---@field public sizeOverLifetime UnityEngine.ParticleSystem+SizeOverLifetimeModule
---@field public sizeBySpeed UnityEngine.ParticleSystem+SizeBySpeedModule
---@field public rotationOverLifetime UnityEngine.ParticleSystem+RotationOverLifetimeModule
---@field public rotationBySpeed UnityEngine.ParticleSystem+RotationBySpeedModule
---@field public externalForces UnityEngine.ParticleSystem+ExternalForcesModule
---@field public noise UnityEngine.ParticleSystem+NoiseModule
---@field public collision UnityEngine.ParticleSystem+CollisionModule
---@field public trigger UnityEngine.ParticleSystem+TriggerModule
---@field public subEmitters UnityEngine.ParticleSystem+SubEmittersModule
---@field public textureSheetAnimation UnityEngine.ParticleSystem+TextureSheetAnimationModule
---@field public lights UnityEngine.ParticleSystem+LightsModule
---@field public trails UnityEngine.ParticleSystem+TrailModule
---@field public customData UnityEngine.ParticleSystem+CustomDataModule
---@field public safeCollisionEventSize System.Int32
---@field public startDelay System.Single
---@field public loop System.Boolean
---@field public playOnAwake System.Boolean
---@field public duration System.Single
---@field public playbackSpeed System.Single
---@field public enableEmission System.Boolean
---@field public emissionRate System.Single
---@field public startSpeed System.Single
---@field public startSize System.Single
---@field public startColor UnityEngine.Color
---@field public startRotation System.Single
---@field public startRotation3D UnityEngine.Vector3
---@field public startLifetime System.Single
---@field public gravityModifier System.Single
---@field public maxParticles System.Int32
---@field public simulationSpace UnityEngine.ParticleSystemSimulationSpace
---@field public scalingMode UnityEngine.ParticleSystemScalingMode
---@class UnityEngine.ParticleSystem : UnityEngine.Component
local m = {}

---@param particles UnityEngine.ParticleSystem+Particle[]
---@param size System.Int32
---@return System.Void
function m:SetParticles(particles,size)end
---@param particles UnityEngine.ParticleSystem+Particle[]
---@return System.Int32
function m:GetParticles(particles)end
---@param customData System.Collections.Generic.List`1[[UnityEngine.Vector4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@param streamIndex UnityEngine.ParticleSystemCustomData
---@return System.Void
function m:SetCustomParticleData(customData,streamIndex)end
---@param customData System.Collections.Generic.List`1[[UnityEngine.Vector4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@param streamIndex UnityEngine.ParticleSystemCustomData
---@return System.Int32
function m:GetCustomParticleData(customData,streamIndex)end
---@overload fun(t : System.Single,withChildren : System.Boolean,restart : System.Boolean,fixedTimeStep : System.Boolean) : System.Void
---@overload fun(t : System.Single,withChildren : System.Boolean,restart : System.Boolean,fixedTimeStep : System.Boolean) : System.Void
---@overload fun(t : System.Single,withChildren : System.Boolean,restart : System.Boolean,fixedTimeStep : System.Boolean) : System.Void
---@param t System.Single
---@param withChildren System.Boolean
---@param restart System.Boolean
---@param fixedTimeStep System.Boolean
---@return System.Void
function m:Simulate(t,withChildren,restart,fixedTimeStep)end
---@overload fun(withChildren : System.Boolean) : System.Void
---@param withChildren System.Boolean
---@return System.Void
function m:Play(withChildren)end
---@overload fun(withChildren : System.Boolean) : System.Void
---@param withChildren System.Boolean
---@return System.Void
function m:Pause(withChildren)end
---@overload fun(withChildren : System.Boolean,stopBehavior : UnityEngine.ParticleSystemStopBehavior) : System.Void
---@overload fun(withChildren : System.Boolean,stopBehavior : UnityEngine.ParticleSystemStopBehavior) : System.Void
---@param withChildren System.Boolean
---@param stopBehavior UnityEngine.ParticleSystemStopBehavior
---@return System.Void
function m:Stop(withChildren,stopBehavior)end
---@overload fun(withChildren : System.Boolean) : System.Void
---@param withChildren System.Boolean
---@return System.Void
function m:Clear(withChildren)end
---@overload fun(withChildren : System.Boolean) : System.Boolean
---@param withChildren System.Boolean
---@return System.Boolean
function m:IsAlive(withChildren)end
---@overload fun(count : System.Int32) : System.Void
---@overload fun(count : System.Int32) : System.Void
---@overload fun(count : System.Int32) : System.Void
---@param count System.Int32
---@return System.Void
function m:Emit(count)end
---@overload fun(subEmitterIndex : System.Int32) : System.Void
---@overload fun(subEmitterIndex : System.Int32) : System.Void
---@param subEmitterIndex System.Int32
---@return System.Void
function m:TriggerSubEmitter(subEmitterIndex)end
UnityEngine = {}
UnityEngine.ParticleSystem = m
return m

---@field public lightmapTilingOffset UnityEngine.Vector4
---@field public lightProbeAnchor UnityEngine.Transform
---@field public castShadows System.Boolean
---@field public motionVectors System.Boolean
---@field public useLightProbes System.Boolean
---@field public bounds UnityEngine.Bounds
---@field public enabled System.Boolean
---@field public isVisible System.Boolean
---@field public shadowCastingMode UnityEngine.Rendering.ShadowCastingMode
---@field public receiveShadows System.Boolean
---@field public motionVectorGenerationMode UnityEngine.MotionVectorGenerationMode
---@field public lightProbeUsage UnityEngine.Rendering.LightProbeUsage
---@field public reflectionProbeUsage UnityEngine.Rendering.ReflectionProbeUsage
---@field public renderingLayerMask System.UInt32
---@field public sortingLayerName System.String
---@field public sortingLayerID System.Int32
---@field public sortingOrder System.Int32
---@field public allowOcclusionWhenDynamic System.Boolean
---@field public isPartOfStaticBatch System.Boolean
---@field public worldToLocalMatrix UnityEngine.Matrix4x4
---@field public localToWorldMatrix UnityEngine.Matrix4x4
---@field public lightProbeProxyVolumeOverride UnityEngine.GameObject
---@field public probeAnchor UnityEngine.Transform
---@field public lightmapIndex System.Int32
---@field public realtimeLightmapIndex System.Int32
---@field public lightmapScaleOffset UnityEngine.Vector4
---@field public realtimeLightmapScaleOffset UnityEngine.Vector4
---@field public materials UnityEngine.Material[]
---@field public material UnityEngine.Material
---@field public sharedMaterial UnityEngine.Material
---@field public sharedMaterials UnityEngine.Material[]
---@class UnityEngine.Renderer : UnityEngine.Component
local m = {}

---@return System.Boolean
function m:HasPropertyBlock()end
---@overload fun(properties : UnityEngine.MaterialPropertyBlock) : System.Void
---@param properties UnityEngine.MaterialPropertyBlock
---@return System.Void
function m:SetPropertyBlock(properties)end
---@overload fun(properties : UnityEngine.MaterialPropertyBlock) : System.Void
---@param properties UnityEngine.MaterialPropertyBlock
---@return System.Void
function m:GetPropertyBlock(properties)end
---@param m System.Collections.Generic.List`1[[UnityEngine.Material, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Void
function m:GetMaterials(m)end
---@param m System.Collections.Generic.List`1[[UnityEngine.Material, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Void
function m:GetSharedMaterials(m)end
---@param result System.Collections.Generic.List`1[[UnityEngine.Rendering.ReflectionProbeBlendInfo, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Void
function m:GetClosestReflectionProbes(result)end
UnityEngine = {}
UnityEngine.Renderer = m
return m

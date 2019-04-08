---@field public activeColorGamut UnityEngine.ColorGamut
---@field public activeTier UnityEngine.Rendering.GraphicsTier
---@field public activeColorBuffer UnityEngine.RenderBuffer
---@field public activeDepthBuffer UnityEngine.RenderBuffer
---@field public deviceName System.String
---@field public deviceVendor System.String
---@field public deviceVersion System.String
---@class UnityEngine.Graphics : System.Object
local m = {}

---@return System.Void
function m.ClearRandomWriteTargets()end
---@param buffer UnityEngine.Rendering.CommandBuffer
---@return System.Void
function m.ExecuteCommandBuffer(buffer)end
---@param buffer UnityEngine.Rendering.CommandBuffer
---@param queueType UnityEngine.Rendering.ComputeQueueType
---@return System.Void
function m.ExecuteCommandBufferAsync(buffer,queueType)end
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@overload fun(rt : UnityEngine.RenderTexture,mipLevel : System.Int32,face : UnityEngine.CubemapFace,depthSlice : System.Int32) : System.Void
---@param rt UnityEngine.RenderTexture
---@param mipLevel System.Int32
---@param face UnityEngine.CubemapFace
---@param depthSlice System.Int32
---@return System.Void
function m.SetRenderTarget(rt,mipLevel,face,depthSlice)end
---@overload fun(index : System.Int32,uav : UnityEngine.RenderTexture) : System.Void
---@overload fun(index : System.Int32,uav : UnityEngine.RenderTexture) : System.Void
---@param index System.Int32
---@param uav UnityEngine.RenderTexture
---@return System.Void
function m.SetRandomWriteTarget(index,uav)end
---@overload fun(src : UnityEngine.Texture,dst : UnityEngine.Texture) : System.Void
---@overload fun(src : UnityEngine.Texture,dst : UnityEngine.Texture) : System.Void
---@overload fun(src : UnityEngine.Texture,dst : UnityEngine.Texture) : System.Void
---@param src UnityEngine.Texture
---@param dst UnityEngine.Texture
---@return System.Void
function m.CopyTexture(src,dst)end
---@overload fun(src : UnityEngine.Texture,dst : UnityEngine.Texture) : System.Boolean
---@param src UnityEngine.Texture
---@param dst UnityEngine.Texture
---@return System.Boolean
function m.ConvertTexture(src,dst)end
---@overload fun(stage : UnityEngine.Rendering.SynchronisationStage) : UnityEngine.Rendering.GPUFence
---@param stage UnityEngine.Rendering.SynchronisationStage
---@return UnityEngine.Rendering.GPUFence
function m.CreateGPUFence(stage)end
---@overload fun(fence : UnityEngine.Rendering.GPUFence,stage : UnityEngine.Rendering.SynchronisationStage) : System.Void
---@param fence UnityEngine.Rendering.GPUFence
---@param stage UnityEngine.Rendering.SynchronisationStage
---@return System.Void
function m.WaitOnGPUFence(fence,stage)end
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@overload fun(screenRect : UnityEngine.Rect,texture : UnityEngine.Texture,sourceRect : UnityEngine.Rect,leftBorder : System.Int32,rightBorder : System.Int32,topBorder : System.Int32,bottomBorder : System.Int32,color : UnityEngine.Color,mat : UnityEngine.Material,pass : System.Int32) : System.Void
---@param screenRect UnityEngine.Rect
---@param texture UnityEngine.Texture
---@param sourceRect UnityEngine.Rect
---@param leftBorder System.Int32
---@param rightBorder System.Int32
---@param topBorder System.Int32
---@param bottomBorder System.Int32
---@param color UnityEngine.Color
---@param mat UnityEngine.Material
---@param pass System.Int32
---@return System.Void
function m.DrawTexture(screenRect,texture,sourceRect,leftBorder,rightBorder,topBorder,bottomBorder,color,mat,pass)end
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,materialIndex : System.Int32) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,materialIndex : System.Int32) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,materialIndex : System.Int32) : System.Void
---@param mesh UnityEngine.Mesh
---@param position UnityEngine.Vector3
---@param rotation UnityEngine.Quaternion
---@param materialIndex System.Int32
---@return System.Void
function m.DrawMeshNow(mesh,position,rotation,materialIndex)end
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion,material : UnityEngine.Material,layer : System.Int32,camera : UnityEngine.Camera,submeshIndex : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : System.Boolean,receiveShadows : System.Boolean,useLightProbes : System.Boolean) : System.Void
---@param mesh UnityEngine.Mesh
---@param position UnityEngine.Vector3
---@param rotation UnityEngine.Quaternion
---@param material UnityEngine.Material
---@param layer System.Int32
---@param camera UnityEngine.Camera
---@param submeshIndex System.Int32
---@param properties UnityEngine.MaterialPropertyBlock
---@param castShadows System.Boolean
---@param receiveShadows System.Boolean
---@param useLightProbes System.Boolean
---@return System.Void
function m.DrawMesh(mesh,position,rotation,material,layer,camera,submeshIndex,properties,castShadows,receiveShadows,useLightProbes)end
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,matrices : UnityEngine.Matrix4x4[],count : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@param mesh UnityEngine.Mesh
---@param submeshIndex System.Int32
---@param material UnityEngine.Material
---@param matrices UnityEngine.Matrix4x4[]
---@param count System.Int32
---@param properties UnityEngine.MaterialPropertyBlock
---@param castShadows UnityEngine.Rendering.ShadowCastingMode
---@param receiveShadows System.Boolean
---@param layer System.Int32
---@param camera UnityEngine.Camera
---@param lightProbeUsage UnityEngine.Rendering.LightProbeUsage
---@param lightProbeProxyVolume UnityEngine.LightProbeProxyVolume
---@return System.Void
function m.DrawMeshInstanced(mesh,submeshIndex,material,matrices,count,properties,castShadows,receiveShadows,layer,camera,lightProbeUsage,lightProbeProxyVolume)end
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,bounds : UnityEngine.Bounds,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,bounds : UnityEngine.Bounds,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,bounds : UnityEngine.Bounds,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,bounds : UnityEngine.Bounds,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,bounds : UnityEngine.Bounds,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,bounds : UnityEngine.Bounds,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,bounds : UnityEngine.Bounds,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@overload fun(mesh : UnityEngine.Mesh,submeshIndex : System.Int32,material : UnityEngine.Material,bounds : UnityEngine.Bounds,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32,properties : UnityEngine.MaterialPropertyBlock,castShadows : UnityEngine.Rendering.ShadowCastingMode,receiveShadows : System.Boolean,layer : System.Int32,camera : UnityEngine.Camera,lightProbeUsage : UnityEngine.Rendering.LightProbeUsage,lightProbeProxyVolume : UnityEngine.LightProbeProxyVolume) : System.Void
---@param mesh UnityEngine.Mesh
---@param submeshIndex System.Int32
---@param material UnityEngine.Material
---@param bounds UnityEngine.Bounds
---@param bufferWithArgs UnityEngine.ComputeBuffer
---@param argsOffset System.Int32
---@param properties UnityEngine.MaterialPropertyBlock
---@param castShadows UnityEngine.Rendering.ShadowCastingMode
---@param receiveShadows System.Boolean
---@param layer System.Int32
---@param camera UnityEngine.Camera
---@param lightProbeUsage UnityEngine.Rendering.LightProbeUsage
---@param lightProbeProxyVolume UnityEngine.LightProbeProxyVolume
---@return System.Void
function m.DrawMeshInstancedIndirect(mesh,submeshIndex,material,bounds,bufferWithArgs,argsOffset,properties,castShadows,receiveShadows,layer,camera,lightProbeUsage,lightProbeProxyVolume)end
---@overload fun(topology : UnityEngine.MeshTopology,vertexCount : System.Int32,instanceCount : System.Int32) : System.Void
---@param topology UnityEngine.MeshTopology
---@param vertexCount System.Int32
---@param instanceCount System.Int32
---@return System.Void
function m.DrawProcedural(topology,vertexCount,instanceCount)end
---@overload fun(topology : UnityEngine.MeshTopology,bufferWithArgs : UnityEngine.ComputeBuffer,argsOffset : System.Int32) : System.Void
---@param topology UnityEngine.MeshTopology
---@param bufferWithArgs UnityEngine.ComputeBuffer
---@param argsOffset System.Int32
---@return System.Void
function m.DrawProceduralIndirect(topology,bufferWithArgs,argsOffset)end
---@overload fun(source : UnityEngine.Texture,dest : UnityEngine.RenderTexture) : System.Void
---@overload fun(source : UnityEngine.Texture,dest : UnityEngine.RenderTexture) : System.Void
---@overload fun(source : UnityEngine.Texture,dest : UnityEngine.RenderTexture) : System.Void
---@overload fun(source : UnityEngine.Texture,dest : UnityEngine.RenderTexture) : System.Void
---@overload fun(source : UnityEngine.Texture,dest : UnityEngine.RenderTexture) : System.Void
---@param source UnityEngine.Texture
---@param dest UnityEngine.RenderTexture
---@return System.Void
function m.Blit(source,dest)end
---@param source UnityEngine.Texture
---@param dest UnityEngine.RenderTexture
---@param mat UnityEngine.Material
---@param offsets UnityEngine.Vector2[]
---@return System.Void
function m.BlitMultiTap(source,dest,mat,offsets)end
UnityEngine = {}
UnityEngine.Graphics = m
return m

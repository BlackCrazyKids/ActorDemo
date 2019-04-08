---@field public depth System.Int32
---@field public width System.Int32
---@field public height System.Int32
---@field public dimension UnityEngine.Rendering.TextureDimension
---@field public useMipMap System.Boolean
---@field public sRGB System.Boolean
---@field public format UnityEngine.RenderTextureFormat
---@field public vrUsage UnityEngine.VRTextureUsage
---@field public memorylessMode UnityEngine.RenderTextureMemoryless
---@field public autoGenerateMips System.Boolean
---@field public volumeDepth System.Int32
---@field public antiAliasing System.Int32
---@field public bindTextureMS System.Boolean
---@field public enableRandomWrite System.Boolean
---@field public useDynamicScale System.Boolean
---@field public isPowerOfTwo System.Boolean
---@field public active UnityEngine.RenderTexture
---@field public colorBuffer UnityEngine.RenderBuffer
---@field public depthBuffer UnityEngine.RenderBuffer
---@field public descriptor UnityEngine.RenderTextureDescriptor
---@field public generateMips System.Boolean
---@field public isCubemap System.Boolean
---@field public isVolume System.Boolean
---@field public enabled System.Boolean
---@class UnityEngine.RenderTexture : UnityEngine.Texture
local m = {}

---@param temp UnityEngine.RenderTexture
---@return System.Void
function m.ReleaseTemporary(temp)end
---@return System.IntPtr
function m:GetNativeDepthBufferPtr()end
---@overload fun(discardColor : System.Boolean,discardDepth : System.Boolean) : System.Void
---@param discardColor System.Boolean
---@param discardDepth System.Boolean
---@return System.Void
function m:DiscardContents(discardColor,discardDepth)end
---@return System.Void
function m:MarkRestoreExpected()end
---@overload fun() : System.Void
---@return System.Void
function m:ResolveAntiAliasedSurface()end
---@param propertyName System.String
---@return System.Void
function m:SetGlobalShaderProperty(propertyName)end
---@return System.Boolean
function m:Create()end
---@return System.Void
function m:Release()end
---@return System.Boolean
function m:IsCreated()end
---@return System.Void
function m:GenerateMips()end
---@param equirect UnityEngine.RenderTexture
---@param eye UnityEngine.Camera+MonoOrStereoscopicEye
---@return System.Void
function m:ConvertToEquirect(equirect,eye)end
---@param rt UnityEngine.RenderTexture
---@return System.Boolean
function m.SupportsStencil(rt)end
---@overload fun(desc : UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
---@overload fun(desc : UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
---@overload fun(desc : UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
---@overload fun(desc : UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
---@overload fun(desc : UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
---@overload fun(desc : UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
---@overload fun(desc : UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
---@overload fun(desc : UnityEngine.RenderTextureDescriptor) : UnityEngine.RenderTexture
---@param desc UnityEngine.RenderTextureDescriptor
---@return UnityEngine.RenderTexture
function m.GetTemporary(desc)end
---@param color UnityEngine.Color
---@return System.Void
function m:SetBorderColor(color)end
---@return UnityEngine.Vector2
function m:GetTexelOffset()end
UnityEngine = {}
UnityEngine.RenderTexture = m
return m

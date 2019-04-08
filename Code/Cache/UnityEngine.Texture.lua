---@field public masterTextureLimit System.Int32
---@field public anisotropicFiltering UnityEngine.AnisotropicFiltering
---@field public width System.Int32
---@field public height System.Int32
---@field public dimension UnityEngine.Rendering.TextureDimension
---@field public wrapMode UnityEngine.TextureWrapMode
---@field public wrapModeU UnityEngine.TextureWrapMode
---@field public wrapModeV UnityEngine.TextureWrapMode
---@field public wrapModeW UnityEngine.TextureWrapMode
---@field public filterMode UnityEngine.FilterMode
---@field public anisoLevel System.Int32
---@field public mipMapBias System.Single
---@field public texelSize UnityEngine.Vector2
---@field public updateCount System.UInt32
---@field public imageContentsHash UnityEngine.Hash128
---@field public totalTextureMemory System.UInt64
---@field public desiredTextureMemory System.UInt64
---@field public targetTextureMemory System.UInt64
---@field public currentTextureMemory System.UInt64
---@field public nonStreamingTextureMemory System.UInt64
---@field public streamingMipmapUploadCount System.UInt64
---@field public streamingRendererCount System.UInt64
---@field public streamingTextureCount System.UInt64
---@field public nonStreamingTextureCount System.UInt64
---@field public streamingTexturePendingLoadCount System.UInt64
---@field public streamingTextureLoadingCount System.UInt64
---@field public streamingTextureForceLoadAll System.Boolean
---@field public streamingTextureDiscardUnusedMips System.Boolean
---@class UnityEngine.Texture : UnityEngine.Object
local m = {}

---@param forcedMin System.Int32
---@param globalMax System.Int32
---@return System.Void
function m.SetGlobalAnisotropicFilteringLimits(forcedMin,globalMax)end
---@return System.IntPtr
function m:GetNativeTexturePtr()end
---@return System.Int32
function m:GetNativeTextureID()end
---@return System.Void
function m:IncrementUpdateCount()end
---@return System.Void
function m.SetStreamingTextureMaterialDebugProperties()end
UnityEngine = {}
UnityEngine.Texture = m
return m

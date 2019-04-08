---@field public alphaIsTransparency System.Boolean
---@field public mipmapCount System.Int32
---@field public format UnityEngine.TextureFormat
---@field public whiteTexture UnityEngine.Texture2D
---@field public blackTexture UnityEngine.Texture2D
---@field public streamingMipmaps System.Boolean
---@field public streamingMipmapsPriority System.Int32
---@field public requestedMipmapLevel System.Int32
---@field public desiredMipmapLevel System.Int32
---@field public loadingMipmapLevel System.Int32
---@field public loadedMipmapLevel System.Int32
---@class UnityEngine.Texture2D : UnityEngine.Texture
local m = {}

---@param nativeTex System.IntPtr
---@return System.Void
function m:UpdateExternalTexture(nativeTex)end
---@overload fun(colors : UnityEngine.Color32[]) : System.Void
---@overload fun(colors : UnityEngine.Color32[]) : System.Void
---@overload fun(colors : UnityEngine.Color32[]) : System.Void
---@param colors UnityEngine.Color32[]
---@return System.Void
function m:SetPixels32(colors)end
---@overload fun() : System.Byte[]
---@return System.Byte[]
function m:GetRawTextureData()end
---@overload fun() : UnityEngine.Color[]
---@overload fun() : UnityEngine.Color[]
---@overload fun() : UnityEngine.Color[]
---@return UnityEngine.Color[]
function m:GetPixels()end
---@overload fun(miplevel : System.Int32) : UnityEngine.Color32[]
---@param miplevel System.Int32
---@return UnityEngine.Color32[]
function m:GetPixels32(miplevel)end
---@overload fun(textures : UnityEngine.Texture2D[],padding : System.Int32,maximumAtlasSize : System.Int32,makeNoLongerReadable : System.Boolean) : UnityEngine.Rect[]
---@overload fun(textures : UnityEngine.Texture2D[],padding : System.Int32,maximumAtlasSize : System.Int32,makeNoLongerReadable : System.Boolean) : UnityEngine.Rect[]
---@param textures UnityEngine.Texture2D[]
---@param padding System.Int32
---@param maximumAtlasSize System.Int32
---@param makeNoLongerReadable System.Boolean
---@return UnityEngine.Rect[]
function m:PackTextures(textures,padding,maximumAtlasSize,makeNoLongerReadable)end
---@param highQuality System.Boolean
---@return System.Void
function m:Compress(highQuality)end
---@return System.Void
function m:ClearRequestedMipmapLevel()end
---@return System.Boolean
function m:IsRequestedMipmapLevelLoaded()end
---@param width System.Int32
---@param height System.Int32
---@param format UnityEngine.TextureFormat
---@param mipChain System.Boolean
---@param linear System.Boolean
---@param nativeTex System.IntPtr
---@return UnityEngine.Texture2D
function m.CreateExternalTexture(width,height,format,mipChain,linear,nativeTex)end
---@param x System.Int32
---@param y System.Int32
---@param color UnityEngine.Color
---@return System.Void
function m:SetPixel(x,y,color)end
---@overload fun(x : System.Int32,y : System.Int32,blockWidth : System.Int32,blockHeight : System.Int32,colors : UnityEngine.Color[],miplevel : System.Int32) : System.Void
---@overload fun(x : System.Int32,y : System.Int32,blockWidth : System.Int32,blockHeight : System.Int32,colors : UnityEngine.Color[],miplevel : System.Int32) : System.Void
---@overload fun(x : System.Int32,y : System.Int32,blockWidth : System.Int32,blockHeight : System.Int32,colors : UnityEngine.Color[],miplevel : System.Int32) : System.Void
---@param x System.Int32
---@param y System.Int32
---@param blockWidth System.Int32
---@param blockHeight System.Int32
---@param colors UnityEngine.Color[]
---@param miplevel System.Int32
---@return System.Void
function m:SetPixels(x,y,blockWidth,blockHeight,colors,miplevel)end
---@param x System.Int32
---@param y System.Int32
---@return UnityEngine.Color
function m:GetPixel(x,y)end
---@param x System.Single
---@param y System.Single
---@return UnityEngine.Color
function m:GetPixelBilinear(x,y)end
---@overload fun(data : System.IntPtr,size : System.Int32) : System.Void
---@overload fun(data : System.IntPtr,size : System.Int32) : System.Void
---@param data System.IntPtr
---@param size System.Int32
---@return System.Void
function m:LoadRawTextureData(data,size)end
---@overload fun(updateMipmaps : System.Boolean,makeNoLongerReadable : System.Boolean) : System.Void
---@overload fun(updateMipmaps : System.Boolean,makeNoLongerReadable : System.Boolean) : System.Void
---@param updateMipmaps System.Boolean
---@param makeNoLongerReadable System.Boolean
---@return System.Void
function m:Apply(updateMipmaps,makeNoLongerReadable)end
---@overload fun(width : System.Int32,height : System.Int32) : System.Boolean
---@param width System.Int32
---@param height System.Int32
---@return System.Boolean
function m:Resize(width,height)end
---@overload fun(source : UnityEngine.Rect,destX : System.Int32,destY : System.Int32,recalculateMipMaps : System.Boolean) : System.Void
---@param source UnityEngine.Rect
---@param destX System.Int32
---@param destY System.Int32
---@param recalculateMipMaps System.Boolean
---@return System.Void
function m:ReadPixels(source,destX,destY,recalculateMipMaps)end
---@param sizes UnityEngine.Vector2[]
---@param padding System.Int32
---@param atlasSize System.Int32
---@param results System.Collections.Generic.List`1[[UnityEngine.Rect, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Boolean
function m.GenerateAtlas(sizes,padding,atlasSize,results)end
UnityEngine = {}
UnityEngine.Texture2D = m
return m

---@field public assetBundle UnityEngine.AssetBundle
---@field public audioClip UnityEngine.Object
---@field public bytes System.Byte[]
---@field public movie UnityEngine.Object
---@field public size System.Int32
---@field public bytesDownloaded System.Int32
---@field public error System.String
---@field public isDone System.Boolean
---@field public oggVorbis UnityEngine.Object
---@field public progress System.Single
---@field public responseHeaders System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@field public data System.String
---@field public text System.String
---@field public texture UnityEngine.Texture2D
---@field public textureNonReadable UnityEngine.Texture2D
---@field public threadPriority UnityEngine.ThreadPriority
---@field public uploadProgress System.Single
---@field public url System.String
---@field public keepWaiting System.Boolean
---@class UnityEngine.WWW : UnityEngine.CustomYieldInstruction
local m = {}

---@overload fun(s : System.String) : System.String
---@param s System.String
---@return System.String
function m.EscapeURL(s)end
---@overload fun(s : System.String) : System.String
---@param s System.String
---@return System.String
function m.UnEscapeURL(s)end
---@overload fun(url : System.String,version : System.Int32) : UnityEngine.WWW
---@overload fun(url : System.String,version : System.Int32) : UnityEngine.WWW
---@overload fun(url : System.String,version : System.Int32) : UnityEngine.WWW
---@overload fun(url : System.String,version : System.Int32) : UnityEngine.WWW
---@param url System.String
---@param version System.Int32
---@return UnityEngine.WWW
function m.LoadFromCacheOrDownload(url,version)end
---@param texture UnityEngine.Texture2D
---@return System.Void
function m:LoadImageIntoTexture(texture)end
---@return System.Void
function m:Dispose()end
---@overload fun() : UnityEngine.AudioClip
---@overload fun() : UnityEngine.AudioClip
---@overload fun() : UnityEngine.AudioClip
---@return UnityEngine.AudioClip
function m:GetAudioClip()end
---@overload fun() : UnityEngine.AudioClip
---@overload fun() : UnityEngine.AudioClip
---@return UnityEngine.AudioClip
function m:GetAudioClipCompressed()end
---@return UnityEngine.MovieTexture
function m:GetMovieTexture()end
UnityEngine = {}
UnityEngine.WWW = m
return m

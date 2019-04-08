---@field public length System.Single
---@field public samples System.Int32
---@field public channels System.Int32
---@field public frequency System.Int32
---@field public isReadyToPlay System.Boolean
---@field public loadType UnityEngine.AudioClipLoadType
---@field public preloadAudioData System.Boolean
---@field public ambisonic System.Boolean
---@field public loadState UnityEngine.AudioDataLoadState
---@field public loadInBackground System.Boolean
---@class UnityEngine.AudioClip : UnityEngine.Object
local m = {}

---@return System.Boolean
function m:LoadAudioData()end
---@return System.Boolean
function m:UnloadAudioData()end
---@param data System.Single[]
---@param offsetSamples System.Int32
---@return System.Boolean
function m:GetData(data,offsetSamples)end
---@param data System.Single[]
---@param offsetSamples System.Int32
---@return System.Boolean
function m:SetData(data,offsetSamples)end
---@overload fun(name : System.String,lengthSamples : System.Int32,channels : System.Int32,frequency : System.Int32,_3D : System.Boolean,stream : System.Boolean) : UnityEngine.AudioClip
---@overload fun(name : System.String,lengthSamples : System.Int32,channels : System.Int32,frequency : System.Int32,_3D : System.Boolean,stream : System.Boolean) : UnityEngine.AudioClip
---@overload fun(name : System.String,lengthSamples : System.Int32,channels : System.Int32,frequency : System.Int32,_3D : System.Boolean,stream : System.Boolean) : UnityEngine.AudioClip
---@overload fun(name : System.String,lengthSamples : System.Int32,channels : System.Int32,frequency : System.Int32,_3D : System.Boolean,stream : System.Boolean) : UnityEngine.AudioClip
---@overload fun(name : System.String,lengthSamples : System.Int32,channels : System.Int32,frequency : System.Int32,_3D : System.Boolean,stream : System.Boolean) : UnityEngine.AudioClip
---@param name System.String
---@param lengthSamples System.Int32
---@param channels System.Int32
---@param frequency System.Int32
---@param _3D System.Boolean
---@param stream System.Boolean
---@return UnityEngine.AudioClip
function m.Create(name,lengthSamples,channels,frequency,_3D,stream)end
UnityEngine = {}
UnityEngine.AudioClip = m
return m

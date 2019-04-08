---@field public globalShaderHardwareTier UnityEngine.Rendering.ShaderHardwareTier
---@field public maximumLOD System.Int32
---@field public globalMaximumLOD System.Int32
---@field public isSupported System.Boolean
---@field public globalRenderPipeline System.String
---@field public renderQueue System.Int32
---@class UnityEngine.Shader : UnityEngine.Object
local m = {}

---@param propertyName System.String
---@param mode UnityEngine.TexGenMode
---@return System.Void
function m.SetGlobalTexGenMode(propertyName,mode)end
---@param propertyName System.String
---@param matrixName System.String
---@return System.Void
function m.SetGlobalTextureMatrixName(propertyName,matrixName)end
---@param name System.String
---@return UnityEngine.Shader
function m.Find(name)end
---@param keyword System.String
---@return System.Void
function m.EnableKeyword(keyword)end
---@param keyword System.String
---@return System.Void
function m.DisableKeyword(keyword)end
---@param keyword System.String
---@return System.Boolean
function m.IsKeywordEnabled(keyword)end
---@return System.Void
function m.WarmupAllShaders()end
---@param name System.String
---@return System.Int32
function m.PropertyToID(name)end
---@overload fun(name : System.String,value : System.Single) : System.Void
---@param name System.String
---@param value System.Single
---@return System.Void
function m.SetGlobalFloat(name,value)end
---@overload fun(name : System.String,value : System.Int32) : System.Void
---@param name System.String
---@param value System.Int32
---@return System.Void
function m.SetGlobalInt(name,value)end
---@overload fun(name : System.String,value : UnityEngine.Vector4) : System.Void
---@param name System.String
---@param value UnityEngine.Vector4
---@return System.Void
function m.SetGlobalVector(name,value)end
---@overload fun(name : System.String,value : UnityEngine.Color) : System.Void
---@param name System.String
---@param value UnityEngine.Color
---@return System.Void
function m.SetGlobalColor(name,value)end
---@overload fun(name : System.String,value : UnityEngine.Matrix4x4) : System.Void
---@param name System.String
---@param value UnityEngine.Matrix4x4
---@return System.Void
function m.SetGlobalMatrix(name,value)end
---@overload fun(name : System.String,value : UnityEngine.Texture) : System.Void
---@param name System.String
---@param value UnityEngine.Texture
---@return System.Void
function m.SetGlobalTexture(name,value)end
---@overload fun(name : System.String,value : UnityEngine.ComputeBuffer) : System.Void
---@param name System.String
---@param value UnityEngine.ComputeBuffer
---@return System.Void
function m.SetGlobalBuffer(name,value)end
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[System.Single, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]) : System.Void
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[System.Single, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]) : System.Void
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[System.Single, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]) : System.Void
---@param name System.String
---@param values System.Collections.Generic.List`1[[System.Single, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@return System.Void
function m.SetGlobalFloatArray(name,values)end
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[UnityEngine.Vector4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]) : System.Void
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[UnityEngine.Vector4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]) : System.Void
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[UnityEngine.Vector4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]) : System.Void
---@param name System.String
---@param values System.Collections.Generic.List`1[[UnityEngine.Vector4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Void
function m.SetGlobalVectorArray(name,values)end
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[UnityEngine.Matrix4x4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]) : System.Void
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[UnityEngine.Matrix4x4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]) : System.Void
---@overload fun(name : System.String,values : System.Collections.Generic.List`1[[UnityEngine.Matrix4x4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]) : System.Void
---@param name System.String
---@param values System.Collections.Generic.List`1[[UnityEngine.Matrix4x4, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Void
function m.SetGlobalMatrixArray(name,values)end
---@overload fun(name : System.String) : System.Single
---@param name System.String
---@return System.Single
function m.GetGlobalFloat(name)end
---@overload fun(name : System.String) : System.Int32
---@param name System.String
---@return System.Int32
function m.GetGlobalInt(name)end
---@overload fun(name : System.String) : UnityEngine.Vector4
---@param name System.String
---@return UnityEngine.Vector4
function m.GetGlobalVector(name)end
---@overload fun(name : System.String) : UnityEngine.Color
---@param name System.String
---@return UnityEngine.Color
function m.GetGlobalColor(name)end
---@overload fun(name : System.String) : UnityEngine.Matrix4x4
---@param name System.String
---@return UnityEngine.Matrix4x4
function m.GetGlobalMatrix(name)end
---@overload fun(name : System.String) : UnityEngine.Texture
---@param name System.String
---@return UnityEngine.Texture
function m.GetGlobalTexture(name)end
---@overload fun(name : System.String) : System.Single[]
---@overload fun(name : System.String) : System.Single[]
---@overload fun(name : System.String) : System.Single[]
---@param name System.String
---@return System.Single[]
function m.GetGlobalFloatArray(name)end
---@overload fun(name : System.String) : UnityEngine.Vector4[]
---@overload fun(name : System.String) : UnityEngine.Vector4[]
---@overload fun(name : System.String) : UnityEngine.Vector4[]
---@param name System.String
---@return UnityEngine.Vector4[]
function m.GetGlobalVectorArray(name)end
---@overload fun(name : System.String) : UnityEngine.Matrix4x4[]
---@overload fun(name : System.String) : UnityEngine.Matrix4x4[]
---@overload fun(name : System.String) : UnityEngine.Matrix4x4[]
---@param name System.String
---@return UnityEngine.Matrix4x4[]
function m.GetGlobalMatrixArray(name)end
UnityEngine = {}
UnityEngine.Shader = m
return m

---@class UnityEngine.Resources : System.Object
local m = {}

---@overload fun(type : System.Type) : UnityEngine.Object[]
---@param type System.Type
---@return UnityEngine.Object[]
function m.FindObjectsOfTypeAll(type)end
---@overload fun(path : System.String) : UnityEngine.Object
---@overload fun(path : System.String) : UnityEngine.Object
---@param path System.String
---@return UnityEngine.Object
function m.Load(path)end
---@overload fun(path : System.String) : UnityEngine.ResourceRequest
---@overload fun(path : System.String) : UnityEngine.ResourceRequest
---@param path System.String
---@return UnityEngine.ResourceRequest
function m.LoadAsync(path)end
---@overload fun(path : System.String,systemTypeInstance : System.Type) : UnityEngine.Object[]
---@overload fun(path : System.String,systemTypeInstance : System.Type) : UnityEngine.Object[]
---@param path System.String
---@param systemTypeInstance System.Type
---@return UnityEngine.Object[]
function m.LoadAll(path,systemTypeInstance)end
---@overload fun(type : System.Type,path : System.String) : UnityEngine.Object
---@param type System.Type
---@param path System.String
---@return UnityEngine.Object
function m.GetBuiltinResource(type,path)end
---@param assetToUnload UnityEngine.Object
---@return System.Void
function m.UnloadAsset(assetToUnload)end
---@return UnityEngine.AsyncOperation
function m.UnloadUnusedAssets()end
---@overload fun(assetPath : System.String,type : System.Type) : UnityEngine.Object
---@param assetPath System.String
---@param type System.Type
---@return UnityEngine.Object
function m.LoadAssetAtPath(assetPath,type)end
UnityEngine = {}
UnityEngine.Resources = m
return m

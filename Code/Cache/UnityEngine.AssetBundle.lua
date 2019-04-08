---@field public mainAsset UnityEngine.Object
---@field public isStreamedSceneAssetBundle System.Boolean
---@class UnityEngine.AssetBundle : UnityEngine.Object
local m = {}

---@param unloadAllObjects System.Boolean
---@return System.Void
function m.UnloadAllAssetBundles(unloadAllObjects)end
---@return System.Collections.Generic.IEnumerable`1[[UnityEngine.AssetBundle, UnityEngine.AssetBundleModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
function m.GetAllLoadedAssetBundles()end
---@overload fun(path : System.String) : UnityEngine.AssetBundleCreateRequest
---@overload fun(path : System.String) : UnityEngine.AssetBundleCreateRequest
---@param path System.String
---@return UnityEngine.AssetBundleCreateRequest
function m.LoadFromFileAsync(path)end
---@overload fun(path : System.String) : UnityEngine.AssetBundle
---@overload fun(path : System.String) : UnityEngine.AssetBundle
---@param path System.String
---@return UnityEngine.AssetBundle
function m.LoadFromFile(path)end
---@overload fun(binary : System.Byte[]) : UnityEngine.AssetBundleCreateRequest
---@param binary System.Byte[]
---@return UnityEngine.AssetBundleCreateRequest
function m.LoadFromMemoryAsync(binary)end
---@overload fun(binary : System.Byte[]) : UnityEngine.AssetBundle
---@param binary System.Byte[]
---@return UnityEngine.AssetBundle
function m.LoadFromMemory(binary)end
---@overload fun(stream : System.IO.Stream,crc : System.UInt32,managedReadBufferSize : System.UInt32) : UnityEngine.AssetBundleCreateRequest
---@overload fun(stream : System.IO.Stream,crc : System.UInt32,managedReadBufferSize : System.UInt32) : UnityEngine.AssetBundleCreateRequest
---@param stream System.IO.Stream
---@param crc System.UInt32
---@param managedReadBufferSize System.UInt32
---@return UnityEngine.AssetBundleCreateRequest
function m.LoadFromStreamAsync(stream,crc,managedReadBufferSize)end
---@overload fun(stream : System.IO.Stream,crc : System.UInt32,managedReadBufferSize : System.UInt32) : UnityEngine.AssetBundle
---@overload fun(stream : System.IO.Stream,crc : System.UInt32,managedReadBufferSize : System.UInt32) : UnityEngine.AssetBundle
---@param stream System.IO.Stream
---@param crc System.UInt32
---@param managedReadBufferSize System.UInt32
---@return UnityEngine.AssetBundle
function m.LoadFromStream(stream,crc,managedReadBufferSize)end
---@param name System.String
---@return System.Boolean
function m:Contains(name)end
---@overload fun(name : System.String) : UnityEngine.Object
---@param name System.String
---@return UnityEngine.Object
function m:Load(name)end
---@overload fun() : UnityEngine.Object[]
---@return UnityEngine.Object[]
function m:LoadAll()end
---@overload fun(name : System.String) : UnityEngine.Object
---@overload fun(name : System.String) : UnityEngine.Object
---@param name System.String
---@return UnityEngine.Object
function m:LoadAsset(name)end
---@overload fun(name : System.String) : UnityEngine.AssetBundleRequest
---@overload fun(name : System.String) : UnityEngine.AssetBundleRequest
---@param name System.String
---@return UnityEngine.AssetBundleRequest
function m:LoadAssetAsync(name)end
---@overload fun(name : System.String) : UnityEngine.Object[]
---@overload fun(name : System.String) : UnityEngine.Object[]
---@param name System.String
---@return UnityEngine.Object[]
function m:LoadAssetWithSubAssets(name)end
---@overload fun(name : System.String) : UnityEngine.AssetBundleRequest
---@overload fun(name : System.String) : UnityEngine.AssetBundleRequest
---@param name System.String
---@return UnityEngine.AssetBundleRequest
function m:LoadAssetWithSubAssetsAsync(name)end
---@overload fun() : UnityEngine.Object[]
---@overload fun() : UnityEngine.Object[]
---@return UnityEngine.Object[]
function m:LoadAllAssets()end
---@overload fun() : UnityEngine.AssetBundleRequest
---@overload fun() : UnityEngine.AssetBundleRequest
---@return UnityEngine.AssetBundleRequest
function m:LoadAllAssetsAsync()end
---@return System.String[]
function m:AllAssetNames()end
---@param unloadAllLoadedObjects System.Boolean
---@return System.Void
function m:Unload(unloadAllLoadedObjects)end
---@return System.String[]
function m:GetAllAssetNames()end
---@return System.String[]
function m:GetAllScenePaths()end
---@param path System.String
---@return UnityEngine.AssetBundle
function m.CreateFromFile(path)end
---@param binary System.Byte[]
---@return UnityEngine.AssetBundleCreateRequest
function m.CreateFromMemory(binary)end
---@param binary System.Byte[]
---@return UnityEngine.AssetBundle
function m.CreateFromMemoryImmediate(binary)end
UnityEngine = {}
UnityEngine.AssetBundle = m
return m

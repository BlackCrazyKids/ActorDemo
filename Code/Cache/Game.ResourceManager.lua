---@field public MaxTaskCount System.Int32
---@field public IsPreLoadDone System.Boolean
---@field public PreloadPrograss System.Single
---@class Game.ResourceManager : System.Object
local m = {}

---@return System.Void
function m:Init()end
---@param taskId System.UInt32
---@return System.Boolean
function m:IsLoading(taskId)end
---@param taskId System.UInt32
---@param action System.Action`1[[UnityEngine.Object, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Void
function m:RemoveTask(taskId,action)end
---@param bundlename System.String
---@return System.Void
function m:AddRefCount(bundlename)end
---@param bundlename System.String
---@return System.Void
function m:RemoveRefCount(bundlename)end
---@overload fun(file : System.String,action : XLua.LuaFunction,loadType : System.Int32) : System.UInt32
---@param file System.String
---@param action XLua.LuaFunction
---@param loadType System.Int32
---@return System.UInt32
function m:AddTask(file,action,loadType)end
---@return System.Void
function m:Update()end
---@param relative System.String
---@return System.String
function m:GetResPath(relative)end
---@return System.Void
function m:CleanupMemoryInterval()end
---@return System.Void
function m:CleanupMemoryNow()end
---@return System.Void
function m:CleanupBundlesInterval()end
---@return System.Void
function m:CleanupBundlesNow()end
---@return System.Void
function m:Dispose()end
---@return System.Collections.IEnumerator
function m:CheckUnzipData()end
Game = {}
Game.ResourceManager = m
return m

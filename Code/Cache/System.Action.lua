---@class System.Action : System.MulticastDelegate
local m = {}

---@return System.Void
function m:Invoke()end
---@param callback System.AsyncCallback
---@param object System.Object
---@return System.IAsyncResult
function m:BeginInvoke(callback,object)end
---@param result System.IAsyncResult
---@return System.Void
function m:EndInvoke(result)end
System = {}
System.Action = m
return m

---@class System.Action`1[[System.Boolean, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] : System.MulticastDelegate
local m = {}

---@param obj System.Boolean
---@return System.Void
function m:Invoke(obj)end
---@param obj System.Boolean
---@param callback System.AsyncCallback
---@param object System.Object
---@return System.IAsyncResult
function m:BeginInvoke(obj,callback,object)end
---@param result System.IAsyncResult
---@return System.Void
function m:EndInvoke(result)end
System = {}
System.Action`1[[System = {}
System.Action`1[[System.Boolean, mscorlib, Version=2 = {}
System.Action`1[[System.Boolean, mscorlib, Version=2.0 = {}
System.Action`1[[System.Boolean, mscorlib, Version=2.0.0 = {}
System.Action`1[[System.Boolean, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]] = m
return m

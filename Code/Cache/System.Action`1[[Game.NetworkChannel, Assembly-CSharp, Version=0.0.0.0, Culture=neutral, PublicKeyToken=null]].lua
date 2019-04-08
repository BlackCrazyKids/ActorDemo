---@class System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] : System.MulticastDelegate
local m = {}

---@param obj Game.NetworkChannel
---@return System.Void
function m:Invoke(obj)end
---@param obj Game.NetworkChannel
---@param callback System.AsyncCallback
---@param object System.Object
---@return System.IAsyncResult
function m:BeginInvoke(obj,callback,object)end
---@param result System.IAsyncResult
---@return System.Void
function m:EndInvoke(result)end
System = {}
System.Action`1[[Game = {}
System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0 = {}
System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0.0 = {}
System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0 = {}
System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] = m
return m

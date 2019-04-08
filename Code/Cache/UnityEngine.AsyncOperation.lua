---@field public isDone System.Boolean
---@field public progress System.Single
---@field public priority System.Int32
---@field public allowSceneActivation System.Boolean
---@class UnityEngine.AsyncOperation : UnityEngine.YieldInstruction
local m = {}

---@param value System.Action`1[[UnityEngine.AsyncOperation, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Void
function m:add_completed(value)end
---@param value System.Action`1[[UnityEngine.AsyncOperation, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@return System.Void
function m:remove_completed(value)end
UnityEngine = {}
UnityEngine.AsyncOperation = m
return m

---@class UnityEngine.TrackedReference : System.Object
local m = {}

---@param x UnityEngine.TrackedReference
---@param y UnityEngine.TrackedReference
---@return System.Boolean
function m.op_Equality(x,y)end
---@param x UnityEngine.TrackedReference
---@param y UnityEngine.TrackedReference
---@return System.Boolean
function m.op_Inequality(x,y)end
---@param o System.Object
---@return System.Boolean
function m:Equals(o)end
---@return System.Int32
function m:GetHashCode()end
---@param exists UnityEngine.TrackedReference
---@return System.Boolean
function m.op_Implicit(exists)end
UnityEngine = {}
UnityEngine.TrackedReference = m
return m

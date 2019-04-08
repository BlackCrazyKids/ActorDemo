---@class System.Object
local m = {}

---@overload fun(obj : System.Object) : System.Boolean
---@param obj System.Object
---@return System.Boolean
function m:Equals(obj)end
---@return System.Int32
function m:GetHashCode()end
---@return System.Type
function m:GetType()end
---@return System.String
function m:ToString()end
---@param objA System.Object
---@param objB System.Object
---@return System.Boolean
function m.ReferenceEquals(objA,objB)end
System = {}
System.Object = m
return m

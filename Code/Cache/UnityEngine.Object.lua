---@field public name System.String
---@field public hideFlags UnityEngine.HideFlags
---@class UnityEngine.Object : System.Object
local m = {}

---@return System.Int32
function m:GetInstanceID()end
---@return System.Int32
function m:GetHashCode()end
---@param other System.Object
---@return System.Boolean
function m:Equals(other)end
---@param exists UnityEngine.Object
---@return System.Boolean
function m.op_Implicit(exists)end
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@overload fun(original : UnityEngine.Object,position : UnityEngine.Vector3,rotation : UnityEngine.Quaternion) : UnityEngine.Object
---@param original UnityEngine.Object
---@param position UnityEngine.Vector3
---@param rotation UnityEngine.Quaternion
---@return UnityEngine.Object
function m.Instantiate(original,position,rotation)end
---@overload fun(obj : UnityEngine.Object,t : System.Single) : System.Void
---@param obj UnityEngine.Object
---@param t System.Single
---@return System.Void
function m.Destroy(obj,t)end
---@overload fun(obj : UnityEngine.Object,allowDestroyingAssets : System.Boolean) : System.Void
---@param obj UnityEngine.Object
---@param allowDestroyingAssets System.Boolean
---@return System.Void
function m.DestroyImmediate(obj,allowDestroyingAssets)end
---@overload fun(type : System.Type) : UnityEngine.Object[]
---@param type System.Type
---@return UnityEngine.Object[]
function m.FindObjectsOfType(type)end
---@param target UnityEngine.Object
---@return System.Void
function m.DontDestroyOnLoad(target)end
---@overload fun(obj : UnityEngine.Object,t : System.Single) : System.Void
---@param obj UnityEngine.Object
---@param t System.Single
---@return System.Void
function m.DestroyObject(obj,t)end
---@param type System.Type
---@return UnityEngine.Object[]
function m.FindSceneObjectsOfType(type)end
---@param type System.Type
---@return UnityEngine.Object[]
function m.FindObjectsOfTypeIncludingAssets(type)end
---@overload fun() : 
function m.FindObjectOfType()end
---@param type System.Type
---@return UnityEngine.Object[]
function m.FindObjectsOfTypeAll(type)end
---@return System.String
function m:ToString()end
---@param x UnityEngine.Object
---@param y UnityEngine.Object
---@return System.Boolean
function m.op_Equality(x,y)end
---@param x UnityEngine.Object
---@param y UnityEngine.Object
---@return System.Boolean
function m.op_Inequality(x,y)end
UnityEngine = {}
UnityEngine.Object = m
return m

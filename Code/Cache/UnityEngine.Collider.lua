---@field public enabled System.Boolean
---@field public attachedRigidbody UnityEngine.Rigidbody
---@field public isTrigger System.Boolean
---@field public contactOffset System.Single
---@field public bounds UnityEngine.Bounds
---@field public sharedMaterial UnityEngine.PhysicMaterial
---@field public material UnityEngine.PhysicMaterial
---@class UnityEngine.Collider : UnityEngine.Component
local m = {}

---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ClosestPoint(position)end
---@param ray UnityEngine.Ray
---@param hitInfo UnityEngine.RaycastHit&
---@param maxDistance System.Single
---@return System.Boolean
function m:Raycast(ray,hitInfo,maxDistance)end
---@param position UnityEngine.Vector3
---@return UnityEngine.Vector3
function m:ClosestPointOnBounds(position)end
UnityEngine = {}
UnityEngine.Collider = m
return m

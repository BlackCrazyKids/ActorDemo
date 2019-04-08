---@field public rect UnityEngine.Rect
---@field public anchorMin UnityEngine.Vector2
---@field public anchorMax UnityEngine.Vector2
---@field public anchoredPosition UnityEngine.Vector2
---@field public sizeDelta UnityEngine.Vector2
---@field public pivot UnityEngine.Vector2
---@field public anchoredPosition3D UnityEngine.Vector3
---@field public offsetMin UnityEngine.Vector2
---@field public offsetMax UnityEngine.Vector2
---@class UnityEngine.RectTransform : UnityEngine.Transform
local m = {}

---@param value UnityEngine.RectTransform+ReapplyDrivenProperties
---@return System.Void
function m.add_reapplyDrivenProperties(value)end
---@param value UnityEngine.RectTransform+ReapplyDrivenProperties
---@return System.Void
function m.remove_reapplyDrivenProperties(value)end
---@return System.Void
function m:ForceUpdateRectTransforms()end
---@param fourCornersArray UnityEngine.Vector3[]
---@return System.Void
function m:GetLocalCorners(fourCornersArray)end
---@param fourCornersArray UnityEngine.Vector3[]
---@return System.Void
function m:GetWorldCorners(fourCornersArray)end
---@param edge UnityEngine.RectTransform+Edge
---@param inset System.Single
---@param size System.Single
---@return System.Void
function m:SetInsetAndSizeFromParentEdge(edge,inset,size)end
---@param axis UnityEngine.RectTransform+Axis
---@param size System.Single
---@return System.Void
function m:SetSizeWithCurrentAnchors(axis,size)end
UnityEngine = {}
UnityEngine.RectTransform = m
return m

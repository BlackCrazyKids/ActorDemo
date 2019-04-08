---@field public hovered System.Collections.Generic.List`1[[UnityEngine.GameObject, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public pointerEnter UnityEngine.GameObject
---@field public lastPress UnityEngine.GameObject
---@field public rawPointerPress UnityEngine.GameObject
---@field public pointerDrag UnityEngine.GameObject
---@field public pointerCurrentRaycast UnityEngine.EventSystems.RaycastResult
---@field public pointerPressRaycast UnityEngine.EventSystems.RaycastResult
---@field public eligibleForClick System.Boolean
---@field public pointerId System.Int32
---@field public position UnityEngine.Vector2
---@field public delta UnityEngine.Vector2
---@field public pressPosition UnityEngine.Vector2
---@field public worldPosition UnityEngine.Vector3
---@field public worldNormal UnityEngine.Vector3
---@field public clickTime System.Single
---@field public clickCount System.Int32
---@field public scrollDelta UnityEngine.Vector2
---@field public useDragThreshold System.Boolean
---@field public dragging System.Boolean
---@field public button UnityEngine.EventSystems.PointerEventData+InputButton
---@field public enterEventCamera UnityEngine.Camera
---@field public pressEventCamera UnityEngine.Camera
---@field public pointerPress UnityEngine.GameObject
---@class UnityEngine.EventSystems.PointerEventData : UnityEngine.EventSystems.BaseEventData
local m = {}

---@return System.Boolean
function m:IsPointerMoving()end
---@return System.Boolean
function m:IsScrolling()end
---@return System.String
function m:ToString()end
UnityEngine = {}
UnityEngine.EventSystems = {}
UnityEngine.EventSystems.PointerEventData = m
return m

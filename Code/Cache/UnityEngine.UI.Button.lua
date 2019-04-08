---@field public onClick UnityEngine.UI.Button+ButtonClickedEvent
---@class UnityEngine.UI.Button : UnityEngine.UI.Selectable
local m = {}

---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnPointerClick(eventData)end
---@param eventData UnityEngine.EventSystems.BaseEventData
---@return System.Void
function m:OnSubmit(eventData)end
UnityEngine = {}
UnityEngine.UI = {}
UnityEngine.UI.Button = m
return m

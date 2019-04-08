---@field public delegates System.Collections.Generic.List`1[[UnityEngine.EventSystems.EventTrigger+Entry, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public triggers System.Collections.Generic.List`1[[UnityEngine.EventSystems.EventTrigger+Entry, UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@class UnityEngine.EventSystems.EventTrigger : UnityEngine.MonoBehaviour
local m = {}

---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnPointerEnter(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnPointerExit(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnDrag(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnDrop(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnPointerDown(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnPointerUp(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnPointerClick(eventData)end
---@param eventData UnityEngine.EventSystems.BaseEventData
---@return System.Void
function m:OnSelect(eventData)end
---@param eventData UnityEngine.EventSystems.BaseEventData
---@return System.Void
function m:OnDeselect(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnScroll(eventData)end
---@param eventData UnityEngine.EventSystems.AxisEventData
---@return System.Void
function m:OnMove(eventData)end
---@param eventData UnityEngine.EventSystems.BaseEventData
---@return System.Void
function m:OnUpdateSelected(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnInitializePotentialDrag(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnBeginDrag(eventData)end
---@param eventData UnityEngine.EventSystems.PointerEventData
---@return System.Void
function m:OnEndDrag(eventData)end
---@param eventData UnityEngine.EventSystems.BaseEventData
---@return System.Void
function m:OnSubmit(eventData)end
---@param eventData UnityEngine.EventSystems.BaseEventData
---@return System.Void
function m:OnCancel(eventData)end
UnityEngine = {}
UnityEngine.EventSystems = {}
UnityEngine.EventSystems.EventTrigger = m
return m

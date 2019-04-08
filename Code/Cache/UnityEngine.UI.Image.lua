---@field public sprite UnityEngine.Sprite
---@field public overrideSprite UnityEngine.Sprite
---@field public type UnityEngine.UI.Image+Type
---@field public preserveAspect System.Boolean
---@field public fillCenter System.Boolean
---@field public fillMethod UnityEngine.UI.Image+FillMethod
---@field public fillAmount System.Single
---@field public fillClockwise System.Boolean
---@field public fillOrigin System.Int32
---@field public eventAlphaThreshold System.Single
---@field public alphaHitTestMinimumThreshold System.Single
---@field public defaultETC1GraphicMaterial UnityEngine.Material
---@field public mainTexture UnityEngine.Texture
---@field public hasBorder System.Boolean
---@field public pixelsPerUnit System.Single
---@field public material UnityEngine.Material
---@field public minWidth System.Single
---@field public preferredWidth System.Single
---@field public flexibleWidth System.Single
---@field public minHeight System.Single
---@field public preferredHeight System.Single
---@field public flexibleHeight System.Single
---@field public layoutPriority System.Int32
---@class UnityEngine.UI.Image : UnityEngine.UI.MaskableGraphic
local m = {}

---@return System.Void
function m:OnBeforeSerialize()end
---@return System.Void
function m:OnAfterDeserialize()end
---@return System.Void
function m:SetNativeSize()end
---@return System.Void
function m:CalculateLayoutInputHorizontal()end
---@return System.Void
function m:CalculateLayoutInputVertical()end
---@param screenPoint UnityEngine.Vector2
---@param eventCamera UnityEngine.Camera
---@return System.Boolean
function m:IsRaycastLocationValid(screenPoint,eventCamera)end
UnityEngine = {}
UnityEngine.UI = {}
UnityEngine.UI.Image = m
return m

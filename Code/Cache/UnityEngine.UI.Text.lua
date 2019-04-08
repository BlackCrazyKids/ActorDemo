---@field public cachedTextGenerator UnityEngine.TextGenerator
---@field public cachedTextGeneratorForLayout UnityEngine.TextGenerator
---@field public mainTexture UnityEngine.Texture
---@field public font UnityEngine.Font
---@field public text System.String
---@field public supportRichText System.Boolean
---@field public resizeTextForBestFit System.Boolean
---@field public resizeTextMinSize System.Int32
---@field public resizeTextMaxSize System.Int32
---@field public alignment UnityEngine.TextAnchor
---@field public alignByGeometry System.Boolean
---@field public fontSize System.Int32
---@field public horizontalOverflow UnityEngine.HorizontalWrapMode
---@field public verticalOverflow UnityEngine.VerticalWrapMode
---@field public lineSpacing System.Single
---@field public fontStyle UnityEngine.FontStyle
---@field public pixelsPerUnit System.Single
---@field public minWidth System.Single
---@field public preferredWidth System.Single
---@field public flexibleWidth System.Single
---@field public minHeight System.Single
---@field public preferredHeight System.Single
---@field public flexibleHeight System.Single
---@field public layoutPriority System.Int32
---@class UnityEngine.UI.Text : UnityEngine.UI.MaskableGraphic
local m = {}

---@return System.Void
function m:FontTextureChanged()end
---@param extents UnityEngine.Vector2
---@return UnityEngine.TextGenerationSettings
function m:GetGenerationSettings(extents)end
---@param anchor UnityEngine.TextAnchor
---@return UnityEngine.Vector2
function m.GetTextAnchorPivot(anchor)end
---@return System.Void
function m:CalculateLayoutInputHorizontal()end
---@return System.Void
function m:CalculateLayoutInputVertical()end
---@return System.Void
function m:OnRebuildRequested()end
UnityEngine = {}
UnityEngine.UI = {}
UnityEngine.UI.Text = m
return m

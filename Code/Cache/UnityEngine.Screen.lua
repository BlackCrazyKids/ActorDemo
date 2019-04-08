---@field public width System.Int32
---@field public height System.Int32
---@field public dpi System.Single
---@field public orientation UnityEngine.ScreenOrientation
---@field public sleepTimeout System.Int32
---@field public autorotateToPortrait System.Boolean
---@field public autorotateToPortraitUpsideDown System.Boolean
---@field public autorotateToLandscapeLeft System.Boolean
---@field public autorotateToLandscapeRight System.Boolean
---@field public currentResolution UnityEngine.Resolution
---@field public fullScreen System.Boolean
---@field public fullScreenMode UnityEngine.FullScreenMode
---@field public safeArea UnityEngine.Rect
---@field public resolutions UnityEngine.Resolution[]
---@field public GetResolution UnityEngine.Resolution[]
---@field public showCursor System.Boolean
---@field public lockCursor System.Boolean
---@class UnityEngine.Screen : System.Object
local m = {}

---@overload fun(width : System.Int32,height : System.Int32,fullscreenMode : UnityEngine.FullScreenMode,preferredRefreshRate : System.Int32) : System.Void
---@overload fun(width : System.Int32,height : System.Int32,fullscreenMode : UnityEngine.FullScreenMode,preferredRefreshRate : System.Int32) : System.Void
---@overload fun(width : System.Int32,height : System.Int32,fullscreenMode : UnityEngine.FullScreenMode,preferredRefreshRate : System.Int32) : System.Void
---@param width System.Int32
---@param height System.Int32
---@param fullscreenMode UnityEngine.FullScreenMode
---@param preferredRefreshRate System.Int32
---@return System.Void
function m.SetResolution(width,height,fullscreenMode,preferredRefreshRate)end
UnityEngine = {}
UnityEngine.Screen = m
return m

---@field public TRIANGLES System.Int32
---@field public TRIANGLE_STRIP System.Int32
---@field public QUADS System.Int32
---@field public LINES System.Int32
---@field public LINE_STRIP System.Int32
---@field public wireframe System.Boolean
---@field public sRGBWrite System.Boolean
---@field public invertCulling System.Boolean
---@field public modelview UnityEngine.Matrix4x4
---@class UnityEngine.GL : System.Object
local m = {}

---@param x System.Single
---@param y System.Single
---@param z System.Single
---@return System.Void
function m.Vertex3(x,y,z)end
---@param v UnityEngine.Vector3
---@return System.Void
function m.Vertex(v)end
---@param x System.Single
---@param y System.Single
---@param z System.Single
---@return System.Void
function m.TexCoord3(x,y,z)end
---@param v UnityEngine.Vector3
---@return System.Void
function m.TexCoord(v)end
---@param x System.Single
---@param y System.Single
---@return System.Void
function m.TexCoord2(x,y)end
---@param unit System.Int32
---@param x System.Single
---@param y System.Single
---@param z System.Single
---@return System.Void
function m.MultiTexCoord3(unit,x,y,z)end
---@param unit System.Int32
---@param v UnityEngine.Vector3
---@return System.Void
function m.MultiTexCoord(unit,v)end
---@param unit System.Int32
---@param x System.Single
---@param y System.Single
---@return System.Void
function m.MultiTexCoord2(unit,x,y)end
---@param c UnityEngine.Color
---@return System.Void
function m.Color(c)end
---@return System.Void
function m.Flush()end
---@return System.Void
function m.RenderTargetBarrier()end
---@param m UnityEngine.Matrix4x4
---@return System.Void
function m.MultMatrix(m)end
---@overload fun(eventID : System.Int32) : System.Void
---@param eventID System.Int32
---@return System.Void
function m.IssuePluginEvent(eventID)end
---@param revertBackFaces System.Boolean
---@return System.Void
function m.SetRevertBackfacing(revertBackFaces)end
---@return System.Void
function m.PushMatrix()end
---@return System.Void
function m.PopMatrix()end
---@return System.Void
function m.LoadIdentity()end
---@return System.Void
function m.LoadOrtho()end
---@overload fun() : System.Void
---@return System.Void
function m.LoadPixelMatrix()end
---@param mat UnityEngine.Matrix4x4
---@return System.Void
function m.LoadProjectionMatrix(mat)end
---@return System.Void
function m.InvalidateState()end
---@param proj UnityEngine.Matrix4x4
---@param renderIntoTexture System.Boolean
---@return UnityEngine.Matrix4x4
function m.GetGPUProjectionMatrix(proj,renderIntoTexture)end
---@param mode System.Int32
---@return System.Void
function m.Begin(mode)end
---@return System.Void
function m.End()end
---@overload fun(clearDepth : System.Boolean,clearColor : System.Boolean,backgroundColor : UnityEngine.Color,depth : System.Single) : System.Void
---@param clearDepth System.Boolean
---@param clearColor System.Boolean
---@param backgroundColor UnityEngine.Color
---@param depth System.Single
---@return System.Void
function m.Clear(clearDepth,clearColor,backgroundColor,depth)end
---@param pixelRect UnityEngine.Rect
---@return System.Void
function m.Viewport(pixelRect)end
---@param clearDepth System.Boolean
---@param camera UnityEngine.Camera
---@return System.Void
function m.ClearWithSkybox(clearDepth,camera)end
UnityEngine = {}
UnityEngine.GL = m
return m

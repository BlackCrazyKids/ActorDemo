---@field public quality UnityEngine.SkinQuality
---@field public updateWhenOffscreen System.Boolean
---@field public rootBone UnityEngine.Transform
---@field public bones UnityEngine.Transform[]
---@field public sharedMesh UnityEngine.Mesh
---@field public skinnedMotionVectors System.Boolean
---@field public localBounds UnityEngine.Bounds
---@class UnityEngine.SkinnedMeshRenderer : UnityEngine.Renderer
local m = {}

---@param index System.Int32
---@return System.Single
function m:GetBlendShapeWeight(index)end
---@param index System.Int32
---@param value System.Single
---@return System.Void
function m:SetBlendShapeWeight(index,value)end
---@param mesh UnityEngine.Mesh
---@return System.Void
function m:BakeMesh(mesh)end
UnityEngine = {}
UnityEngine.SkinnedMeshRenderer = m
return m

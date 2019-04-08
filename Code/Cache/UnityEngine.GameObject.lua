---@field public transform UnityEngine.Transform
---@field public layer System.Int32
---@field public active System.Boolean
---@field public activeSelf System.Boolean
---@field public activeInHierarchy System.Boolean
---@field public isStatic System.Boolean
---@field public tag System.String
---@field public scene UnityEngine.SceneManagement.Scene
---@field public gameObject UnityEngine.GameObject
---@field public rigidbody UnityEngine.Component
---@field public rigidbody2D UnityEngine.Component
---@field public camera UnityEngine.Component
---@field public light UnityEngine.Component
---@field public animation UnityEngine.Component
---@field public constantForce UnityEngine.Component
---@field public renderer UnityEngine.Component
---@field public audio UnityEngine.Component
---@field public guiText UnityEngine.Component
---@field public networkView UnityEngine.Component
---@field public guiElement UnityEngine.Component
---@field public guiTexture UnityEngine.Component
---@field public collider UnityEngine.Component
---@field public collider2D UnityEngine.Component
---@field public hingeJoint UnityEngine.Component
---@field public particleEmitter UnityEngine.Component
---@field public particleSystem UnityEngine.Component
---@class UnityEngine.GameObject : UnityEngine.Object
local m = {}

---@param type UnityEngine.PrimitiveType
---@return UnityEngine.GameObject
function m.CreatePrimitive(type)end
---@overload fun() : 
---@overload fun() : 
function m:GetComponent()end
---@overload fun(type : System.Type,includeInactive : System.Boolean) : UnityEngine.Component
---@overload fun(type : System.Type,includeInactive : System.Boolean) : UnityEngine.Component
---@overload fun(type : System.Type,includeInactive : System.Boolean) : UnityEngine.Component
---@param type System.Type
---@param includeInactive System.Boolean
---@return UnityEngine.Component
function m:GetComponentInChildren(type,includeInactive)end
---@overload fun(type : System.Type) : UnityEngine.Component
---@param type System.Type
---@return UnityEngine.Component
function m:GetComponentInParent(type)end
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@param type System.Type
---@return UnityEngine.Component[]
function m:GetComponents(type)end
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@param type System.Type
---@return UnityEngine.Component[]
function m:GetComponentsInChildren(type)end
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@overload fun(type : System.Type) : UnityEngine.Component[]
---@param type System.Type
---@return UnityEngine.Component[]
function m:GetComponentsInParent(type)end
---@param tag System.String
---@return UnityEngine.GameObject
function m.FindWithTag(tag)end
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@param methodName System.String
---@param options UnityEngine.SendMessageOptions
---@return System.Void
function m:SendMessageUpwards(methodName,options)end
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@param methodName System.String
---@param options UnityEngine.SendMessageOptions
---@return System.Void
function m:SendMessage(methodName,options)end
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@overload fun(methodName : System.String,options : UnityEngine.SendMessageOptions) : System.Void
---@param methodName System.String
---@param options UnityEngine.SendMessageOptions
---@return System.Void
function m:BroadcastMessage(methodName,options)end
---@overload fun(componentType : System.Type) : UnityEngine.Component
---@overload fun(componentType : System.Type) : UnityEngine.Component
---@param componentType System.Type
---@return UnityEngine.Component
function m:AddComponent(componentType)end
---@param value System.Boolean
---@return System.Void
function m:SetActive(value)end
---@param state System.Boolean
---@return System.Void
function m:SetActiveRecursively(state)end
---@param tag System.String
---@return System.Boolean
function m:CompareTag(tag)end
---@param tag System.String
---@return UnityEngine.GameObject
function m.FindGameObjectWithTag(tag)end
---@param tag System.String
---@return UnityEngine.GameObject[]
function m.FindGameObjectsWithTag(tag)end
---@param name System.String
---@return UnityEngine.GameObject
function m.Find(name)end
---@param clip UnityEngine.Object
---@param time System.Single
---@return System.Void
function m:SampleAnimation(clip,time)end
---@param animation UnityEngine.Object
---@return System.Void
function m:PlayAnimation(animation)end
---@return System.Void
function m:StopAnimation()end
UnityEngine = {}
UnityEngine.GameObject = m
return m

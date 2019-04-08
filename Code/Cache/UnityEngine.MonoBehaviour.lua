---@field public useGUILayout System.Boolean
---@field public runInEditMode System.Boolean
---@class UnityEngine.MonoBehaviour : UnityEngine.Behaviour
local m = {}

---@overload fun() : System.Boolean
---@return System.Boolean
function m:IsInvoking()end
---@overload fun() : System.Void
---@return System.Void
function m:CancelInvoke()end
---@param methodName System.String
---@param time System.Single
---@return System.Void
function m:Invoke(methodName,time)end
---@param methodName System.String
---@param time System.Single
---@param repeatRate System.Single
---@return System.Void
function m:InvokeRepeating(methodName,time,repeatRate)end
---@overload fun(methodName : System.String) : UnityEngine.Coroutine
---@overload fun(methodName : System.String) : UnityEngine.Coroutine
---@param methodName System.String
---@return UnityEngine.Coroutine
function m:StartCoroutine(methodName)end
---@param routine System.Collections.IEnumerator
---@return UnityEngine.Coroutine
function m:StartCoroutine_Auto(routine)end
---@overload fun(routine : System.Collections.IEnumerator) : System.Void
---@overload fun(routine : System.Collections.IEnumerator) : System.Void
---@param routine System.Collections.IEnumerator
---@return System.Void
function m:StopCoroutine(routine)end
---@return System.Void
function m:StopAllCoroutines()end
---@param message System.Object
---@return System.Void
function m.print(message)end
UnityEngine = {}
UnityEngine.MonoBehaviour = m
return m

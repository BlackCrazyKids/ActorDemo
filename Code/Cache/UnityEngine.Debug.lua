---@field public unityLogger UnityEngine.ILogger
---@field public developerConsoleVisible System.Boolean
---@field public isDebugBuild System.Boolean
---@field public logger UnityEngine.ILogger
---@class UnityEngine.Debug : System.Object
local m = {}

---@overload fun(start : UnityEngine.Vector3,end : UnityEngine.Vector3,color : UnityEngine.Color,duration : System.Single) : System.Void
---@overload fun(start : UnityEngine.Vector3,end : UnityEngine.Vector3,color : UnityEngine.Color,duration : System.Single) : System.Void
---@overload fun(start : UnityEngine.Vector3,end : UnityEngine.Vector3,color : UnityEngine.Color,duration : System.Single) : System.Void
---@param start UnityEngine.Vector3
---@param end UnityEngine.Vector3
---@param color UnityEngine.Color
---@param duration System.Single
---@return System.Void
function m.DrawLine(start,end,color,duration)end
---@overload fun(start : UnityEngine.Vector3,dir : UnityEngine.Vector3,color : UnityEngine.Color,duration : System.Single) : System.Void
---@overload fun(start : UnityEngine.Vector3,dir : UnityEngine.Vector3,color : UnityEngine.Color,duration : System.Single) : System.Void
---@overload fun(start : UnityEngine.Vector3,dir : UnityEngine.Vector3,color : UnityEngine.Color,duration : System.Single) : System.Void
---@param start UnityEngine.Vector3
---@param dir UnityEngine.Vector3
---@param color UnityEngine.Color
---@param duration System.Single
---@return System.Void
function m.DrawRay(start,dir,color,duration)end
---@return System.Void
function m.Break()end
---@return System.Void
function m.DebugBreak()end
---@overload fun(message : System.Object) : System.Void
---@param message System.Object
---@return System.Void
function m.Log(message)end
---@overload fun(format : System.String,args : System.Object[]) : System.Void
---@param format System.String
---@param args System.Object[]
---@return System.Void
function m.LogFormat(format,args)end
---@overload fun(message : System.Object) : System.Void
---@param message System.Object
---@return System.Void
function m.LogError(message)end
---@overload fun(format : System.String,args : System.Object[]) : System.Void
---@param format System.String
---@param args System.Object[]
---@return System.Void
function m.LogErrorFormat(format,args)end
---@return System.Void
function m.ClearDeveloperConsole()end
---@overload fun(exception : System.Exception) : System.Void
---@param exception System.Exception
---@return System.Void
function m.LogException(exception)end
---@overload fun(message : System.Object) : System.Void
---@param message System.Object
---@return System.Void
function m.LogWarning(message)end
---@overload fun(format : System.String,args : System.Object[]) : System.Void
---@param format System.String
---@param args System.Object[]
---@return System.Void
function m.LogWarningFormat(format,args)end
---@overload fun(condition : System.Boolean) : System.Void
---@overload fun(condition : System.Boolean) : System.Void
---@overload fun(condition : System.Boolean) : System.Void
---@overload fun(condition : System.Boolean) : System.Void
---@overload fun(condition : System.Boolean) : System.Void
---@overload fun(condition : System.Boolean) : System.Void
---@param condition System.Boolean
---@return System.Void
function m.Assert(condition)end
---@overload fun(condition : System.Boolean,format : System.String,args : System.Object[]) : System.Void
---@param condition System.Boolean
---@param format System.String
---@param args System.Object[]
---@return System.Void
function m.AssertFormat(condition,format,args)end
---@overload fun(message : System.Object) : System.Void
---@param message System.Object
---@return System.Void
function m.LogAssertion(message)end
---@overload fun(format : System.String,args : System.Object[]) : System.Void
---@param format System.String
---@param args System.Object[]
---@return System.Void
function m.LogAssertionFormat(format,args)end
UnityEngine = {}
UnityEngine.Debug = m
return m

---@field public isLoadingLevel System.Boolean
---@field public streamedBytes System.Int32
---@field public webSecurityEnabled System.Boolean
---@field public isPlaying System.Boolean
---@field public isFocused System.Boolean
---@field public platform UnityEngine.RuntimePlatform
---@field public buildGUID System.String
---@field public isMobilePlatform System.Boolean
---@field public isConsolePlatform System.Boolean
---@field public runInBackground System.Boolean
---@field public isBatchMode System.Boolean
---@field public dataPath System.String
---@field public streamingAssetsPath System.String
---@field public persistentDataPath System.String
---@field public temporaryCachePath System.String
---@field public absoluteURL System.String
---@field public unityVersion System.String
---@field public version System.String
---@field public installerName System.String
---@field public identifier System.String
---@field public installMode UnityEngine.ApplicationInstallMode
---@field public sandboxType UnityEngine.ApplicationSandboxType
---@field public productName System.String
---@field public companyName System.String
---@field public cloudProjectId System.String
---@field public targetFrameRate System.Int32
---@field public systemLanguage UnityEngine.SystemLanguage
---@field public stackTraceLogType UnityEngine.StackTraceLogType
---@field public backgroundLoadingPriority UnityEngine.ThreadPriority
---@field public internetReachability UnityEngine.NetworkReachability
---@field public genuine System.Boolean
---@field public genuineCheckAvailable System.Boolean
---@field public isShowingSplashScreen System.Boolean
---@field public isPlayer System.Boolean
---@field public isEditor System.Boolean
---@field public levelCount System.Int32
---@field public loadedLevel System.Int32
---@field public loadedLevelName System.String
---@class UnityEngine.Application : System.Object
local m = {}

---@return System.Void
function m.Quit()end
---@return System.Void
function m.CancelQuit()end
---@return System.Void
function m.Unload()end
---@overload fun(levelIndex : System.Int32) : System.Single
---@param levelIndex System.Int32
---@return System.Single
function m.GetStreamProgressForLevel(levelIndex)end
---@overload fun(levelIndex : System.Int32) : System.Boolean
---@param levelIndex System.Int32
---@return System.Boolean
function m.CanStreamedLevelBeLoaded(levelIndex)end
---@return System.String[]
function m.GetBuildTags()end
---@param buildTags System.String[]
---@return System.Void
function m.SetBuildTags(buildTags)end
---@return System.Boolean
function m.HasProLicense()end
---@param script System.String
---@return System.Void
function m.ExternalEval(script)end
---@param delegateMethod UnityEngine.Application+AdvertisingIdentifierCallback
---@return System.Boolean
function m.RequestAdvertisingIdentifierAsync(delegateMethod)end
---@param url System.String
---@return System.Void
function m.OpenURL(url)end
---@param mode System.Int32
---@return System.Void
function m.ForceCrash(mode)end
---@param logType UnityEngine.LogType
---@return UnityEngine.StackTraceLogType
function m.GetStackTraceLogType(logType)end
---@param logType UnityEngine.LogType
---@param stackTraceType UnityEngine.StackTraceLogType
---@return System.Void
function m.SetStackTraceLogType(logType,stackTraceType)end
---@param mode UnityEngine.UserAuthorization
---@return UnityEngine.AsyncOperation
function m.RequestUserAuthorization(mode)end
---@param mode UnityEngine.UserAuthorization
---@return System.Boolean
function m.HasUserAuthorization(mode)end
---@param value UnityEngine.Application+LowMemoryCallback
---@return System.Void
function m.add_lowMemory(value)end
---@param value UnityEngine.Application+LowMemoryCallback
---@return System.Void
function m.remove_lowMemory(value)end
---@param value UnityEngine.Application+LogCallback
---@return System.Void
function m.add_logMessageReceived(value)end
---@param value UnityEngine.Application+LogCallback
---@return System.Void
function m.remove_logMessageReceived(value)end
---@param value UnityEngine.Application+LogCallback
---@return System.Void
function m.add_logMessageReceivedThreaded(value)end
---@param value UnityEngine.Application+LogCallback
---@return System.Void
function m.remove_logMessageReceivedThreaded(value)end
---@param functionName System.String
---@param args System.Object[]
---@return System.Void
function m.ExternalCall(functionName,args)end
---@param o UnityEngine.Object
---@return System.Void
function m.DontDestroyOnLoad(o)end
---@overload fun(filename : System.String,superSize : System.Int32) : System.Void
---@param filename System.String
---@param superSize System.Int32
---@return System.Void
function m.CaptureScreenshot(filename,superSize)end
---@param value UnityEngine.Events.UnityAction
---@return System.Void
function m.add_onBeforeRender(value)end
---@param value UnityEngine.Events.UnityAction
---@return System.Void
function m.remove_onBeforeRender(value)end
---@param value System.Func`1[[System.Boolean, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@return System.Void
function m.add_wantsToQuit(value)end
---@param value System.Func`1[[System.Boolean, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@return System.Void
function m.remove_wantsToQuit(value)end
---@param value System.Action
---@return System.Void
function m.add_quitting(value)end
---@param value System.Action
---@return System.Void
function m.remove_quitting(value)end
---@param handler UnityEngine.Application+LogCallback
---@return System.Void
function m.RegisterLogCallback(handler)end
---@param handler UnityEngine.Application+LogCallback
---@return System.Void
function m.RegisterLogCallbackThreaded(handler)end
---@overload fun(index : System.Int32) : System.Void
---@param index System.Int32
---@return System.Void
function m.LoadLevel(index)end
---@overload fun(index : System.Int32) : System.Void
---@param index System.Int32
---@return System.Void
function m.LoadLevelAdditive(index)end
---@overload fun(index : System.Int32) : UnityEngine.AsyncOperation
---@param index System.Int32
---@return UnityEngine.AsyncOperation
function m.LoadLevelAsync(index)end
---@overload fun(index : System.Int32) : UnityEngine.AsyncOperation
---@param index System.Int32
---@return UnityEngine.AsyncOperation
function m.LoadLevelAdditiveAsync(index)end
---@overload fun(index : System.Int32) : System.Boolean
---@param index System.Int32
---@return System.Boolean
function m.UnloadLevel(index)end
UnityEngine = {}
UnityEngine.Application = m
return m

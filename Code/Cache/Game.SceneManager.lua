---@field public SceneName System.String
---@field public AsyncOpt UnityEngine.AsyncOperation
---@class Game.SceneManager : System.Object
local m = {}

---@return System.Void
function m:Init()end
---@return System.Void
function m:Release()end
---@return System.Void
function m:Dispose()end
---@param action System.Action`1[[System.Boolean, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@return System.Void
function m:RegisteOnSceneLoadFinish(action)end
---@param sceneName System.String
---@return System.Void
function m:ChangeMap(sceneName)end
Game = {}
Game.SceneManager = m
return m

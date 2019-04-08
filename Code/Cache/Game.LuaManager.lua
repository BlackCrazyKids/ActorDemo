---@field public LuaEnv XLua.LuaEnv
---@class Game.LuaManager : System.Object
local m = {}

---@return System.Void
function m:Init()end
---@return System.Void
function m:Dispose()end
---@param path System.String
---@return System.Void
function m:AddSearchPath(path)end
---@param viewName System.String
---@return System.Boolean
function m:HasScript(viewName)end
---@return System.Void
function m:Tick()end
---@return System.Void
function m:InitScripts()end
---@param name System.String
---@return XLua.LuaTable
function m:GetTable(name)end
Game = {}
Game.LuaManager = m
return m

---@field public name_hash_map XLua.LuaTable
---@field public hash_name_map System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@class Game.LuaHelper : System.Object
local m = {}

---@return System.Void
function m.Init()end
---@param name System.String
---@return System.Void
function m.StringToHash(name)end
---@param viewName System.String
---@return System.Boolean
function m.HasScript(viewName)end
Game = {}
Game.LuaHelper = m
return m

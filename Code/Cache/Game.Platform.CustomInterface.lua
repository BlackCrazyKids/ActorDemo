---@class Game.Platform.CustomInterface : Game.Platform.Interface
local m = {}

---@return System.Void
function m:Init()end
---@return System.Void
function m:Release()end
---@param url System.String
---@return System.Void
function m:OpenUrl(url)end
Game = {}
Game.Platform = {}
Game.Platform.CustomInterface = m
return m

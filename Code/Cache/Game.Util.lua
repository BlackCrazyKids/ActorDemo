---@field public DataPath System.String
---@field public StreamingPath System.String
---@field public NetAvailable System.Boolean
---@field public IsWifi System.Boolean
---@class Game.Util : System.Object
local m = {}

---@param source System.String
---@return System.String
function m.ComputeMD5(source)end
---@param file System.String
---@return System.String
function m.ComputeMD5File(file)end
---@param path System.String
---@return System.String
function m.StandardlizePath(path)end
---@param resolution System.Int32
---@return System.Void
function m.SetResolution(resolution)end
---@param go UnityEngine.Transform
---@return System.Void
function m.ClearChild(go)end
---@param obj UnityEngine.Object
---@param path System.String
---@return UnityEngine.GameObject
function m.Instantiate(obj,path)end
---@param obj UnityEngine.Object
---@return UnityEngine.GameObject
function m.Copy(obj)end
---@param str System.String
---@return System.Void
function m.Log(str)end
---@param str System.String
---@return System.Void
function m.LogWarning(str)end
---@param str System.String
---@return System.Void
function m.LogError(str)end
---@param input FairyGUI.GTextInput
---@param icon System.String
---@return System.Void
function m.InputIcon(input,icon)end
---@param list FairyGUI.GList
---@param function XLua.LuaFunction
---@return System.Void
function m.ListItemRenderer(list,function)end
---@param list FairyGUI.GList
---@param function XLua.LuaFunction
---@return System.Void
function m.ListItemProvider(list,function)end
Game = {}
Game.Util = m
return m

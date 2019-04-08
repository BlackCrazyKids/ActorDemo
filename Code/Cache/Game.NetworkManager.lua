---@field public OnNetworkConnected System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public OnNetworkClosed System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public OnNetworkMissHeartBeat System.Action`2[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@field public NetworkChannelCount System.Int32
---@class Game.NetworkManager : System.Object
local m = {}

---@return System.Void
function m:Init()end
---@return System.Void
function m:Dispose()end
---@param value System.Action`3[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[Game.NetworkErrorCode, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@return System.Void
function m:add_NetworkError(value)end
---@param value System.Action`3[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[Game.NetworkErrorCode, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@return System.Void
function m:remove_NetworkError(value)end
---@return System.Void
function m:Update()end
---@param name System.String
---@return System.Boolean
function m:HasNetworkChannel(name)end
---@param name System.String
---@return Game.INetworkChannel
function m:GetNetworkChannel(name)end
---@overload fun() : Game.INetworkChannel[]
---@return Game.INetworkChannel[]
function m:GetAllNetworkChannels()end
---@param name System.String
---@return Game.INetworkChannel
function m:CreateNetworkChannel(name)end
---@param name System.String
---@return System.Boolean
function m:DestroyNetworkChannel(name)end
Game = {}
Game.NetworkManager = m
return m

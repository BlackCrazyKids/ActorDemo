---@field public NetworkChannelConnected System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public NetworkChannelClosed System.Action`1[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
---@field public NetworkChannelMissHeartBeat System.Action`2[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@field public NetworkChannelError System.Action`3[[Game.NetworkChannel, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[Game.NetworkErrorCode, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@field public NetworkReceive System.Action`2[[System.Int32, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Object, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
---@field public Name System.String
---@field public Connected System.Boolean
---@field public NetworkType Game.NetworkType
---@field public LocalIPAddress System.Net.IPAddress
---@field public LocalPort System.Int32
---@field public RemoteIPAddress System.Net.IPAddress
---@field public RemotePort System.Int32
---@field public SendPacketCount System.Int32
---@field public SentPacketCount System.Int32
---@field public ReceivePacketCount System.Int32
---@field public ReceivedPacketCount System.Int32
---@field public ResetHeartBeatElapseSecondsWhenReceivePacket System.Boolean
---@field public MissHeartBeatCount System.Int32
---@field public HeartBeatInterval System.Single
---@field public HeartBeatElapseSeconds System.Single
---@field public ReceiveBufferSize System.Int32
---@field public SendBufferSize System.Int32
---@class Game.NetworkChannel : System.Object
local m = {}

---@param elapseSeconds System.Single
---@param realElapseSeconds System.Single
---@return System.Void
function m:Update(elapseSeconds,realElapseSeconds)end
---@overload fun(ip : System.String,port : System.Int32) : System.Void
---@param ip System.String
---@param port System.Int32
---@return System.Void
function m:Connect(ip,port)end
---@return System.Void
function m:Close()end
---@overload fun(type : System.Int32,msg : System.Byte[]) : System.Void
---@param type System.Int32
---@param msg System.Byte[]
---@return System.Void
function m:Send(type,msg)end
---@return System.Void
function m:Dispose()end
Game = {}
Game.NetworkChannel = m
return m

--[[
json 格式做网络协议
class 字段顺序排列,则数据在json中也顺序排列
字段为kv时,数据可乱序,此时的key也是一种数据
借鉴M10项目
--]]
local Json = require('rapidjson')
local pb = require "pb"
local protoc = require "protoc"
local Event = Event
local GameEvent = GameEvent
---@type Game.NetworkManager
local NetworkMgr = Game.Manager.NetworkMgr

---@class NetworkManager
local NetworkManager = {}
local networkEvt = Event.NewSimple('NetworkEvent')
---@type Game.NetworkChannel
local channel

---@param channel Game.NetworkChannel
local onChannelConnected = function(channel)
    printcolor('orange', channel.Name .. "正常连接到服务器",
            channel.RemoteIPAddress, channel.RemotePort)
end
---@param channel Game.NetworkChannel
local onChannelClosed = function(channel)
    printcolor('orange', channel.Name .. "Channel Closed")
end
---@param channel Game.NetworkChannel
---@param count number
local onMissHeartBeat = function(channel, count)
    printcolor('orange', channel.Name .. 'Miss Heart Beat', count)
end
local onProtocolError = function(msg)

end
---@param type int
---@param msg byte[]
local onReceive = function(type, data)
    local msg = pb.decode("Person", data)
    printyellow(dump(msg, 'Person'))
end

local function SecondUpdate()

end

--local ip = "192.168.50.90"
local ip = "192.168.0.132"
local port = 8686

function NetworkManager.Init()
    channel = NetworkMgr:CreateNetworkChannel("NetworkChannel")
    channel.NetworkReceive = onReceive
    channel.NetworkChannelConnected = onChannelConnected
    channel.NetworkChannelClosed = onChannelClosed
    channel.NetworkChannelMissHeartBeat = onMissHeartBeat
    channel:Connect(ip, port)

    local secondTimer = Timer:new(SecondUpdate, 1, -1, false)
    secondTimer:Start()
end

---@param msg table
function NetworkManager.Send(type, msg)
    local encode = pb.encode("Person", msg)
    channel:Send(type, encode)
end

function NetworkManager.Destroy()
    NetworkMgr:DestroyNetworkChannel(channel.Name)
end

return NetworkManager


---lua-protobuf
--assert(protoc:load([[
--    message Phone {
--      optional string name        = 1;
--      optional int64  phonenumber = 2;
--    }
--    message Person {
--      optional string name     = 1;
--      optional int32  age      = 2;
--      optional string address  = 3;
--      repeated Phone  contacts = 4;
--    }
--    ]]))
--
--local data = {
--    name = "ilse",
--    age = 18,
--    address = "黄河大道西",
--    contacts = {
--        { name = "alice", phonenumber = 12312341234 },
--        { name = "bob", phonenumber = 45645674567 }
--    }
--}
---Json
--local t = { 1, 2, 3, 'nil', 4, 5 }
--local json = Json.encode(t)
--printyellow(json)
--local t1 = Json.decode(json)
--printyellow(dump(t1, "json"))

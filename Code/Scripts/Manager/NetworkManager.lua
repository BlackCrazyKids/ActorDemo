--[[
json 格式做网络协议
class 字段顺序排列,则数据在json中也顺序排列
字段为kv时,数据可乱序,此时的key也是一种数据
借鉴M10项目
--]]
local Json = require('rapidjson')
local pb = require "pb"
local protoc = require "protoc"
local format = string.format
local Event = Event
local GameEvent = GameEvent
local Protocol = Protocol

---@type Game.NetworkManager
local NetworkMgr = Game.Manager.NetworkMgr
---@type Game.NetworkChannel
local channel

---@class NetworkManager
local NetworkManager = {}
local networkEvt = Event.New('NetworkEvent')
--local ip = "192.168.50.90"
local ip = "192.168.0.132"
local port = 8686

---@param channel Game.NetworkChannel
local onChannelConnected = function(channel)
    printcolor('orange', channel.Name .. "Normal connection to the server.",
            channel.RemoteIPAddress, channel.RemotePort)
end
---@param channel Game.NetworkChannel
local onChannelClosed = function(channel)
    printcolor('orange', channel.Name .. "Channel Closed.")
end
---@param channel Game.NetworkChannel
---@param count number
local onMissHeartBeat = function(channel, count)
    printcolor('orange', channel.Name .. 'Miss Heart Beat', count)
end
---@param id int
---@param msg byte[]
local Receive = function(id, data)
    local name = Protocol[id]
    local msg = pb.decode(name, data)
    if networkEvt.eventMap[id] then
        printmodule(Local.LogProtocol, dump(msg, format("[Recevie]%s:%s", name, id), 10))
        networkEvt:Trigger(id, msg)
    else
        printmodule(Local.LogProtocol, dump(msg, format("[Recevie]%s:%s - No events can start!", name, id), 10))
    end
end

function NetworkManager.Init()
    local descriptor = require("Protocol.Descriptor")
    assert(protoc:load(descriptor))
    for name, id in pairs(Protocol) do
        Protocol[id] = name
    end
    Message.__send = NetworkManager.Send

    channel = NetworkMgr:CreateNetworkChannel("NetworkChannel")
    channel.NetworkReceive = Receive
    channel.NetworkChannelConnected = onChannelConnected
    channel.NetworkChannelClosed = onChannelClosed
    channel.NetworkChannelMissHeartBeat = onMissHeartBeat
    channel:Connect(ip, port)
end
---@param type number
---@param msg table
function NetworkManager.Send(id, msg)
    local name = Protocol[id]
    local encode = pb.encode(name, msg)
    channel:Send(id, encode)
    printmodule(Local.LogProtocol, dump(msg, format("[Send]%s:%s", name, id), 10))
end
function NetworkManager.AddListener(id, handle)
    networkEvt:Add(id, handle)
end
function NetworkManager.RemoveListener(id, handle)
    networkEvt:Remove(id, handle)
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

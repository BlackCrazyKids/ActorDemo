local NetworkMgr = require "Manager.NetworkManager"

local Timer = Timer

local Login = {}

local _timer

local function SecondUpdate()
    ---反复Ping(time, recvCount)=>心跳?
    ---超时自动重连
    ---[可选]定时检查资源版本并做出提示
    print("Login", Time.time)
end

function Login.Init()
    _timer = Timer:new(SecondUpdate, 1, -1)
    _timer:Start()
end

return Login
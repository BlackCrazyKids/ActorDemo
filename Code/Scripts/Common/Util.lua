local insert = table.insert
local concat = table.concat
local schar = string.char
local sbyte = string.byte
local find = string.find
local sub = string.sub
local pairs = pairs
local ipairs = ipairs
local math = math
local traceback = debug.traceback
local xpcall = xpcall
local LogError = LogError

---@class Util
local Util = {}
local this = Util

--错误处理机制
function Util.ErrHandler(e)
    LogError(e .. traceback())
end

function Util.Myxpcall(func, data)
    return xpcall(function()
        func(data)
    end, this.ErrHandler)
end

--字符串转整数
function Util.ToLong(s)
    local n = 0
    for i = #s, 1, -1 do
        n = n * 256 + sbyte(s, i)
    end
    return n
end

function Util.BytesToString(bytes)
    local d = {}
    for i = 1, bytes.Length do
        local x = bytes[i]
        insert(d, schar(x >= 0 and x or x + 256))
    end
    return concat(d)
end
--获取或创建类表
function Util.GetOrCreate(namespace)
    local t = _G
    local idx = 1
    while true do
        local start, ends = find(namespace, ".", idx, true)
        local subname = sub(namespace, idx, start and start - 1)
        local subt = t[subname]
        if not subt then
            subt = {}
            t[subname] = subt
        end
        t = subt
        if start then
            idx = ends + 1
        else
            return t
        end
    end
end

function Util.InsidePolygon(polygon, p)
    local N = #polygon
    local counter = 0
    local i
    local xinters
    local p1
    local p2
    p1 = polygon[1]
    for i = 2, (N + 1) do
        if (i % N == 0) then
            p2 = polygon[i]
        else
            p2 = polygon[i % N]
        end
        if (p.z > math.min(p1.z, p2.z)) then
            if (p.z <= math.max(p1.z, p2.z)) then
                if (p.x <= math.max(p1.x, p2.x)) then
                    if (p1.z ~= p2.z) then
                        xinters = (p.z - p1.z) * (p2.x - p1.x) / (p2.z - p1.z) + p1.x
                        if ((p1.x == p2.x) or (p.x <= xinters)) then
                            counter = counter + 1
                        end
                    end
                end
            end
        end
        p1 = p2
    end
    if (counter % 2 == 0) then
        return false
    else
        return true
    end
end
--判断一个点是否在一个任意多边形内
function Util.InsideAnyPolygon(polygon, p)
    local count = 0
    local n = #polygon
    local a
    local b
    a = polygon[1]
    for i = 2, n + 1 do
        if (i % n == 0) then
            b = polygon[i]
        else
            b = polygon[i % n]
        end
        if ((a.x <= p.x and p.x <= b.x) or (b.x <= p.x and p.x <= a.x)) then
            local r = (p.x - a.x) * (a.z - b.z) - (p.z - a.z) * (a.x - b.x)
            if (r == 0) then
                -- 在边上
                if (a.x ~= p.x or p.x ~= b.x or (a.z <= p.z and p.z <= b.z) or (b.z <= p.z and p.z <= a.z)) then
                    return true
                end
            elseif (r / (a.x - b.x) > 0) then
                count = count + 1
            end
        end
        a = b
    end
    return (count % 2 == 1)
end
--获取枚举名
function Util.GetEnumName(enumtype, enumvalue)
    for name, value in pairs(enumtype) do
        if value == enumvalue then
            return name
        end
    end
    return ""
end
--设置粒子特效scale
function Util.SetParticleSystemScale(gameObject, scale)
    if not gameObject or not scale or scale <= 0 or scale == 1 then
        return
    end

    local allParticleSystem = gameObject:GetComponentsInChildren(CS.UnityEngine.ParticleSystem, true)
    for i = 1, allParticleSystem.Length do
        local particleSystem = allParticleSystem[i]
        if particleSystem then
            particleSystem.startSize = particleSystem.startSize * scale
            particleSystem.startSpeed = particleSystem.startSpeed * scale
        end
    end
end
--获取默认组件
function Util.GetDefaultComponent(go, com)
    if go then
        local component = go:GetComponent(com)
        if not component then
            component = go:AddComponent(com)
        end
        return component
    end
    return nil
end

function Util.IsChinese(val)
    --4e00-u9fa5
    local ret = val >= 0x4E00 and val <= 0x9FA5
    if ret then
        printcolor("IsChinese")
    end
    return ret
end
--对应ACSII表
local ExpCharacters = { 91, 93, 62, 60, 63, 92, 47, 46, 44, 42, 35, 33, 38, 40, 41, 96, 58, 61, 59 }
function Util.IsInExpCharacters(c)
    for _, char in ipairs(ExpCharacters) do
        if c == char then
            return true
        end
    end
    return false
end
function Util.IsCharacter(bt)
    local ret = (bt >= 48 and bt <= 57) or (bt >= 65 and bt <= 90) or (bt >= 97 and bt <= 122) or this.IsInExpCharacters(bt)
    if ret then
        printcolor("IsCharacter")
    end
    return ret
end

return Util
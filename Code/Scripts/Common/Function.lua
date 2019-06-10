local type = type
local ipairs = ipairs
local select = select
local tostring = tostring
local concat = table.concat
local format = string.format
local traceback = debug.traceback
local collectgarbage = collectgarbage

local dump = dump
local Debug = CS.UnityEngine.Debug
local GameObject = GameObject

printf = function(fmt, ...)
    print(format(fmt, ...))
end

print = function(...)
    if not Local.LogManager then
        return
    end
    local args = { ... }
    for k, v in ipairs(args) do
        args[k] = tostring(v)
    end
    if Local.LogTraceback then
        Debug.Log(concat(args, '\t') .. '\n' .. traceback())
    else
        Debug.Log(concat(args, '\t'))
    end
end

local DEFAULT_LOG_COLOR = "yellow"
local color = DEFAULT_LOG_COLOR
--输出黄色日志-无格式
local function printwitchcolor(...)
    if not Local.LogManager then
        return
    end
    local args = { ... }
    for k, v in ipairs(args) do
        args[k] = tostring(v)
    end

    local log = format("<color=%s>%s</color>\t\n", color, concat(args, '\t'))
    if Local.LogTraceback then
        Debug.Log(log .. traceback())
    else
        Debug.Log(log)
    end
end
function printcolor(c, ...)
    color = c
    printwitchcolor(...)
    color = DEFAULT_LOG_COLOR
end
function printyellow(...)
    printcolor("yellow", ...)
end
function printt(t, des)
    if not Local.LogManager then
        return
    end
    des = des or ''
    if type(t) == "table" then
        print(format("<color=orange>%s</color>\n%s", des, dump(t, 'table')))
    else
        print(format("%s\n%s", des, tostring(t)))
    end
end
function printmodule(m, ...)
    if not Local.LogManager then
        return
    end
    if m then
        print(...)
    end
end

--输出日志--
function Log(...)
    if not Local.LogManager then
        return
    end
    Debug.Log(format(...))-- or "Log Nil"
end
--错误日志--
function LogError(...)
    if not Local.LogManager then
        return
    end
    Debug.LogError(format(...))-- or "LogError Nil"
end
--警告日志--
function LogWarning(...)
    if not Local.LogManager then
        return
    end
    Debug.LogWarning(format(...))-- or "LogWarning Nil"
end

--Unity对象操作--
function FindObj(str)
    return GameObject.Find(str)
end
function Destroy(obj)
    GameObject.Destroy(obj)
end
function NewObject(prefab)
    return GameObject.Instantiate(prefab)
end
function DontDestroyOnLoad(obj)
    if not IsNull(obj) then
        if IsNull(obj.transform.parent) then
            GameObject.DontDestroyOnLoad(obj)
        end
    end
    return obj
end
function IsNull(unity_object)
    if unity_object == nil then
        return true
    end

    if type(unity_object) == "userdata" and unity_object.IsNull ~= nil then
        return unity_object:IsNull()
    end

    return false
end

---@param a 判定条件
---@param b true返回b
---@param c false返回c
function ConditionOp(a, b, c)
    if a then
        return b
    else
        return c
    end
end
function LuaGC()
    local c1 = collectgarbage("count")
    collectgarbage("collect")
    local c2 = collectgarbage("count")
    Log("=== gc before:%.1fkb, after %.1fkb", c1, c2)
end


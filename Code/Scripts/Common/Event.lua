local pairs = pairs
local error = error
local type = type
local insert = table.insert
local format = string.format
local ipairs = ipairs

--实现原理类似C#委托事件

local Class = Class
local Util = Util
local List = List
local ilist = ilist
local _event = Class:new("Event")

function _event:ctor(name)
    self.name = name or "__unknown__"
    self.eventMap = {}
    self.statusMap = {}
end

function _event:Add(name, handler)
    if not name or type(name) ~= "string" then
        error("[Event.Add]eventName has to be string, " .. type(name) .. " not right.")
    end
    local handlers = self.eventMap[name]
    if not handlers then
        handlers = List:new()
        self.eventMap[name] = handlers
    end
    handler = handlers:Push(handler)
    return function()
        self:Remove(name, handler)
    end
end

---@param handler LinkListNode
function _event:Remove(name, handler)
    --print("[Event:Remove]remove event handler.", self.name, event)
    if self.eventMap[name] then
        local handlers = self.eventMap[name]
        handlers:Remove(handler)
    else
        print("[Event:Remove]eventName no handler.", self.name)
    end
end

function _event:Destroy(name)
    if self.eventMap[name] then
        self.eventMap[name] = nil
    else
        print("[Event:Destroy]eventlib no event", self.name, name)
    end
end

function _event:AddStatusListener(listener)
    self.statusMap[listener.name] = listener
end

function _event:Trigger(name, ...)
    local handles = self.eventMap[name]
    if not handles then
        --error("[Event:Trigger]eventlib no event:" .. event)
        return
    end
    for _, handler in ilist(handles) do
        --print("[Event:Trigger] ", self.name, target, handler)
        if self.statusMap[name] then
            self.statusMap[name]:BeginSample()
        end
        Util.Myxpcall(handler, ...)
        if self.statusMap[name] then
            self.statusMap[name]:EndSample()
        end
    end
end

function _event:Dump(name)
    local r = {}
    if name then
        if self.eventMap[name] then
            insert(r, format("%s\t\t\t\t%d", name, #self.eventMap[name]))
        else
            printcolor("eventlib no event", name)
        end
    else
        for k, v in pairs(self.eventMap) do
            insert(r, format("%s\t\t\t\t%d", k, #v))
        end
    end
    print('dump event list..', '\n' .. table.concat(r, '\n'))
end

local function New(name)
    return _event:new(name)
end

local global = New("__globalEvent")
-- 如果不指定事件名,则创建一个匿名事件
local function NewSimple(name)
    name = name or ""
    ---@class SimpleEvent
    local evt = {
        Add = function(self, handler)
            return global:Add(name, handler)
        end,
        Remove = function(self, event, handler)
            global:Remove(event, handler)
        end,
        Trigger = function(self, ...)
            global:Trigger(name, ...)
        end,
        AddStatusListener = function(self, listener)
            global:AddStatusListener(listener)
        end,
        Destroy = function(self)
            global:Destroy(name)
        end,
        Dump = function(self, event)
            global:Dump(event)
        end,
        name = name,
    }
    return evt
end


return {
    ---独立事件列表
    New = New,
    ---共用__global事件列表
    NewSimple = NewSimple,
}
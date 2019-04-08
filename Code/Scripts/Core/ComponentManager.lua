local format = string.format
local error = error
local ipairs = ipairs
local assert = assert
local copy = table.copy
local clone = clone
local Class = Class
local Struct = require('ECS.ComponentStruct')
local ObjectPool = require('Common.ObjectPool')
--[[
    #ComponentManager 中Create,Release,Extend,Init字段禁止覆盖
    #Component在任何系统中均以索引来访问
--]]
---@class ComponentManager
local ComponentManager = Class:new('ComponentManager')
local Component = Class:new('Component')

local components = {}
local compsPool = {}
function Component:ctor(index)
    local comp = components[index]
    if comp then
        return clone(comp.fileds)
    else
        error(format('Component: %s not registered in the components.', name))
        return nil
    end
end

function ComponentManager.Create(index)
    ---@type ObjectPool
    local objectPool = compsPool[index]
    if not objectPool then
        objectPool = ObjectPool:new(self, 50)
    end

    return objectPool:Pop(index)
end
function ComponentManager.Release(index, comp)
    local objectPool = compsPool[index]
    if objectPool then
        copy(components[index], comp)
        objectPool:Push(comp)
    else
        error(format('Component:ObjectPool-%d does not exist', index))
    end
end

local function Register(index, name, fileds)
    local fs = components[index]
    if fs then
        error(format('Component: %s-%d already exists', name, index))
    else
        components[index] = fileds
        components[name] = index
    end
end
function ComponentManager.Extend(custom)
    local index = #Struct
    for i, v in ipairs(custom) do
        Register(index + i, v[1], v[2])
    end
end
function ComponentManager.Init()
    for i, v in ipairs(Struct) do
        Register(i, v[1], v[2])
    end
end

return ComponentManager


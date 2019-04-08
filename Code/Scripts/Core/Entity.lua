local assert = assert
local error = error
local pairs = pairs
local type = type
local format = string.format
local Class = Class
local printcolor = printcolor
local ComponentMgr = require('Core.ComponentManager')

---@class Entity
local Entity = Class:new('Entity')

---- 实体初始化也是数据驱动?
function Entity:ctor(world, id)
    self.id = id
    self.world = world
    self.enable = true
end

function Entity:Reset(world, id)
    self:ctor(world, id)
end

function Entity:Destroy()
    if not self.enable then
        error(format('Entity:%d has been destroyed.', self.id))
    end

    self:OnDestroyEntity()
    self.enable = false
    self:RemoveAllComponents()
end

function Entity:RemoveAllComponents()
    for k, v in pairs(self) do
        if type(k) == 'number' then
            self:RemoveComponent(k)
        end
    end
end

function Entity:HasComponent(index)
    return self[index] ~= nil
end
function Entity:HasComponents(indices)
    for i = 1, #indices do
        if not self[indices[i]] then
            return false
        end
    end
    return true
end
function Entity:HasAnyComponent(indices)
    for i = 1, #indices do
        if self[indices[i]] then
            return true
        end
    end
    return false
end

function Entity:AddComponent(index)
    if not self.enable then
        printcolor('red', format('Entity:%d has been destroyed.', self.id))
        return
    end
    if self[index] then
        printcolor('red', "Entity: Trying to add Component '" .. index .. "', but it's already existing.")
        return
    end

    local comp = ComponentMgr.Create(index)
    self[index] = comp
    self.world:OnComponentChanged(self, index)
end
function Entity:RemoveComponent(index)
    if not self.enable then
        printcolor('red', format('Entity:%d has been destroyed.', self.id))
        return
    end
    if not self[index] then
        printcolor('red', "Entity: Trying to remove non-existent component " .. index .. " from Entity. Please fix this")
        return
    end

    ComponentMgr.Release(index, self[index])
    self[index] = nil
    self.world:OnComponentChanged(self, index)
end
function Entity:Modify(index)
    assert(self[index] == nil, "Entity: Trying to remove non-existent component " .. index .. " from Entity. Please fix this")
    self.world:OnComponentModify(self, index)
end

return Entity
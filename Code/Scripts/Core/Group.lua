local type = type
local assert = assert
local error = error
local clear = table.clear
local insert = table.insert
local Class = Class

---@class Group
local Group = Class:new('Group')

function Group:ctor(filter)
    self.filter = filter
    self.entities = {}
    self.systems = {}
end

function Group:Filter(entity)
    local r = self.filter:Handle(entity)
    local index = self.entities[entity]
    if r then
        if not index then
            insert(self.entities, entity)
            self.entities[entity] = #self.entities
        end
    else
        if index then
            local length = #self.entities
            local last = self.entities[length]
            self.entities[index] = last
            self.entities[entity] = nil
            self.entities[length] = nil
            self.entities[last] = index
        end
    end
    return r
end

---检查是否还有所关注的系统,如果无则清空数据
function Group:CheckEmpty()
    local r = #self.systems == 0
    if r and #self.entities > 0 then
        clear(self.entities)
    end
    return r
end
function Group:AddSystem(name)
    insert(self.systems, name)
    self.systems[name] = #self.systems
end
function Group:RemoveSystem(name)
    local index = self.systems[name]
    local length = #self.systems
    self.systems[index] = self.systems[length]
end

return Group
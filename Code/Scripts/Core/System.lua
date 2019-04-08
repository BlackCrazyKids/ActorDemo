local printcolor = printcolor
local printyellow = printyellow
local type = type
local assert = assert
local format = string.format
local sub = string.sub
local byte = string.byte
local concat = table.concat
local sort = table.sort
local clear = table.clear
local error = error
local Class = Class

---@class System
local System = Class:new('System')

--[[
    是否关注一下事件,初始化时手动绑定
    --ComponentAdd
    --ComponentRemove
    --ComponentModify
    -------------------------------
    filter 定义格式固定为table.
    {Comp.Position, Comp.Rotation, Comp.Scale}
--]]

--[[
    两种方式无差异
    local f1 = Filter:new()
    f1:All(2, 1, 3)
    f1:None(4)
    f1:Any(5)
    self.filter = f1

    local f2 = Filter:new()
    f2:All(1, 2, 3, f2:None(4), f2:Any(5))
    self.filter = f2
--]]
function System:Init(world, name)
    self.world = world
    self.name = name
    self.group = nil      --> Update
    self.collector = nil    --> Notify
    if self.filter then
        self.filter:Init()
        self.filterName = self.filter.hashCode
    else
        error('System:filter table is nil.[' .. self.name .. ']')
    end
    assert(self.OnUpdate or self.OnNotify and not (self.OnUpdate and self.OnNotify),
            'System:' .. name .. 'No Define OnUpdate or OnNotify')
end

function System:Notify()
    if self.collector then
        for i = 1, #self.collector do
            local entity = self.collector[i]
            self:OnNotify(entity)
        end
        clear(self.collector)
    end
end

function System:Filter(entity)
    return self.filter:Handle(entity)
end

function System:Destroy()
    self.collector = nil
    self.group = nil
end

return System
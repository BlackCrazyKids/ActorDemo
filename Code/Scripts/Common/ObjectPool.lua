local List = List
local Class = Class

--定时销毁功能后续完善

---@class ObjectPool
local ObjectPool = Class:new()

function ObjectPool:ctor(class, maxnum, createObjectWhenPoolIsFull)
    self.class = class
    self.pool = List:new()
    self.maxnum = maxnum or 5
    self.createObjectWhenPoolIsFull = createObjectWhenPoolIsFull or true
end

function ObjectPool:Pop(params)
    if self.pool.length > 0 then
        return self.pool:Pop()
    end
    if self.createObjectWhenPoolIsFull then
        return self.class:new(params)
    else
        return nil
    end
end

function ObjectPool:Push(object)
    if object == nil then
        return false
    end
    if self.pool.length < self.maxnum then
        self.pool:Push(object)
        return true
    end
    return false
end

return ObjectPool

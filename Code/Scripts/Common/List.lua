local getmetatable = getmetatable
local insert = table.insert
local max = math.max

---@class List
local List = Class:new()

function List:ctor()
    self.length = 0
    self._prev = self
    self._next = self
end

function List:Clear()
    self:ctor()
end
---@param value any 在list最后推入值
function List:Push(value)
    --assert(value)
    local node = { value = value, _prev = 0, _next = 0, removed = false }

    self._prev._next = node
    node._next = self
    node._prev = self._prev
    self._prev = node

    self.length = self.length + 1
    return node
end
---@param node node 在list最后推入节点
function List:PushNode(node)
    if not node.removed then
        return
    end

    self._prev._next = node
    node._next = self
    node._prev = self._prev
    self._prev = node
    node.removed = false
    self.length = self.length + 1
end
---在list最后面取一个值
function List:Pop()
    local _prev = self._prev
    self:Remove(_prev)
    return _prev.value
end
---@param v any 在list最前面插入一个值
function List:Unshift(v)
    local node = { value = v, _prev = 0, _next = 0, removed = false }

    self._next._prev = node
    node._prev = self
    node._next = self._next
    self._next = node

    self.length = self.length + 1
    return node
end
---在list最前面取一个值
function List:Shift()
    local _next = self._next
    self:Remove(_next)
    return _next.value
end
---@param iter node 从list中移除当前节点
function List:Remove(iter)
    if iter.removed then
        return
    end

    local _prev = iter._prev
    local _next = iter._next
    _next._prev = _prev
    _prev._next = _next

    self.length = max(0, self.length - 1)
    iter.removed = true
end
---顺序遍历list
---@param v any 查询值
---@param iter node 开始节点
function List:Find(v, iter)
    iter = iter or self

    repeat
        if v == iter.value then
            return iter
        else
            iter = iter._next
        end
    until iter == self

    return nil
end
---逆序遍历list
---@param v any 查询值
---@param iter node 开始节点
function List:FindLast(v, iter)
    iter = iter or self

    repeat
        if v == iter.value then
            return iter
        end

        iter = iter._prev
    until iter == self

    return nil
end
---@param iter node 节点后面一个节点
function List:Next(iter)
    local _next = iter._next
    if _next ~= self then
        return _next, _next.value
    end

    return nil
end
---@param iter node 节点前面一个节点
function List:Prev(iter)
    local _prev = iter._prev
    if _prev ~= self then
        return _prev, _prev.value
    end

    return nil
end
---@param v any 从list中删除第一个v节点
function List:Erase(v)
    local iter = self:Find(v)

    if iter then
        self:Remove(iter)
    end
end
---非时,将值插入节点后;空时,直接在list最后插入值
---@param v any 插入值
---@param iter node 指定节点
function List:Insert(v, iter)
    if not iter then
        return self:Push(v)
    end

    local node = { value = v, _next = 0, _prev = 0, removed = false }

    if iter._next then
        iter._next._prev = node
        node._next = iter._next
    else
        self.last = node
    end

    node._prev = iter
    iter._next = node
    self.length = self.length + 1
    return node
end
---list头部节点值
function List:Head()
    return self._next.value
end
---list尾部节点值
function List:Tail()
    return self._prev.value
end

function List:Clone()
    local t = List:new()

    for i, v in List.Next, self, self do
        t:Push(v)
    end

    return t
end

ilist = function(_list)
    return List.Next, _list, _list
end
rilist = function(_list)
    return List.Prev, _list, _list
end

function List:ToTable()
    local t = {}
    for _, v in ilist(self) do
        insert(t, v)
    end
    return t
end

return List
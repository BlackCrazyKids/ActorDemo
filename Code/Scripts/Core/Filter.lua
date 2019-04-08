local type = type
local error = error
local pcall = pcall
local select = select
local concat = table.concat
local sort = table.sort
local format = string.format
local insert = table.insert
local sub = string.sub
local byte = string.byte
local copy = table.copy
local unique = table.unique
local getmetatable = getmetatable
local loadstring = loadstring or load

local function filterJoinRaw(filter, prefix, seperator, ...)
    local accum = {}
    local build = {}
    local logic = seperator:trim()
    for i = 1, select('#', ...) do
        local item = select(i, ...)
        if type(item) == 'number' then
            accum[#accum + 1] = ("(e[%s] ~= nil)"):format(item)
            if prefix == 'not' then
                insert(filter.none, item)
            elseif logic == 'and' then
                insert(filter.all, item)
            elseif logic == 'or' then
                insert(filter.any, item)
            end
        elseif type(item) == 'function' then
            build[#build + 1] = ('local subfilter_%d_ = select(%d, ...)')
                    :format(i, i)
            accum[#accum + 1] = ('(subfilter_%d_(f, e))'):format(i)
        else
            printcolor('red', 'Filter token must be a string or a filter function.')
        end
    end
    local source = ('%s\nreturn function(f, e) return %s(%s) end')
            :format(
            concat(build, '\n'),
            prefix,
            concat(accum, seperator))
    local loader, err = loadstring(source)
    if err then
        error(err)
    end
    return loader(...)
end

local function filterJoin(filter, ...)
    local state, value = pcall(filterJoinRaw, filter, ...)
    if state then
        return value
    else
        return nil, value
    end
end

--- Makes a Filter that selects Entities with all specified Components and Filters.
local function RequireAll(filter, ...)
    return filterJoin(filter, '', ' and ', ...)
end

--- Makes a Filter that selects Entities with at least one of the specified Components and Filters.
local function RequireAny(filter, ...)
    return filterJoin(filter, '', ' or ', ...)
end

--- Makes a Filter that rejects Entities with all specified Components and Filters, and selects all other Entities.
local function RequireNone(filter, ...)
    return filterJoin(filter, 'not', ' and ', ...)
end

----- Makes a Filter that rejects Entities with at least one of the specified Components and Filters, and selects all other Entities.
--local function RejectAny(...)
--    return filterJoin('not', ' or ', ...)
--end

local function Hash(str, hash)
    local seed = 131
    for i = 1, #str do
        hash = hash * seed + byte(sub(str, i, i))
    end
    return (hash & 0x7FFFFFFF)
end
-- 为filter 筛选条件表唯一标识
local function GenUnique(filter, hash)
    sort(filter)
    return Hash(concat(filter, '.'), hash or 0)
end
local function EQ(a, b)
    return a.allHash == b.allHash and a.anyHash == b.anyHash and a.noneHash == b.noneHash
end

local Class = Class
---@class Filter
local Filter = Class:new('Filter')

function Filter:All(...)
    self.AllHandle = RequireAll(self, ...)
    return self.AllHandle
end
function Filter:Any(...)
    self.AnyHandle = RequireAny(self, ...)
    return self.AnyHandle
end
function Filter:None(...)
    self.NoneHandle = RequireNone(self, ...)
    return self.NoneHandle
end

function Filter:ctor()
    self.all = {}
    self.any = {}
    self.none = {}
    local mt = getmetatable(self)
    mt.__eq = EQ
end

function Filter:Init()
    if not self.hashCode then
        local hash = GenUnique(self.all)
        hash = GenUnique(self.any, hash)
        self.hashCode = GenUnique(self.none, hash)
    end
    if not self.indices then
        local t = {}
        copy(self.all, t)
        copy(self.any, t)
        copy(self.none, t)
        self.indices = unique(t)
    end
end

function Filter:Handle(e)
    local r = true
    if self.AllHandle then
        r = r and self:AllHandle(e)
    end
    if self.AnyHandle then
        r = r and self:AnyHandle(e)
    end
    if self.NoneHandle then
        r = r and self:NoneHandle(e)
    end
    return r
end

return Filter
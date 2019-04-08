local tostring = tostring
local tonumber = tonumber
local round = math.round
local setmetatable = setmetatable
local getmetatable = getmetatable
local pairs = pairs
local require = require
local pcall = pcall
--local math = math
--local string = string
--local table = table
--local io = io
local pairs = pairs
local ipairs = ipairs
local os = os
local type = type
local debug = debug


--[[--
输出值的内容

### 用法示例

~~~ lua

local t = {comp = "chukong", engine = "quick"}

dump(t)

~~~
]]
---@param value any 要输出的值
---@param desciption string 输出内容前的文字描述,可选
---@param nesting integer 输出时的嵌套层级，默认为 3,可选
function dump(value, desciption, nesting)
    if type(nesting) ~= "number" then
        nesting = 3
    end

    local lookupTable = {}
    local result = {}

    local function _v(v)
        if type(v) == "string" then
            v = "\"" .. v .. "\""
        end
        return tostring(v)
    end
    local function _dump(value, desciption, indent, nest, keylen)
        desciption = desciption or "<var>"
        spc = ""
        if type(keylen) == "number" then
            spc = string.rep(" ", keylen - string.len(_v(desciption)))
        end
        if type(value) ~= "table" then
            result[#result + 1] = string.format("%s%s%s = %s", indent, _v(desciption), spc, _v(value))
        elseif lookupTable[value] then
            result[#result + 1] = string.format("%s%s%s = *REF %s*", indent, desciption, spc, value)
        else
            lookupTable[value] = true
            if nest > nesting then
                result[#result + 1] = string.format("%s%s = *MAX NESTING %s*", indent, desciption, nest)
            else
                result[#result + 1] = string.format("%s%s = {", indent, _v(desciption))
                local indent2 = indent .. "    "
                local keys = {}
                local keylen = 0
                local values = {}
                for k, v in pairs(value) do
                    keys[#keys + 1] = k
                    local vk = _v(k)
                    local vkl = string.len(vk)
                    if vkl > keylen then
                        keylen = vkl
                    end
                    values[k] = v
                end
                table.sort(keys, function(a, b)
                    if type(a) == "number" and type(b) == "number" then
                        return a < b
                    else
                        return tostring(a) < tostring(b)
                    end
                end)
                for i, k in ipairs(keys) do
                    _dump(values[k], k, indent2, nest + 1, keylen)
                end
                result[#result + 1] = string.format("%s}", indent)
            end
        end
    end
    _dump(value, desciption, "- ", 1)

    local content = nil
    for i, line in ipairs(result) do
        if i == 1 then
            content = string.format("%s", line)
        else
            content = string.format("%s\n%s", content, line)
        end
    end
    return content
end

---检查并尝试转换为数值，如果无法转换则返回 0
---@param value any 要检查的值
---@param base int 进制,默认为十进制
---@return number
function checknumber(value, base)
    return tonumber(value, base) or 0
end

---检查并尝试转换为整数，如果无法转换则返回 0
---@param value any 要检查的值
---@return int
function checkint(value)
    return round(checknumber(value))
end

---检查并尝试转换为布尔值，除了 nil 和 false，其他任何值都会返回 true
---@param value any 要检查的值
---@return boolean
function checkbool(value)
    return (value ~= nil and value ~= false)
end

---检查值是否是一个表格，如果不是则返回一个空表格
---@param value any 要检查的值
---@return table
function checktable(value)
    if type(value) ~= "table" then
        value = {}
    end
    return value
end

---如果表格中指定 key 的值为 nil，或者输入值不是表格，返回 false，否则返回 true
---表格中包含指定key
---@param hashtable table 要检查的表格
---@param key any 要检查的键名
---@return table
function isset(hashtable, key)
    local t = type(hashtable)
    return (t == "table" or t == "userdata") and hashtable[key] ~= nil
end

--[[--
深度克隆一个值

~~~ lua

-- 下面的代码，t2 是 t1 的引用，修改 t2 的属性时，t1 的内容也会发生变化
local t1 = {a = 1, b = 2}
local t2 = t1
t2.b = 3    -- t1 = {a = 1, b = 3} <-- t1.b 发生变化

-- clone() 返回 t1 的副本，修改 t2 不会影响 t1
local t1 = {a = 1, b = 2}
local t2 = clone(t1)
t2.b = 3    -- t1 = {a = 1, b = 2} <-- t1.b 不受影响

~~~
]]
---@param object any 要克隆的值
---@return any
function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

---lua对象回调函数
---@param obj luaObj Lua对象
---@param method function Lua对象函数
---@return any
function handler(obj, method)
    return function(...)
        return method(obj, ...)
    end
end

function math.newrandomseed()
    local ok, socket = pcall(function()
        return require("socket")
    end)

    if ok then
        -- 如果集成了 socket 模块，则使用 socket.gettime() 获取随机数种子
        math.randomseed(socket.gettime() * 1000)
    else
        math.randomseed(os.time())
    end
    math.random()
    math.random()
    math.random()
    math.random()
end

---对数值进行四舍五入，如果不是数值则返回 0
---@param value number
function math.round(value)
    return math.floor(value + 0.5)
end

function math.angle2radian(angle)
    return angle * math.pi / 180
end

function math.radian2angle(radian)
    return radian / math.pi * 180
end

---要检查的文件或目录的完全路径
function io.exists(path)
    local file = io.open(path, "r")
    if file then
        io.close(file)
        return true
    end
    return false
end

---读取文件内容，返回包含文件内容的字符串，如果失败返回 nil
---@param path string 文件完全路径
---@return string
function io.readfile(path)
    local file = io.open(path, "r")
    if file then
        local content = file:read("*a")
        io.close(file)
        return content
    end
    return nil
end

--[[
以字符串内容写入文件，成功返回 true，失败返回 false
此外，还可以在 "写入模式" 参数最后追加字符 "b" ，表示以二进制方式写入数据，这样可以避免内容写入不完整。
注意:各个平台下的路径可读写性
--]]
---@param path string 文件完全路径
---@param content string 要写入的内容
---@param mode string 写入模式，默认值为 "w+b",可选参数
---@return boolean
function io.writefile(path, content, mode)
    mode = mode or "w+b"
    local file = io.open(path, mode)
    if file then
        if file:write(content) == nil then
            return false
        end
        io.close(file)
        return true
    else
        return false
    end
end

--[[
拆分一个路径字符串，返回组成路径的各个部分

结果:
pathinfo.dirname  = "/var/app/test/"
pathinfo.filename = "abc.png"
pathinfo.basename = "abc"
pathinfo.extname  = ".png"
--]]
---@param path string 文件完全路径
---@return table
function io.pathinfo(path)
    local pos = string.len(path)
    local extpos = pos + 1
    while pos > 0 do
        local b = string.byte(path, pos)
        if b == 46 then
            -- 46 = char "."
            extpos = pos
        elseif b == 47 then
            -- 47 = char "/"
            break
        end
        pos = pos - 1
    end

    local dirname = string.sub(path, 1, pos)
    local filename = string.sub(path, pos + 1)
    extpos = extpos - pos
    local basename = string.sub(filename, 1, extpos - 1)
    local extname = string.sub(filename, extpos)
    return {
        dirname = dirname,
        filename = filename,
        basename = basename,
        extname = extname
    }
end

---返回指定文件的大小，如果失败返回 false
---@param path string 文件完全路径
---@return integer
function io.filesize(path)
    local size = false
    local file = io.open(path, "r")
    if file then
        local current = file:seek()
        size = file:seek("end")
        file:seek("set", current)
        io.close(file)
    end
    return size
end

--[[
计算表格包含的字段数量
Lua table 的 "#" 操作只对依次排序的数值下标数组有效
table.count() 则计算 table 中所有不为 nil 的值的个数。
--]]
---@param t table 要检查的表格
---@return integer
function table.count(t)
    local count = 0
    for k, v in pairs(t) do
        count = count + 1
    end
    return count
end

---返回指定表格中的所有键
---@param hashtable table 要检查的表格
---@return table
function table.keys(hashtable)
    local keys = {}
    for k, v in pairs(hashtable) do
        keys[#keys + 1] = k
    end
    return keys
end

---返回指定表格中的所有值
---@param hashtable table 要检查的表格
---@return table
function table.values(hashtable)
    local values = {}
    for k, v in pairs(hashtable) do
        values[#values + 1] = v
    end
    return values
end

---将来源表格中所有键及其值复制到目标表格对象中，如果存在同名键，则覆盖其值
---@param src table  来源表格
---@param dst table  目标表格
function table.merge(src, dst)
    for k, v in pairs(dst) do
        src[k] = v
    end
end

---合并数组表
---@param src table  来源表格
---@param dst table  目标表格
function table.mergearray(src, dst)
    local index = #src
    for i = 1, #dst do
        src[index + i] = dst[i]
    end
end

---在目标表格的指定位置插入来源表格，如果没有指定位置则连接两个表格
---@param dest table  目标表格
---@param src table  来源表格
---@param begin integer  插入位置,可选
function table.insertto(dest, src, begin)
    begin = checkint(begin)
    if begin <= 0 then
        begin = #dest + 1
    end

    local len = #src
    for i = 0, len - 1 do
        dest[i + begin] = src[i + 1]
    end
end

---从表格中查找指定值，返回其索引，如果没找到返回 false
---@param array table 表格
---@param value any 要查找的值
---@param begin integer 起始索引值,可选
---@return integer
function table.indexof(array, value, begin)
    for i = begin or 1, #array do
        if array[i] == value then
            return i
        end
    end
    return false
end

---从表格中查找指定值，返回其 key，如果没找到返回 nil
---@param hashtable table 表格
---@param value any 要查找的值
---@return string 该值对应的 key
function table.keyof(hashtable, value)
    for k, v in pairs(hashtable) do
        if v == value then
            return k
        end
    end
    return nil
end

--[[--

从表格中删除指定值，返回删除的值的个数

~~~ lua

local array = {"a", "b", "c", "c"}
print(table.removebyvalue(array, "c", true)) -- 输出 2

~~~
]]
---@param array table 表格
---@param value any 要删除的值
---@param removeall boolean 是否删除所有相同的值,可选
---@return integer
function table.removebyvalue(array, value, removeall)
    local c, i, max = 0, 1, #array
    while i <= max do
        if array[i] == value then
            table.remove(array, i)
            c = c + 1
            i = i - 1
            max = max - 1
            if not removeall then
                break
            end
        end
        i = i + 1
    end
    return c
end

--[[--

对表格中每一个值执行一次指定的函数，并用函数返回值更新表格内容

~~~ lua

local t = {name = "dualface", comp = "chukong"}
table.map(t, function(v, k)
    -- 在每一个值前后添加括号
    return "[" .. v .. "]"
end)

-- 输出修改后的表格内容
for k, v in pairs(t) do
    print(k, v)
end

-- 输出
-- name [dualface]
-- comp [chukong]

~~~

fn 参数指定的函数具有两个参数，并且返回一个值。原型如下：

~~~ lua

function map_function(value, key)
    return value
end

~~~
]]
---@param t table 表格
---@param fn function 函数
function table.map(t, fn)
    for k, v in pairs(t) do
        t[k] = fn(v, k)
    end
end

--[[--

对表格中每一个值执行一次指定的函数，但不改变表格内容

~~~ lua

local t = {name = "dualface", comp = "chukong"}
table.walk(t, function(v, k)
    -- 输出每一个值
    print(v)
end)

~~~

fn 参数指定的函数具有两个参数，没有返回值。原型如下：

~~~ lua

function map_function(value, key)

end

~~~
]]
---@param t table 表格
---@param fn function 函数
function table.walk(t, fn)
    for k, v in pairs(t) do
        fn(v, k)
    end
end

--[[--

对表格中每一个值执行一次指定的函数，如果该函数返回 false，则对应的值会从表格中删除

~~~ lua

local t = {name = "dualface", comp = "chukong"}
table.filter(t, function(v, k)
    return v ~= "dualface" -- 当值等于 dualface 时过滤掉该值
end)

-- 输出修改后的表格内容
for k, v in pairs(t) do
    print(k, v)
end

-- 输出
-- comp chukong

~~~

fn 参数指定的函数具有两个参数，并且返回一个 boolean 值。原型如下：

~~~ lua

function map_function(value, key)
    return true or false
end

~~~
]]
---@param t table 表格
---@param fn function 函数
function table.filter(t, fn)
    for k, v in pairs(t) do
        if not fn(v, k) then
            t[k] = nil
        end
    end
end

--[[--

遍历表格，确保其中的值唯一

~~~ lua

local t = {"a", "a", "b", "c"} -- 重复的 a 会被过滤掉
local n = table.unique(t)

for k, v in pairs(n) do
    print(v)
end

-- 输出
-- a
-- b
-- c

~~~
]]
---@param t table 表格
---@return table 包含所有唯一值的新表格
function table.unique(t)
    local check = {}
    local n = {}
    for k, v in pairs(t) do
        if not check[v] then
            n[k] = v
            check[v] = true
        end
    end
    return n
end

--- 将表格中的数据设置成nil
---@param t table 数据表格
function table.clear(t)
    if t == nil then
        return
    end
    for k, _ in pairs(t) do
        t[k] = nil
    end
end

---浅复制,当键名相同时,则直接覆盖目标数据表键数据
---@param src table 源数据表
---@param dst table 目标数据表
function table.copy(src, dst)
    if src and dst then
        for k, v in pairs(src) do
            dst[k] = v
        end
    end
end

---判断table中是否有内容
---@param t table
function table.isempty(t)
    local check, kv = pairs(t)
    return check(kv) == nil
end

string._htmlspecialchars_set = {}
string._htmlspecialchars_set["&"] = "&amp;"
string._htmlspecialchars_set["\""] = "&quot;"
string._htmlspecialchars_set["'"] = "&#039;"
string._htmlspecialchars_set["<"] = "&lt;"
string._htmlspecialchars_set[">"] = "&gt;"

--[[--

将特殊字符转为 HTML 转义符

~~~ lua

print(string.htmlspecialchars("<ABC>"))
-- 输出 &lt;ABC&gt;

~~~
]]
---@param input string 输入字符串
---@return string 转换结果
function string.htmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, k, v)
    end
    return input
end

--[[--

将 HTML 转义符还原为特殊字符，功能与 string.htmlspecialchars() 正好相反

~~~ lua

print(string.restorehtmlspecialchars("&lt;ABC&gt;"))
-- 输出 <ABC>

~~~
]]
---@param input string 输入字符串
---@return string 转换结果
function string.restorehtmlspecialchars(input)
    for k, v in pairs(string._htmlspecialchars_set) do
        input = string.gsub(input, v, k)
    end
    return input
end

--[[--

将字符串中的 \n 换行符转换为 HTML 标记

~~~ lua

print(string.nl2br("Hello\nWorld"))
-- 输出
-- Hello<br />World

~~~
]]
---@param input string 输入字符串
---@return string 转换结果
function string.nl2br(input)
    return string.gsub(input, "\n", "<br />")
end

--[[--

将字符串中的特殊字符和 \n 换行符转换为 HTML 转移符和标记

~~~ lua

print(string.nl2br("<Hello>\nWorld"))
-- 输出
-- &lt;Hello&gt;<br />World

~~~
]]
---@param input string 输入字符串
---@return string 转换结果
function string.text2html(input)
    input = string.gsub(input, "\t", "    ")
    input = string.htmlspecialchars(input)
    input = string.gsub(input, " ", "&nbsp;")
    input = string.nl2br(input)
    return input
end

--[[--

用指定字符或字符串分割输入字符串，返回包含分割结果的数组

~~~ lua

local input = "Hello,World"
local res = string.split(input, ",")
-- res = {"Hello", "World"}

local input = "Hello-+-World-+-Quick"
local res = string.split(input, "-+-")
-- res = {"Hello", "World", "Quick"}

~~~
]]
---@param input string 输入字符串
---@param delimiter string 分割标记字符或字符串
---@return array 包含分割结果的数组
function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter == '') then
        return false
    end
    local pos, arr = 0, {}
    -- for each divider found
    for st, sp in function()
        return string.find(input, delimiter, pos, true)
    end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end

--[[--

去除输入字符串头部的空白字符，返回结果

~~~ lua

local input = "  ABC"
print(string.ltrim(input))
-- 输出 ABC，输入字符串前面的两个空格被去掉了

~~~

空白字符包括：

-   空格
-   制表符 \t
-   换行符 \n
-   回到行首符 \r
]]
---@param input string 输入字符串
---@return string 结果
---@see string.rtrim, string.trim
function string.ltrim(input)
    return string.gsub(input, "^[ \t\n\r]+", "")
end

--[[--

去除输入字符串尾部的空白字符，返回结果

~~~ lua

local input = "ABC  "
print(string.ltrim(input))
-- 输出 ABC，输入字符串最后的两个空格被去掉了

~~~
]]
---@param input string 输入字符串
---@return string 结果
---@see string.ltrim, string.trim
function string.rtrim(input)
    return string.gsub(input, "[ \t\n\r]+$", "")
end

---去掉字符串首尾的空白字符，返回结果
---@param input string 输入字符串
---@return string 结果
---@see string.ltrim, string.rtrim
function string.trim(input)
    input = string.gsub(input, "^[ \t\n\r]+", "")
    return string.gsub(input, "[ \t\n\r]+$", "")
end

--[[--

将字符串的第一个字符转为大写，返回结果

~~~ lua

local input = "hello"
print(string.ucfirst(input))
-- 输出 Hello

~~~
]]
---@param input string 输入字符串
---@return string 结果
function string.ucfirst(input)
    return string.upper(string.sub(input, 1, 1)) .. string.sub(input, 2)
end

local function urlencodechar(char)
    return "%" .. string.format("%02X", string.byte(char))
end

--[[--

将字符串转换为符合 URL 传递要求的格式，并返回转换结果

~~~ lua

local input = "hello world"
print(string.urlencode(input))
-- 输出
-- hello%20world

~~~
]]
---@param input string 输入字符串
---@return string 转换后的结果
---@see string.urldecode
function string.urlencode(input)
    -- convert line endings
    input = string.gsub(tostring(input), "\n", "\r\n")
    -- escape all characters but alphanumeric, '.' and '-'
    input = string.gsub(input, "([^%w%.%- ])", urlencodechar)
    -- convert spaces to "+" symbols
    return string.gsub(input, " ", "+")
end

--[[--

将 URL 中的特殊字符还原，并返回结果

~~~ lua

local input = "hello%20world"
print(string.urldecode(input))
-- 输出
-- hello world

~~~
]]
---@param input string 输入字符串
---@return string 转换后的结果
---@see string#urlencode
function string.urldecode(input)
    input = string.gsub(input, "+", " ")
    input = string.gsub(input, "%%(%x%x)", function(h)
        return string.char(checknumber(h, 16))
    end)
    input = string.gsub(input, "\r\n", "\n")
    return input
end

--[[--

计算 UTF8 字符串的长度，每一个中文算一个字符

~~~ lua

local input = "你好World"
print(string.utf8len(input))
-- 输出 7

~~~
]]
---@param input string 输入字符串
---@return integer 长度
function string.utf8len(input)
    local len = string.len(input)
    local left = len
    local cnt = 0
    local arr = { 0, 0xc0, 0xe0, 0xf0, 0xf8, 0xfc }
    while left ~= 0 do
        local tmp = string.byte(input, -left)
        local i = #arr
        while arr[i] do
            if tmp >= arr[i] then
                left = left - i
                break
            end
            i = i - 1
        end
        cnt = cnt + 1
    end
    return cnt
end

--[[--

将数值格式化为包含千分位分隔符的字符串

~~~ lua

print(string.formatnumberthousands(1924235))
-- 输出 1,924,235

~~~
]]
---@param num number 数值
---@return string 格式化结果
function string.formatnumberthousands(num)
    local formatted = tostring(checknumber(num))
    local k
    while true do
        formatted, k = string.gsub(formatted, "^(-?%d+)(%d%d%d)", '%1,%2')
        if k == 0 then
            break
        end
    end
    return formatted
end

function string.isempty(s)
    s = string.trim(s)
    return string.len(s) == 0
end

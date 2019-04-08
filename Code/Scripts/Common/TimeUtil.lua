local ipairs = ipairs
local setmetatable = setmetatable
local print = print
local error = error
local network = require("network")

local m_SCTimeShift = 0

------------------------------------------------------------------------------------------------------------
--[[
--功能：获取当前时间
--入参：无
--返回值：
--      [1]               type:number           从1970年到当前时刻的秒数
--]]
------------------------------------------------------------------------------------------------------------
local function getTime()
    return os.time()
end


------------------------------------------------------------------------------------------------------------
--[[
--功能：获取当前时间
--入参：无
--返回值：
--      [1]               type:table{year = 1998, month = 9, day = 16, yday = 259, wday = 4,hour = 23, min = 48, sec = 10, isdst = false}
--]]
------------------------------------------------------------------------------------------------------------
local function getDate()
    return os.date()
end


------------------------------------------------------------------------------------------------------------
--[[
--功能：获取时间段
--入参：seconds           type:int              时间段（n秒）
--返回值：
--      [1]               type:table{ days = 1,hours = 23,minutes=59,seconds=59}
--]]
------------------------------------------------------------------------------------------------------------

local function getDateTime(seconds)
    local datetime = {}
    datetime.days = math.floor(seconds / 86400)
    datetime.hours = math.floor(seconds % 86400 / 3600)
    datetime.minutes = math.floor(seconds % 86400 % 3600 / 60)
    datetime.seconds = math.floor(seconds % 86400 % 3600 % 60)
    return datetime
end


------------------------------------------------------------------------------------------------------------
--[[
-- 功能：获取时间段字符串
-- 入参：seconds           type:int              时间段（n秒）
--       format            type:string           "hh:mm:ss"
-- 返回值：
--       [1]               type:string { 11天 23:59:59}
--]]
------------------------------------------------------------------------------------------------------------
local function getDateTimeString(seconds,format)
    local datetime = getDateTime(seconds)
    local datetimestring = format
    --printt(datetime)
    --print(string.format("%2d",datetime.minutes))
    --datetimestring = string.gsub(datetimestring, "dd",string.format("%d",datetime.days))
    datetimestring = string.gsub(datetimestring, "hh",string.format("%02d",24*datetime.days+datetime.hours))
    datetimestring = string.gsub(datetimestring, "mm",string.format("%02d",datetime.minutes))
    datetimestring = string.gsub(datetimestring, "ss",string.format("%02d",datetime.seconds))
    --printyellow(datetimestring)
    return datetimestring
end



------------------------------------------------------------------------------------------------------------
--[[
--功能：获取时间段
-- 入参：type:table{ days = 1,hours = 23,minutes=59,seconds=59}
--返回值：
--      [1] seconds           type:int              时间段（n秒）
--]]
------------------------------------------------------------------------------------------------------------

local function getSeconds(params)
    local seconds = 0
    if params.days then 
        seconds = seconds + params.days * 86400
    end 
    if params.hours then 
        seconds = seconds + params.hours * 3600
    end 
    if params.minutes then 
        seconds = seconds + params.minutes * 60
    end 
    if params.seconds then 
        seconds = seconds + params.seconds 
    end 
    return seconds
end



------------------------------------------------------------------------------------------------------------
--[[
-- 功能：获取距离现在时间的时间间隔描述（X分钟/小时/天之前）
-- 入参：milisecond           type:int             格林威治绝对时间（单位毫秒）
-- 返回值：
--       [1]               type:string {X分钟前}
--]]
------------------------------------------------------------------------------------------------------------

local function GetServerTime()
    return os.time() + m_SCTimeShift
end

local function GetLocalTime()
    return os.time()
end

local function TimeNow()
    return os.date("*t", os.time()+m_SCTimeShift)
end

local function PeriodFromNow(serverms)
    -- printyellow("on periodfromnow, ms =", serverms)
    if serverms == 0 then
        return LocalString.Time.Online
    end
    local period = getDateTime(os.time()+m_SCTimeShift-serverms/1000)
    -- printt(period)

    if period.days > 0 then
        return string.format(LocalString.Time.TagFromNow, period.days, LocalString.Time.Day)
    elseif period.hours > 0 then
        return string.format(LocalString.Time.TagFromNow, period.hours, LocalString.Time.Hour)
    elseif period.minutes > 0 then
        return string.format(LocalString.Time.TagFromNow, period.minutes, LocalString.Time.Min)
    elseif period.seconds > 0 then
        return string.format(LocalString.Time.TagFromNow, period.seconds, LocalString.Time.Sec)
    end
end

local function TimeStr(serverms)
    return os.date("%c", serverms/1000)
end

local function init()

end

return {
    init                = init,
    getTime             = getTime,
    getDate             = getDate,
    getDateTime         = getDateTime,
    getDateTimeString   = getDateTimeString,
    getSeconds          = getSeconds,
    PeriodFromNow       = PeriodFromNow,
    TimeStr             = TimeStr,
	TimeNow				= TimeNow,
    GetServerTime       = GetServerTime,
	GetLocalTime = GetLocalTime,
}

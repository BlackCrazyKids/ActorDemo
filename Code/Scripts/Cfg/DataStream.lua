local tonumber = tonumber;
local gsub = string.gsub;
local lower = string.lower;
local error = error;
local setmetatable = setmetatable;
local tostring = tostring;
local lines = io.lines;

local Stream = {};
Stream.__index = Stream;

function Stream.new(file)
	local o = {};
	setmetatable(o, Stream)
	o.dataIter = lines(file);
	return o
end

function Stream:Close()
	while self.dataIter() do
	end
end

function Stream:GetNext()
	return self.dataIter()
end

function Stream:GetBool()
	local next = self:GetNext();
	if next == "true" then
		return true
	elseif next == "false" then
		return false
	else
		error(tostring(next) .. " isn't bool! ")
	end
end

function Stream:GetInt()
	local next = self:GetNext();
	return tonumber(next)
end

function Stream:GetLong()
	local next = self:GetNext();
	return tonumber(next)
end

function Stream:GetFloat()
	local next = self:GetNext();
	return tonumber(next)
end

function Stream:GetString()
	local next = self:GetNext();
	return next
end

function Stream:GetList(type)
	local result = {};
	local method = self['Get' .. type];
	local length = self:GetInt();
	for i = 1, length do
		result[i] = method(self);
	end
	return result
end

function Stream:GetDict(key,value)
	local result = {};
	local optKey = self['Get' .. key];
	local optValue = self['Get' .. value];
	local length = self:GetInt();
	for i = 1, length do
		result[optKey(self)] = optValue(self);
	end
	return result
end

return Stream

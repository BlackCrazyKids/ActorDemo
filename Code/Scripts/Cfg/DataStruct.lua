local Stream = require("Cfg.DataStream")
local GetOrCreate = Util.GetOrCreate

local meta
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.AllClass'
meta.ConstString = 'Hello World'
meta.ConstFloat = 3.141527
GetOrCreate('Cfg.AllType')['AllClass'] = meta
function Stream:GetCfgAllTypeAllClass()
	local o = {}
	setmetatable(o, Cfg.AllType.AllClass)
	o.ID = self:GetInt()
	o.Index = self:GetInt()
	o.VarLong = self:GetLong()
	o.VarFloat = self:GetFloat()
	o.VarString = self:GetString()
	o.VarBool = self:GetBool()
	o.VarEnum = self:GetInt()
	o.VarClass = self:GetCfgAllTypeSingleClassMaker()
	o.VarListBase = self:GetList('String')
	o.VarListClass = self:GetList('CfgAllTypeSingleClassMaker')
	o.VarListCardElem = self:GetList('String')
	o.VarDictBase = self:GetDict('Int', 'Float')
	o.VarDictEnum = self:GetDict('Long', 'Int')
	o.VarDictClass = self:GetDict('String', 'CfgAllTypeSingleClassMaker')
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.SingleClass'
GetOrCreate('Cfg.AllType')['SingleClass'] = meta
function Stream:GetCfgAllTypeSingleClassMaker()
	return self['Get' .. self:GetString():gsub('%.', '')](self)
end
function Stream:GetCfgAllTypeSingleClass()
	local o = {}
	setmetatable(o, Cfg.AllType.SingleClass)
	o.Var1 = self:GetString()
	o.Var2 = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.M1'
GetOrCreate('Cfg.AllType')['M1'] = meta
function Stream:GetCfgAllTypeM1()
	local o = self:GetCfgAllTypeSingleClass()
	setmetatable(o, Cfg.AllType.M1)
	o.V3 = self:GetLong()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.M2'
GetOrCreate('Cfg.AllType')['M2'] = meta
function Stream:GetCfgAllTypeM2()
	local o = self:GetCfgAllTypeSingleClass()
	setmetatable(o, Cfg.AllType.M2)
	o.V4 = self:GetBool()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.AllType.Test'
GetOrCreate('Cfg.AllType')['Test'] = meta
function Stream:GetCfgAllTypeTest()
	local o = {}
	setmetatable(o, Cfg.AllType.Test)
	o.TID = self:GetInt()
	o.Name = self:GetString()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Vector2'
GetOrCreate('Cfg')['Vector2'] = meta
function Stream:GetCfgVector2()
	local o = {}
	setmetatable(o, Cfg.Vector2)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	return o
end
meta= {}
meta.__index = meta
meta.class = 'Cfg.Vector3'
GetOrCreate('Cfg')['Vector3'] = meta
function Stream:GetCfgVector3()
	local o = {}
	setmetatable(o, Cfg.Vector3)
	o.X = self:GetFloat()
	o.Y = self:GetFloat()
	o.Z = self:GetFloat()
	return o
end
GetOrCreate('Cfg.AllType')['CardElement'] = {
	NULL = -9,
	Attack = 0,
	Extract = 1,
	Renounce = 2,
	Armor = 3,
	Control = 4,
	Cure = 5,
	Oneself = 6,
	Hand = 7,
	Brary = 8,
	Handack = 9,
}
return Stream

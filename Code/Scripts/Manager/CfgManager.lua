local format = string.format
local ipairs = ipairs
local Stream = require("Cfg.DataStruct")
local CSUtil = CSUtil
local allCfgs = {}

local function LoadCsv(relPath, method, index)
    local path = format('%sconfig/csv/%s', CSUtil.DataPath, relPath)
    local data = Stream.new(path)
    local cfg = {}
    for i = 1, data:GetInt() do
        local value = data[method](data)
        local key = value[index]
        cfg[key] = value
    end
    return cfg
end

local function Init()
    --Memory.snapshot()
    --collectgarbage("collect")
    --Memory.m_cMethods.DumpMemorySnapshot("../", "1-Before", -1)
    --LuaHelper.BeginSample("--LoadConfig--")
    local cfgs = require "Cfg.Config"
    for _, s in ipairs(cfgs) do
        allCfgs[s.name] = LoadCsv(s.output, s.method, s.index)
    end
    --LuaHelper.EndSample()
    --collectgarbage("collect")
    --Memory.m_cMethods.DumpMemorySnapshot("../", "2-After", -1)
    --Memory.m_cMethods.DumpMemorySnapshotComparedFile("./", "Compared", -1, "../LuaMemRefInfo-All-[1-Before].txt", "../LuaMemRefInfo-All-[2-After].txt")
    --print("--LoadConfig--")
end

local function GetConfig(name)
    if allCfgs then
        return allCfgs[name]
    end
    return nil
end
local function GetConfigByIndex(name, index)
    return allCfgs[name][index]
end

---@class CfgManager
local CfgManager = {
    Init = Init,
    GetConfig = GetConfig,
    GetConfigByIndex = GetConfigByIndex,
}
return CfgManager
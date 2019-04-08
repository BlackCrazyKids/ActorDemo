---@type string
local name
---@type UI.
local fields
---@type Game.LuaWindow
local window
local UIMgr = require('Manager.UIManager')
local itemHeadBar
local loadType = Define.ResourceLoadType.LoadBundleFromFile
local fileName = ""

local function LoadA()
    ResMgr:AddTask(fileName, function(obj)

    end, loadType)
end

local function LoadB()
    ResMgr:AddTask(fileName, function(obj)

    end, loadType)
end

local function Show(param)

end
local function Refresh(param)

end
local function Hide()

end
local function Destroy()

end
local function Init(param)
    name, window, fields = unpack(param)
    itemHeadBar = UIMgr.GetPkgItem(name, "ItemHeadBar")

end

return {
    Init = Init,
    Show = Show,
    Refresh = Refresh,
    Hide = Hide,
    Destroy = Destroy,
}
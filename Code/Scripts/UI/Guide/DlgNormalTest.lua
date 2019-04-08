---@type string
local name
---@type UI.Guide.DlgNormalTest
local fields
local window
local UIMgr = require('Manager.UIManager')

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
    fields.Button_Start.onClick:Add(function()
        UIMgr.Show('Guide.DlgGuideLayer', { target = fields.Button_Bag })
    end)
    fields.Button_Bag.onClick:Add(function()
        UIMgr.Hide('Guide.DlgGuideLayer')
    end)
end

return {
    Init = Init,
    Show = Show,
    Refresh = Refresh,
    Hide = Hide,
    Destroy = Destroy,
}
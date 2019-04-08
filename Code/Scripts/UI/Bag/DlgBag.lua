local tostring = tostring

local name
local fields
local window

local function Show(param)

end
---@param context FairyGUI.EventContext
local function OnItemClick(context)
    if not context.inputEvent.isDoubleClick then
        return
    end

    ---@type FairyGUI.GObject
    local item = context.sender
    ---@type FairyGUI.GLabel
    local selectedName = fields.Label_Name
    ---@type FairyGUI.GLoader
    local selectedItem = fields.Loader_Selected
    selectedName.text = 'No.' .. item.text
    selectedItem.url = item.icon
end
local function Refresh(param)
    ---@type FairyGUI.GList
    local bagGrid = fields.List_BagGrid
    for i = 1, bagGrid.numItems do
        local item = bagGrid:GetChildAt(i - 1)
        item.text = tostring(i)
        item.onClick:Add(OnItemClick)
    end
end
local function Hide()

end
local function Destroy()

end
local function Init(param)
    name, window, fields = unpack(param)
end

return {
    Init = Init,
    Show = Show,
    Refresh = Refresh,
    Hide = Hide,
    Destroy = Destroy,
}
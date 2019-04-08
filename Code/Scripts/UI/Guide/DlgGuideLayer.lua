---@type string
local name
---@type UI.Guide.DlgGuideLayer
local fields
---@type Game.LuaWindow
local window
---@type FairyGUI.GRoot
local GRoot = GRoot
local contentPane

local function Show(param)
    ---@type FairyGUI.GObject
    local target = param.target
    local rect = target:TransformRect(Rect(0, 0, target.width, target.height), contentPane);

    fields.Graph_Window.size = Vector2(rect.size.x, rect.size.y)
    fields.Graph_Window:TweenMove(Vector2(rect.position.x, rect.position.y), 0.5)
end
local function Refresh(param)

end
local function Hide()

end
local function Destroy()

end
local function Init(param)
    name, window, fields = unpack(param)

    contentPane = window.contentPane
    contentPane:SetSize(GRoot.inst.width, GRoot.inst.height);
    contentPane:AddRelation(GRoot.inst, RelationType.Size);
end

return {
    Init = Init,
    Show = Show,
    Refresh = Refresh,
    Hide = Hide,
    Destroy = Destroy,
}
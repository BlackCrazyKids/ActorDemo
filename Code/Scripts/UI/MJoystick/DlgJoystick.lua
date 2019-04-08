local Joystick = require("UI.Joystick")

---@type string
local name
---@type UI.MJoystick.DlgJoystick
local fields
---@type Game.LuaWindow
local window

---@type Joystick
local joystick

local function Show(param)

end
local function Refresh(param)

end
local function Hide()

end
local function Destroy()

end

local function OnMove(x, y)
    local angle = Mathf.Atan2(y, x) * 180 / Mathf.PI + 90
    fields.TextField_Degree.text = tostring(angle)
end
local function OnMoveEnd()
    fields.TextField_Degree.text = "0"
end

local function Init(param)
    name, window, fields = unpack(param)
    joystick = Joystick:new('Joystick', 180)
    joystick.onMove = OnMove
    joystick.onMoveEnd = OnMoveEnd
    local thumb = fields.Button_Joystick:GetChild('thumb')
    joystick:Init({ joystick = fields.Button_Joystick,
                    touchArea = fields.Graph_Area,
                    center = fields.Image_Center,
                    thumb = thumb })
end

return {
    Init = Init,
    Show = Show,
    Refresh = Refresh,
    Hide = Hide,
    Destroy = Destroy,
}
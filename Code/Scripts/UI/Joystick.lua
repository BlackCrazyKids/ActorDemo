local Class = require("Common.Class")

local GRoot = GRoot
local Vector2 = Vector2
local Mathf = Mathf
local handler = handler

---@class Joystick
local Joystick = Class:new("Joystick")

local DoMoveStart = nil
local DoMove = nil
local DoMoveEnd = nil

function Joystick:ctor(name, radius, speed)
    self.touchId = -1
    self.name = name or ""
    self.radius = radius or 150
    self.speed = speed or 10

    self.lastPos = Vector2.zero
    self.centerPos = Vector2.zero
    self.startPos = Vector2.zero
    self.initPos = Vector2.zero

    ---@type FairyGUI.GTween
    self.tweener = nil
    self.onMoveStart = nil
    self.onMove = nil
    self.onMoveEnd = nil
end
function Joystick:Init(params)
    ---@type FairyGUI.GButton
    self.button = params.joystick
    self.button.changeStateOnClick = false
    ---@type FairyGUI.GObject
    self.touchArea = params.touchArea
    ---@type FairyGUI.GObject
    self.center = params.center
    ---@type FairyGUI.GObject
    self.thumb = params.thumb

    self.initPos.x = self.center.x + self.center.width / 2
    self.initPos.y = self.center.y + self.center.height / 2

    ---@param context FairyGUI.EventContext
    self.touchArea.onTouchBegin:Add(handler(self, function(obj, context)
        DoMoveStart(obj, context)
        context:CaptureTouch()
    end))
    ---@param context FairyGUI.EventContext
    self.touchArea.onTouchMove:Add(handler(self, function(obj, context)
        DoMove(obj, context)
    end))
    ---@param context FairyGUI.EventContext
    self.touchArea.onTouchEnd:Add(handler(self, function(obj, context)
        DoMoveEnd(obj, context)
    end))
end

DoMoveStart = function(joystick, context)
    if joystick.touchId == -1 then
        ---@type FairyGUI.InputEvent
        local evt = context.data
        joystick.touchId = evt.touchId
        if joystick.tweener then
            joystick.tweener:Kill()
            joystick.tweener = nil
        end
        ---@type FairyGUI.GButton
        local button = joystick.button
        local center = joystick.center

        local pointer = GRoot.inst:GlobalToLocal(Vector2(evt.x, evt.y))
        local x = pointer.x
        local y = pointer.y
        button.selected = true

        if x < 0 then
            x = 0
        elseif x > joystick.touchArea.width then
            x = joystick.touchArea.width
        end
        if y < joystick.touchArea.y then
            y = joystick.touchArea.y
        elseif y > GRoot.inst.height then
            y = GRoot.inst.height
        end

        joystick.lastPos.x = x
        joystick.lastPos.y = y
        joystick.startPos.x = x
        joystick.startPos.y = y

        center.visible = true
        center:SetXY(x - center.width / 2, y - center.height / 2)
        button:SetXY(x - button.width / 2, y - button.height / 2)

        local dtx = x - joystick.initPos.x
        local dty = y - joystick.initPos.y
        local degrees = Mathf.Atan2(dty, dtx) * 180 / Mathf.PI
        joystick.thumb.rotation = degrees + 90

        if joystick.onMoveStart then
            joystick.onMoveStart()
        end
    end
end
DoMove = function(joystick, context)
    local evt = context.data
    if joystick.touchId ~= -1 and evt.touchId == joystick.touchId then
        local lastPos = joystick.lastPos
        local startPos = joystick.startPos
        local button = joystick.button
        local thumb = joystick.thumb

        local pointer = GRoot.inst:GlobalToLocal(Vector2(evt.x, evt.y))
        local x = pointer.x
        local y = pointer.y
        local movex = x - lastPos.x
        local movey = y - lastPos.y
        lastPos.x = x
        lastPos.y = y
        local buttonX = button.x + movex
        local buttonY = button.y + movey

        local offsetX = buttonX + button.width / 2 - startPos.x
        local offsetY = buttonY + button.height / 2 - startPos.y
        local rad = Mathf.Atan2(offsetY, offsetX)
        local degrees = rad * 180 / Mathf.PI
        thumb.rotation = degrees + 90

        local maxX = joystick.radius * Mathf.Cos(rad)
        local maxY = joystick.radius * Mathf.Sin(rad)
        if Mathf.Abs(offsetX) > Mathf.Abs(maxX) then
            offsetX = maxX
        end
        if Mathf.Abs(offsetY) > Mathf.Abs(maxY) then
            offsetY = maxY
        end
        buttonX = startPos.x + offsetX
        buttonY = startPos.y + offsetY
        if buttonY < 0 then
            buttonY = 0
        end
        if buttonX > GRoot.inst.height then
            buttonX = GRoot.inst.height
        end
        button:SetXY(buttonX - button.width / 2, buttonY - button.height / 2)

        if joystick.onMove then
            joystick.onMove(offsetX, offsetY)
        end
    end
end
DoMoveEnd = function(joystick, context)
    local evt = context.data
    if joystick.touchId ~= -1 and evt.touchId == joystick.touchId then
        local thumb = joystick.thumb
        local button = joystick.button
        local center = joystick.center

        joystick.touchId = -1
        thumb.rotation = thumb.rotation + 180
        center.visible = false
        local dst = joystick.initPos - Vector2(button.width / 2, button.height / 2)
        joystick.tweener = button:TweenMove(dst, 0.3):OnComplete(function()
            joystick.tweener = nil
            button.selected = false
            thumb.rotation = 0
            center.visible = true
            center:SetXY(joystick.initPos.x - center.width / 2, joystick.initPos.y - center.height / 2)

            if joystick.onMoveEnd then
                joystick.onMoveEnd()
            end
        end)
    end
end

return Joystick

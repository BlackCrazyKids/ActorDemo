--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
local setmetatable = setmetatable
local CoUpdateBeat = CoUpdateBeat
local GameEvent = GameEvent
local Class = Class
local Time = Time
local handler = handler

Timer = Class:new("Timer")

local Timer = Timer

--unscaled false 采用deltaTime计时，true 采用 unscaledDeltaTime计时
function Timer:ctor(func, duration, loop, unscaled)
    self.func = func
    self.duration = duration
    self.time = duration
    self.loop = loop or 1
    self.unscaled = unscaled or false and true
    self.running = false
end

function Timer:Start()
    self.running = true
    if not self.dispose then
        self.dispose = GameEvent.UpdateEvent:Add(handler(self, self.Update))
    end
end

function Timer:Reset(func, duration, loop, unscaled)
    self.duration = duration
    self.loop = loop or 1
    self.unscaled = unscaled
    self.func = func
    self.time = duration
end

function Timer:Stop()
    self.running = false

    if self.dispose then
        self.dispose()
    end
end

function Timer:Update()
    if not self.running then
        return
    end

    local delta = self.unscaled and Time.unscaledDeltaTime or Time.deltaTime
    self.time = self.time - delta

    if self.time <= 0 then
        self:func()

        if self.loop > 0 then
            self.loop = self.loop - 1
            self.time = self.time + self.duration
        end

        if self.loop == 0 then
            self:Stop()
        elseif self.loop < 0 then
            self.time = self.time + self.duration
        end
    end
end

--帧计数timer
FrameTimer = Class:new("FrameTimer")

local FrameTimer = FrameTimer

function FrameTimer:ctor(func, count, loop)
    self.func = func
    self.loop =  loop or 1
    self.duration = count
    self.count = Time.frameCount + count
    self.running = false
end

function FrameTimer:Reset(func, count, loop)
    self.func = func
    self.duration = count
    self.loop = loop
    self.count = Time.frameCount + count
end

function FrameTimer:Start()
    self.running = true
    if not self.dispose then
        self.dispose = GameEvent.LateUpdateEvent:Add(handler(self, self.Update))
    end
end

function FrameTimer:Stop()
    self.running = false

    if self.dispose then
        self.dispose()
    end
end

function FrameTimer:Update()
    if not self.running then
        return
    end

    if Time.frameCount >= self.count then
        self:func()

        if self.loop > 0 then
            self.loop = self.loop - 1
        end

        if self.loop == 0 then
            self:Stop()
        else
            self.count = Time.frameCount + self.duration
        end
    end
end
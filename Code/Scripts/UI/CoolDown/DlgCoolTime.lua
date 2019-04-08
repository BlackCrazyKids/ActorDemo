local Timer = Timer

local name
local fields
local window

local time = 0
local coolTime = 6
local timer

local function Show(param)

end
local function Refresh(param)

end
local function Hide()

end
local function Destroy()
    timer = nil
end

local function HandleCoolTime(timer)
    time = time - timer.duration
    fields.Image_maskCircle.fillAmount = time / coolTime
    fields.Image_maskRect.fillAmount = time / coolTime
    fields.Label_CoolTime.text = math.ceil(time)
    print(time, Time.realtimeSinceStartup)
    if time < 0 then
        time = coolTime
    end
end
local function Init(param)
    name, window, fields = unpack(param)
    timer = Timer:new(HandleCoolTime, 0.05, -1)
    timer:Start()
    time = coolTime
end

return {
    Init = Init,
    Show = Show,
    Refresh = Refresh,
    Hide = Hide,
    Destroy = Destroy,
    Update = Update,
}
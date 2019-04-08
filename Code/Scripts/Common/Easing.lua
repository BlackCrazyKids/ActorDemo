local Mathf = Mathf

local function Linear(from, to, t)
    to = to - from
    return to * t + from
end

local function Clerp(from, to, t)
    local min = 0
    local max = 360
    local half = Mathf.Abs((max - min) * 0.5)
    local retval = 0
    local diff = 0
    if (from - to) < -half then
        diff = (max - from + to) * t
        retval = from + diff
    elseif (to - from) > half then
        diff = -((max - to) + from) * t
        retval = from + diff
    else
        retval = from + (to - from) * t
    end
    return retval
end

local function Spring(from, to, t)
    t = Mathf.Clamp01(t)
    t = (Mathf.Sin(t * Mathf.PI * (0.2 + 2.5 * t * t * t)) * Mathf.Pow(1 - t, 2.2) + t) * (1 + (1.2 * (1 - t)))
    return from + (to - from) * t
end

local function EaseInQuad(from, to, t)
    to = to - from
    return to * t * t + from
end

local function EaseOutQuad(from, to, t)
    to = to - from
    return -to * t * (t - 2) + from
end

local function EaseInOutQuad(from, to, t)
    t = t / 0.5
    to = to - from
    if t < 1 then
        return to * 0.5 * t * t + from
    end
    t = t - 1
    return -to * 0.5 * (t * (t - 2) - 1) + from
end

local function EaseInCubic(from, to, t)
    to = to - from
    return to * t * t * t + from
end

local function EaseOutCubic(from, to, t)
    t = t - 1
    to = to - from
    return to * (t * t * t + 1) + from
end

local function EaseInOutCubic(from, to, t)
    t = t / 0.5
    to = to - from
    if t < 1 then
        return to * 0.5 * t * t * t + from
    end
    t = t - 2
    return to * 0.5 * (t * t * t + 2) + from
end

local function EaseInQuart(from, to, t)
    to = to - from
    return to * t * t * t * t + from
end

local function EaseOutQuart(from, to, t)
    t = t - 1
    to = to - from
    return -to * (t * t * t * t - 1) + from
end

local function EaseInOutQuart(from, to, t)
    t = t / 0.5
    to = to - from
    if t < 1 then
        return to * 0.5 * t * t * t * t + from
    end
    t = t - 2
    return -to * 0.5 * (t * t * t * t - 2) + from
end

local function EaseInQuint(from, to, t)
    to = to - from
    return to * t * t * t * t * t + from
end

local function EaseOutQuint(from, to, t)
    t = t - 1
    to = to - from
    return to * (t * t * t * t * t + 1) + from
end

local function EaseInOutQuint(from, to, t)
    t = t / 0.5
    to = to - from
    if t < 1 then
        return to * 0.5 * t * t * t * t * t + from
    end
    t = t - 2
    return to * 0.5 * (t * t * t * t * t + 2) + from
end

local function EaseInSine(from, to, t)
    to = to - from
    return -to * Mathf.Cos(t * (Mathf.PI * 0.5)) + to + from
end

local function EaseOutSine(from, to, t)
    to = to - from
    return to * Mathf.Sin(t * (Mathf.PI * 0.5)) + from
end

local function EaseInOutSine(from, to, t)
    to = to - from
    return -to * 0.5 * (Mathf.Cos(Mathf.PI * t) - 1) + from
end

local function EaseInExpo(from, to, t)
    to = to - from
    return to * Mathf.Pow(2, 10 * (t - 1)) + from
end

local function EaseOutExpo(from, to, t)
    to = to - from
    return to * (-Mathf.Pow(2, -10 * t) + 1) + from
end

local function EaseInOutExpo(from, to, t)
    t = t / 0.5
    to = to - from
    if t < 1 then
        return to * 0.5 * Mathf.Pow(2, 10 * (t - 1)) + from
    end
    t = t - 1
    return to * 0.5 * (-Mathf.Pow(2, -10 * t) + 2) + from
end

local function EaseInCirc(from, to, t)
    to = to - from
    return -to * (Mathf.Sqrt(1 - t * t) - 1) + from
end

local function EaseOutCirc(from, to, t)
    t = t - 1
    to = to - from
    return to * Mathf.Sqrt(1 - t * t) + from
end

local function EaseInOutCirc(from, to, t)
    t = t / 0.5
    to = to - from
    if t < 1 then
        return -to * 0.5 * (Mathf.Sqrt(1 - t * t) - 1) + from
    end
    t = t - 2
    return to * 0.5 * (Mathf.Sqrt(1 - t * t) + 1) + from
end

local function EaseOutBounce(from, to, t)
    t = t / 1
    to = to - from
    if t < (1 / 2.75) then
        return to * (7.5625 * t * t) + from
    elseif t < (2 / 2.75) then
        t = t - 1.5 / 2.75
        return to * (7.5625 * (t) * t + 0.75) + from
    elseif t < (2.5 / 2.75) then
        t = t - 2.25 / 2.75
        return to * (7.5625 * t * t + 0.9375) + from
    else
        t = t - 2.625 / 2.75
        return to * (7.5625 * t * t + 0.984375) + from
    end
end

local function EaseInBounce(from, to, t)
    to = to - from
    local d = 1
    return to - EaseOutBounce(0, to, d - t) + from
end

local function EaseInOutBounce(from, to, t)
    to = to - from
    local d = 1
    if t < (d * 0.5) then
        return EaseInBounce(0, to, t * 2) * 0.5 + from
    else
        return EaseOutBounce(0, to, t * 2 - d) * 0.5 + to * 0.5 + from
    end
end

local function EaseInBack(from, to, t)
    to = to - from
    t = t / 1
    local s = 1.70158
    return to * t * t * ((s + 1) * t - s) + from
end

local function EaseOutBack(from, to, t)
    local s = 1.70158
    to = to - from
    t = t - 1
    return to * (t * t * ((s + 1) * t + s) + 1) + from
end

local function EaseInOutBack(from, to, t)
    local s = 1.70158
    to = to - from
    t = t / 0.5
    if t < 1 then
        s = s * 1.525
        return to * 0.5 * (t * t * ((s + 1) * t - s)) + from
    end
    t = t - 2
    s = s * 1.525
    return to * 0.5 * ((t) * t * (((s) + 1) * t + s) + 2) + from
end

local function EaseInElastic(from, to, t)
    to = to - from

    local d = 1
    local p = d * 0.3
    local s = 0
    local a = 0

    if (t == 0) then
        return from
    end
    t = t / d
    if t == 1 then
        return from + to
    end

    if a == 0 or a < Mathf.Abs(to) then
        a = to
        s = p / 4
    else
        s = p / (2 * Mathf.PI) * Mathf.Asin(to / a)
    end
    t = t - 1
    return -(a * Mathf.Pow(2, 10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + from
end

local function EaseOutElastic(from, to, t)
    to = to - from

    local d = 1
    local p = d * 0.3
    local s = 0
    local a = 0

    if t == 0 then
        return from
    end
    t = t / d
    if t == 1 then
        return from + to
    end

    if a == 0 or a < Mathf.Abs(to) then
        a = to
        s = p * 0.25
    else
        s = p / (2 * Mathf.PI) * Mathf.Asin(to / a)
    end
    return (a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + to + from)
end

local function EaseInOutElastic(from, to, t)
    to = to - from

    local d = 1
    local p = d * 0.3
    local s = 0
    local a = 0

    if t == 0 then
        return from
    end
    t = t / d * 0.5
    if t == 2 then
        return from + to
    end

    if (a == 0 or a < Mathf.Abs(to)) then
        a = to
        s = p / 4
    else
        s = p / (2 * Mathf.PI) * Mathf.Asin(to / a)
    end

    if (t < 1) then
        t = t - 1
        return -0.5 * (a * Mathf.Pow(2, 10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + from
    end
    t = t - 1
    return a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * 0.5 + to + from
end

local function Punch(amplitude, t)
    local s = 9
    if t == 0 or t == 1 then
        return 0
    end
    local period = 1 * 0.3
    s = period / (2 * Mathf.PI) * Mathf.Asin(0)
    return amplitude * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 1 - s) * (2 * Mathf.PI) / period)
end

return {
    Linear = Linear,
    Clerp = Clerp,
    Spring = Spring,
    EaseInQuad = EaseInQuad,
    EaseOutQuad = EaseOutQuad,
    EaseInOutQuad = EaseInOutQuad,
    EaseInCubic = EaseInCubic,
    EaseOutCubic = EaseOutCubic,
    EaseInOutCubic = EaseInOutCubic,
    EaseInQuart = EaseInQuart,
    EaseOutQuart = EaseOutQuart,
    EaseInOutQuart = EaseInOutQuart,
    EaseInQuint = EaseInQuint,
    EaseOutQuint = EaseOutQuint,
    EaseInOutQuint = EaseInOutQuint,
    EaseInSine = EaseInSine,
    EaseOutSine = EaseOutSine,
    EaseInOutSine = EaseInOutSine,
    EaseInExpo = EaseInExpo,
    EaseOutExpo = EaseOutExpo,
    EaseInOutExpo = EaseInOutExpo,
    EaseInCirc = EaseInCirc,
    EaseOutCirc = EaseOutCirc,
    EaseInOutCirc = EaseInOutCirc,
    EaseInBounce = EaseInBounce,
    EaseOutBounce = EaseOutBounce,
    EaseInOutBounce = EaseInOutBounce,
    EaseInBack = EaseInBack,
    EaseOutBack = EaseOutBack,
    EaseInOutBack = EaseInOutBack,
    EaseInElastic = EaseInElastic,
    EaseOutElastic = EaseOutElastic,
    EaseInOutElastic = EaseInOutElastic,
}
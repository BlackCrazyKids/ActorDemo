local name
local fields
local window

local function Show(param)

end
local function Refresh(param)

end
local function Hide()

end
local function Destroy()

end
local function Init(param)
    name, window, fields = table.unpack(param)
end

return {
    Init = Init,
    Show = Show,
    Refresh = Refresh,
    Hide = Hide,
    Destroy = Destroy,
}
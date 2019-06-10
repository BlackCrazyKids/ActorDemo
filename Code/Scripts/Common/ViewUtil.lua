-- 组件类型全名称可直接获取组件,如不存在则直接返回游戏对象GameObject
-- 组件名称映射table
local prefixs = {
    DTAnim = 'DOTweenAnimation',
}
-- 默认递归遍历
-- 组:子对象不遍历
local group = {Canvas = true, Dropdown = true, ScrollView = true, Group = true}
local function ConvertComp(prefix, gobj)
    local comp = nil
    if prefixs[prefix] then
        comp = gobj:GetComponent(prefixs[prefix])
    else
        comp = gobj:GetComponent(prefix) or gobj
    end
    --printyellow("ViewUtil", comp.name, comp[".name"])
    return comp
end

local function CollectExportedGameObject(transform, sets)
    local childCount = transform.childCount
    for i = 0, childCount - 1 do
        local child = transform:GetChild(i)
        local name = child.name

        local _, pos = name:find("_", 2, true)
        local isGroup = false
        if pos and pos > 1 then
            local prefix = name:sub(1, pos - 1)
            isGroup = group[prefix]
            if sets[name] then
                printyellow("warn:" .. name .. " dumplicate! skip!")
            else
                local com = ConvertComp(prefix, child)
                if com then
                    sets[name] = com
                else
                    print("invalid component type: " .. prefix)
                end
            end
        end
        if not isGroup and child.childCount and child.childCount > 0 then
            CollectExportedGameObject(child, sets)
        end
    end
end

local function ExportFields(transform)
    local fields = {}
    CollectExportedGameObject(transform, fields)
    return fields
end

return {
    ExportFields = ExportFields
}

-- 1 表示需要递归导出子GameObject的fields
-- 2 表示只导到当前GameObject,不再导出子GameObject
-- List项对象,命名约Item_*.
-- 一个界面模块一个包.
-- 直接解析指定组件,非GameObject
local prefixs = {
    Image = 1,
    MovieClip = 1,
    Graph = 1,
    Loader = 1,
    TextField = 1,
    RichTextField = 1,
    TextInput = 1,
    Label = 1,
    Button = 1,
    ProgressBar = 1,
    Slider = 1,

    ComboBox = 2,
    Group = 2,
    List = 2,
}
---@param prefix string
---@param gobj FairyGUI.GObject
local function ConvertComp(prefix, gobj)
    local comp = nil
    if prefixs[prefix] then
        comp =  gobj['as'..prefix] or gobj
    else
        comp = gobj.asCom or gobj
        print("invalid extension base: " .. prefix)
    end
    return comp
end

---@param gcomp FairyGUI.GComponent
---@param sets table<string, FairyGUI.GComponent>
local function CollectExportedGameObject(gcomp, sets)
    local children = gcomp:GetChildren()
    for i = 0, children.Length - 1 do
        local child = children[i]
        local name = child.name

        local _, pos = name:find('_', 2, true)
        local isGroup = false
        if pos and pos > 1 then
            local prefix = name:sub(1, pos - 1)
            local ftype = prefixs[prefix]
            if ftype then
                if sets[name] then
                    printyellow("warn:" .. name .. " dumplicate! skip!")
                else
                    local com = ConvertComp(prefix, child)
                    if com then
                        sets[name] = com
                    end
                    isGroup = (ftype == 2)
                end
            end
        end
        if not isGroup and child.numChildren and child.numChildren > 0 then
            CollectExportedGameObject(child, sets)
        end
    end
end

---@param gcomp FairyGUI.GComponent
local function ExportFields(gcomp)
    local fields = {}
    CollectExportedGameObject(gcomp, fields)
    return fields
end

return {
    ExportFields = ExportFields,
}


--if "Label" == prefix then
--    comp = gobj.asLabel
--elseif "Image" == prefix then
--    comp = gobj.asImage
--elseif "Button" == prefix then
--    comp = gobj.asButton
--elseif "Progress" == prefix then
--    comp = gobj.asProgress
--elseif "Slider" == prefix then
--    comp = gobj.asSlider
--elseif "TextField" == prefix then
--    comp = gobj.asTextField
--elseif "RichTextField" == prefix then
--    comp = gobj.asRichTextField
--elseif "TextInput" == prefix then
--    comp = gobj.asTextInput
--elseif "Loader" == prefix then
--    comp = gobj.asLoader
--elseif "Graph" == prefix then
--    comp = gobj.asGraph
--elseif "MovieClip" == prefix then
--    comp = gobj.asMovieClip
--elseif "ComboBox" == prefix then
--    comp = gobj.asComboBox
--elseif "Graph" == prefix then
--    comp = gobj.asGraph
--elseif "Group" == prefix then
--    comp = gobj.asGroup
--elseif "List" == prefix then
--    comp = gobj.asList

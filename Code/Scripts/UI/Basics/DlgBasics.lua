---@type ExampleBasics
local Basics = {}
---@type FairyGUI.GComponent
local mainCom = nil
---@type FairyGUI.GComponent
local container = nil
---@type FairyGUI.Controller
local ctrl = nil
---@type FairyGUI.GButton
local btnBack = nil
local dlgTable = {}
local package = "Basics"

-----------------严重问题:移除对象,未释放委托!除非指明要释放资源

function Basics:Init()
    local main = UIPackage.CreateObject(package, "Main")
    mainCom = GRoot.inst:AddChild(main).asCom
    Basics:Show(mainCom)
end

---@param context FairyGUI.EventContext
local function SetButton(context)
    ---@type FairyGUI.GButton
    local button = context.sender
    printyellow("--->>>", button.name)
    local type = button.name:sub(5)
    ---@type FairyGUI.GComponent
    local dlg = dlgTable[type]
    if not dlg then
        dlg = UIPackage.CreateObject(package, "Demo_" .. type).asCom
        dlgTable[type] = dlg
    end
    --0, -1, true
    container:RemoveChildren()
    container:AddChild(dlg)
    ctrl.selectedIndex = 1
    btnBack.visible = true

    if type == "Text" then
        local rich = dlg:GetChild("n12").asRichTextField
        rich.onClickLink:Add(function(context)
            local rich = context.sender
            rich.text = "[img]ui://Basics/pet[/img][color=#FF0000]点击链接触发事件[/color]：" .. context.data
        end)
        local input = dlg:GetChild("n22").asTextInput
        local result = dlg:GetChild("n24").asTextField
        local inputBtn = dlg:GetChild("n25").asButton
        inputBtn.onClick:Add(function()
            result.text = input.text
        end)
    elseif type == "Grid" then
        local list1 = dlg:GetChild("list1").asList
        list1:RemoveChildrenToPool()
        local ls = { "A", "B", "C", "D", "E" }
        for i = 1, #ls do
            local item = list1:AddItemFromPool().asButton
            local order = item:GetChild("t0").asTextField
            local name = item:GetChild("t1").asTextField
            local star = item:GetChild("star").asProgress

            order.text = tostring(i)
            name.text = ls[i]
            star.value = math.random(0, 3) / 3 * 100
        end
        local list2 = dlg:GetChild("list2").asList
        list2:RemoveChildrenToPool()
        ---@param context FairyGUI.EventContext
        for i = 1, #ls do
            local item = list2:AddItemFromPool().asButton
            local check = item:GetChild("cb").asButton
            check.selected = false
            local name = item:GetChild("t1").asTextField
            local movie = item:GetChild("mc").asMovieClip
            movie.playing = i % 2 == 1
            local value = item:GetChild("t3").asTextField

            name.text = ls[i]
            value.text = tostring(math.random(5, 1000))
        end
    elseif type == "ProgressBar" then
        local time = 3
        local p1 = dlg:GetChild("n1").asProgress
        p1.value = 0
        p1:TweenValue(100, time)

        local p2 = dlg:GetChild("n2").asProgress
        p2.value = 0
        p2.titleType = 1
        p2:TweenValue(100, time)

        local p4 = dlg:GetChild("n4").asProgress
        p4.value = 0
        p4:TweenValue(100, time)

        local p9 = dlg:GetChild("n9").asProgress
        p9.value = 0
        p9:TweenValue(100, time)
    elseif type == "Drag&Drop" then
        local a = dlg:GetChild('a').asButton
        a.draggable = true

        local b = dlg:GetChild('b').asButton
        b.draggable = true
        ---@param context FairyGUI.EventContext
        b.onDragStart:Add(function (context)
            context:PreventDefault()
            DragDropManager.inst:StartDrag(b, b.icon, b.icon, context.data)
        end)

        local c = dlg:GetChild('c').asButton
        c.draggable = true
        c.onDrop:Add(function (context)
            c.icon = context.data
        end)

        local d = dlg:GetChild('d').asButton
        local bound = dlg:GetChild('n7').asGraph
        ---TODO


    end
end

---@param main FairyGUI.GComponent
function Basics:Show(main)
    container = main:GetChild("container").asCom
    ctrl = main:GetController("c1")
    ctrl.selectedIndex = 0
    btnBack = main:GetChild("btn_Back")
    btnBack.visible = false
    btnBack.onClick:Add(function()
        ctrl.selectedIndex = 0
        btnBack.visible = false
    end)

    local num = main.numChildren
    for i = 1, num do
        local child = main:GetChildAt(i - 1)
        if child.group ~= nil and child.group.name == "btns" then
            child.onClick:Add(SetButton)
        end
    end
end

function Basics:OnDestroy()
    mainCom:Dispose()
    for i, v in pairs(dlgTable) do
        v:Dispose()
    end
end

return Basics
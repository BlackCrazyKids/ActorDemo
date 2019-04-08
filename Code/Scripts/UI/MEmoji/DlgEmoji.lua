---@type string
local name
---@type UI.MEmoji.DlgEmoji
local fields
---@type Game.LuaWindow
local window
local UIMgr = require('Manager.UIManager')
local ViewUtil = require("Common.ViewUtil")
local msgs = {}

local function Show(param)

end
local function Refresh(param)

end
local function Hide()

end
local function Destroy()

end
local count = 1
local function SendChatLeft()
    local msg = nil
    if count == 1 then
        msg = '你好呀!<b>嘻嘻哈哈.</b>'
    elseif count == 2 then
        msg = '你好呀!<I>嘻嘻哈哈.</I>'
    else
        msg = '你好呀!<U>嘻嘻哈哈.</U>'
    end
    table.insert(msgs, { msg = msg, isMe = false, url = 'texture/r1' })
    count = count + 1
end
local function SendChatRight(input)
    table.insert(msgs, { msg = input.text, isMe = true, url = 'texture/r0' })
end
local function ItemProvider(index)
    local node = msgs[index + 1]
    if node.isMe then
        return 'ui://MEmoji/ItemChatRight'
    else
        return 'ui://MEmoji/ItemChatLeft'
    end
end
---@param item UI.MEmoji.ItemChatLeft
local function ItemRender(index, comp)
    local node = msgs[index + 1]
    local item = ViewUtil.ExportFields(comp)
    item.Loader_icon.url = node.url
    if node.isMe then
        item.RichTextField_RMsg.text = node.msg
    else
        item.RichTextField_LMsg.text = node.msg
    end
end
local function AddMsg(input)
    if string.isempty(input.text) then
        return
    end

    SendChatRight(input)
    SendChatLeft()
    fields.List_Chats.numItems = #msgs
    input.text = ''
end

----TextField下的图文混排操作
local function Init(param)
    name, window, fields = unpack(param)
    local inputEmoji = fields.TextInput_Emoji
    local inputIOS = fields.TextInput_IOS
    local size = 55
    fields.Button_SEmoji.onClick:Add(function(context)
        ---@type FairyGUI.GList
        local pop = UIMgr.GetPkgItem(name, 'ListEmoji')
        pop.fairyBatching = true
        local list = pop:GetChild("List_Emoji").asList
        list.onClickItem:Add(function(c)
            local content = string.format("<img src=\'%s\' width=\'%d\' height=\'%d\'/>", c.data.icon, size, size)
            inputEmoji:ReplaceSelection(content)
        end)
        UIMgr.ShowPopup(pop, context.sender)
    end)
    fields.Button_SIOS.onClick:Add(function(context)
        ---@type FairyGUI.GList
        local pop = UIMgr.GetPkgItem(name, 'ListEmojiIOS')
        pop.fairyBatching = true
        local list = pop:GetChild("List_EmojiIOS")
        list.onClickItem:Add(function(c)
            --CSUtil.InputIcon(inputIOS, c.data.icon)
            local content = string.format("<img src=\'%s\' width=\'%d\' height=\'%d\'/>", c.data.icon, size, size)
            inputIOS:ReplaceSelection(content)
        end)
        UIMgr.ShowPopup(pop, context.sender)
    end)

    fields.Button_SendEmoji.onClick:Add(function(context)
        AddMsg(inputEmoji)
    end)
    fields.Button_SendIOS.onClick:Add(function(context)
        AddMsg(inputIOS)
    end)
    local list = fields.List_Chats
    CSUtil.ListItemProvider(list, ItemProvider)
    CSUtil.ListItemRenderer(list, ItemRender)
end

return {
    Init = Init,
    Show = Show,
    Refresh = Refresh,
    Hide = Hide,
    Destroy = Destroy,
}
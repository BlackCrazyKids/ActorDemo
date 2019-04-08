local ResourceLoadType = {
    Default = 0, -- 资源已加载, 直接取资源
    Persistent = 1 << 0, -- 永驻内存的资源
    Cache = 1 << 1, -- Asset需要缓存

    UnLoad = 1 << 4, -- 利用www加载并且处理后是否立即unload ab, 如不卸载, 则在指定时间后清理
    Immediate = 1 << 5, -- 需要立即加载
    -- 加载方式
    LoadBundleFromFile = 1 << 6, -- 利用AssetBundle.LoadFromFile加载
    LoadBundleFromWWW = 1 << 7, -- 利用WWW 异步加载 AssetBundle
    ReturnAssetBundle = 1 << 8, -- 返回scene AssetBundle, 默认unload ab
}

--手写枚举定义
local CtrlNames = {
    Prompt = "PromptCtrl",
    Message = "MessageCtrl"
}

local PanelNames = {
    "PromptPanel",
    "MessagePanel",
}

--协议类型--
local ProtocalType = {
    BINARY = 0,
    PB_LUA = 1,
    PBC = 2,
    SPROTO = 3,
}
--当前使用的协议类型--
local TestProtoType = ProtocalType.BINARY;

return {
    ResourceLoadType = ResourceLoadType,
    CtrlNames,
    PanelNames,
    ProtocalType,
    TestProtoType,
}


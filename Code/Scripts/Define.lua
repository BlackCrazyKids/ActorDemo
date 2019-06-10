-------------------------------------------------------
-------------------UI管理模块
-------------------------------------------------------
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

---#注:未完成功能
---#模态窗口,避免对当前窗口后面物体进行交互
---#界面返回,至上一次界面
---窗口定义UIShowType函数指明显示类型
local UIShowType = {
    Default = 1 << 0, --默认策略显示
    ReturnType = 1 << 1, --界面可回退到上一个界面[栈结构],适用于全屏窗口
    HideOther = 1 << 2, --显示界面且隐藏其他,适用于非全屏界面

    DestroyWhenHide = 1 << 3, --Hide时释放资源,默认隐藏不销毁资源,仅在资源销毁时调用
}
-- UI显示层
local UILayoutType = {
    Backgroud = 1, -- 背景层
    Normal = 2, -- 默认层
    Pupop = 3,  -- 弹窗层 模态窗口只有一层
}

-------------------------------------------------------
-------------------场景加载模块
-------------------------------------------------------
local LoadSceneType = {
    LoadSceneStart = "LoadSceneStart",
    LoadSceneEnd = "LoadSceneEnd",
}

return {
    ResourceLoadType = ResourceLoadType,
    UIShowType = UIShowType,
    UILayoutType = UILayoutType,

    LoadSceneType = LoadSceneType,
}


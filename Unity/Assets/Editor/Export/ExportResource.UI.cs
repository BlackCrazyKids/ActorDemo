using System.Collections.Generic;
using UnityEditor;

public partial class ExportResource
{
    //冗余资源:Texture,Audio,VFX:图集[RGB|A]
    //上述资源,在每次打包UI时,需要重新打包
    //Android 平台特效有待继续处理

    //-----问题
    //1.界面图集
    //2.界面描述
    //3.界面字体(艺术字和动态字体)
    //4.音频                              -另作处理
    //5.动态加载图片(大图)                -另作处理
    //6.2D特效/3D特效?麻烦!!


    //---一个FGUI包对应一个AB包

    static string guiFolder = "Assets/Interface/GUI";
    static string atlasFolder = "Assets/Interface/Atlas";
    static Dictionary<string, string> assetFonts = new Dictionary<string, string>()
    { {"Assets/Interface/Font/font.otf", "ui/atlas/font.bundle"} };//Unity资源路径 -> Bundle资源路径

    //rgb:atlas0.png        -RGB通道贴图
    //a:atlas0!a.png        -A通道贴图
    //des:fui.bytes         -UI描述文件
    static void SetAtlasPackage(string srcFolder, string dstFolder)
    {
        //---功能:图集分des和res包
        //        Dictionary<string, string> assets = new Dictionary<string, string>();
        //        GetAssetsRecursively(srcFolder, "*_fui.bytes", dstFolder, null, ref assets);
        //        GetAssetsRecursively(srcFolder, "*_atlas0.png", dstFolder, null, ref assets);
        //#if UNITY_ANDROID
        //        GetAssetsRecursively(srcFolder, "*_atlas0!a.png", dstFolder, null, ref assets);
        //#endif
        //        SetAssetBundleName(assets, new string[] { EXT_PNG, EXT_TGA }, DEPTEX_FOLDER);


        //---des和res打到同一个包中作为图集
        Dictionary<string, string> temps = new Dictionary<string, string>();
        GetAssetsRecursively(srcFolder, "*_fui.bytes", dstFolder, null, ref temps);
        Dictionary<string, string> assets = new Dictionary<string, string>();
        List<string> others = new List<string>() {
            "_atlas0.png",
#if UNITY_ANDROID
            "_atlas0!a.png"
#endif
        };
        foreach (var item in temps)
        {
            string value = item.Value.Replace("_fui.bundle", ".bundle");
            assets.Add(item.Key, value);
            for (int i = 0; i < others.Count; i++)
            {
                string key = item.Key.Replace("_fui.bytes", others[i]);
                assets.Add(key, value);
            }
        }
        SetAssetBundleName(assets);
    }
    static Dictionary<string, string> GetGUIPackage(string srcFolder, string dstFolder)
    {
        Dictionary<string, string> assets = new Dictionary<string, string>();
        GetAssetsRecursively(srcFolder, "*_fui.bytes", dstFolder, null, ref assets);
        return assets;
    }

    static void ExportSelectedUIs(BuildTarget target)
    {
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selection.Length > 0)
        {
            SetAtlasPackage(atlasFolder, "ui/atlas/");

            var assetUIs = GetGUIPackage(guiFolder, "ui/");
            var assets = GetSelectedAssets(assetUIs, selection);

            CombineAssets(new Dictionary<string, string>[] { assetFonts }, ref assets);
            assets = ModifyABName(assets);
            SetAssetBundleName(assets, new string[] { EXT_PNG, EXT_TGA }, DEPTEX_FOLDER);

            BuildAssetBundles(target);
        }
    }
    static void ExportAllUIs(BuildTarget target)
    {
        SetAtlasPackage(atlasFolder, "ui/atlas/");
        var assets = GetGUIPackage(guiFolder, "ui/");
        CombineAssets(new Dictionary<string, string>[] { assetFonts }, ref assets);
        assets = ModifyABName(assets);
        SetAssetBundleName(assets, new string[] { EXT_PNG, EXT_TGA }, DEPTEX_FOLDER);

        BuildAssetBundles(target);
    }
    static Dictionary<string, string> ModifyABName(Dictionary<string, string> assets)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (var item in assets)
        {
            string value = item.Value.Replace("_fui.bundle", ".bundle");
            dict.Add(item.Key, value);
        }
        return dict;
    }
    static void CombineAssets(Dictionary<string, string>[] dics, ref Dictionary<string, string> assets)
    {
        for (int i = 0; i < dics.Length; ++i)
        {
            Dictionary<string, string> dic = dics[i];
            foreach (KeyValuePair<string, string> pair in dic)
            {
                assets[pair.Key] = pair.Value;
            }
        }
    }


    [MenuItem("Assets/Build Selected for Windows/UI")]
    static void ExportSelectedUIsForWindows()
    {
        ExportSelectedUIs(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for Android/UI")]
    static void ExportSelectedUIsForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportSelectedUIs(BuildTarget.Android);
    }
    [MenuItem("Assets/Build Selected for iOS/UI")]
    static void ExportSelectedUIsForiOS()
    {
        ExportSelectedUIs(BuildTarget.iOS);
    }
    [MenuItem("Build Windows/Build UIs for Windows")]
    static void ExportAllUIsForWindows()
    {
        ExportAllUIs(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Build Android/Build UIs for Android")]
    static void ExportAllUIsForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportAllUIs(BuildTarget.Android);
    }
    [MenuItem("Build iOS/Build UIs for iOS")]
    static void ExportAllUIsForiOS()
    {
        ExportAllUIs(BuildTarget.iOS);
    }
}

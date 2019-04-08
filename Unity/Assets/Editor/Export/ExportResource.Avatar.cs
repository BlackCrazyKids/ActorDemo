using System.Collections.Generic;
using UnityEditor;

public partial class ExportResource
{
    static Dictionary<string, string> assetAvatars = new Dictionary<string, string>();

    static void GetAvatarAssets()
    {
        assetAvatars.Clear();
        GetAssetsRecursively("Assets/Avatar", "*.prefab", "avatar/", "ar", ref assetAvatars);
    }

    static void ExportSelectedAvatars(BuildTarget target)
    {
        GetAvatarAssets();
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selection.Length > 0)
        {
            var assets = GetSelectedAssets(assetAvatars, selection);
            SetAssetBundleName(assets, new string[] { EXT_SHADER }, SHADER_FOLDER);
            SetAssetBundleName(assets);
            BuildAssetBundles(target);
        }
    }

    static void ExportAllAvatars(BuildTarget target)
    {
        GetAvatarAssets();
        SetAssetBundleName(assetAvatars, new string[] { EXT_SHADER }, SHADER_FOLDER);
        SetAssetBundleName(assetAvatars);
        BuildAssetBundles(target);
    }

    [MenuItem("Assets/Build Selected for Windows/Avatar")]
    static void ExportSelectedAvatarsForWindows()
    {
        ExportSelectedAvatars(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for Android/Avatar")]
    static void ExportSelectedAvatarsForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportSelectedAvatars(BuildTarget.Android);
    }
    [MenuItem("Assets/Build Selected for iOS/Avatar")]
    static void ExportSelectedAvatarsForiOS()
    {
        ExportSelectedAvatars(BuildTarget.iOS);
    }

    [MenuItem("Build Windows/Build Avatars for Windows")]
    static void ExportAllAvatarsForWindows()
    {
        ExportAllAvatars(BuildTarget.StandaloneWindows);
    }


    [MenuItem("Build Android/Build Avatars for Android")]
    static void ExportAllAvatarsForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportAllAvatars(BuildTarget.Android);
    }


    [MenuItem("Build iOS/Build Avatars for iOS")]
    static void ExportAllAvatarsForiOS()
    {
        ExportAllAvatars(BuildTarget.iOS);
    }
}

using GameEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public partial class ExportResource
{
    //特效图集在Android平台需要分离A通道
    //TODO

    static Dictionary<string, string> assetScenes = new Dictionary<string, string>();
    static List<string> excludeScenes = new List<string>() { "ResourcesUpdate", "TEST" };
    static void GetSceneAssets()
    {
        assetScenes.Clear();
        GetAssetsRecursively("Assets/Environment/Scenes/", "*.unity", "scene/", "s", ref assetScenes);
        for (int i = 0; i < excludeScenes.Count; i++)
        {
            string path = EUtil.StandardlizePath("Assets/Environment/Scenes/" + excludeScenes[i] + ".unity");
            if (assetScenes.ContainsKey(path))
                assetScenes.Remove(path);
        }
    }

    static void ExportAllScenes(BuildTarget target)
    {
        GetSceneAssets();
        SetAssetBundleName(assetScenes, new string[] { EXT_PNG, EXT_TGA }, DEPTEX_FOLDER);
        SetAssetBundleName(assetScenes, new string[] { EXT_MP3, EXT_OGG }, AUDIO_FOLDER);
        SetAssetBundleName(assetScenes, new string[] { EXT_SHADER }, SHADER_FOLDER);
        BuildAssetBundles(target);
    }

    static void ExportSelectedScenes(BuildTarget target)
    {
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selection.Length > 0)
        {
            GetSceneAssets();
            var assets = GetSelectedAssets(assetScenes, selection);
            SetAssetBundleName(assets, new string[] { EXT_PNG, EXT_TGA }, DEPTEX_FOLDER);
            SetAssetBundleName(assets, new string[] { EXT_MP3, EXT_OGG }, AUDIO_FOLDER);
            SetAssetBundleName(assets, new string[] { EXT_SHADER }, SHADER_FOLDER);
            BuildAssetBundles(target);
        }
    }



    [MenuItem("Assets/Build Selected for Windows/Scene")]
    static void ExportSceneForWindows()
    {
        ExportSelectedScenes(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for iOS/Scene")]
    static void ExportSceneForiOS()
    {
        ExportSelectedScenes(BuildTarget.iOS);
    }
    [MenuItem("Assets/Build Selected for Android/Scene")]
    static void ExportSceneForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportSelectedScenes(BuildTarget.Android);
    }
    [MenuItem("Build Windows/Build Scenes for Windows")]
    static void ExportAllScenesForWindows()
    {
        ExportAllScenes(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Build Android/Build Scenes for Android")]
    static void ExportAllScenesForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportAllScenes(BuildTarget.Android);
    }
    [MenuItem("Build iOS/Build Scenes for iOS")]
    static void ExportAllScenesForiOS()
    {
        ExportAllScenes(BuildTarget.iOS);
    }
}

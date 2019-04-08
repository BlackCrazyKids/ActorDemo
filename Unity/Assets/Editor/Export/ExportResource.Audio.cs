using System.Collections.Generic;
using System.IO;
using UnityEditor;

public partial class ExportResource
{
    static Dictionary<string, string> assetAudios = new Dictionary<string, string>();

    static void ExportSelectedAudios(BuildTarget target)
    {
        UnityEngine.Object[] selections = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selections.Length > 0)
        {
            assetAudios.Clear();
            GetAssetsRecursively("Assets/audio/music", "*.mp3", "audio/", "a", ref assetAudios);
            GetAssetsRecursively("Assets/audio/sound", "*.ogg", "audio/", "a", ref assetAudios);
            var assets = GetSelectedAssets(assetAudios, selections);
            SetAssetBundleName(assets);
            BuildAssetBundles(target);
        }
    }

    static void ExportAllAudios(BuildTarget target)
    {
        assetAudios.Clear();
        GetAssetsRecursively("Assets/audio/music", "*.mp3", "audio/", "a", ref assetAudios);
        GetAssetsRecursively("Assets/audio/sound", "*.ogg", "audio/", "a", ref assetAudios);
        SetAssetBundleName(assetAudios);
        BuildAssetBundles(target);
    }


    [MenuItem("Assets/Build Selected for Windows/Audio")]
    static void ExportAudioForWindows()
    {
        ExportSelectedAudios(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for iOS/Audio")]
    static void ExportAudioForiOS()
    {
        ExportSelectedAudios(BuildTarget.iOS);
    }
    [MenuItem("Assets/Build Selected for Android/Audio")]
    static void ExportAudioForAndroid()
    {
        ExportSelectedAudios(BuildTarget.Android);
    }
    [MenuItem("Build Windows/Build Audios for Windows")]
    static void ExportAllAudiosForWindows()
    {
        ExportAllAudios(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Build Android/Build Audios for Android")]
    static void ExportAllAudiosForAndroid()
    {
        ExportAllAudios(BuildTarget.Android);
    }
    [MenuItem("Build iOS/Build Audios for iOS")]
    static void ExportAllAudiosForiOS()
    {
        ExportAllAudios(BuildTarget.iOS);
    }
}

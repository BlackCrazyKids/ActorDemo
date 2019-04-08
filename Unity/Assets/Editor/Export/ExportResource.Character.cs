using System.Collections.Generic;
using UnityEditor;

public partial class ExportResource
{
    static Dictionary<string, string> assetCharacters = new Dictionary<string, string>();

    static void GetCharacterAssets()
    {
        assetCharacters.Clear();
        GetAssetsRecursively("Assets/Character", "*.prefab", "character/", "c", ref assetAvatars);
    }

    static void ExportSelectedChars(BuildTarget target)
    {
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selection.Length > 0)
        {
            GetCharacterAssets();
            var assets = GetSelectedAssets(assetCharacters, selection);
            SetAssetBundleName(assets, new string[] { ".shader" }, "shaders/");
            BuildAssetBundles(target);
        }
    }
    static void ExportAllChars(BuildTarget target)
    {
        GetCharacterAssets();
        SetAssetBundleName(assetCharacters, new string[] { ".shader" }, "shaders/");
        BuildAssetBundles(target);
    }

    [MenuItem("Assets/Build Selected for Windows/Character")]
    static void ExportSelectCharacterForWindows()
    {
        ExportSelectedChars(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for Android/Character")]
    static void ExportSelectCharacterForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportSelectedChars(BuildTarget.Android);
    }
    [MenuItem("Assets/Build Selected for iOS/Character")]
    static void ExportSelectCharacterForiOS()
    {
        ExportSelectedChars(BuildTarget.iOS);
    }


    [MenuItem("Build Windows/Build Characters for Windows")]
    static void ExportAllCharactersForWindows()
    {
        ExportAllChars(BuildTarget.StandaloneWindows);
    }


    [MenuItem("Build Android/Build Characters for Android")]
    static void ExportAllCharactersForAndroid()
    {
        //ForAndroidSfxProcessBug(BuildTarget.Android);
        ExportAllChars(BuildTarget.Android);
    }

    [MenuItem("Build iOS/Build Characters for iOS")]
    static void ExportAllCharactersForiOS()
    {
        ExportAllChars(BuildTarget.iOS);
    }
}

using System.Collections.Generic;
using UnityEditor;

public partial class ExportResource
{
    static Dictionary<string, string> assetTextures = new Dictionary<string, string>();

    static void GetTexturesAssets()
    {
        assetTextures.Clear();
        GetAssetsRecursively("Assets/Texture/", "*.png", "texture/", "t", ref assetTextures);
        GetAssetsRecursively("Assets/Texture/", "*.tga", "texture/", "t", ref assetTextures);
    }

    static void ExportSelectedTextures(BuildTarget target)
    {
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selection.Length > 0)
        {
            GetTexturesAssets();
            var assets = GetSelectedAssets(assetTextures, selection);
            SetAssetBundleName(assets);
            BuildAssetBundles(target);
        }
    }

    static void ExportAllTextures(BuildTarget target)
    {
        GetTexturesAssets();
        SetAssetBundleName(assetTextures);
        BuildAssetBundles(target);
    }


    [MenuItem("Assets/Build Selected for Windows/Texture")]
    static void ExportSelectedTexturesForWindows()
    {
        ExportSelectedTextures(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for Android/Texture")]
    static void ExportSelectedTexturesForAndroid()
    {
        ExportSelectedTextures(BuildTarget.Android);
    }
    [MenuItem("Assets/Build Selected for iOS/Texture")]
    static void ExportSelectedTexturesForiOS()
    {
        ExportSelectedTextures(BuildTarget.iOS);
    }
    [MenuItem("Build Windows/Build Textures for Windows")]
    static void ExportAllTexturesForWindows()
    {
        ExportAllTextures(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Build Android/Build Textures for Android")]
    static void ExportAllTexturesForAndroid()
    {
        ExportAllTextures(BuildTarget.Android);
    }
    [MenuItem("Build iOS/Build Textures for iOS")]
    static void ExportAllTexturesForiOS()
    {
        ExportAllTextures(BuildTarget.iOS);
    }
}

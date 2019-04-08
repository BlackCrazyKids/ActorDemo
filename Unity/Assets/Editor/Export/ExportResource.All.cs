using System.Collections.Generic;
using System.IO;
using UnityEditor;

public partial class ExportResource
{
    static void ExportAllResources(BuildTarget target)
    {
        ExportAllAnimationClips(target);
        ExportAllChars(target);
        ExportAllAvatars(target);
        //ExportAllSfx(target);
        ExportAllAudios(target);
        ExportAllTextures(target);
        ExportAllUIs(target);
        ExportAllScenes(target);
    }

    static void ReExportAllResources(BuildTarget target)
    {
        RemoveAssetBundles(target);
        ResetAssetBundleNames();
        ExportAllResources(target);
    }

    static void ResetAssetBundleNames()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;

        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
    }

    static void RemoveAssetBundles(BuildTarget target)
    {
        string path = GetBundleSaveDir(target);
        foreach (string dir in Directory.GetDirectories(path))
        {
            Directory.Delete(dir, true);
        }
        foreach (string file in Directory.GetFiles(path))
        {
            File.Delete(file);
        }
    }

    [MenuItem("Build Windows/Replace Default Mat And Shaders In sfx|UI")]
    static void ReplaceDefaultMatAndShaderssForWindows()
    {
        ReplaceDefaultMatAndShaders();
    }

    [MenuItem("Build Android/Replace Default Mat And Shaders In sfx|UI")]
    static void ReplaceDefaultMatAndShadersForAndroid()
    {
        ReplaceDefaultMatAndShaders();
    }

    [MenuItem("Build iOS/Replace Default Mat And Shaders In sfx|UI")]
    static void ReplaceDefaultMatAndShadersForiOS()
    {
        ReplaceDefaultMatAndShaders();
    }

    [MenuItem("Build Windows/Build All AssetBundles for Windows")]
    static void ExportAllResourcesForWindows()
    {
        ExportAllResources(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Build Android/Build All AssetBundles for Android")]
    static void ExportAllResourcesForAndroid()
    {
        ExportAllResources(BuildTarget.Android);
    }


    [MenuItem("Build iOS/Build All AssetBundles for iOS")]
    static void ExportAllResourcesForiOS()
    {
        ExportAllResources(BuildTarget.iOS);
    }

    [MenuItem("Build Windows/ReBuild All AssetBundles for Windows")]
    static void ReExportAllResourcesForWindows()
    {
        ReExportAllResources(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Build Android/ReBuild All AssetBundles for Android")]
    static void ReExportAllResourcesForAndroid()
    {
        ReExportAllResources(BuildTarget.Android);
    }


    [MenuItem("Build iOS/ReBuild All AssetBundles for iOS")]
    static void ReExportAllResourcesForiOS()
    {
        ReExportAllResources(BuildTarget.iOS);
    }

    [MenuItem("Build Windows/Clean AssetBundle Names")]
    static void CleanAllAssetBundleNames()
    {
        ResetAssetBundleNames();
    }

    [MenuItem("Tools/Clear ProgressBar")]
    static void ClearProgressBar()
    {
        EditorUtility.ClearProgressBar();
    }
}

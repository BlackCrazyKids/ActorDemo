using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public partial class ExportResource
{
    //需要角色动作配置器
    //依据配置器来决定需要打包哪些Clip

    static Dictionary<string, string> assetAnimationClips = new Dictionary<string, string>();

    //static void AddAssetAnimationClip(SkillConfig.ActionHost host, ModelActionBase action, string animname)
    //{
    //    var clippath = ActionSettings.GetClipPath(host, action, animname);
    //    if (!clippath.IsNullOrEmpty() && !assetAnimationClips.ContainsKey(clippath))
    //    {
    //        assetAnimationClips[clippath] = ActionSettings.GetAnimationClipBundleName(host, action, animname);
    //    }
    //}

    //static void GetAnimationClipAssets()
    //{
    //    EditorUtility.DisplayProgressBar("Init Animator Config", "Please wait ...", 0.3f);
    //    ActionSettings.ReLoadAll();
    //    EditorUtility.DisplayProgressBar("Init Animator Config", "Please wait ...", 0.7f);
    //    assetAnimationClips.Clear();
    //    foreach (var host in ActionSettings.AllHosts)
    //    {
    //        if (host.Model.modeltype != cfg.character.ModelType.Base &&
    //            host.Model.modeltype != cfg.character.ModelType.Template)
    //        {
    //            var allactions = host.AllActions;
    //            foreach (var action in allactions)
    //            {
    //                AddAssetAnimationClip(host, action, action.foreactionfile);
    //                AddAssetAnimationClip(host, action, action.actionfile);
    //                AddAssetAnimationClip(host, action, action.succactionfile);
    //            }

    //        }

    //    }
    //    EditorUtility.DisplayProgressBar("Init Animator Config", "Please wait ...", 1f);
    //}

    static void ExportAllAnimationClips(BuildTarget target)
    {
        //GetAnimationClipAssets();
        SetAssetBundleName(assetAnimationClips);
        BuildAssetBundles(target);
    }
    static void ExportSelectedAnimationClips(BuildTarget target)
    {
        UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
        if (selection.Length > 0)
        {
            //GetAnimationClipAssets();
            var assets = GetSelectedAssets(assetAnimationClips, selection);
            SetAssetBundleName(assets);
            BuildAssetBundles(target);
        }
    }


    [MenuItem("Assets/Build Selected for Windows/AnimationClip")]
    static void ExportSelectAnimationClipForWindows()
    {
        ExportSelectedAnimationClips(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build Selected for Android/AnimationClip")]
    static void ExportSelectAnimationClipForAndroid()
    {
        ExportSelectedAnimationClips(BuildTarget.Android);
    }
    [MenuItem("Assets/Build Selected for iOS/AnimationClip")]
    static void ExportSelectAnimationClipForiOS()
    {
        ExportSelectedAnimationClips(BuildTarget.iOS);
    }
    [MenuItem("Build Windows/Build AnimationClips for Windows")]
    static void ExportAllAnimationClipsForWindows()
    {
        ExportAllAnimationClips(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Build Android/Build AnimationClips for Android")]
    static void ExportAllAnimationClipsForAndroid()
    {
        ExportAllAnimationClips(BuildTarget.Android);
    }
    [MenuItem("Build iOS/Build AnimationClips for iOS")]
    static void ExportAllAnimationClipsForiOS()
    {
        ExportAllAnimationClips(BuildTarget.iOS);
    }
}

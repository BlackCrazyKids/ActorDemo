using AssetMgr;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimCompressTool
{
    //---遗留问题
    //1.单独生成表格数据
    //2.表格数据初始化
    //3.补全剩下代码


    /// <summary>
    /// 压缩所有
    /// </summary>
    [MenuItem("Tools/AssetMgr/Compress All Anim Key-Float", false, 500)]
    static void CompressAllKeyAndFloatPrecision()
    {

    }

    /// <summary>
    /// 选择性压缩
    /// </summary>
    [MenuItem("Assets/Compress Anim Key-Float", false, 500)]
    static void CompressKeyAndFloatPrecision()
    {
        //List<ClipInfo> clipInfos = GetSelectedAnimationClips();
        //for (int i = 0; i < clipInfos.Count; i++)
        //{
        //    var info = clipInfos[i];
        //    CompressKey(info.clip);
        //    CompressFloatPrecision(info.clip, "f3");
        //    SaveClip(info);
        //    if (EditorUtility.DisplayCancelableProgressBar("Only Compress Key&Float", info.clip.name, (i + 1f) / clipInfos.Count)) break;
        //}
        //EditorUtility.ClearProgressBar();

        List<ClipInfo> clipInfos = GetSelectedAnimationClips();
        for (int i = 0; i < clipInfos.Count; i++)
        {
            var info = clipInfos[i];
            //CompressKey(info.clip);
            CompressFloatPrecision(info.clip, "f3");
            SaveClip(info);
            if (EditorUtility.DisplayCancelableProgressBar("Only Compress Key&Float", info.clip.name, (i + 1f) / clipInfos.Count)) break;
        }
        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Assets/Compress Anim Key-Float", true, 500)]
    static bool RequireAnimation()
    {
        bool isOk = false;
        string[] guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip != null)
            {
                isOk = true;
                break;
            }
        }
        return isOk;
    }



    /// <summary>
    /// 预览动作
    /// </summary>
    const string PREVIEW_CLIP = "__preview__Take 001";
    /// <summary>
    /// 默认排除骨骼
    /// </summary>
    const string BIP001 = "bip001";
    /// <summary>
    /// 动画剪辑资源根目录
    /// </summary>
    const string CLIPS_PATH = "";

    static List<ClipInfo> _infos = new List<ClipInfo>();
    /// <summary>
    /// 需要特殊处理的模型压缩数据
    /// </summary>
    static List<CompressClip> _compresses = new List<CompressClip>();
    /// <summary>
    /// 默认压缩配置,即所有模型均进行此操作
    /// </summary>
    static CompressClip _defaultCompress = null;


    class CompressClip
    {
        public int CompressType = 0;
        public List<string> ExcludeBones = new List<string>();
        public List<string> ExcludeClips = new List<string>();
    }
    class ClipInfo
    {
        public string path;
        public AnimationClip clip;
    }

    static void LoadCfg()
    {
        //加载配置数据
        //TODO
    }
    static void ClearCfg()
    {
        _infos.Clear();
        _compresses.Clear();
        _defaultCompress = null;
    }
    static List<ClipInfo> GetAllAnimationClips()
    {
        List<string> fs = new List<string>(Directory.GetFiles(CLIPS_PATH, "*.fbx", SearchOption.AllDirectories));
        List<ClipInfo> clips = new List<ClipInfo>();
        for (int i = 0; i < fs.Count; i++)
        {
            string path = AMTool.GetUnityPath(fs[i]);
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (Object obj in objs)
            {
                AnimationClip clip = obj as AnimationClip;
                if (clip != null && clip.name != PREVIEW_CLIP)
                {
                    ClipInfo info = new ClipInfo()
                    {
                        path = path,
                        clip = Object.Instantiate(clip),
                    };
                    clips.Add(info);
                }
            }
        }

        return clips;
    }
    static List<ClipInfo> GetSelectedAnimationClips()
    {
        List<ClipInfo> clips = new List<ClipInfo>();
        string[] guids = Selection.assetGUIDs;
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (Object obj in objs)
            {
                AnimationClip clip = obj as AnimationClip;
                if (clip != null && clip.name != PREVIEW_CLIP)
                {
                    ClipInfo info = new ClipInfo()
                    {
                        path = path,
                        clip = Object.Instantiate(clip),
                    };
                    clips.Add(info);
                }
            }
        }
        return clips;
    }
    static void SaveClip(ClipInfo info)
    {
        string dirPath = string.Format("{0}/clips", Path.GetDirectoryName(info.path));
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        string path = string.Format("{0}/{1}.anim", dirPath, info.clip.name.Replace("(Clone)", ""));
        AssetDatabase.CreateAsset(info.clip, path);
        AssetDatabase.Refresh();

    }

    /// <summary>
    /// 删除指定帧
    /// </summary>
    /// <param name="clip">动画剪辑</param>
    /// <param name="keyName">帧名</param>
    static void CompressKey(CompressClip compress)
    {
        // 部分骨骼带位移
        // 部分动画不做减帧压缩
        // 动画压缩分类型:All,OnlyScale,None;All只包含Scale和Position
        for (int i = 0; i < _infos.Count; i++)
        {
            AnimationClip clip = _infos[i].clip;
            if (!IsExcludeClip(compress, clip.name)) continue;

            EditorCurveBinding[] curves = AnimationUtility.GetCurveBindings(clip);
            AnimationClip tempClip = Object.Instantiate(clip);
            clip.ClearCurves();
            for (int j = 0; j < curves.Length; j++)
            {
                EditorCurveBinding curveBinding = curves[j];

                int index = curveBinding.path.LastIndexOf("/");
                string boneName = curveBinding.path.Substring(index + 1);
                if (!IsExcludeBone(compress, boneName))
                {
                    if (curveBinding.propertyName.Contains("Position")
                        || curveBinding.propertyName.Contains("Scale"))
                        continue;
                }

                AnimationCurve curve = AnimationUtility.GetEditorCurve(tempClip, curveBinding);
                clip.SetCurve(curveBinding.path, curveBinding.type, curveBinding.propertyName, curve);
            }
        }
    }
    /// <summary>
    /// 是否排除骨骼不做减帧操作
    /// </summary>
    static bool IsExcludeBone(CompressClip compress, string bone)
    {
        if (bone.ToLower() == BIP001)
            return true;
        else if (_defaultCompress != null && _defaultCompress.ExcludeBones.Contains(bone))
            return true;
        else if (compress != null && compress.ExcludeBones.Contains(bone))
            return true;
        return false;
    }
    /// <summary>
    /// 是否排除动作剪辑,不做减帧操作
    /// </summary>
    static bool IsExcludeClip(CompressClip compress, string clip)
    {
        if (_defaultCompress != null && _defaultCompress.ExcludeBones.Contains(clip))
            return true;
        else if (compress != null && compress.ExcludeClips.Contains(clip))
            return true;
        return false;
    }

    /// <summary>
    /// 降低浮点数精度
    /// </summary>
    /// <param name="clip">动画剪辑</param>
    /// <param name="precision">精度格式f3,默认保留3位小数</param>
    static void CompressFloatPrecision(AnimationClip clip, string precision = "f3")
    {
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);

        for (int j = 0; j < bindings.Length; j++)
        {
            EditorCurveBinding curveBinding = bindings[j];
            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, curveBinding);

            if (curve == null || curve.keys == null) continue;

            Keyframe[] keys = curve.keys;
            for (int k = 0; k < keys.Length; k++)
            {
                Keyframe key = keys[k];
                key.value = float.Parse(key.value.ToString(precision));
                key.inTangent = float.Parse(key.inTangent.ToString(precision));
                key.outTangent = float.Parse(key.outTangent.ToString(precision));
                keys[k] = key;
            }
            curve.keys = keys;

            clip.SetCurve(curveBinding.path, curveBinding.type, curveBinding.propertyName, curve);
        }
    }
}


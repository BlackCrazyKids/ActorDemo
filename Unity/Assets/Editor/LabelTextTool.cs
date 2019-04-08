using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.VersionControl;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class LabelTextTool : EditorWindow
{
    [MenuItem("Tools/FindReplaceTool")]
    static void ReplaceTool()
    {
        var window = GetWindow<LabelTextTool>("预制Label查找工具");
    }

    Object _asset;
    string _find = "";
    string _src = "";
    string _dst = "";

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择资源", GUILayout.Width(60));
        _asset = EditorGUILayout.ObjectField(_asset, typeof(Object));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        _find = GUILayout.TextField(_find);
        if (GUILayout.Button("查找", GUILayout.Width(60)) && !string.IsNullOrEmpty(_find) && _asset != null)
        {
            Debug.LogError("----------------------查找----------------------");
            List<GameObject> ls = GetGameObjects();
            for (int i = 0; i < ls.Count; i++)
            {
                string title = string.Format("查找-{0}[{1}/{2}]", _find, i + 1, ls.Count);
                Find(ls[i].transform, _find, title);
                EditorUtility.DisplayProgressBar(title, ls[i].name, (i + 1f) / ls.Count);
            }
            EditorUtility.ClearProgressBar();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        _src = GUILayout.TextField(_src);
        GUILayout.Label("->", GUILayout.Width(20));
        _dst = GUILayout.TextField(_dst);
        if (GUILayout.Button("替换", GUILayout.Width(60)) && _asset != null
            && !string.IsNullOrEmpty(_src) && !string.IsNullOrEmpty(_dst))
        {
            Debug.LogError("----------------------替换----------------------");
            List<GameObject> ls = GetGameObjects();
            for (int i = 0; i < ls.Count; i++)
            {
                string title = string.Format("替换-{0}:{1}[{2}/{3}]", _src, _dst, i + 1, ls.Count);
                Replace(ls[i], _src, _dst, title);
                EditorUtility.DisplayProgressBar(title, ls[i].name, (i + 1f) / ls.Count);
            }
            EditorUtility.ClearProgressBar();
        }
        GUILayout.EndHorizontal();
    }

    List<GameObject> GetGameObjects()
    {
        List<GameObject> ls = new List<GameObject>();
        if (typeof(DefaultAsset) == _asset.GetType())
        {
            string path = AssetDatabase.GetAssetPath(_asset).Replace("Assets", Application.dataPath);
            string[] files = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                string refPath = f.Replace(Application.dataPath, "Assets");
                ls.Add(AssetDatabase.LoadAssetAtPath<GameObject>(refPath));
            }
        }
        else if (typeof(GameObject) == _asset.GetType())
            ls.Add(_asset as GameObject);
        return ls;
    }
    string GetPath(Transform t)
    {
        StringBuilder builder = new StringBuilder(t.name);
        while (t.parent != null)
        {
            t = t.parent;
            builder.Insert(0, t.name + "\\");
        }
        return builder.ToString();
    }

    void Find(Transform t, string str, string title)
    {
        StringBuilder builder = new StringBuilder();
        //var labels = t.GetComponentsInChildren<UILabel>();    
        var labels = t.GetComponentsInChildren<Text>();
        for (int i = 0; i < labels.Length; i++)
        {
            var l = labels[i];
            string path = GetPath(l.transform);
            if (l.text.IndexOf(str) > -1)
                builder.AppendLine(path);
            else
            {
                char[] chars = str.ToCharArray();
                bool isContain = true;
                for (int j = 0; j < chars.Length; j++)
                {
                    isContain &= l.text.IndexOf(chars[j]) != -1;
                    if (!isContain) break;
                }
                if (isContain)
                    Debug.LogError("可能需要替换:" + path);
            }
        }
        string log = builder.ToString();
        if (!string.IsNullOrEmpty(log))
            Debug.LogError(log);
    }
    void Replace(GameObject go, string a, string b, string title)
    {
        StringBuilder builder = new StringBuilder();
        var ins = PrefabUtility.InstantiatePrefab(go) as GameObject;
        //var labels = ins.transform.GetComponentsInChildren<UILabel>(true);
        var labels = ins.transform.GetComponentsInChildren<Text>(true);
        bool needReplace = false;
        for (int i = 0; i < labels.Length; i++)
        {
            var l = labels[i];
            if (l.text.IndexOf(a) > -1)
            {
                builder.AppendLine(GetPath(l.transform));
                l.text = l.text.Replace(a, b);
                needReplace = true;
            }           
        }

        if (needReplace)
            PrefabUtility.ReplacePrefab(ins, go, ReplacePrefabOptions.ConnectToPrefab);
        DestroyImmediate(ins);
        AssetDatabase.Refresh();

        string log = builder.ToString();
        if (!string.IsNullOrEmpty(log))
            Debug.LogError(log);
    }


}

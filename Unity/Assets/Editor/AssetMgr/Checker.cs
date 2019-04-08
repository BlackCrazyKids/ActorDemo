using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace AssetMgr
{
    public class Checker
    {
        [MenuItem("Tools/AssetMgr/资源重名检查")]
        static void Open()
        {
            //var window = GetWindow<Checker>();
            List<string> fs = new List<string>();
            var guids = Selection.assetGUIDs;
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                fs.AddRange(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories));
            }
            fs.RemoveAll(f => {
                string ext = Path.GetExtension(f);
                return ext.Equals(".meta") || ext.Equals(".fbx") || ext.Equals(".FBX") 
                || ext.Equals(".mat") || ext.Equals(".prefab");
            });
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var item in fs)
            {
                string name = Path.GetFileName(item);
                if(!dict.ContainsKey(name))
                    dict.Add(name, item);
            }
            foreach(var item in dict)
                fs.Remove(item.Value);

            Debug.LogError(AMTool.List2String(fs));
        }


    }
}
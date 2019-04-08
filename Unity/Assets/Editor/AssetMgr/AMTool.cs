using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssetMgr
{
    public class AMTool
    {
        public static List<string> GetTextures(string path)
        {
            List<string> files = new List<string>(Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
               .Where(f => f.EndsWith(".png") || f.EndsWith(".jpg") || f.EndsWith(".tga") || f.EndsWith(".psd") || f.EndsWith(".exr")));
            return files;
        }

        public static string StandardlizePath(string path)
        {
            return path.Replace("\\", "/");
        }
        public static List<string> StandardlizeList(List<string> list)
        {
            List<string> ls = new List<string>();
            list.ForEach(p => ls.Add(StandardlizePath(p)));
            return ls;
        }

        public static string GetUnityPath(string path)
        {
            return StandardlizePath(path).Replace(Application.dataPath, "Assets");
        }
        public static string GetAbsPath(string path)
        {
            return StandardlizePath(path).Replace("Assets", Application.dataPath);
        }

        public static string List2String<T>(List<T> list, string split = "\r\n")
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
                builder.AppendFormat("{0}{1}", list[i], split);
            return builder.ToString();
        }

        #region 树形结构操作
        /// <summary>
        /// 加载目录树形结构
        /// </summary>
        public static void LoadDirSetting<T>(string parent, Dictionary<string, T> dict) where T : DirectorySetting<T>, new()
        {
            if (dict != null && dict.Count == 0)
            {
                Debug.LogError("字典中必须包含根节点的数据!");
                return;
            }

            //---对子对象进行操作
            List<string> ds = new List<string>();
            ds.AddRange(Directory.GetDirectories(parent, "*", SearchOption.TopDirectoryOnly));
            ds = AMTool.StandardlizeList(ds);
            if (dict.ContainsKey(parent))
            {
                //移除不存在目录
                HashSet<string> hash0 = new HashSet<string>(dict.Keys);
                hash0.ExceptWith(ds);
                foreach (var path in hash0)
                    dict[parent].Children.Remove(dict[path]);
            }

            //处理新增目录
            HashSet<string> hash = new HashSet<string>(ds);
            hash.ExceptWith(dict.Keys);
            foreach (var path in hash)
            {
                var setting = new T();
                setting.RelPath = AMTool.GetUnityPath(path);
                setting.Parent = dict[parent];
                dict[parent].Children.Add(setting);
                dict.Add(path, setting);
                LoadDirSetting(path, dict);
            }
        }
        /// <summary>
        /// 将树列表转换成字典
        /// </summary>
        public static Dictionary<string, T> GetDictionary<T>(List<T> trees) where T : DirectorySetting<T>, new()
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            for (int i = 0; i < trees.Count; i++)
                GetCurrentSetting(trees[i], dict);
            return dict;
        }
        /// <summary>
        /// 从当前节点开始,将树结构转换成字典
        /// </summary>
        public static void GetCurrentSetting<T>(T setting, Dictionary<string, T> dict) where T : DirectorySetting<T>, new()
        {
            if (setting == null) return;

            string path = GetAbsPath(setting.RelPath);
            if (!dict.ContainsKey(path))
                dict.Add(path, setting);
            for (int i = 0; i < setting.Children.Count; i++)
            {
                var sub = setting.Children[i] as T;
                GetCurrentSetting(sub, dict);
            }
        }
        #endregion
    }
}
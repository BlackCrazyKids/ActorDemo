using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace GameEditor
{
    //ToolKit 
    public class EUtil
    {
        #region 文件路径操作
        public static string FilePath2UnityPath(string path)
        {
            return path.Replace(Application.dataPath, "Assets");
        }
        public static string UnityPath2FilePath(string path)
        {
            if (path.IndexOf("Assets") != 0)
            {
                Debug.LogError("非UnityPath - " + path);
                return path;
            }
            return Application.dataPath + path.Substring(6, path.Length - 6);
        }        
        public static string StandardlizePath(string path)
        {
            return StandardlizePath(path, false);
        }
        /// <summary>
        /// 标准化路径,'\'转化'/'
        /// </summary>
        /// <param name="path">资源路径</param>
        public static string StandardlizePath(string path, bool toLower = true)
        {
            string pathReplace = path.Replace(@"\", @"/");
            return toLower ? pathReplace.ToLower() : pathReplace;
        }
        #endregion

        #region 文件/目录内容读取
        /// <summary>  
        /// 得到选中资产路径列表    
        /// </summary>   
        /// <returns></returns> 
        public static List<string> GetSelectionAssetPaths()
        {
            List<string> assetPaths = new List<string>();
            // 这个接口才能取到两列模式时候的文件夹  
            foreach (var guid in Selection.assetGUIDs)
            {
                if (string.IsNullOrEmpty(guid))
                {
                    continue;
                }
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!string.IsNullOrEmpty(path))
                {
                    assetPaths.Add(path);
                }
            }
            return assetPaths;
        }

        /// <summary>
        /// 子资源不可重名! key-文件名    value-路径
        /// </summary>
        public static void GetAssetsInSubFolderRecursively(string srcFolder, string searchPattern, ref Dictionary<string, string> fileName2Path, List<string> sb = null)
        {
            string searchFolder = StandardlizePath(srcFolder);
            if (!Directory.Exists(searchFolder))
                return;

            string[] files = Directory.GetFiles(searchFolder, searchPattern);
            foreach (string oneFile in files)
            {
                string srcFile = StandardlizePath(oneFile);
                if (!File.Exists(srcFile))
                    continue;
                string srcFileName = Path.GetFileNameWithoutExtension(srcFile);
                if (!fileName2Path.ContainsKey(srcFileName))
                {
                    fileName2Path.Add(srcFileName, srcFile);
                }
                else
                {
                    string error = "资源名重复 filename:" + srcFileName;
                    UnityEngine.Debug.LogError(error);
                    if (sb != null && !sb.Contains(error))
                    {
                        sb.Add(error);
                    }

                }

            }

            string[] dirs = Directory.GetDirectories(searchFolder);
            foreach (string oneDir in dirs)
            {
                GetAssetsInSubFolderRecursively(oneDir, searchPattern, ref fileName2Path, sb);
            }
        }

        /// <summary>
        /// 按扩展名筛选文件
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="exts">需要过滤的扩展名</param>
        /// <param name="searchOption">查询模式</param>
        /// <returns></returns>
        public static string[] GetFiles(string path, string[] exts, SearchOption searchOption)
        {
            List<string> list = new List<string>();
            string[] fs = Directory.GetFiles(path, "*.*", searchOption);
            HashSet<string> hash = new HashSet<string>(exts);
            for (int i = 0; i < fs.Length; i++)
            {
                string ext = Path.GetExtension(fs[i]);
                if (!hash.Contains(ext))
                    list.Add(fs[i]);
            }
            return list.ToArray();
        }
        #endregion
    }
}
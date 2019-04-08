using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using GameEditor;

public partial class ExportResource
{
    const string EXT_SHADER = ".shader";
    const string EXT_PNG = ".png";
    const string EXT_TGA = ".tga";
    const string EXT_MP3 = ".mp3";
    const string EXT_OGG = ".ogg";

    const string UI_FOLDER = "ui/";
    const string SHADER_FOLDER = "shaders/";
    const string DEPTEX_FOLDER = "texture/t_";
    const string AUDIO_FOLDER = "audio/a_";

    static BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle
        | BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle;

    /// <summary>
    /// 平台文件目录名
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    static string GetPlatfomrPath(BuildTarget target)
    {
        string platformPath = string.Empty;
        switch (target)
        {
            case BuildTarget.Android:
                platformPath = "Dist/Android/Data";
                break;
            case BuildTarget.iOS:
                platformPath = "Dist/IOS/Data";
                break;
            default:
                {
                    platformPath = "GamePlayer/Data";
                }
                break;
        }
        return platformPath;
    }
    /// <summary>
    /// AB包路径
    /// </summary>
    /// <param name="target">平台</param>
    /// <param name="relativePath">平台内部相对路径</param>
    /// <returns></returns>
    public static string GetBundleSavePath(BuildTarget target, string relativePath)
    {
        string path;
        switch (target)
        {
            case BuildTarget.Android:
            case BuildTarget.iOS:
                path = string.Format("{0}/../../{1}/{2}", Application.dataPath, GetPlatfomrPath(target), relativePath);
                break;
            default:
                path = string.Format("{0}/../../{1}/{2}", Application.dataPath, GetPlatfomrPath(target), relativePath);
                break;
        }
        return path;
    }
    /// <summary>
    /// 对应分类资源目录
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static string GetBundleSaveDir(BuildTarget target)
    {
        return string.Format("{0}/../../{1}/", Application.dataPath, GetPlatfomrPath(target));
    }

    /// <summary>
    /// 设置主体资源AB包名及依赖资源包名;[分离依赖单独打包]
    /// </summary>
    /// <param name="assets">资源</param>
    /// <param name="depFormats">依赖资源格式,此格式单独打AB包</param>
    /// <param name="depPath">依赖存储相对路径</param>
    /// <param name="bPrefix">是否修正前缀</param>
    static void SetAssetBundleName(Dictionary<string, string> assets, string[] depFormats, string depPath)
    {
        AssetImporter importer = null;
        foreach (KeyValuePair<string, string> pair in assets)
        {
            string[] dependencies = AssetDatabase.GetDependencies(pair.Key);
            foreach (string sdep in dependencies)
            {
                string dep = sdep.ToLower();
                foreach (string format in depFormats)
                {
                    if (dep.EndsWith(format))
                    {
                        importer = AssetImporter.GetAtPath(dep);

                        string newName = string.Format("{0}{1}.bundle", depPath, Path.GetFileNameWithoutExtension(dep));
                        newName = newName.Replace(" ", "");
                        newName = newName.ToLower();
                        if (importer.assetBundleName == null || importer.assetBundleName != newName)
                        {
                            importer.assetBundleName = newName;
                        }
                    }
                }
            }
            importer = AssetImporter.GetAtPath(pair.Key);
            if (importer == null)
            {
                Debug.LogError("未发现资源: " + pair.Key);
            }
            if (importer.assetBundleName == null || importer.assetBundleName != pair.Value)
            {
                importer.assetBundleName = pair.Value;
            }
        }
    }
    /// <summary>
    /// 设置AB包名,无依赖资源
    /// </summary>
    /// <param name="assets">资源</param>
    static void SetAssetBundleName(Dictionary<string, string> assets)
    {
        AssetImporter importer = null;
        foreach (KeyValuePair<string, string> pair in assets)
        {
            importer = AssetImporter.GetAtPath(pair.Key);
            if (importer.assetBundleName == null || importer.assetBundleName != pair.Value)
            {
                importer.assetBundleName = pair.Value;
            }
        }
    }
    /// <summary>
    /// 构建AB包
    /// </summary>
    static void BuildAssetBundles(BuildTarget target)
    {
        string dir = GetBundleSaveDir(target);
        Directory.CreateDirectory(Path.GetDirectoryName(dir));

        BuildPipeline.BuildAssetBundles(dir, options, target);

        ResetAssetBundleNames();
        SaveDependency(target);
    }
    /// <summary>
    /// 对有依赖的资源做依赖资源路径记录
    /// </summary>
    /// <param name="target"></param>
    static void SaveDependency(BuildTarget target)
    {
        string dir = GetBundleSaveDir(target);
        string depfile = dir.Substring(dir.TrimEnd('/').LastIndexOf("/") + 1);
        depfile = depfile.TrimEnd('/');
        string path = GetBundleSavePath(target, depfile);

        AssetBundle ab = AssetBundle.LoadFromFile(path);

        AssetBundleManifest manifest = (AssetBundleManifest)ab.LoadAsset("AssetBundleManifest");

        ab.Unload(false);

        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

        LoadOldDependency(target, dic);

        Debug.LogError("Length:" + manifest.GetAllAssetBundles().Length);
        foreach (string asset in manifest.GetAllAssetBundles())
        {
            List<string> list = new List<string>();
            string[] deps = manifest.GetDirectDependencies(asset);
            foreach (string dep in deps)
            {
                list.Add(dep);
            }
            if (deps.Length > 0)
                dic[asset] = list;
            else if (dic.ContainsKey(asset))
                dic.Remove(asset);
        }

        WriteDependenceConfig(target, dic);
    }
    static void LoadOldDependency(BuildTarget target, Dictionary<string, List<string>> dic)
    {
        string dataPath = GetBundleSavePath(target, "config/dependency");
        if (!File.Exists(dataPath))
        {
            return;
        }

        FileStream fs = new FileStream(dataPath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);

        int size = br.ReadInt32();
        string resname;
        string textureBundleName;

        for (int i = 0; i < size; i++)
        {
            resname = br.ReadString();
            int count = br.ReadInt32();
            if (!dic.ContainsKey(resname))
                dic[resname] = new List<string>();
            //Debug.Log(sfxname + "  " + count);
            for (int j = 0; j < count; ++j)
            {
                textureBundleName = br.ReadString();
                dic[resname].Add(textureBundleName);
            }
        }
        br.Close();
        fs.Close();
    }
    static void WriteDependenceConfig(BuildTarget target, Dictionary<string, List<string>> m_Dependencies)
    {
        string fileName = GetBundleSavePath(target, "config/dependency");
        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
        FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
        BinaryWriter w = new BinaryWriter(fs);
        w.Write(m_Dependencies.Count);

        foreach (KeyValuePair<string, List<string>> pair in m_Dependencies)
        {
            w.Write(pair.Key);
            w.Write(pair.Value.Count);
            foreach (string s in pair.Value)
            {
                w.Write(s);
            }
        }
        w.Close();
        fs.Close();

        if (true)
        {
            using (StreamWriter sw = File.CreateText(fileName + "text"))
            {
                sw.WriteLine("size = " + m_Dependencies.Count);

                foreach (KeyValuePair<string, List<string>> pair in m_Dependencies)
                {
                    sw.WriteLine(pair.Key);
                    sw.WriteLine(pair.Value.Count);
                    foreach (string s in pair.Value)
                    {
                        sw.WriteLine(s);
                    }
                }
                sw.Close();
            }

        }
    }

    /// <summary>
    /// 标准化路径,'\'转化'/'
    /// </summary>
    /// <param name="obj">资源对象</param>
    /// <returns></returns>
    static string StandardlizePath(UnityEngine.Object obj)
    {
        return EUtil.StandardlizePath(AssetDatabase.GetAssetPath(obj));
    }  
    /// <summary>
    /// 限定选择的资源必须在指定范围内
    /// </summary>
    /// <param name="allassets">所有资源</param>
    /// <param name="selection">选择的资源</param>
    /// <returns></returns>
    static Dictionary<string, string> GetSelectedAssets(Dictionary<string, string> allassets, Object[] selection)
    {
        Dictionary<string, string> assets = new Dictionary<string, string>();
        for (int i = 0; i < selection.Length; ++i)
        {
            var assetpath = AssetDatabase.GetAssetPath(selection[i]);
            var assetstandardlizepath = StandardlizePath(selection[i]);
            if (allassets.ContainsKey(assetpath))
            {
                assets[assetpath] = allassets[assetpath];
            }
            else if (allassets.ContainsKey(assetstandardlizepath))
            {
                assets[assetstandardlizepath] = allassets[assetstandardlizepath];
            }
        }
        return assets;
    }
    /// <summary>
    /// 获取目录下指定格式资源,包含子目录
    /// </summary>
    /// <param name="srcFolder"></param>
    /// <param name="searchPattern"></param>
    /// <param name="dstFolder"></param>
    /// <param name="prefix"></param>
    /// <param name="assets"></param>
    static void GetAssetsRecursively(string srcFolder, string searchPattern, string dstFolder, string prefix, ref Dictionary<string, string> assets)
    {
        string searchFolder = EUtil.StandardlizePath(srcFolder);
        if (!Directory.Exists(searchFolder))
            return;

        string suffix = "bundle";
        string srcDir = searchFolder;
        string[] files = Directory.GetFiles(srcDir, searchPattern);
        foreach (string oneFile in files)
        {
            string srcFile = EUtil.StandardlizePath(oneFile);
            if (!File.Exists(srcFile))
                continue;

            string dstFile;
            if (string.IsNullOrEmpty(prefix))
            {
                dstFile = Path.Combine(dstFolder, string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(srcFile), suffix));
            }
            else
            {
                dstFile = Path.Combine(dstFolder, string.Format("{0}_{1}.{2}", prefix, Path.GetFileNameWithoutExtension(srcFile), suffix));
            }
            dstFile = EUtil.StandardlizePath(dstFile);
            assets[srcFile] = dstFile;
            UnityEngine.Debug.Log("srcFile: " + srcFile + " => dstFile: " + dstFile);
        }

        string[] dirs = Directory.GetDirectories(searchFolder);
        foreach (string oneDir in dirs)
        {
            GetAssetsRecursively(oneDir, searchPattern, dstFolder, prefix, ref assets);
        }
    }

    /// <summary>
    /// Android平台时,图集更换材质
    /// </summary>
    static void ReplaceDefaultMatAndShaders()
    {

    }
}

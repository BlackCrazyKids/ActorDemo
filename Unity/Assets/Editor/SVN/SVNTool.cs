using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/*参数说明
    如果想在进度对话框执行完毕后自动关闭，而又不必设置永久性的参数，可以传递/closeonend参数。
    /closeonend:0 不自动关闭对话框
    /closeonend:1 如果没发生错误则自动关闭对话框
    /closeonend:2 如果没发生错误和冲突则自动关闭对话框
    /closeonend:3如果没有错误、冲突和合并，会自动关闭

    Status命令结果说明[默认无提示,则表示版本一致]
    ? 未纳入版本控制
    A 增加
    C 冲突
    M 改变
*/

/// 工作目录是基于项目目录,即Assets文件夹所在目录
/// 路径示例：Assets/1.png
public static class SVNTool
{
    /// <summary>
    /// 通用菜单列表
    /// </summary>
    public static void DrawGenericMenu(ref GenericMenu menu, List<string> paths)
    {
        menu.AddItem(new GUIContent("更新"), false, () => Update(paths));
        menu.AddItem(new GUIContent("提交"), false, () => Commit(paths));
        menu.AddItem(new GUIContent("还原"), false, () => Revert(paths));
        menu.AddItem(new GUIContent("解决"), false, () => Resolve(paths));
    }

    #region SVN操作
    public static void UpdateAtPath(string path)
    {
        Update(new List<string>() { path });
    }
    public static void CommitAtPath(string path)
    {
        Commit(new List<string>() { path });
    }
    public static void RevertAtPath(string path)
    {
        Revert(new List<string>() { path });
    }
    public static void ResolveAtPath(string path)
    {
        Resolve(new List<string>() { path });
    }


    /// <summary>       
    /// SVN更新指定的路径        
    /// 路径示例：Assets/1.png        
    /// </summary>        
    ///<param name="assetPaths">       
    public static void Update(List<string> paths)
    {
        if (paths.Count == 0) return;
        string arg = AssemblyPathsCMD("update", paths);
        RunSVNCMD(arg);
    }
    public static void Commit(List<string> paths, string logmsg = null)
    {
        if (paths.Count == 0) return;
        string arg = AssemblyPathsCMD("commit", paths);
        if (!string.IsNullOrEmpty(logmsg))
            arg += " /logmsg:\"" + logmsg + "\"";
        RunSVNCMD(arg);
    }
    public static void Revert(List<string> paths)
    {
        if (paths.Count == 0) return;
        string arg = AssemblyPathsCMD("revert", paths);
        RunSVNCMD(arg);
    }
    public static void Resolve(List<string> paths)
    {
        if (paths.Count == 0) return;
        string arg = AssemblyPathsCMD("resolve", paths);
        RunSVNCMD(arg);
    }
    public static Dictionary<string, char> Status(List<string> paths)
    {
        var result = new Dictionary<string, char>();
        if (paths.Count == 0) return result;
        string arg = AssemblyPathsCMD("resolve", paths);
        var stream = RunSVNCMD(arg);
        while (stream.EndOfStream)
        {
            string line = stream.ReadLine();
            if (!string.IsNullOrEmpty(line) && line.Substring(1, 7).Trim(' ').Length == 0)
            {
                string name = line.Substring(8);
                result.Add(name, line[0]);
            }
        }
        return result;
    }


    /// <summary>
    /// 对当前命令进行多路径组装
    /// </summary>
    private static string AssemblyPathsCMD(string cmd, List<string> paths, int closeCode = 0)
    {
        string arg = string.Format("/command:{0} /closeonend:{1} /path:\"", cmd, closeCode);
        for (int i = 0; i < paths.Count; i++)
        {
            var assetPath = paths[i];
            if (i != 0)
            {
                arg += "*";
            }
            arg += assetPath;
        }
        arg += "\"";
        return arg;
    }
    /// <summary>  
    /// SVN命令运行     
    /// </summary>       
    ///<param name="arg">  
    private static StreamReader RunSVNCMD(string arg)
    {
        string workDirectory = Application.dataPath.Remove(Application.dataPath.LastIndexOf("/Assets", StringComparison.Ordinal));
        var result = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            FileName = "TortoiseProc",
            Arguments = arg,
            WorkingDirectory = workDirectory
        });
        return result.StandardOutput;
    }
    #endregion
}


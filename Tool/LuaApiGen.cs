using UnityEngine;
using System.Collections.Generic;
using XLua;
using System.IO;
using System.Text;
using System.Linq;
using CSObjectWrapEditor;
using System.Diagnostics;
using FairyGUI;

public class LuaApiGen
{
    static string Template
    {
        get
        {
            string path = Application.dataPath + @"\XLua\Src\Editor\LuaApi\LuaApiGen.tpl.txt";
            return File.ReadAllText(path);
        }
    }

    protected class Info
    {
        public string ClassName;
        public string InhertName;
        public List<FieldInfo> Fields;
        public List<FuncInfo> Funcs;
    }
    protected class FieldInfo
    {
        public string Name;
        public string Type;
    }
    protected class FuncInfo
    {
        public string Name;
        public List<FieldInfo> ParamNames;
        public string ReturnType;
        public List<FuncInfo> Overload;
    }

    public static IEnumerable<CustomGenTask> GetTasks(LuaEnv lua_env, UserConfig user_cfg)
    {
        for (int i = 0; i < infoList.Count; i++)
        {
            LuaTable data = lua_env.NewTable();
            data.Set("info", infoList[i]);
            string path = string.Format("{0}/{1}.lua", cacheDir, infoList[i].ClassName);
            yield return new CustomGenTask
            {
                Data = data,
                Output = new StreamWriter(path, false, Encoding.UTF8)
            };
        }
    }

    static readonly string codeDir = Application.dataPath + "/../../Code";
    static readonly string apiDir = codeDir + "/Api";
    static readonly string cacheDir = codeDir + "/Cache";
    static readonly string winRAR = Application.dataPath + "/../../Tool/WinRAR.exe";
    static List<Info> infoList = new List<Info>();
    protected static void GenLuaApi(string name, List<Info> ls)
    {
        string[] fs = Directory.GetFiles(cacheDir, "*.lua");
        for (int i = 0; i < fs.Length; i++)
            File.Delete(fs[i]);

        infoList = ls;
        Generator.CustomGen(Template, GetTasks);
        infoList.Clear();


        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = winRAR;
        info.Arguments = string.Format("a -r {0}.zip {1}", winRAR, name, cacheDir);
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = Application.platform == RuntimePlatform.WindowsEditor;
        info.ErrorDialog = true;
        Process pro = Process.Start(info);
        pro.WaitForExit();
    }
}
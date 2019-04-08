using System.IO;
using System.Text;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using System;

namespace GameEditor
{
    public class LuaApiGen
    {
        /// <summary>
        /// 一个Api脚本信息
        /// </summary>
        public class Info
        {
            public string ClassName;
            public string InhertName;
            public List<FieldInfo> Fields = new List<FieldInfo>();
            public List<FuncInfo> Funcs = new List<FuncInfo>();
        }
        public class FieldInfo
        {
            public string Name;
            public string Type;
        }
        public class FuncInfo
        {
            public string Name;
            public bool IsStatic;
            public List<FieldInfo> Params = new List<FieldInfo>();
            public string ReturnType;
            public List<FuncInfo> Overload = new List<FuncInfo>();
        }

        static readonly string codeDir = Application.dataPath + "/../../Code";
        static readonly string apiDir = codeDir + "/Api";
        static readonly string cacheDir = codeDir + "/Cache";
        static readonly string winRAR = Application.dataPath + "/../../Tool/WinRAR.exe";
        static List<Info> infoList = new List<Info>();
        static Stopwatch timer = new Stopwatch();
        /// <summary>
        /// Lua Api生成
        /// </summary>
        /// <param name="name">Zip名称</param>
        /// <param name="ls">类型信息列表</param>
        protected static void GenLuaApi(string name, List<Info> ls)
        {
            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);
            string[] fs = Directory.GetFiles(cacheDir, "*.lua");
            for (int i = 0; i < fs.Length; i++)
                File.Delete(fs[i]);

            for (int i = 0; i < ls.Count; i++)
            {
                StringBuilder sb = new StringBuilder();
                string className = ls[i].ClassName;
                var fileds = ls[i].Fields;
                for (int j = 0; j < fileds.Count; j++)
                    sb.AppendFormat("---@field public {0} {1}\n", fileds[j].Name, fileds[j].Type);
                if (string.IsNullOrEmpty(ls[i].InhertName))
                    sb.AppendFormat("---@class {0}\n", className);
                else
                    sb.AppendFormat("---@class {0} : {1}\n", className, ls[i].InhertName);
                sb.AppendLine("local m = {}");
                sb.AppendLine();
                var funcs = ls[i].Funcs;
                for (int j = 0; j < funcs.Count; j++)
                {
                    var overrides = funcs[j].Overload;
                    for (int k = 0; k < overrides.Count; k++)
                    {
                        var ps = overrides[k].Params;
                        StringBuilder paramls = new StringBuilder();
                        for (int l = 0; l < ps.Count; l++)
                        {
                            if (ps.Count - 1 == l)
                                paramls.AppendFormat("{0} : {1}", ps[l].Name, ps[l].Type);
                            else
                                paramls.AppendFormat("{0} : {1},", ps[l].Name, ps[l].Type);
                        }

                        sb.AppendFormat("---@overload fun({0}) : {1}\n", paramls.ToString(), overrides[k].ReturnType);
                    }
                    var cps = funcs[j].Params;
                    StringBuilder args = new StringBuilder();
                    for (int l = 0; l < cps.Count; l++)
                    {
                        sb.AppendFormat("---@param {0} {1}\n", cps[l].Name, cps[l].Type);
                        if (cps.Count - 1 == l)
                            args.AppendFormat("{0}", cps[l].Name);
                        else
                            args.AppendFormat("{0},", cps[l].Name);
                    }
                    if (!string.IsNullOrEmpty(funcs[j].ReturnType))
                        sb.AppendFormat("---@return {0}\n", funcs[j].ReturnType);
                    if (funcs[j].IsStatic)
                        sb.AppendFormat("function m.{0}({1})end\n", funcs[j].Name, args.ToString());
                    else
                        sb.AppendFormat("function m:{0}({1})end\n", funcs[j].Name, args.ToString());
                }
                string[] nodes = className.Split('.');
                StringBuilder combine = new StringBuilder();
                for (int j = 0; j < nodes.Length; j++)
                {
                    if (j == 0)
                    {
                        combine.AppendFormat("{0}", nodes[j]);
                        sb.AppendFormat("{0} = {{}}\n", combine);
                    }
                    else if (0 < j && j < nodes.Length - 1)
                    {
                        combine.AppendFormat(".{0}", nodes[j]);
                        sb.AppendFormat("{0} = {{}}\n", combine);
                    }
                    else
                    {
                        combine.AppendFormat(".{0}", nodes[j]);
                        sb.AppendFormat("{0} = m\n", combine);
                    }
                }
                sb.AppendFormat("return m\n", className);

                string path = string.Format("{0}/{1}.lua", cacheDir, className);
                File.WriteAllText(path, sb.ToString());
            }

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = winRAR;
            info.Arguments = string.Format("a -ep {0}/{1}.zip {2}", codeDir, name, cacheDir);
            //UnityEngine.Debug.Log(info.Arguments);
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = Application.platform == RuntimePlatform.WindowsEditor;
            info.ErrorDialog = true;
            Process pro = Process.Start(info);
            pro.WaitForExit();
        }

        protected static Info CollectedTypeInfo(Type type)
        {
            Info info = new Info();
            info.ClassName = type.FullName;
            if (type.IsClass)
            {
                info.InhertName = type.BaseType == null ? "" : type.BaseType.FullName;
                var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Static |
                    BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < fields.Length; i++)
                {
                    FieldInfo field = new FieldInfo()
                    {
                        Name = fields[i].Name,
                        Type = fields[i].FieldType.FullName,
                    };
                    info.Fields.Add(field);
                }

                var funcs = new Dictionary<string, FuncInfo>();
                var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static |
                    BindingFlags.Public | BindingFlags.InvokeMethod
                    | BindingFlags.Instance);
                for (int i = 0; i < methods.Length; i++)
                {
                    if (methods[i].Name.StartsWith("get_") || methods[i].Name.StartsWith("set_")) continue;

                    FuncInfo func = new FuncInfo();
                    func.Name = methods[i].Name;
                    func.ReturnType = methods[i].ReturnType.FullName;
                    func.IsStatic = methods[i].IsStatic;
                    var args = methods[i].GetParameters();
                    for (int j = 0; j < args.Length; j++)
                    {
                        FieldInfo arg = new FieldInfo()
                        {
                            Name = args[j].Name,
                            Type = args[j].ParameterType.FullName,
                        };
                        func.Params.Add(arg);
                    }

                    if (funcs.ContainsKey(methods[i].Name))
                    {
                        var overload = funcs[methods[i].Name];
                        overload.Overload.Add(overload);
                    }
                    else
                    {
                        info.Funcs.Add(func);
                        funcs.Add(methods[i].Name, func);
                    }
                }

                var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Static |
                    BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < properties.Length; i++)
                {
                    FieldInfo field = new FieldInfo()
                    {
                        Name = properties[i].Name,
                        Type = properties[i].PropertyType.FullName,
                    };
                    info.Fields.Add(field);
                }
            }
            else
            {
                var fields = type.GetFields(BindingFlags.Public);
                for (int i = 0; i < fields.Length; i++)
                {
                    FieldInfo field = new FieldInfo()
                    {
                        Name = fields[i].Name,
                        Type = fields[i].GetValue(null).ToString(),
                    };
                    info.Fields.Add(field);
                }
            }
            return info;
        }

        protected static void Start()
        {
            timer.Reset();
            timer.Start();
        }

        protected static void Stop()
        {
            timer.Stop();
            UnityEngine.Debug.LogFormat(">Generating API time: {0} ms", timer.ElapsedMilliseconds);
        }
    }
}

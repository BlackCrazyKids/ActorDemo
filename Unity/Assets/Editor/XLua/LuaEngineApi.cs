using System;
using UnityEditor;
using System.Collections.Generic;


namespace GameEditor
{
    public class LuaEngineApi : LuaApiGen
    {
        [MenuItem("XLua/Gen UnityEngine Api", false, 501)]
        static void GenFairyGUIApi()
        {
            Start();
            List<Info> infos = new List<Info>();
            var ls = ExportConfig.LuaCallCSharp;
            for (int i = 0; i < ls.Count; i++)
            {
                Info info = CollectedTypeInfo(ls[i]);
                infos.Add(info);
            }
            GenLuaApi("UnityEngineApi", infos);
            Stop();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Game
{
    public static class LuaHelper
    {
        public static LuaTable name_hash_map;
        public static Dictionary<int, string> hash_name_map = new Dictionary<int, string>();
        public static void Init()
        {
            name_hash_map = Client.LuaMgr.LuaEnv.NewTable();
        }
        public static void StringToHash(string name)
        {
            if (name != null)
            {
                var hashcode = Animator.StringToHash(name);
                name_hash_map[name] = hashcode;
                hash_name_map[hashcode] = name;
            }
        }
        public static bool HasScript(string viewName)
        {
            return Client.LuaMgr.HasScript(viewName);
        }
        public static void DestroyLauncherResource()
        {
            Launcher.Ins.Dispose();
        }
    }
}


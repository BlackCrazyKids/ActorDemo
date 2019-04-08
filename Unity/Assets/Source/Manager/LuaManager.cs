using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace Game
{
    public class LuaManager : IManager
    {
        public LuaManager() { }

        private LuaEnv _luaEnv;
        private List<string> _searchPaths = new List<string>();
        private XLuaDelegate _luaDelegate;
        public LuaEnv LuaEnv { get { return _luaEnv; } }
        public void Init()
        {
            _luaEnv = new LuaEnv();
            _luaEnv.AddLoader(CustomLoader);
            LuaHelper.Init();
        }
        public void Dispose()
        {
            if (_luaEnv != null)
            {
                try
                {
                    if (_luaDelegate != null)
                        _luaDelegate.Dispose();
                    _searchPaths.Clear();
                    _luaEnv.Dispose();
                    _luaEnv = null;
                }
                catch (Exception e)
                {
                    Debug.LogError(string.Format("xlua exception : {0}\n {1}", e.Message, e.StackTrace));
                }
            }
        }
        public void AddSearchPath(string path)
        {
            _searchPaths.Add(path);
        }

        public bool HasScript(string viewName)
        {
            return _searchPaths.Exists(s => s.Equals(viewName));
        }
        private byte[] CustomLoader(ref string filePath)
        {
            for (int i = 0; i < _searchPaths.Count; i++)
            {
                string fullPath = string.Format("{0}/{1}.lua", _searchPaths[i], filePath.Replace(".", "/")).ToLower();
                if (File.Exists(fullPath))
                    return File.ReadAllBytes(fullPath);
            }

            return null;
        }

        public void Tick()
        {
            if (_luaEnv != null && Time.frameCount % 100 == 0)
            {
                _luaEnv.GC();
                _luaEnv.FullGc();
            }
        }
        public void InitScripts()
        {
            var luaMain = GetTable("Main");
            luaMain.Get<Action>("Init")();

            var main = Client.Ins.gameObject;
            _luaDelegate = main.GetComponent<XLuaDelegate>();
            if (_luaDelegate == null)
                _luaDelegate = main.AddComponent<XLuaDelegate>();
            _luaDelegate.Init(luaMain);
            luaMain.Dispose();
        }
        public LuaTable GetTable(string name)
        {
            string require = string.Format("require '{0}'", name);
            _luaEnv.DoString(require, name, _luaEnv.Global);
            return _luaEnv.Global.GetInPath<LuaTable>(name);
        }
        public T GetLuaFunc<T>(string name)
        {
            return _luaEnv.Global.Get<T>(name);
        }
    }

}

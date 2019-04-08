using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using XLua;

namespace Game
{
    public class XLuaDelegate : MonoBehaviour
    {
        Action<float, float> luaUpdate;
        Action luaLateUpdate;
        Action<float> luaFixedUpdate;
        Action luaDestroy;

        float time = -1;

        public void Init(LuaTable luaMain)
        {
            luaUpdate += luaMain.Get<Action<float, float>>("Update");
            luaLateUpdate += luaMain.Get<Action>("LateUpdate");
            luaFixedUpdate += luaMain.Get<Action<float>>("FixedUpdate");
            luaDestroy += luaMain.Get<Action>("OnDestroy");
        }

        public void Dispose()
        {
            if (luaDestroy != null)
                luaDestroy();

            luaUpdate = null;
            luaLateUpdate = null;
            luaFixedUpdate = null;
            luaDestroy = null;
        }

        void Update()
        {
            Client.LuaMgr.Tick();

            if (luaUpdate != null)
                luaUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void LateUpdate()
        {
            if (luaUpdate != null)
                luaLateUpdate();
        }

        void FixedUpdate()
        {
            if (luaUpdate != null)
                luaFixedUpdate(Time.fixedDeltaTime);
        }
    }
}
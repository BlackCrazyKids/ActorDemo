using System;
using UnityEngine;

namespace Game
{
    public class Client : MonoBehaviour
    {
        public static Client Ins { get; private set; }

        public static ServerListManager ServerMgr = new ServerListManager();
        public static UpdateManager UpdateMgr = new UpdateManager();
        public static AtlasManager AtlasMgr = new AtlasManager();
        public static ResourceManager ResMgr = new ResourceManager();
        public static LuaManager LuaMgr = new LuaManager();
        public static GameManager GameMgr = new GameManager();

        public static SceneManager SceneMgr = new SceneManager();
        public static PoolManager PoolMgr = new PoolManager();
        public static NetworkManager NetworkMgr = new NetworkManager();


        void Awake()
        {
            //Cfg.CfgManager.ConfigDir = Application.dataPath + "/../../GamePlayer/Data/config/csv/";
            //Cfg.CfgManager.LoadAll();          

            Ins = this;
            DontDestroyOnLoad(gameObject);

            Util.SetResolution(ConstSetting.Resolution);
            Application.targetFrameRate = ConstSetting.FrameRate;
            QualitySettings.blendWeights = ConstSetting.Blend;
            Screen.sleepTimeout = ConstSetting.SleepTime;

            GameMgr.Init();

            Debug.LogError(Util.DataPath);
        }

        void Update()
        {
            ResMgr.Update();
            NetworkMgr.Update();
            LuaMgr.Update();
        }
        void FixedUpdate()
        {
            LuaMgr.FixedUpdate();
        }
        void LateUpdate()
        {
            LuaMgr.LateUpdate();
        }
        void OnDestroy()
        {
            NetworkMgr.Dispose();
            ServerMgr.Dispose();
            UpdateMgr.Dispose();
            PoolMgr.Dispose();
            SceneMgr.Dispose();
            ResMgr.Dispose();
            LuaMgr.Dispose();
            GameMgr.Dispose();
            AtlasMgr.Dispose();

            SceneMgr = null;
            NetworkMgr = null;
            ServerMgr = null;
            UpdateMgr = null;
            ResMgr = null;
            GameMgr = null;
            AtlasMgr = null;
            PoolMgr = null;
            LuaMgr = null;

            Ins = null;
        }

        private void OnGUI()
        {
            Launcher.Ins.OnGUI();
        }
    }
}
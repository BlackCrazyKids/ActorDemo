using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    public class ServerListManager : Manager
    {
        /// <summary>
        /// 各渠道App下载链接配置文件
        /// </summary>
        public string UpdateAppUrl { get { return _config.UpdateAppUrl; } }
        /// <summary>
        /// 资源更新列表
        /// </summary>
        public List<string> UpdateResServer { get { return _config.UpdateResServer; } }
        /// <summary>
        /// 服务器区服列表
        /// </summary>
        public string ServerTable { get { return _serverTable; } }

        internal class UrlConfig
        {
            public string UpdateAppUrl;
            public List<string> UpdateResServer;

            public string AndroidServerListUrl;
            public string IosServerListUrl;
            public string PCServerListUrl;
        }
        private UrlConfig _config;
        private string _serverTable;

        public override void Dispose() { _config = null; }
        public IEnumerator GetServerList()
        {
            string file = string.Format("{0}config/{1}", Util.DataPath, ConstSetting.UrlConfig);
            string json = File.ReadAllText(file);
            _config = JsonUtility.FromJson<UrlConfig>(json);
            if (_config == null)
            {
                Debug.LogErrorFormat("本地未配置{0}文件.", ConstSetting.UrlConfig);
                yield break;
            }

            string url = "";
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    url = _config.AndroidServerListUrl;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    url = _config.IosServerListUrl;
                    break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    url = _config.PCServerListUrl;
                    break;
                default:
                    Debug.LogErrorFormat("URL配置中未定义平台{0}!", Application.platform);
                    break;
            }
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
                Debug.LogErrorFormat("无法正常加载{0}配置文件.\n{1}", url, www.error);
            else
                _serverTable = www.text;
        }
    }
}


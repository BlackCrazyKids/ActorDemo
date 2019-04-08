using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    internal enum LaunchState
    {
        Launcher,                    //启动界面,动态字体加载后再显示界面

        CheckDevice,                 //SDK初始化完成
        CheckUnzipData,              //并解压脚本和配置文件
        GetServerList,               //加载服务器列表
        CheckVersion,                //检查版本,更新App和资源
        DownloadHotfixRes,           //下载热更资源
        PreloadAssets,               //开始预加载资源,重新加载字体
        InitScripts,                 //初始化lua,并开始监听login场景是否加载成功
        StartGame,                   //进入游戏登入场景

        //--错误提示
        MemeryNotEnough,             //检查设备内存大小
        Offline,                     //网络断线
        DownloadVersionFailed,       //无法正常下载Version信息
        DownloadMD5TableFailed,      //无法正常下载MD5Table信息
        ServerListFailed,            //获取服务器列表失败
    }

    class Launcher
    {
        public static Launcher Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new Launcher();
                return _ins;
            }
        }

        private static Launcher _ins;
        private Dictionary<LaunchState, string> _launcher = new Dictionary<LaunchState, string>()
        {
            {LaunchState.CheckDevice                                 ,  "检查设备"},
            {LaunchState.CheckUnzipData                              ,  "检查解压数据"},
            {LaunchState.GetServerList                               ,  "加载服务器列表"},
            {LaunchState.CheckVersion                                ,  "检查版本"},
            {LaunchState.DownloadHotfixRes                           ,  "下载热更资源"},
            {LaunchState.PreloadAssets                               ,  "预加载资源"},
            {LaunchState.InitScripts                                 ,  "初始化Lua脚本"},
            {LaunchState.StartGame                                   ,  "开始游戏"},

            {LaunchState.MemeryNotEnough                             ,  "内存不足"},
            {LaunchState.Offline                                     ,  "网络断线"},
            {LaunchState.DownloadVersionFailed                       ,  "无法正常下载Version信息"},
            {LaunchState.DownloadMD5TableFailed                      ,  "无法正常下载MD5Table信息"},
            {LaunchState.ServerListFailed                            ,  "服务器列表加载失败"},//区服列表信息
        };

        public IEnumerator StartLaunch()
        {
            yield return null;
            //--加载字体
            //--加载界面
        }

        /// <summary>
        /// 释放启动过程中无用资源
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// 刷新启动器状态
        /// </summary>
        /// <param name="state">启动器状态</param>
        /// <param name="value">启动进度百分比,保留小数点后2位.格式{0:N2}</param>
        public void SetLaunchState(LaunchState state, float value)
        {
            string name = "";
            if (!_launcher.TryGetValue(state, out name))
            {
                Debug.LogErrorFormat("_launcher[{0}] = null ~", state);
                return;
            }
            //--设置状态标题
            //--设置百分比

            //Debug.LogFormat("[{0}]{1}\t{2}", state, name, value);
            _title = name;
            _progress = value;
        }
        /// <summary>
        /// 设置启动器子标题
        /// </summary>
        /// <param name="subtitle"></param>
        public void SetSubtitle(string subtitle)
        {

        }
        public void DisableSubtitle()
        {

        }

        string _title = "";
        float _progress = 0f;

        public void OnGUI()
        {
            float height = Screen.height * 0.1f;
            GUILayout.Label(_title, GUILayout.Height(height));
            GUILayout.Label(_progress.ToString("P"), GUILayout.Height(height), GUILayout.Width(200));
        }
    }
}

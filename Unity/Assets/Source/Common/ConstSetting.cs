using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //GAME_SIMULATION:游戏模拟正式操作宏,例如热更,解压等操作

    public class ConstSetting
    {
        #region 游戏配置
        public const int FrameRate                    = 30;           //游戏帧率

#if UNITY_IPHONE
        public const int Resolution                   = 1080;         //分辨率配置
#else
        public const int Resolution                   = 810;          //分辨率配置
#endif
        public const BlendWeights Blend               = BlendWeights.TwoBones;
        public const int SleepTime                    = SleepTimeout.NeverSleep;
        #endregion


        #region 文件配置
        /// <summary>
        /// 格式:a.r.t;
        /// a:app版本号
        /// r:资源版本号
        /// t:当前版本资源构建时间,yymmddhh年月日时
        /// </summary>
        public static readonly string ResVersionFile  = "version.txt";
        /// <summary>
        /// 资源版本信息记录格式:path,md5,pathtype,size
        /// path:资源相对路径
        /// md5:文件MD5信息
        /// pathtype:路径类型.r:流目录[只读];rw:持续化目录[读写]
        /// size:文件大小,单位字节
        /// </summary>
        public static readonly string ResMD5File      = "resmd5.txt";
        /// <summary>
        /// 当前已下载资源名称,中断后不重复下载
        /// </summary>
        public static readonly string HasDownloadFile = "hasdownload.txt";
        /// <summary>
        /// 服务器Url配置
        /// </summary>
        public static readonly string UrlConfig       = "urlconfig.json";
        #endregion


#if UNITY_EDITOR && !GAME_SIMULATION
        public static readonly string LuaDir          = Util.DataPath + "../../Code/Scripts";
#else
        public static readonly string LuaDir          = Util.DataPath + "Scripts"; 
#endif
        public const string LuaMain                   = "Main";
    }
}


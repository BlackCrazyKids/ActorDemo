using UnityEngine;
using System;

namespace Game.Platform
{
    public enum NetworkMode
    {
        offline, WiFi, Mobile
    }

    public abstract class Interface
    {
        private static Interface _instance = null;

        protected Interface()
        {
        }

        public static Interface Instance { get { return _instance; } }

        public static void Create()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    {

                    }
                    break;
                case RuntimePlatform.IPhonePlayer:
                    {

                    }
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                    {

                    }
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                    {
                        _instance = new CustomInterface();
                    }
                    break;
                default:
                    {
                        _instance = new CustomInterface();
                    }
                    break;
            }
        }

        public virtual void Init()
        {
            throw new ArgumentNullException("interface");
        }
        public virtual void Release()
        {
        }
        public virtual int GetAvailableMemory()
        {
            return 1000;
        }

        public virtual string Install(string file)
        {
            return string.Empty;
        }

        // 渠道类型
        public virtual int GetSDKPlatform()
        {
            return 0;       //0 - 测试版 其他平台看枚举 EPlatformType
        }

        public virtual string GetSDKPlatformName()
        {
            return "";
        }

        public virtual NetworkMode GetNetWorkMode()
        {
            return NetworkMode.WiFi;
        }

        public virtual void InitSDK()
        {
        }

        public virtual bool EscapeSDK()
        {
            return false;
        }

        public virtual bool IsSDKFinished()
        {
            return true;
        }
        public virtual void Login()
        {
        }
        public virtual void Logout()
        {
        }

        public virtual void SubmitUserInfo(string type, string roleid, string rolename, string level, int zoneid, string zonename, string createtime)
        {
        }

        public virtual void Pay(string roleid, string rolename, string level, string vip, string orderId, int serverid, string servername, int cash, string pid, string desc)
        {
        }
        //0:未登录
        //1:临时用户登录
        //2:正式用户登录
        public virtual int GetLoginStatus()
        {
            return 1;
        }
        public virtual long GetUserId()
        {
            return 0;
        }
        public virtual string GetUserName()
        {
            return "";
        }
        public virtual string GetToken()
        {
            return "";
        }
        public virtual void SetToken(string token, int deadline)
        {
        }
        public virtual void SetUserInfo(string userId, string userName)
        {
        }

        public virtual void CheckGuestAccount()
        {
        }
        public virtual void ChangeAccount()
        {
        }
        public virtual void StartAccountHome()
        {
        }
        public virtual void StartForum()
        {
        }
        public virtual void ShowToolBar()
        {
        }
        public virtual void HideToolBar()
        {
        }
        //是否越狱版，1为越狱，只对IOS有效
        public virtual bool IsCydia()
        {
            return true;
        }
        // 是否为平板设备。用来判别显示Pad-UI或Mini-UI
        // 在iOS上检测准确可靠。Android上不够准确，默认设置为手机版。
        public virtual bool IsPad()
        {
            return true;
        }
        public virtual bool IsHdVersion()
        {
            return false;
        }
        public virtual void StartWebView(string strURL)
        {
        }
        public virtual void OpenUrl(string url)
        {
        }
        public virtual void SendWeibo(string content)
        {
        }
        public virtual void SendWeChat(string content)
        {
        }
        /// <summary>
        /// 获取设备内存信息,单位:字节
        /// </summary>
        /// <returns></returns>
        public virtual long GetMemInfo()
        {
            return 0;
        }

        public virtual string GetUDID()
        {
            return "";
        }

        ///语音小纸条
        public virtual int Voice_GetFileLength()
        {
            return -1;
        }

        public virtual int Voice_GetVolume()
        {
            return 0;
        }

        public virtual bool Voice_Start(bool record, string filename, bool recognize)
        {
            return false;
        }

        public virtual void Voice_Stop()
        {
            return;
        }

        public virtual string Voice_GetAsrText()
        {
            return "";
        }


        public virtual void InitLogSystem(string filename)
        {

        }

        public virtual void MonitorBatteryState()
        {

        }

        public virtual void OpenAlbum(string fileNameBig, string fileNameSmall, int scaleFactorBig, int scaleFactorSmall, string resultCode)
        {

        }

        public virtual void StartScanCode()
        {

        }

    }
}

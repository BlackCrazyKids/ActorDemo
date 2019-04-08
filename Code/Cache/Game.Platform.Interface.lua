---@field public Instance Game.Platform.Interface
---@class Game.Platform.Interface : System.Object
local m = {}

---@return System.Void
function m.Create()end
---@return System.Void
function m:Init()end
---@return System.Void
function m:Release()end
---@return System.Int32
function m:GetAvailableMemory()end
---@param file System.String
---@return System.String
function m:Install(file)end
---@return System.Int32
function m:GetSDKPlatform()end
---@return System.String
function m:GetSDKPlatformName()end
---@return Game.Platform.NetworkMode
function m:GetNetWorkMode()end
---@return System.Void
function m:InitSDK()end
---@return System.Boolean
function m:EscapeSDK()end
---@return System.Boolean
function m:IsSDKFinished()end
---@return System.Void
function m:Login()end
---@return System.Void
function m:Logout()end
---@param type System.String
---@param roleid System.String
---@param rolename System.String
---@param level System.String
---@param zoneid System.Int32
---@param zonename System.String
---@param createtime System.String
---@return System.Void
function m:SubmitUserInfo(type,roleid,rolename,level,zoneid,zonename,createtime)end
---@param roleid System.String
---@param rolename System.String
---@param level System.String
---@param vip System.String
---@param orderId System.String
---@param serverid System.Int32
---@param servername System.String
---@param cash System.Int32
---@param pid System.String
---@param desc System.String
---@return System.Void
function m:Pay(roleid,rolename,level,vip,orderId,serverid,servername,cash,pid,desc)end
---@return System.Int32
function m:GetLoginStatus()end
---@return System.Int64
function m:GetUserId()end
---@return System.String
function m:GetUserName()end
---@return System.String
function m:GetToken()end
---@param token System.String
---@param deadline System.Int32
---@return System.Void
function m:SetToken(token,deadline)end
---@param userId System.String
---@param userName System.String
---@return System.Void
function m:SetUserInfo(userId,userName)end
---@return System.Void
function m:CheckGuestAccount()end
---@return System.Void
function m:ChangeAccount()end
---@return System.Void
function m:StartAccountHome()end
---@return System.Void
function m:StartForum()end
---@return System.Void
function m:ShowToolBar()end
---@return System.Void
function m:HideToolBar()end
---@return System.Boolean
function m:IsCydia()end
---@return System.Boolean
function m:IsPad()end
---@return System.Boolean
function m:IsHdVersion()end
---@param strURL System.String
---@return System.Void
function m:StartWebView(strURL)end
---@param url System.String
---@return System.Void
function m:OpenUrl(url)end
---@param content System.String
---@return System.Void
function m:SendWeibo(content)end
---@param content System.String
---@return System.Void
function m:SendWeChat(content)end
---@return System.Int64
function m:GetMemInfo()end
---@return System.String
function m:GetUDID()end
---@return System.Int32
function m:Voice_GetFileLength()end
---@return System.Int32
function m:Voice_GetVolume()end
---@param record System.Boolean
---@param filename System.String
---@param recognize System.Boolean
---@return System.Boolean
function m:Voice_Start(record,filename,recognize)end
---@return System.Void
function m:Voice_Stop()end
---@return System.String
function m:Voice_GetAsrText()end
---@param filename System.String
---@return System.Void
function m:InitLogSystem(filename)end
---@return System.Void
function m:MonitorBatteryState()end
---@param fileNameBig System.String
---@param fileNameSmall System.String
---@param scaleFactorBig System.Int32
---@param scaleFactorSmall System.Int32
---@param resultCode System.String
---@return System.Void
function m:OpenAlbum(fileNameBig,fileNameSmall,scaleFactorBig,scaleFactorSmall,resultCode)end
---@return System.Void
function m:StartScanCode()end
Game = {}
Game.Platform = {}
Game.Platform.Interface = m
return m

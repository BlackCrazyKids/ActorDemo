FairyGUI = CS.FairyGUI
EventContext  = FairyGUI.EventContext
EventListener = FairyGUI.EventListener
EventDispatcher = FairyGUI.EventDispatcher
InputEvent = FairyGUI.InputEvent
NTexture = FairyGUI.NTexture
Container = FairyGUI.Container
Image = FairyGUI.Image
Stage = FairyGUI.Stage
Controller = FairyGUI.Controller
---@type FairyGUI.GObject
GObject = FairyGUI.GObject
---@type FairyGUI.GGraph
GGraph = FairyGUI.GGraph
---@type FairyGUI.GGroup
GGroup = FairyGUI.GGroup
---@type FairyGUI.GImage
GImage = FairyGUI.GImage
---@type FairyGUI.GLoader
GLoader = FairyGUI.GLoader
PlayState = FairyGUI.PlayState
GMovieClip = FairyGUI.GMovieClip
TextFormat = FairyGUI.TextFormat
---@type FairyGUI.GTextField
GTextField = FairyGUI.GTextField
GRichTextField = FairyGUI.GRichTextField
GTextInput = FairyGUI.GTextInput
---@type FairyGUI.GComponent
GComponent = FairyGUI.GComponent
---@type FairyGUI.GList
GList = FairyGUI.GList
---@type FairyGUI.GRoot
GRoot = FairyGUI.GRoot
---@type FairyGUI.GLabel
GLabel = FairyGUI.GLabel
---@type FairyGUI.GButton
GButton = FairyGUI.GButton
GComboBox = FairyGUI.GComboBox
GProgressBar = FairyGUI.GProgressBar
GSlider = FairyGUI.GSlider
PopupMenu = FairyGUI.PopupMenu
ScrollPane = FairyGUI.ScrollPane
Transition = FairyGUI.Transition
---@type FairyGUI.UIPackage
UIPackage = FairyGUI.UIPackage
---@type FairyGUI.Window
Window = FairyGUI.Window
GObjectPool = FairyGUI.GObjectPool
Relations = FairyGUI.Relations
RelationType = FairyGUI.RelationType
UIPanel = FairyGUI.UIPanel
UIPainter = FairyGUI.UIPainter
TypingEffect = FairyGUI.TypingEffect

local setmetatable = setmetatable
local getmetatable = getmetatable
local rawget = rawget
local rawset = rawset
local internal = {}

FGUI = {}

--[[
用于继承FairyGUI的Window类，例如
WindowBase = fgui.window_class()
同时派生的Window类可以被继续被继承，例如
MyWindow = fgui.window_class(WindowBase)
可以重写的方法有（与Window类里的同名方法含义完全相同）
OnInit
DoHideAnimation
DoShowAnimation
OnShown
OnHide
]]
function FGUI.WindowClass(base)
	local o = {}

	local base = base or FairyGUI.LuaWindow
	setmetatable(o, base)

	o.__index = o
	o.base = base

	o.New = function(...)
		local t = {}
		setmetatable(t, o)

		local ins = FairyGUI.LuaWindow()
		xutil.state(ins, t)
		ins:ConnectLua(t)
		t.EventDelegates = {}
		if t.ctor then
			t.ctor(ins,...)
		end

		return ins
	end

	return o
end

--[[
注册组件扩展，例如
fgui.register_extension(UIPackage.GetItemURL("包名","组件名"), my_extension)
my_extension的定义方式见fgui.extension_class
]]
function FGUI.RegisterExtension(url, extension)
	local base = extension.base
	if base==GComponent then base=FairyGUI.GLuaComponent
	elseif base==GLabel then base=FairyGUI.GLuaLabel
	elseif base==GButton then base=FairyGUI.GLuaButton
	elseif base==GSlider then base=FairyGUI.GLuaSlider
	elseif base==GProgressBar then base=FairyGUI.GLuaProgressBar
	elseif base==GComboBox then base=FairyGUI.GLuaComboBox
	else
		print("invalid extension base: "..base)
		return
	end
	FairyGUI.LuaUIHelper.SetExtension(url, typeof(base), extension.Extend)
end

--[[
用于继承FairyGUI原来的组件类，例如
MyComponent = fgui.extension_class(GComponent)
function MyComponent:ctor() --当组件构建完成时此方法被调用
	print(self:GetChild("n1"))
end
]]
function FGUI.ExtensionClass(base)
	local o = {}
	o.__index = o

	o.base = base or GComponent

	o.Extend = function(ins)
		local t = {}
		setmetatable(t, o)
		xutil.state(ins, t)
		ins:ConnectLua(t)
		t.EventDelegates = {}
		if t.ctor then
			t.ctor(ins)
		end

		return t
	end

	return o
end

--[[
FairyGUI自带的定时器，有需要可以使用。例如：
每秒回调，无限次数
fgui.add_timer(1,0,callback)
可以带self参数
fgui.add_timer(1,0,callback,self)
可以自定义参数
fgui.add_timer(1,0,callback,self,data)

！！警告，定时器回调函数不要与UI事件回调函数共用
]]
function FGUI.AddTimer(interval, repeatCount, func, obj, param)
	local callbackParam
	if param~=nil then
		if obj==nil then
			callbackParam=param
		else
			callbackParam=obj
			func = function(p)
				func(p, param)
			end
		end
	end

	local delegate = internal.GetDelegate(func, obj, true, FairyGUI.TimerCallback)
	FairyGUI.Timers.inst:Add(interval, repeatCount, delegate, callbackParam)
end

function FGUI.RemoveTimer(func, obj)
	local delegate = internal.GetDelegate(func, obj, false)
	if delegate~=nil then
		FairyGUI.Timers.inst:Remove(delegate)
	end
end

---以下是内部使用的代码---
--这里建立一个c# delegate到lua函数的映射，是为了支持self参数，和方便地进行remove操作
internal.EventDelegates = {}
setmetatable(internal.EventDelegates, { __mode = "k"})
function internal.GetDelegate(func, obj, createIfNone, delegateType)
	local mapping
	if obj~=nil then
		mapping = obj.EventDelegates
		if mapping==nil then
			mapping = {}
			setmetatable(mapping, {__mode = "k"})
			obj.EventDelegates = mapping
		end
	else
		mapping = internal.EventDelegates
	end

	local delegate = mapping[func]
	if createIfNone and delegate==nil then
		local realFunc
		if obj~=nil then
			realFunc = function(context)
				func(obj,context)
			end
		else
			realFunc = func
		end
		delegateType = delegateType or FairyGUI.EventCallback1
		delegate = delegateType(realFunc)

		mapping[func] = delegate
	end

	return delegate
end

--将EventListener.Add和EventListener.Remove重新进行定义，以适应lua的使用习惯
local EventListener_mt = getmetatable(EventListener)
local oldListenerAdd = rawget(EventListener_mt, 'Add')
local oldListenerRemove = rawget(EventListener_mt, 'Remove')
local oldListenerSet = rawget(EventListener_mt, 'Set')
local oldListenerAddCapture = rawget(EventListener_mt, 'AddCapture')
local oldListenerRemoveCapture = rawget(EventListener_mt, 'RemoveCapture')

local function AddListener(listener, func, obj)
	local delegate = internal.GetDelegate(func, obj, true)
	oldListenerAdd(listener, delegate)
end

local function RemoveListener(listener, func, obj)
	local delegate = internal.GetDelegate(func, obj, false)
	if delegate ~= nil then
		oldListenerRemove(listener, delegate)
	end
end

local function SetListener(listener, func, obj)
	if func==nil then
		oldListenerSet(listener, nil)
	else
		local delegate = internal.GetDelegate(func, obj, true)
		oldListenerSet(listener, delegate)
	end
end

local function AddListenerCapture(listener, func, obj)
	local delegate = internal.GetDelegate(func, obj, true)
	oldListenerAddCapture(listener, delegate)
end

local function RemoveListenerCapture(listener, func, obj)
	local delegate = internal.GetDelegate(func, obj, false)
	if delegate ~= nil then
		oldListenerRemoveCapture(listener, delegate)
	end
end

rawset(EventListener_mt, 'Add', AddListener)
rawset(EventListener_mt, 'Remove', RemoveListener)
rawset(EventListener_mt, 'Set', SetListener)
rawset(EventListener_mt, 'AddCapture', AddListenerCapture)
rawset(EventListener_mt, 'RemoveCapture', RemoveListenerCapture)
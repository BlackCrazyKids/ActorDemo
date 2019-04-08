---@field public compensateSensors System.Boolean
---@field public isGyroAvailable System.Boolean
---@field public gyro UnityEngine.Gyroscope
---@field public mousePosition UnityEngine.Vector3
---@field public mouseScrollDelta UnityEngine.Vector2
---@field public mousePresent System.Boolean
---@field public simulateMouseWithTouches System.Boolean
---@field public anyKey System.Boolean
---@field public anyKeyDown System.Boolean
---@field public inputString System.String
---@field public acceleration UnityEngine.Vector3
---@field public accelerationEvents UnityEngine.AccelerationEvent[]
---@field public accelerationEventCount System.Int32
---@field public touches UnityEngine.Touch[]
---@field public touchCount System.Int32
---@field public eatKeyPressOnTextFieldFocus System.Boolean
---@field public touchPressureSupported System.Boolean
---@field public stylusTouchSupported System.Boolean
---@field public touchSupported System.Boolean
---@field public multiTouchEnabled System.Boolean
---@field public location UnityEngine.LocationService
---@field public compass UnityEngine.Compass
---@field public deviceOrientation UnityEngine.DeviceOrientation
---@field public imeCompositionMode UnityEngine.IMECompositionMode
---@field public compositionString System.String
---@field public imeIsSelected System.Boolean
---@field public compositionCursorPos UnityEngine.Vector2
---@field public backButtonLeavesApp System.Boolean
---@class UnityEngine.Input : System.Object
local m = {}

---@param axisName System.String
---@return System.Single
function m.GetAxis(axisName)end
---@param axisName System.String
---@return System.Single
function m.GetAxisRaw(axisName)end
---@param buttonName System.String
---@return System.Boolean
function m.GetButton(buttonName)end
---@param buttonName System.String
---@return System.Boolean
function m.GetButtonDown(buttonName)end
---@param buttonName System.String
---@return System.Boolean
function m.GetButtonUp(buttonName)end
---@overload fun(name : System.String) : System.Boolean
---@param name System.String
---@return System.Boolean
function m.GetKey(name)end
---@overload fun(name : System.String) : System.Boolean
---@param name System.String
---@return System.Boolean
function m.GetKeyDown(name)end
---@overload fun(name : System.String) : System.Boolean
---@param name System.String
---@return System.Boolean
function m.GetKeyUp(name)end
---@return System.String[]
function m.GetJoystickNames()end
---@param joystickName System.String
---@return System.Boolean
function m.IsJoystickPreconfigured(joystickName)end
---@param button System.Int32
---@return System.Boolean
function m.GetMouseButton(button)end
---@param button System.Int32
---@return System.Boolean
function m.GetMouseButtonDown(button)end
---@param button System.Int32
---@return System.Boolean
function m.GetMouseButtonUp(button)end
---@return System.Void
function m.ResetInputAxes()end
---@param index System.Int32
---@return UnityEngine.AccelerationEvent
function m.GetAccelerationEvent(index)end
---@param index System.Int32
---@return UnityEngine.Touch
function m.GetTouch(index)end
UnityEngine = {}
UnityEngine.Input = m
return m

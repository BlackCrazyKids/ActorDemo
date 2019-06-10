local Protocol = Protocol
---@class Message
local Message = {}
function Message.SendLogin(openid, token, device)
	return Message.__send(Protocol.Login, {openid = openid, token = token, device = device})
end
function Message.SendTaskList(taskid)
	return Message.__send(Protocol.TaskList, {taskid = taskid})
end
return Message
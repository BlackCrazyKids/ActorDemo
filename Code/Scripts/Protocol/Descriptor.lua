return [[
message CLogin {
  optional string openid = 1;
  optional string token = 2;
  optional string device = 3;
}
message SLogin {
  optional int32 error = 1;
  optional string token = 2;
  optional string device_info = 3; 
  repeated string contacts = 4;
}
message CTaskList {
    optional int32 taskid = 1;
}
message STaskList{
    optional int32 completeid = 1;        
}

message NLogoutRoom {
    optional string uid = 1;
    optional int32 level = 2;
}
]]
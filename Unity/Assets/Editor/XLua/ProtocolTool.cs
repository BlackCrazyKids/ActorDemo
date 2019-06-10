using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    public class ProtocolTool
    {
        static string c2s_dir = Util.DataPath + "../../Protocol/C2S";
        static string notice_file = Util.DataPath + "../../Protocol/Notice.proto";
        static string protoc_file = Util.DataPath + "../../Protocol/Protocol.json";   //暂时未定,具体使用格式 [Type = 编号]


        [MenuItem("XLua/Gen Protocol", false, 1000)]
        static void GenProtocol()
        {
            LuaManager luaMgr = new LuaManager();
            Action<string, string> gen = null;
            XLua.LuaTable protocol = null;
            try
            {
                luaMgr.Init();
                luaMgr.AddSearchPath(EditorConst.EditorDir);
                luaMgr.AddSearchPath(EditorConst.ScriptDir);
                protocol = luaMgr.GetTable("Protocol");

                List<string> files = new List<string>(Directory.GetFiles(c2s_dir));
                files.Add(notice_file);
                StringBuilder des = new StringBuilder();
                for (int i = 0; i < files.Count; i++)
                {
                    string content = File.ReadAllText(files[i]);
                    des.AppendFormat("{0}\n", content);
                }

                string rawProto = File.ReadAllText(protoc_file);
                string desString = string.Format("return [[\n{0}]]", des.ToString());
                gen = protocol.Get<Action<string, string>>("GenProtocol");
                gen(desString, rawProto);
                gen = null;

                string messageString = protocol.Get<Func<string>>("GetMessageString")();
                string protoString = protocol.Get<Func<string>>("GetProtoString")();
                string dir = EditorConst.ScriptDir + "/Protocol/";
                File.WriteAllText(dir + "Descriptor.lua", desString);
                File.WriteAllText(dir + "Protocol.lua", protoString);
                File.WriteAllText(dir + "Message.lua", messageString);
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("{0}\n{1}", e.Message, e.StackTrace);
            }
            finally
            {
                protocol.Dispose();
                protocol = null;
                luaMgr.Dispose();
            }
        }
    }
}
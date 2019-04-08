using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolFactory
{
    class Program
    {
        public const string HELP = "-help";
        public const string RESVERSION = "-resversion";//计算路径下文件md5,格式:path,md5,size

        static void Usage()
        {
            Console.WriteLine("-help 打印指令说明");
            Console.WriteLine("-resversion 计算路径下文件md5,格式:path,md5,size");
        }

        static void Main(string[] args)
        {
            var cmdArgs = new Dictionary<string, List<string>>();
            string currentArg = "";
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.StartsWith("-"))
                {
                    currentArg = arg;
                    if (!cmdArgs.ContainsKey(arg))
                        cmdArgs.Add(arg, new List<string>());
                    else
                        Util.LogWarning(string.Format("忽略重复命令{0},以第一个为主.", arg));
                }
                else if (cmdArgs.ContainsKey(currentArg))
                    cmdArgs[currentArg].Add(arg);
                else
                    Util.LogWarning(string.Format("异常参数{0},未正常读取.", arg));
            }

            try
            {
                foreach (var cmd in cmdArgs)
                {
                    string cmdName = cmd.Key;
                    switch (cmdName)
                    {
                        case HELP:
                            Usage();
                            break;
                        case RESVERSION:
                            if (!CheckArgList(cmdName, cmd.Value)) break;
                            new ResVersion(cmd.Value[0], cmd.Value[1]);
                            break;
                        default:
                            Util.LogErrorFormat("应用无法执行此命令{0}.", cmdName);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Util.LogError("初始化命令参数失败!");
                Util.LogErrorFormat("{0}\r\n{1}", e.Message, e.StackTrace);
            }

            Console.ReadKey();
        }

        static bool CheckArgList(string cmdName, List<string> list)
        {
            if (list.Count == 0)
            {
                Util.LogErrorFormat("[{0}]命令参数未配置!", cmdName);
                return false;
            }
            return true;
        }

    }
}

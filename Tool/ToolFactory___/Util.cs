using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolFactory
{
    class Util
    {
        /// <summary>
        /// 标准化路径,'\'转化'/'
        /// </summary>
        /// <param name="path">资源路径</param>
        public static string StandardlizePath(string path, bool toLower = true)
        {
            string pathReplace = path.Replace(@"\", @"/");
            return toLower ? pathReplace.ToLower() : pathReplace;
        }

        public static void Log(string logString, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(logString);
        }
        public static void LogWarning(string warningString)
        {
            Log(warningString, ConsoleColor.Yellow);
        }
        public static void LogError(string errorString)
        {
            Log(errorString, ConsoleColor.Red);
        }
        public static void LogFormat(string format, params string[] logString)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(format, logString);
        }
        public static void LogWarningFormat(string format, params string[] warningString)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, warningString);
        }
        public static void LogErrorFormat(string format, params string[] errorString)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, errorString);
        }
    }
}

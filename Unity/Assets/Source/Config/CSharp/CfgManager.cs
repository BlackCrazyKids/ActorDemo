using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Cfg
{
	/// <summary>
	/// 数据管理类
	/// <summary>
	public class CfgManager
	{
		/// <summary>
		/// 配置文件文件夹路径
		/// <summary>
		public static string ConfigDir;
		
		public static readonly Dictionary<int, Cfg.AllType.AllClass> AllClass = new Dictionary<int, Cfg.AllType.AllClass>();
		public static readonly Dictionary<int, Cfg.AllType.Test> Test = new Dictionary<int, Cfg.AllType.Test>();
		
		public static int _row;
		/// <summary>
		/// constructor参数为指定类型的构造函数
		/// <summary>
		public static List<T> Load<T>(string path, Func<DataStream, T> constructor)
		{
			if (!File.Exists(path))
			{
				UnityEngine.Debug.LogError(path + "配置路径不存在");
				return new List<T>();
			}
			DataStream data = new DataStream(path, Encoding.UTF8);
			List<T> list = new List<T>();
			int length = data.GetInt();			for (int i = 0; i < length; i++)
			{
				_row = i;
				list.Add(constructor(data));
			}
			return list;
		}
		
		public static void LoadAll()
		{
			string path = "Data Path Empty";
			try
			{
				path = ConfigDir + "alltype/allclass.data";
				var allclasss = Load(path, (d) => new Cfg.AllType.AllClass(d));
				allclasss.ForEach(v => AllClass.Add(v.ID, v));
				path = ConfigDir + "alltype/test.data";
				var tests = Load(path, (d) => new Cfg.AllType.Test(d));
				tests.ForEach(v => Test.Add(v.TID, v));
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogErrorFormat("{0}[r{3}]\n{1}\n{2}", path, e.Message, e.StackTrace, _row);
			}
		}
		
		public static void Clear()
		{
			AllClass.Clear();
Test.Clear();
		}
		
	}
}

using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.AllType
{
	/// <summary>
	/// 所有类型
	/// <summary>
	public class AllClass : CfgObject
	{
		/// <summary>
		/// 常量字符串
		/// <summary>
		public  readonly string ConstString = @"Hello World";
		/// <summary>
		/// 常量浮点值
		/// <summary>
		public  readonly float ConstFloat = 3.141527f;
		/// <summary>
		/// ID
		/// <summary>
		public readonly int ID;
		/// <summary>
		/// Test.TID
		/// <summary>
		public readonly int Index;
		/// <summary>
		/// 长整型
		/// <summary>
		public readonly long VarLong;
		/// <summary>
		/// 浮点型
		/// <summary>
		public readonly float VarFloat;
		/// <summary>
		/// 字符串
		/// <summary>
		public readonly string VarString;
		/// <summary>
		/// 布尔型
		/// <summary>
		public readonly bool VarBool;
		/// <summary>
		/// 枚举类型
		/// <summary>
		public readonly Cfg.AllType.CardElement VarEnum;
		/// <summary>
		/// 类类型
		/// <summary>
		public readonly Cfg.AllType.SingleClass VarClass;
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> VarListBase = new List<string>();
		/// <summary>
		/// Class列表
		/// <summary>
		public readonly List<Cfg.AllType.SingleClass> VarListClass = new List<Cfg.AllType.SingleClass>();
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> VarListCardElem = new List<string>();
		/// <summary>
		/// 基础类型字典
		/// <summary>
		public readonly Dictionary<int, float> VarDictBase = new Dictionary<int, float>();
		/// <summary>
		/// 枚举类型字典
		/// <summary>
		public readonly Dictionary<long, Cfg.AllType.CardElement> VarDictEnum = new Dictionary<long, Cfg.AllType.CardElement>();
		/// <summary>
		/// 类类型字典
		/// <summary>
		public readonly Dictionary<string, Cfg.AllType.SingleClass> VarDictClass = new Dictionary<string, Cfg.AllType.SingleClass>();
		
		public AllClass(DataStream data)
		{
			ID = data.GetInt();
			Index = data.GetInt();
			VarLong = data.GetLong();
			VarFloat = data.GetFloat();
			VarString = data.GetString();
			VarBool = data.GetBool();
			VarEnum = (Cfg.AllType.CardElement)data.GetInt();
			VarClass = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
			for (int n = data.GetInt(); n-- > 0;)
			{
				string v = data.GetString();
				VarListBase.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				Cfg.AllType.SingleClass v = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
				VarListClass.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				string v = data.GetString();
				VarListCardElem.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				int k = data.GetInt();
				VarDictBase[k] = data.GetFloat();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				long k = data.GetLong();
				VarDictEnum[k] = (Cfg.AllType.CardElement)data.GetInt();
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				string k = data.GetString();
				VarDictClass[k] = (Cfg.AllType.SingleClass)data.GetObject(data.GetString());
			}
		}
	}
}

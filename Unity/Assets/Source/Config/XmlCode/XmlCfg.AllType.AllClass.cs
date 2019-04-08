using System;
using System.Linq;
using System.IO;
using XmlCfg;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.AllType
{
	/// <summary>
	/// 所有类型
	/// <summary>
	public class AllClass : XmlObject
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
		public int ID;
		/// <summary>
		/// Test.TID
		/// <summary>
		public int Index;
		/// <summary>
		/// 长整型
		/// <summary>
		public long VarLong;
		/// <summary>
		/// 浮点型
		/// <summary>
		public float VarFloat;
		/// <summary>
		/// 字符串
		/// <summary>
		public string VarString;
		/// <summary>
		/// 布尔型
		/// <summary>
		public bool VarBool;
		/// <summary>
		/// 枚举类型
		/// <summary>
		public XmlCfg.AllType.CardElement VarEnum;
		/// <summary>
		/// 类类型
		/// <summary>
		public XmlCfg.AllType.SingleClass VarClass;
		/// <summary>
		/// 字符串列表
		/// <summary>
		public readonly List<string> VarListBase = new List<string>();
		/// <summary>
		/// Class列表
		/// <summary>
		public readonly List<XmlCfg.AllType.SingleClass> VarListClass = new List<XmlCfg.AllType.SingleClass>();
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
		public readonly Dictionary<long, XmlCfg.AllType.CardElement> VarDictEnum = new Dictionary<long, XmlCfg.AllType.CardElement>();
		/// <summary>
		/// 类类型字典
		/// <summary>
		public readonly Dictionary<string, XmlCfg.AllType.SingleClass> VarDictClass = new Dictionary<string, XmlCfg.AllType.SingleClass>();

		public override void Write(TextWriter _1)
		{
			Write(_1, "ID", this.ID);
			Write(_1, "Index", this.Index);
			Write(_1, "VarLong", this.VarLong);
			Write(_1, "VarFloat", this.VarFloat);
			Write(_1, "VarString", this.VarString);
			Write(_1, "VarBool", this.VarBool);
			Write(_1, "VarEnum", this.VarEnum);
			Write(_1, "VarClass", this.VarClass);
			Write(_1, "VarListBase", this.VarListBase);
			Write(_1, "VarListClass", this.VarListClass);
			Write(_1, "VarListCardElem", this.VarListCardElem);
			Write(_1, "VarDictBase", this.VarDictBase);
			Write(_1, "VarDictEnum", this.VarDictEnum);
			Write(_1, "VarDictClass", this.VarDictClass);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "ID": ID = ReadInt(_2); break;
				case "Index": Index = ReadInt(_2); break;
				case "VarLong": VarLong = ReadLong(_2); break;
				case "VarFloat": VarFloat = ReadFloat(_2); break;
				case "VarString": VarString = ReadString(_2); break;
				case "VarBool": VarBool = ReadBool(_2); break;
				case "VarEnum": VarEnum = (XmlCfg.AllType.CardElement)ReadInt(_2); break;
				case "VarClass": VarClass = ReadObject<XmlCfg.AllType.SingleClass>(_2, "XmlCfg.AllType.SingleClass"); break;
				case "VarListBase": GetChilds(_2).ForEach (_3 => VarListBase.Add(ReadString(_3))); break;
				case "VarListClass": GetChilds(_2).ForEach (_3 => VarListClass.Add(ReadObject<XmlCfg.AllType.SingleClass>(_3, "XmlCfg.AllType.SingleClass"))); break;
				case "VarListCardElem": GetChilds(_2).ForEach (_3 => VarListCardElem.Add(ReadString(_3))); break;
				case "VarDictBase": GetChilds(_2).ForEach (_3 => VarDictBase.Add(ReadInt(GetOnlyChild(_3, "Key")), ReadFloat(GetOnlyChild(_3, "Value")))); break;
				case "VarDictEnum": GetChilds(_2).ForEach (_3 => VarDictEnum.Add(ReadLong(GetOnlyChild(_3, "Key")), (XmlCfg.AllType.CardElement)ReadInt(GetOnlyChild(_3, "Value")))); break;
				case "VarDictClass": GetChilds(_2).ForEach (_3 => VarDictClass.Add(ReadString(GetOnlyChild(_3, "Key")), ReadObject<XmlCfg.AllType.SingleClass>(GetOnlyChild(_3, "Value"), "XmlCfg.AllType.SingleClass"))); break;
			}
		}
	}
}

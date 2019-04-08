using System;
using System.Linq;
using System.IO;
using XmlCfg;
using System.Xml;
using System.Collections.Generic;

namespace XmlCfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class SingleClass : XmlObject
	{
		/// <summary>
		/// Var1
		/// <summary>
		public string Var1;
		/// <summary>
		/// Var2
		/// <summary>
		public float Var2;

		public override void Write(TextWriter _1)
		{
			Write(_1, "Var1", this.Var1);
			Write(_1, "Var2", this.Var2);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "Var1": Var1 = ReadString(_2); break;
				case "Var2": Var2 = ReadFloat(_2); break;
			}
		}
	}
}

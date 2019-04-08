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
	public class Test : XmlObject
	{
		/// <summary>
		/// 继承2
		/// <summary>
		public int TID;
		/// <summary>
		/// 继承2
		/// <summary>
		public string Name;

		public override void Write(TextWriter _1)
		{
			Write(_1, "TID", this.TID);
			Write(_1, "Name", this.Name);
		}
		public override void Read(XmlNode _1)
		{
			foreach (System.Xml.XmlNode _2 in GetChilds (_1))
			switch (_2.Name)
			{
				case "TID": TID = ReadInt(_2); break;
				case "Name": Name = ReadString(_2); break;
			}
		}
	}
}

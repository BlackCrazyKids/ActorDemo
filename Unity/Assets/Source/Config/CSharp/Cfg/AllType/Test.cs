using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class Test : CfgObject
	{
		/// <summary>
		/// 继承2
		/// <summary>
		public readonly int TID;
		/// <summary>
		/// 继承2
		/// <summary>
		public readonly string Name;
		
		public Test(DataStream data)
		{
			TID = data.GetInt();
			Name = data.GetString();
		}
	}
}

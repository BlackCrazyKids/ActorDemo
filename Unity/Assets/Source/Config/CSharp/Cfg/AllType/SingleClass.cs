using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class SingleClass : CfgObject
	{
		/// <summary>
		/// Var1
		/// <summary>
		public readonly string Var1;
		/// <summary>
		/// Var2
		/// <summary>
		public readonly float Var2;
		
		public SingleClass(DataStream data)
		{
			Var1 = data.GetString();
			Var2 = data.GetFloat();
		}
	}
}

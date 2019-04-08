using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.AllType
{
	/// <summary>
	/// 
	/// <summary>
	public class M1 : Cfg.AllType.SingleClass
	{
		/// <summary>
		/// 继承1
		/// <summary>
		public readonly long V3;
		
		public M1(DataStream data) : base(data)
		{
			V3 = data.GetLong();
		}
	}
}

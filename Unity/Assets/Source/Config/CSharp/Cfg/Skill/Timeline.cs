using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class Timeline : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly int Start;
		/// <summary>
		/// 
		/// <summary>
		public readonly int End;
		
		public Timeline(DataStream data)
		{
			Start = data.GetInt();
			End = data.GetInt();
		}
	}
}

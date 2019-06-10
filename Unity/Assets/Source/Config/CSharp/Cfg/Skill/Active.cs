using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class Active : Cfg.Skill.Controller
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly bool Enable;
		
		public Active(DataStream data) : base(data)
		{
			Enable = data.GetBool();
		}
	}
}

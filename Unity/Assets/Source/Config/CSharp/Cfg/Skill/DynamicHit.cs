using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class DynamicHit : Cfg.Skill.StaticHit
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Target;
		
		public DynamicHit(DataStream data) : base(data)
		{
			Target = data.GetString();
		}
	}
}

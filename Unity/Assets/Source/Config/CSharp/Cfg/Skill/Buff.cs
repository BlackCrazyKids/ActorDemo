using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class Buff : Cfg.Skill.PlayParticle
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly int Id;
		
		public Buff(DataStream data) : base(data)
		{
			Id = data.GetInt();
		}
	}
}

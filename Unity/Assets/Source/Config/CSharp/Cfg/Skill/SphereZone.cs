using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class SphereZone : Cfg.Skill.HitZone
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float Radius;
		
		public SphereZone(DataStream data) : base(data)
		{
			Radius = data.GetFloat();
		}
	}
}

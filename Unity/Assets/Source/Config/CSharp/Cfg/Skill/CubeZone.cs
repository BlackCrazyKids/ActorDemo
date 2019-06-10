using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class CubeZone : Cfg.Skill.HitZone
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 Scale;
		
		public CubeZone(DataStream data) : base(data)
		{
			Scale = new Cfg.Vector3(data);
		}
	}
}

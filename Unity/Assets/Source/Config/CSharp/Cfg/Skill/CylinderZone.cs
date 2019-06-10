using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class CylinderZone : Cfg.Skill.HitZone
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float Radius;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Height;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Angle;
		
		public CylinderZone(DataStream data) : base(data)
		{
			Radius = data.GetFloat();
			Height = data.GetFloat();
			Angle = data.GetFloat();
		}
	}
}

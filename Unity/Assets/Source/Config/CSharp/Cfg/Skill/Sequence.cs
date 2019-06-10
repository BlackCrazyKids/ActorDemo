using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class Sequence : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Id;
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Skill.HitZone> HitZones = new List<Cfg.Skill.HitZone>();
		/// <summary>
		/// 
		/// <summary>
		public readonly List<Cfg.Skill.Timeline> Timelines = new List<Cfg.Skill.Timeline>();
		
		public Sequence(DataStream data)
		{
			Id = data.GetString();
			for (int n = data.GetInt(); n-- > 0;)
			{
				Cfg.Skill.HitZone v = (Cfg.Skill.HitZone)data.GetObject(data.GetString());
				HitZones.Add(v);
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				Cfg.Skill.Timeline v = (Cfg.Skill.Timeline)data.GetObject(data.GetString());
				Timelines.Add(v);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class PlayParticle : Cfg.Skill.Timeline
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Path;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool IsRelateSelf;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool FollowDir;
		/// <summary>
		/// 
		/// <summary>
		public readonly string NodeName;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 Position;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 EulerAngles;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 Scale;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Skill.EffectAlignType AlignType;
		
		public PlayParticle(DataStream data) : base(data)
		{
			Path = data.GetString();
			IsRelateSelf = data.GetBool();
			FollowDir = data.GetBool();
			NodeName = data.GetString();
			Position = new Cfg.Vector3(data);
			EulerAngles = new Cfg.Vector3(data);
			Scale = new Cfg.Vector3(data);
			AlignType = (Cfg.Skill.EffectAlignType)data.GetInt();
		}
	}
}

using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class ShakeScreen : Cfg.Skill.Timeline
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Skill.ShakeType Type;
		/// <summary>
		/// 
		/// <summary>
		public readonly int Frequency;
		/// <summary>
		/// 
		/// <summary>
		public readonly float FrequencyDuration;
		/// <summary>
		/// 
		/// <summary>
		public readonly float FrequencyAtten;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Amplitude;
		/// <summary>
		/// 
		/// <summary>
		public readonly float AmplitudeAtten;
		/// <summary>
		/// 
		/// <summary>
		public readonly float MinRange;
		/// <summary>
		/// 
		/// <summary>
		public readonly float MaxRange;
		
		public ShakeScreen(DataStream data) : base(data)
		{
			Type = (Cfg.Skill.ShakeType)data.GetInt();
			Frequency = data.GetInt();
			FrequencyDuration = data.GetFloat();
			FrequencyAtten = data.GetFloat();
			Amplitude = data.GetFloat();
			AmplitudeAtten = data.GetFloat();
			MinRange = data.GetFloat();
			MaxRange = data.GetFloat();
		}
	}
}

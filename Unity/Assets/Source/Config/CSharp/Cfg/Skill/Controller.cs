using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class Controller : Cfg.Skill.Timeline
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Path;
		
		public Controller(DataStream data) : base(data)
		{
			Path = data.GetString();
		}
	}
}

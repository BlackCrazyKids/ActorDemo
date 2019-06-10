using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class ReplaceObject : Cfg.Skill.Controller
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string NewObject;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 Offset;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 EulerAngles;
		
		public ReplaceObject(DataStream data) : base(data)
		{
			NewObject = data.GetString();
			Offset = new Cfg.Vector3(data);
			EulerAngles = new Cfg.Vector3(data);
		}
	}
}

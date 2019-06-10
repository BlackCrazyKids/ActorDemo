using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class CastObject : Cfg.Skill.Controller
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly bool IsTraceTarget;
		/// <summary>
		/// 
		/// <summary>
		public readonly int CurveId;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool PassBody;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 Position;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 EulerAngles;
		
		public CastObject(DataStream data) : base(data)
		{
			IsTraceTarget = data.GetBool();
			CurveId = data.GetInt();
			PassBody = data.GetBool();
			Position = new Cfg.Vector3(data);
			EulerAngles = new Cfg.Vector3(data);
		}
	}
}

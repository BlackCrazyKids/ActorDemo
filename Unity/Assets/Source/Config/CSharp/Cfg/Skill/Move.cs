using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class Move : Cfg.Skill.Controller
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Skill.MoveType Type;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool IsRelateSelf;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Vector3 Offset;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Angle;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Speed;
		
		public Move(DataStream data) : base(data)
		{
			Type = (Cfg.Skill.MoveType)data.GetInt();
			IsRelateSelf = data.GetBool();
			Offset = new Cfg.Vector3(data);
			Angle = data.GetFloat();
			Speed = data.GetFloat();
		}
	}
}

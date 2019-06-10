using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class SkillAction : Cfg.Skill.GeneralAction
	{
		/// <summary>
		/// 
		/// <summary>
		public  readonly float EXPIRE_TIME = 1f;
		/// <summary>
		/// 
		/// <summary>
		public readonly float SkillExpireTime;
		/// <summary>
		/// 
		/// <summary>
		public readonly float SkillEndTime;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool CanInterrupt;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Skill.LockObjectType LockType;
		/// <summary>
		/// 
		/// <summary>
		public readonly float SkillRange;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool CanShowSkillRange;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool CanRotate;
		/// <summary>
		/// 
		/// <summary>
		public readonly bool CanMove;
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<int, Cfg.Skill.Sequence> SequenceDict = new Dictionary<int, Cfg.Skill.Sequence>();
		
		public SkillAction(DataStream data) : base(data)
		{
			SkillExpireTime = data.GetFloat();
			SkillEndTime = data.GetFloat();
			CanInterrupt = data.GetBool();
			LockType = (Cfg.Skill.LockObjectType)data.GetInt();
			SkillRange = data.GetFloat();
			CanShowSkillRange = data.GetBool();
			CanRotate = data.GetBool();
			CanMove = data.GetBool();
			for (int n = data.GetInt(); n-- > 0;)
			{
				int k = data.GetInt();
				SequenceDict[k] = new Cfg.Skill.Sequence(data);
			}
		}
	}
}

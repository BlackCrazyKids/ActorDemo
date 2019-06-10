using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Skill
{
	/// <summary>
	/// 
	/// <summary>
	public class ActorConfig : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string ModelName;
		/// <summary>
		/// 
		/// <summary>
		public readonly string BaseModelName;
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, Cfg.Skill.GeneralAction> GeneralActions = new Dictionary<string, Cfg.Skill.GeneralAction>();
		/// <summary>
		/// 
		/// <summary>
		public readonly Dictionary<string, Cfg.Skill.SkillAction> SkillActions = new Dictionary<string, Cfg.Skill.SkillAction>();
		
		public ActorConfig(DataStream data)
		{
			ModelName = data.GetString();
			BaseModelName = data.GetString();
			for (int n = data.GetInt(); n-- > 0;)
			{
				string k = data.GetString();
				GeneralActions[k] = (Cfg.Skill.GeneralAction)data.GetObject(data.GetString());
			}
			for (int n = data.GetInt(); n-- > 0;)
			{
				string k = data.GetString();
				SkillActions[k] = new Cfg.Skill.SkillAction(data);
			}
		}
	}
}

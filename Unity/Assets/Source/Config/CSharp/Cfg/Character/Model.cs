using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg.Character
{
	/// <summary>
	/// 
	/// <summary>
	public class Model : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly string Name;
		/// <summary>
		/// 
		/// <summary>
		public readonly Cfg.Character.GroupType GroupType;
		/// <summary>
		/// 
		/// <summary>
		public readonly string ModelPath;
		/// <summary>
		/// 
		/// <summary>
		public readonly string AvatarPath;
		/// <summary>
		/// 
		/// <summary>
		public readonly float BodyRadius;
		/// <summary>
		/// 
		/// <summary>
		public readonly float BodyHeight;
		/// <summary>
		/// 
		/// <summary>
		public readonly float ModelScale;
		
		public Model(DataStream data)
		{
			Name = data.GetString();
			GroupType = (Cfg.Character.GroupType)data.GetInt();
			ModelPath = data.GetString();
			AvatarPath = data.GetString();
			BodyRadius = data.GetFloat();
			BodyHeight = data.GetFloat();
			ModelScale = data.GetFloat();
		}
	}
}

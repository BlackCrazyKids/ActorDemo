using System;
using System.Collections.Generic;
using Cfg;

namespace Cfg
{
	/// <summary>
	/// 
	/// <summary>
	public class Vector2 : CfgObject
	{
		/// <summary>
		/// 
		/// <summary>
		public readonly float X;
		/// <summary>
		/// 
		/// <summary>
		public readonly float Y;
		
		public Vector2(DataStream data)
		{
			X = data.GetFloat();
			Y = data.GetFloat();
		}
	}
}

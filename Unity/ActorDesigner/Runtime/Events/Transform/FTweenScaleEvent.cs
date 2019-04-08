using UnityEngine;

namespace Flux
{
	[FEvent("模型控制/缩放动画", typeof(FTransformTrack))]
	public class FTweenScaleEvent : FTransformEvent
	{
		protected override void SetDefaultValues ()
		{
			_tween = new FTweenVector3( Vector3.zero, Vector3.one );
		}

		protected override void ApplyProperty( float t )
		{
			Owner.localScale = _tween.GetValue( t );
		}
	}
}

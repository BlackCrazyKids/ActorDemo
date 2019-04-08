using UnityEngine;
using System.Collections;

namespace Flux
{
	[FEvent("播放器/全局音频音量")]
	public class FGlobalVolumeAudioEvent : FTweenEvent<FTweenFloat>
	{
		protected override void ApplyProperty( float t )
		{
			AudioListener.volume = _tween.GetValue( t );
		}

		protected override void SetDefaultValues ()
		{
			_tween = new FTweenFloat( 0, 1 );
		}
	}
}

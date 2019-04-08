using UnityEngine;
using System.Collections;

namespace Flux
{
    [FEvent("播放器/音频音量")]
    public class FVolumeAudioEvent : FTweenEvent<FTweenFloat>
    {
        [SerializeField, HideInInspector]
        private AudioSource _source;

        public override string Text
        {
            get
            {
                return _source== null? "Miss" : _source.name;
            }
            set
            {
            }
        }

        protected override void ApplyProperty(float t)
        {
            if (_source == null) return;
            _source.volume = _tween.GetValue(t);
        }
    }
}

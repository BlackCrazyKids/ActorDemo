using System;
using UnityEngine;

namespace Flux
{
    [FEvent("调用序列", typeof(FSequenceTrack), true)]
    public class FPlaySequenceEvent : FEvent
    {
        private FSequence _sequence = null;

        /// <summary>
        /// 触发帧
        /// </summary>
        public int StartOffset { get { return Start; } }

        public override string Text
        {
            get
            {
                return _sequence == null ? "Miss" : _sequence.name;
            }
            set
            { }
        }

        public int SeqID
        {
            get
            {
                int index = _sequence.name.IndexOf('_');
                int startIndex = index + 1;
                string num = _sequence.name.Substring(startIndex, _sequence.name.Length - startIndex);
                return Convert.ToInt32(num);
            }
        }

        public bool HasSequence()
        {
            return _sequence != null;
        }

        protected override void OnInit()
        {
            _sequence = ((FSequenceTrack)Track).OwnerSequence;
            //if (!HasSequence())
            //    Debug.LogError("未指定序列对象,无法正常播放.");
        }

        protected override void OnTrigger(float timeSinceTrigger)
        {
            if (Sequence.IsPlaying && /*Application.isPlaying &&*/ HasSequence())
            {
                _sequence.Play(StartOffset * _sequence.InverseFrameRate + timeSinceTrigger);
            }
        }

        //protected override void OnUpdateEvent(float timeSinceTrigger)
        //{
        //    if (HasSequence())
        //        _sequence.Speed = Mathf.Sign(Sequence.Speed) * Mathf.Abs(_sequence.Speed);
        //}

        //		protected override void OnUpdateEvent( float timeSinceTrigger )
        //		{
        ////			if( !Application.isPlaying )
        ////				_sequence.SetCurrentTime( StartOffset * Sequence.InverseFrameRate + timeSinceTrigger );
        //		}
        //
        //		protected override void OnUpdateEventEditor( float timeSinceTrigger )
        //		{
        ////			_sequence.SetCurrentTime( StartOffset * Sequence.InverseFrameRate + timeSinceTrigger );
        ////			_sequence.SetCurrentFrameEditor( _startOffset + framesSinceTrigger );
        ////			_sequence.SetCurrentFrame( _startOffset + framesSinceTrigger );
        //		}

        protected override void OnStop()
        {
            if (HasSequence())
                _sequence.Stop(true);
        }

        protected override void OnFinish()
        {
            if (HasSequence())
                _sequence.Pause();
        }

        protected override void OnPause()
        {
            if (HasSequence())
                _sequence.Pause();
        }

        protected override void OnResume()
        {
            if (HasSequence())
                _sequence.Play(_sequence.CurrentTime);
            //			_sequence.Resume();
        }
    }
}

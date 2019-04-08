using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Flux;

namespace FluxEditor
{
    [FEditor(typeof(FSequenceTrack))]
    public class FSequenceTrackEditor : FTrackEditor
    {

        private FSequenceEditor _sequenceEditor = null;

        public override void Init(FObject obj, FEditor owner)
        {
            base.Init(obj, owner);

            FSequenceTrack sequenceTrack = (FSequenceTrack)Obj;
            if (sequenceTrack.OwnerSequence == null)
            {
                FSequence newSequence = FSequenceEditorWindow.CreateSequence();
                newSequence.transform.parent = sequenceTrack.transform;
                sequenceTrack.OwnerSequence = newSequence;
                foreach (FPlaySequenceEvent evt in sequenceTrack.Events)
                {
                    evt.Init();
                }
            }

            if (_sequenceEditor == null)
            {
                _sequenceEditor = FSequenceEditor.CreateInstance<FSequenceEditor>();
                _sequenceEditor.Init((Editor)null/*SequenceEditor*/ );
                _sequenceEditor.OpenSequence(sequenceTrack.OwnerSequence);

                //				if( Track.PreviewDirtiesScene && !Track.HasCache )
                //				{
                //					_sequenceEditor.TurnOnAllPreviews( false );
                //				}
            }
        }

        public override void UpdateEventsEditor(int frame, float time)
        {
            base.UpdateEventsEditor(frame, time);

            FEvent[] evts = new FEvent[2];

            int numEvents = Track.GetEventsAt(frame, evts);

            if (numEvents > 0)
            {
                int startOffset = ((FPlaySequenceEvent)evts[0]).StartOffset;
                _sequenceEditor.SetCurrentFrame(startOffset + frame - evts[0].Start); /// @TODO handle offset

                if (numEvents > 1)
                {
                    startOffset = ((FPlaySequenceEvent)evts[1]).StartOffset;
                    _sequenceEditor.SetCurrentFrame(startOffset + frame - evts[1].Start);
                }
            }
        }
    }
}

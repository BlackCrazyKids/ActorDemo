using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline
{
    public abstract class BaseClip : PlayableAsset, ITimelineClipAsset
    {
        public virtual ClipCaps clipCaps
        {
            get
            {
                return ClipCaps.Blending;
            }
        }
        public virtual string DisplayName { get { return name; } }
    }
}
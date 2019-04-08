using UnityEngine;
using System.Collections.Generic;

namespace Flux
{
    public class FSequenceTrack : FTrack
    {
        [SerializeField, HideInInspector]
        private FSequence _ownerSequence = null;
        public FSequence OwnerSequence
        {
            get
            {
                return _ownerSequence;
            }
            set
            {
                _ownerSequence = value;
            }
        }

        public override CacheMode RequiredCacheMode
        {
            get
            {
                return CacheMode.Editor | CacheMode.RuntimeBackwards;
            }
        }

        public override CacheMode AllowedCacheMode
        {
            get
            {
                return RequiredCacheMode | CacheMode.RuntimeForward;
            }
        }

        public override void CreateCache()
        {
            if (HasCache)
                return;

            Cache = new FSequenceTrackCache(this);
            Cache.Build();
        }

        public override void ClearCache()
        {
            if (!HasCache)
                return;

            Cache.Clear();
            Cache = null;
        }

        public override void Init()
        {
            base.Init();

            if (Application.isPlaying)
                enabled = true;
        }
    }
}

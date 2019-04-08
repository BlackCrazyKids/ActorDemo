using UnityEngine;
using System.Collections;

namespace Flux
{
    public class FCharacterTrack : FTrack
    {
        public override void Init()
        {
            base.Init();
            //Sequence.
            SetOwner(null);
        }
    }
}

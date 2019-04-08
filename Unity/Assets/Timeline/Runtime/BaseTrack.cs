using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline
{
    public abstract class BaseTrack : TrackAsset
    {
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {            
#if UNITY_EDITOR
            Component trackBinding = director.GetGenericBinding(this) as Component;
            if (trackBinding == null)
                return;

            var serializedObject = new UnityEditor.SerializedObject(trackBinding);
            var iterator = serializedObject.GetIterator();
            while (iterator.NextVisible(true))
            {
                if (iterator.hasVisibleChildren)
                    continue;

                driver.AddFromName(trackBinding.gameObject, iterator.propertyPath);
            }
#endif
            base.GatherProperties(director, driver);
        }
    }
}
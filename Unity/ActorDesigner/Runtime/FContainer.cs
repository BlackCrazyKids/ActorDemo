using UnityEngine;
using System.Collections.Generic;

namespace Flux
{
    public enum FContainerEnum
    {
        FContainer,
        FCfgContainer,//基础配置
        FTimelineContainer,//时间事件     
    }

    public class FContainer : FObject
    {
        public static readonly Color DEFAULT_COLOR = new Color(0.14f, 0.14f, 0.14f, 0.7f);

        /// <summary>
        /// 容器类型,用于区别处理数据
        /// </summary>
        [HideInInspector]
        public FContainerEnum ConatinerType = FContainerEnum.FContainer;
        [HideInInspector]
        public int ContainerNo = 0;
        public static readonly Dictionary<string, FContainerEnum> ContainerMap = new Dictionary<string, FContainerEnum>()
        {
            {"Default",  FContainerEnum.FContainer},
            {"基础配置",  FContainerEnum.FCfgContainer},
            {"时间事件",  FContainerEnum.FTimelineContainer},
        };


        [SerializeField]
        [HideInInspector]
        private FSequence _sequence = null;

        [SerializeField]
        [HideInInspector]
        private Color _color;
        public Color Color { get { return _color; } set { _color = value; } }

        [SerializeField]
        private List<FTrack> _tracks = new List<FTrack>();
        public List<FTrack> Tracks { get { return _tracks; } }

        public override FSequence Sequence { get { return _sequence; } }
        public override Transform Owner { get { return null; } }

        public static FContainer Create(string name, Color color)
        {
            GameObject go = new GameObject(name);
            FContainer container = go.AddComponent<FContainer>();
            container.Color = color;
            container.ConatinerType = ContainerMap[name];

            return container;
        }

        internal void SetSequence(FSequence sequence)
        {
            _sequence = sequence;
            if (_sequence)
                transform.parent = _sequence.Content;
            else
                transform.parent = null;
        }

        public override void Init()
        {
            foreach (FTrack track in _tracks)
            {
                track.Init();
            }
        }

        public override void Stop()
        {
            foreach (FTrack track in _tracks)
            {
                track.Stop();
            }
        }

        public void Resume()
        {
            foreach (FTrack track in _tracks)
            {
                track.Resume();
            }
        }

        public void Pause()
        {
            foreach (FTrack track in _tracks)
            {
                track.Pause();
            }
        }

        public bool IsEmpty()
        {
            foreach (FTrack track in _tracks)
            {
                if (!track.IsEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        public void UpdateTracks(int frame, float time)
        {
            for (int i = 0; i != _tracks.Count; ++i)
            {
                if (!_tracks[i].enabled) continue;
                _tracks[i].UpdateEvents(frame, time);
            }
        }

        public void UpdateTracksEditor(int frame, float time)
        {
            for (int i = 0; i != _tracks.Count; ++i)
            {
                if (!_tracks[i].enabled) continue;
                _tracks[i].UpdateEventsEditor(frame, time);
            }
        }

        public FTrack Add<T>(FrameRange range) where T : FEvent
        {
            FTrack track = FTrack.Create<T>();

            Add(track);

            FEvent evt = FEvent.Create<T>(range);

            track.Add(evt);

            return track;
        }

        /// @brief Adds new timeline at the end of the list.
        /// @param timeline New timeline.
        public void Add(FTrack track)
        {
            int id = _tracks.Count;

            _tracks.Add(track);
            track.SetId(id);

            //			timeline.SetSequence( this );
            track.SetContainer(this);
        }

        /// @brief Removes timeline and updates their ids.
        /// @param timeline CTimeline to remove.
        /// @note After calling this function, the ids of the timelines after this
        /// one in the list will have an id smaller by 1.
        public void Remove(FTrack track)
        {
            for (int i = 0; i != _tracks.Count; ++i)
            {
                if (_tracks[i] == track)
                {
                    Remove(i);
                    break;
                }
            }
        }

        /// @brief Removes timeline with id.
        /// @oaram id Id of the CTimeline to remove.
        /// @note After calling this function, the ids of the timelines after this
        /// one in the list will have an id smaller by 1.
        /// @warning Does not check if id is valid (i.e. between -1 & GetTimelines().Count)
        public void Remove(int id)
        {
            FTrack track = _tracks[id];
            _tracks.RemoveAt(id);
            track.SetContainer(null);

            UpdateTrackIds();
        }

        public void Rebuild()
        {
            _tracks.Clear();
            Transform t = transform;
            for (int i = 0; i != t.childCount; ++i)
            {
                FTrack track = t.GetChild(i).GetComponent<FTrack>();
                if (track != null)
                {
                    _tracks.Add(track);

                    track.SetContainer(this);
                    track.Rebuild();
                }
            }

            UpdateTrackIds();
        }

        // Updates the ids of the tracks
        private void UpdateTrackIds()
        {
            for (int i = 0; i != _tracks.Count; ++i)
                _tracks[i].SetId(i);
        }
    }
}

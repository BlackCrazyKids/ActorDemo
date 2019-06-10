using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Image))]
    public class SpriteAnimation : MonoBehaviour
    {
        [HideInInspector] [SerializeField] protected int _fps = 30;
        [HideInInspector] [SerializeField] protected string _atlasName = "";
        [HideInInspector] [SerializeField] protected string _prefix = "";
        [HideInInspector] [SerializeField] protected bool _loop = true;
        protected Image _image;
        protected SpriteAtlas _atlas;
        protected float _delta = 0f;
        protected int _index = 0;
        protected bool _isPlaying = false;

        public int FramesPerSecond { get { return _fps; } set { _fps = value; } }
        public string Prefix
        {
            get { return _prefix; }
            set
            {
                if (_prefix != value)
                    _prefix = value;
            }
        }
        public bool Loop { get { return _loop; } set { _loop = value; } }
        public bool IsPlaying { get { return _isPlaying; } }

        protected virtual void Start()
        {
            RebuildSpriteAtlas();
        }

        protected virtual void Update()
        {
            if (_isPlaying && _atlas != null && Application.isPlaying && _fps > 0)
            {
                _delta += Time.deltaTime;
                float rate = 1f / _fps;

                if (rate < _delta)
                {
                    _delta = (rate > 0f) ? _delta - rate : 0f;
                    if (++_index >= _atlas.spriteCount)
                    {
                        _index = 0;
                        _isPlaying = _loop;
                    }

                    if (_isPlaying)
                        ResetSprite();
                }
            }
        }
        protected void ResetSprite()
        {
            if (_atlas == null || _image == null)
                return;

            string spriteName = string.Format("{0}{1}", _prefix, _index);
            _image.sprite = _atlas.GetSprite(spriteName);
        }

        public void RebuildSpriteAtlas()
        {
            Client.AtlasMgr.GetSpriteAtlas(_atlasName, atlas => _atlas = atlas);
        }
        public void ReStart()
        {
            _index = 0;
            ResetSprite();

            Play();
        }
        public void Rewind()
        {
            Stop();
            _index = 0;
            ResetSprite();
        }
        public void Play()
        {
            _isPlaying = true;
        }
        public void Stop()
        {
            _isPlaying = false;
        }

        public void Reset()
        {
            if (_image == null)
                _image = GetComponent<Image>();
            _isPlaying = false;
            _index = 0;
            if (_image != null && _atlas != null)
            {
                string spriteName = string.Format("{0}{1}", _prefix, _index);
                _image.sprite = _atlas.GetSprite(spriteName);
                _image.SetNativeSize();
            }
        }
    }
}
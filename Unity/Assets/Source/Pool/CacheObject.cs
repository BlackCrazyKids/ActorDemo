using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CacheObject<T> : IPoolItem<T> where T : UnityEngine.Object
    {
        public string Name { get { return _name; } }
        public int MaxSize { get { return 1; } }
        public float LastUseTime { get { return _lastUseTime; } }
        public float CacheTime { get { return _cacheTime; } }
        public T Original { get { return _original; } }

        protected string _name = "Null";
        protected float _lastUseTime = 0;
        /// <summary>
        /// time 小于 0时,不做记时处理;反之,计时处理
        /// </summary>
        protected float _cacheTime = -1;
        protected T _original;
        public Action<T> OnGet;
        public Action<T> OnRelease;


        public CacheObject(string name, T original, float cacheTime = -1)
        {
            _name = name;
            _original = original;
            _lastUseTime = cacheTime < 0 ? 0 : Time.time;
            _cacheTime = cacheTime;
        }

        public virtual bool CanCleanup()
        {
            return Time.time - _lastUseTime > _cacheTime;
        }
        public virtual void Cleanup()
        {
            if (_original is GameObject)
                UnityEngine.Object.DestroyImmediate(_original, true);
            else
                Resources.UnloadAsset(_original);
        }

        public virtual T Get()
        {
            if (OnGet != null)
                OnGet(_original);
            return _original;
        }
        public virtual bool Release(T obj)
        {
            _lastUseTime = Time.time;
            if (OnRelease != null)
                OnRelease(_original);
            return true;
        }
    }
}

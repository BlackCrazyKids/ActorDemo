using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 创建一个对象的Clone集合
    /// </summary>
    public class CachePool<T> : IPoolItem<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// 池最大上限,实际大小为 Limit + MaxSize
        /// </summary>
        readonly int Capacity = 50;

        public string Name { get { return _name; } }
        public int MaxSize { get { return _maxSize; } }
        public float LastUseTime { get { return _lastUseTime; } }
        public float CacheTime { get { return _cacheTime; } }
        /// <summary>
        /// Clone对象
        /// </summary>
        public T Original { get { return _original; } }

        protected string _name = "Null";
        protected int _maxSize = 10;
        protected float _lastUseTime = 0;
        /// <summary>
        /// time 小于 0时,不做记时处理;反之,计时处理
        /// </summary>
        protected float _cacheTime = -1;
        protected T _original;
        protected SimplePool<T> _cache;
        protected HashSet<T> _usingObjects;

        public Func<T> AutoCreate;
        public Action<T> OnGet;
        public Action<T> OnRelease;

        public CachePool(string name, T original, int maxSize, float cacheTime = -1)
        {
            _name = name;
            _original = original;
            _maxSize = maxSize;
            _lastUseTime = cacheTime < 0 ? 0 : Time.time;
            _cacheTime = cacheTime;
            _cache = new SimplePool<T>(original.name, maxSize, AutoCreate);
            _usingObjects = new HashSet<T>();

            Capacity += _maxSize;
        }

        public virtual bool CanCleanup()
        {
            return _usingObjects.Count == 0 && Time.time - _lastUseTime > _cacheTime;
        }
        public virtual void Cleanup()
        {
            T del = _cache.Get();
            while (del != null)
            {
                if (del is GameObject)
                    UnityEngine.Object.DestroyImmediate(del, true);
                else
                    Resources.UnloadAsset(del);
            }
            if (_original is GameObject)
                UnityEngine.Object.DestroyImmediate(_original, true);
            else
                Resources.UnloadAsset(_original);
        }

        public virtual T Get()
        {
            if (_cache.Count + _usingObjects.Count >= Capacity)
                throw new Exception(string.Format("{0} CachePool存储数量达到极限[Capacity={1}]", Name, Capacity));

            T obj = null;
            if (_cache.Count == 0)
                obj = UnityEngine.Object.Instantiate<T>(_original);
            else
                obj = _cache.Get();
            _usingObjects.Add(obj);
            if (OnGet != null)
                OnGet(obj);
            return obj;
        }
        public virtual bool Release(T obj)
        {
            if (!_usingObjects.Contains(obj))
                throw new Exception(string.Format("{0} CachePool中不包含实例{1}对象", Name, obj.name));

            _usingObjects.Remove(obj);
            _cache.Release(obj);
            _lastUseTime = Time.time;
            if (OnRelease != null)
                OnRelease(obj);
            return true;
        }
        public virtual void ReleaseAllUsing()
        {
            List<T> usingObjs = new List<T>(_usingObjects);
            for (int i = 0; i < usingObjs.Count; i++)
            {
                var obj = usingObjs[i];
                _usingObjects.Remove(obj);
                _cache.Release(obj);
                if (OnRelease != null)
                    OnRelease(obj);
            }
            _lastUseTime = Time.time;
        }
    }
}

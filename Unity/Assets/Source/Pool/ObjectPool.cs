using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ObjectPool<T> : IObjectPool where T : UnityEngine.Object
    {
        readonly int Capacity = 50;

        public string Name { get { return _name; } }
        public int MaxSize { get { return _maxSize; } }
        public int Count { get { return _cache.Count; } }

        private string _name;
        private int _maxSize;
        /// <summary>
        /// 定时清理时间间隔
        /// </summary>
        private float _interval = 0;
        private Func<List<IPoolItem<T>>, List<IPoolItem<T>>> _onCleanup;

        private float _cleanupLastTime = 0;
        private Dictionary<string, IPoolItem<T>> _cache;

        public ObjectPool(string name, int maxSize, float interval, Func<List<IPoolItem<T>>, List<IPoolItem<T>>> onCleanup)
        {
            _name = string.IsNullOrEmpty(name) ? "Null" : name;
            _maxSize = maxSize;
            _interval = interval;
            _onCleanup = onCleanup == null ? DefaultOnCleanup : onCleanup;

            _cleanupLastTime = Time.realtimeSinceStartup;
            _cache = new Dictionary<string, IPoolItem<T>>();
        }


        public bool HasItem(string name)
        {
            return _cache.ContainsKey(name);
        }
        public T GetItem(string name)
        {
            if (HasItem(name))
                return _cache[name].Get();
            return null;
        }
        public bool ReleaseItem(string name, T obj)
        {
            if (HasItem(name))
            {
                _cache[name].Release(obj);
                return true;
            }

            throw new Exception(name + " IPoolItem 对象不存在,请先在ObjectPool中实例化项!");
        }
        /// <summary>
        /// CacheObject - 默认情况,释放后,定期清理
        /// CachePool   - 默认情况,保留MaxSize个对象在池中,清除多余;不够MaxSize,则无清除操作.
        /// </summary>
        public void ReleaseAllUsing()
        {
            var iter = _cache.GetEnumerator();
            while (iter.MoveNext())
            {
                var item = iter.Current.Value;
                if (item.MaxSize == 1)
                    item.Release(item.Original);
                else
                {
                    var pool = item as CachePool<T>;
                    pool.ReleaseAllUsing();
                }
            }
        }

        /// <summary>
        /// 创建实例到池
        /// </summary>
        public void CreateObject(string name, T original, float cacheTime = -1)
        {
            CreateObject(name, original, cacheTime, null, null);
        }
        public void CreateObject(string name, T original, Action<T> onGet, Action<T> onRelease)
        {
            CreateObject(name, original, -1, null, null);
        }
        public void CreateObject(string name, T original, float cacheTime,
            Action<T> onGet, Action<T> onRelease)
        {
            Create(name, original, 1, cacheTime, onGet, onRelease);
        }

        /// <summary>
        /// 创建实例缓存池到池
        /// </summary>
        public void CreatePool(string name, T original, int maxSize, float cacheTime = -1)
        {
            if (maxSize == 1)
                Debug.LogError("如果只创建一个实例对象,请使用CreateObject方法");
            Create(name, original, maxSize, cacheTime, null, null);
        }
        public void CreatePool(string name, T original, int maxSize, Action<T> onGet, Action<T> onRelease)
        {
            if (maxSize == 1)
                Debug.LogError("如果只创建一个实例对象,请使用CreateObject方法");
            Create(name, original, maxSize, -1, onGet, onRelease);
        }

        /// <summary>
        /// maxSize = 1时,只构建对象;
        /// maxSize > 1时,构建实例缓存池
        /// </summary>
        public void Create(string name, T original, int maxSize, float cacheTime,
            Action<T> onGet, Action<T> onRelease)
        {
            if (maxSize <= 0)
                throw new Exception("IPoolItem.MaxSize 必须大于0");
            else if (maxSize == 1)
            {
                CacheObject<T> cacheObject = new CacheObject<T>(name, original, cacheTime);
                cacheObject.OnGet = onGet;
                cacheObject.OnRelease = onRelease;
                _cache.Add(name, cacheObject);
            }
            else
            {
                CachePool<T> cachePool = new CachePool<T>(name, original, maxSize, cacheTime);
                cachePool.OnGet = onGet;
                cachePool.OnRelease = onRelease;
                _cache.Add(name, cachePool);
            }
        }

        /// <summary>
        /// 清理过期对象
        /// </summary>
        public void CleanupUnusedItems()
        {
            var unusedItems = GetAllUnusedItems();
            if (_onCleanup != null)
                unusedItems = _onCleanup(unusedItems);
            for (int i = 0; i < unusedItems.Count; i++)
            {
                var item = unusedItems[i];
                _cache.Remove(item.Name);
                item.Cleanup();
            }
        }
        /// <summary>
        /// 间歇性检查并清理对象,尽可能多的缓存对象.
        /// </summary>
        public void CleanupInterval()
        {
            if (Time.realtimeSinceStartup > _cleanupLastTime + _interval)
            {
                _cleanupLastTime = Time.realtimeSinceStartup;
                Cleanup();
            }
        }
        /// <summary>
        /// 尽可能多的缓存对象,清理多余对象那个.
        /// </summary>
        public void Cleanup()
        {
            if (_cache.Count > Capacity)
                throw new Exception(string.Format("{0} ObjectPool存储数量达到极限[Capacity={1}]", Name, Capacity));

            if (_cache.Count > _maxSize)
            {
                var unusedItems = GetAllUnusedItems();
                if (_onCleanup != null)
                    unusedItems = _onCleanup(unusedItems);
                else
                    unusedItems = DefaultOnCleanup(unusedItems);

                int cleanupCount = Mathf.Min(_cache.Count - _maxSize, unusedItems.Count);
                for (int i = 0; i < cleanupCount; i++)
                {
                    var item = unusedItems[i];
                    _cache.Remove(item.Name);
                    item.Cleanup();
                }
            }
        }
        /// <summary>
        /// 销毁池
        /// </summary>
        public void Dispose()
        {
            ReleaseAllUsing();
            var unusedItems = GetAllUnusedItems();
            for (int i = 0; i < unusedItems.Count; i++)
            {
                var item = unusedItems[i];
                _cache.Remove(item.Name);
                item.Cleanup();
            }
            _cache = null;
        }

        private List<IPoolItem<T>> GetAllUnusedItems()
        {
            var ls = new List<IPoolItem<T>>();
            var iter = _cache.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.CanCleanup())
                    ls.Add(iter.Current.Value);
            }
            return ls;
        }
        private List<IPoolItem<T>> DefaultOnCleanup(List<IPoolItem<T>> unusedItems)
        {
            unusedItems.Sort((a, b) => a.LastUseTime.CompareTo(b.LastUseTime));
            return unusedItems;
        }
    }

}

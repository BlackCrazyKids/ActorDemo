using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PoolManager : IManager
    {
        public PoolManager() { }

        public int Count { get { return _objectPools.Count; } }

        private Dictionary<string, IObjectPool> _objectPools;

        public void Init()
        {
            _objectPools = new Dictionary<string, IObjectPool>();
        }
        public void Dispose()
        {
            var iter = _objectPools.GetEnumerator();
            while (iter.MoveNext())
            {
                var item = iter.Current.Value;
                item.Dispose();
                _objectPools.Remove(item.Name);
            }
        }

        public ObjectPool<T> CreateObjectPool<T>(string name) where T : UnityEngine.Object, new()
        {
            var objectPool = CreateObjectPool<T>(name, 10);
            return objectPool;
        }
        public ObjectPool<T> CreateObjectPool<T>(string name, int maxSize) where T : UnityEngine.Object, new()
        {
            var objectPool = CreateObjectPool<T>(name, maxSize, 0);
            return objectPool;
        }
        public ObjectPool<T> CreateObjectPool<T>(string name, int maxSize, float interval) where T : UnityEngine.Object, new()
        {
            var objectPool = CreateObjectPool<T>(name, maxSize, interval, null);
            return objectPool;
        }
        public ObjectPool<T> CreateObjectPool<T>(string name, int maxSize, float interval,
            Func<List<IPoolItem<T>>, List<IPoolItem<T>>> onCleanup) where T : UnityEngine.Object, new()
        {
            var objectPool = new ObjectPool<T>(name, maxSize, interval, onCleanup);
            if (HasPool(name))
                Debug.LogErrorFormat("PoolManager中已存在 {0} ObjectPool对象.", name);
            else
                _objectPools.Add(name, objectPool);
            return objectPool;
        }

        public bool HasPool(string name)
        {
            return _objectPools.ContainsKey(name);
        }
        public IObjectPool GetObjectPool(string name)
        {
            if (HasPool(name))
                return _objectPools[name];
            return null;
        }
        public bool DisposePool(string name)
        {
            if (HasPool(name))
            {
                _objectPools[name].Dispose();
                _objectPools.Remove(name);
                return true;
            }
            return false;
        }
    }
}

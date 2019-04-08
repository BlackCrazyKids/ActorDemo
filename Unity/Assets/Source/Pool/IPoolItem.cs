using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IPoolItem<T> where T : UnityEngine.Object
    {
        string Name { get; }
        int MaxSize { get; }
        float LastUseTime { get; }
        float CacheTime { get; }
        T Original { get; }

        T Get();
        bool Release(T obj);
        bool CanCleanup();
        void Cleanup();
    }
}

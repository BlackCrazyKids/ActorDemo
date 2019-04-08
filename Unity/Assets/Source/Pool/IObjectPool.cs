using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IObjectPool
    {
        string Name { get; }
        int MaxSize { get; }
        int Count { get; }

        bool HasItem(string name);
        void Dispose();
    }
}

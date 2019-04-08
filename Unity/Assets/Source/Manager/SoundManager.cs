using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SoundManager : IManager
    {
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SoundManager();
                return _instance;
            }
        }
        static SoundManager _instance;
        protected SoundManager() { }

        public void Init()
        {

        }
        public void Dispose()
        {
             
        }
    }
}

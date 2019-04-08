using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using XmlCfg;

namespace Timeline
{
    public abstract class BaseBehaviour<T> : PlayableBehaviour where T : XmlObject
    {
        /// <summary>
        /// 数据对象
        /// </summary>
        public abstract T Xml { get; }
    }
}
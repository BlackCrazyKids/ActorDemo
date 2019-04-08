using UnityEngine;
using System.Collections;
using System;

namespace Flux
{
    public abstract class FController : FEvent
    {
        [SerializeField, HideInInspector]
        [FEventField("资源路径"), FFindPathBtn]
        protected string _path;

        public override string Text
        {
            get
            {
                string[] nodes = _path == null ? null : _path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string tip = "Miss";
                if (nodes != null && nodes.Length > 0)
                    tip = nodes[nodes.Length - 1];
                return tip;
            }
            set
            {
            }
        }
    }
}

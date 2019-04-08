using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;
using System.IO;
using System.Collections.Generic;

namespace AssetMgr
{
    [Serializable]
    public class DirectorySetting<T> where T : DirectorySetting<T>, new()
    {
        [HideInInspector]
        public string Name { get { return Path.GetFileName(RelPath); } }
        [HideInInspector, SerializeField]
        public string RelPath;

        [BoxGroup("基础配置"), LabelText("是否处理文件夹")]
        public bool IsActive = true;
        [BoxGroup("基础配置"), LabelText("是否重写配置")]
        [EnableIf("OverrideEnableIf")]
        public bool IsOverride = false;

        bool OverrideEnableIf
        {
            get
            {
                if (IsActive == false)
                    IsOverride = IsActive;

                return IsActive;
            }
        }
       
        
        [HideInInspector, SerializeField]
        public T Parent = null;
        [HideInInspector, SerializeField]
        public List<T> Children = new List<T>();
        public bool IsNull
        {
            get
            {
                return string.IsNullOrEmpty(Name);
            }
        }

        public virtual void DoHandle() { }

    }
}


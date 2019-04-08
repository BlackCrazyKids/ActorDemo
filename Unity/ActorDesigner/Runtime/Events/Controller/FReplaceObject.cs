using UnityEngine;
using System.Collections;
using System;

namespace Flux
{
    [FEvent("控制器/对象替换", isSingle: true)]
    public class FReplaceObject : FController
    {
        [SerializeField, HideInInspector]
        [FEventField("新对象"), FFindPathBtn]
        private string _newObject;
        [SerializeField, HideInInspector]
        [FEventField("相对位置")]
        private Vector3 _offset;
        [SerializeField, HideInInspector]
        [FEventField("相对旋转")]
        private Vector3 _eulerAngles;

        private bool _iniState;
        private GameObject _oldObj;
        private GameObject _newObj;
        private TransformSnapshot _newSnapshot;

        public override string Text
        {
            get
            {
                string[] oldNodes = _path == null ? null : _path.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] newNodes = _path == null ? null : _newObject.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string tip = "Miss";
                if (oldNodes != null && oldNodes.Length > 0
                    && newNodes != null && newNodes.Length > 0)
                {
                    string oldObj = oldNodes[oldNodes.Length - 1];
                    string newObj = newNodes[newNodes.Length - 1];
                    tip = string.Format("{0}->{1}", oldObj, newObj);
                }
                return tip;
            }
            set
            {
            }
        }

        protected override void OnInit()
        {
            base.OnInit();
            _oldObj = FUtility.FindGameObject(_path);
            _newObj = FUtility.FindGameObject(_newObject);            
        }

        protected override void OnTrigger(float timeSinceTrigger)
        {
            base.OnTrigger(timeSinceTrigger);

            if (_newObj != null)
                _newSnapshot = new TransformSnapshot(_newObj.transform, false);

            if (_oldObj != null)
                _oldObj.SetActive(false);
            if (_newObj != null)
            {
                _newObj.transform.parent = _oldObj.transform.parent;
                _newObj.transform.localPosition = _offset;
                _newObj.transform.localEulerAngles = _eulerAngles;
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (_oldObj != null)
                _oldObj.SetActive(true);
            if (_newSnapshot != null)
                _newSnapshot.Restore();
        }
    }
}

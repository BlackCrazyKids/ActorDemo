using UnityEngine;
using System.Collections;
using XmlCfg.Skill;

namespace Flux
{
    [FEvent("控制器/投射物体")]
    public class FCastObject : FController
    {
        [SerializeField, HideInInspector]
        private int _curveId;
        [SerializeField, HideInInspector]
        private bool _isTraceTarget;
        [SerializeField, HideInInspector]
        private bool _passBody;
        [SerializeField, HideInInspector]
        private Vector3 _position;
        [SerializeField, HideInInspector]
        private Vector3 _eulerAngles;

        private Transform _targetObj;
        private float _startTime;

        protected override void OnTrigger(float timeSinceTrigger)
        {
            base.OnTrigger(timeSinceTrigger);

            var go = FUtility.FindGameObject(_path);
            if (go != null)
            {
                _targetObj = go.transform;
                _targetObj.localPosition = _position;
                _targetObj.localEulerAngles = _eulerAngles;
            }
            _startTime = timeSinceTrigger;
        }

        protected override void OnUpdateEvent(float timeSinceTrigger)
        {
            base.OnUpdateEvent(timeSinceTrigger);

            if (_targetObj == null) return;


        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}

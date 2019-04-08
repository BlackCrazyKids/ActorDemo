using UnityEngine;
using System.Collections;
using XmlCfg.Skill;

namespace Flux
{
    [FEvent("控制器/直线位移")]
    public class FMove : FController
    {
        [SerializeField, HideInInspector]
        private MoveType _moveType;
        [SerializeField, HideInInspector]
        [FEventField("从释放者位置开始")]
        private bool _isRelateSelf;
        [SerializeField, HideInInspector]
        [FEventField("相对偏移")]
        private Vector3 _offset;
        [SerializeField, HideInInspector]
        [FEventField("Y轴旋转角度")]
        private float _angle;
        [SerializeField, HideInInspector]
        [FEventField("速度", "位移速度m/s"), Range(0f, 999f)]
        private float _speed;

        private Transform _targetObj;
        private float _startTime;

        protected override void OnTrigger(float timeSinceTrigger)
        {
            base.OnTrigger(timeSinceTrigger);

            var go = FUtility.FindGameObject(_path);
            if (go != null)
            {
                _targetObj = go.transform;
                _targetObj.localPosition += _offset;
                Vector3 eulerAngles = _targetObj.localEulerAngles;
                eulerAngles.y = _angle;
                _targetObj.localEulerAngles = eulerAngles;
            }
            _startTime = timeSinceTrigger;
        }

        protected override void OnUpdateEvent(float timeSinceTrigger)
        {
            base.OnUpdateEvent(timeSinceTrigger);

            if (_targetObj == null) return;
            float t = timeSinceTrigger - _startTime;
            Vector3 dir = Vector3.zero;
            switch (_moveType)
            {
                case MoveType.MoveToTarget:
                    break;
                case MoveType.MoveInDirection:
                    dir = _targetObj.forward; 
                    break;
                default:
                    break;
            }

        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}

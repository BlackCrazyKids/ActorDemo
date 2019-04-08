using UnityEngine;
using System.Collections;
using XmlCfg.Skill;

namespace Flux
{
    [FEvent("播放器/技能特效")]
    public class FSkillEffectEvent : FPlayParticleEvent
    {
        [SerializeField, HideInInspector]
        [FEventField("资源路径"), FFindPathBtn()]
        private string _path;
        //[SerializeField, HideInInspector]
        //[FEventField("淡出时间")]
        //private float _fadeOutTime;
        [SerializeField, HideInInspector]
        [FEventField("跟随目标")]
        private bool _followDir;
        [SerializeField, HideInInspector]
        [FEventField("节点名称")]
        private string _nodeName;
        [SerializeField, HideInInspector]
        [FEventField("相对于释放者", "选定设置目标,自己或者对方")]
        private bool _isRelateSelf;
        [SerializeField, HideInInspector]
        [FEventField("相对坐标")]
        private Vector3 _position;
        [SerializeField, HideInInspector]
        [FEventField("欧拉角")]
        private Vector3 _eulerAngles;
        [SerializeField, HideInInspector]
        [FEventField("缩放")]
        private Vector3 _scale;
        [SerializeField, HideInInspector]
        [FEventField("屏幕对齐方式")]
        private EffectAlignType _effectAlignType;

        //private float _startTime;
        private Vector3 _fixEulerAngle;

        protected override void OnTrigger(float timeSinceTrigger)
        {
            base.OnTrigger(timeSinceTrigger);
            //_startTime = timeSinceTrigger;

            var effect = FUtility.FindGameObject(_path);
            if (effect != null && _particleSystem == null)
                _particleSystem = effect.GetComponent<ParticleSystem>();

            if (_particleSystem == null) return;

            if (!string.IsNullOrEmpty(_nodeName))
            {
                //查询目标下的相对节点,再绑定特效到节点
                //Transform node = 
                //_particleSystem.transform.parent
            }

            _particleSystem.transform.localPosition = _position;
            _particleSystem.transform.localEulerAngles = _eulerAngles;
            _particleSystem.transform.localScale = _scale;

            if (!_followDir)
            {

            }
        }

        protected override void OnUpdateEvent(float timeSinceTrigger)
        {
            base.OnUpdateEvent(timeSinceTrigger);
            //float t = timeSinceTrigger - _startTime + _fadeOutTime;
            //if (t < LengthTime) return;

            if (!_followDir)
            {

            }
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}

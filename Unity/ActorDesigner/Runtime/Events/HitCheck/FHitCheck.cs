using UnityEngine;
using System.Collections;
using XmlCfg.Skill;

namespace Flux
{
    [FEvent("碰撞检测")]
    public class FHitCheck : FEvent
    {
        [SerializeField, HideInInspector]
        private Transform _target;//碰撞绑定对象
        [SerializeField, HideInInspector]
        private bool _isDynamic = false;
        [SerializeField, HideInInspector]
        private FHitZone _zone;
        [SerializeField, HideInInspector]
        private FSequence _sequence;

        public override string Text { get { return string.Format("区域{0}", _zone == null ? "?" : _zone.ZoneID.ToString()); } set { } }

        private TransformSnapshot _targetSnapshot;


        protected static bool HitCube(FHitZone zone)
        {
            return false;
        }
        protected static bool HitSphere(FHitZone zone)
        {
            return false;
        }
        protected static bool HitCylinder(FHitZone zone)
        {
            return false;
        }
        protected static bool CheckHit(FHitZone zone)
        {
            bool result = false;
            switch (zone.SharpType)
            {
                case HitSharpType.Cube:
                    result = HitCube(zone);
                    break;
                case HitSharpType.Sphere:
                    result = HitSphere(zone);
                    break;
                case HitSharpType.Cylinder:
                    result = HitCylinder(zone);
                    break;
                default:
                    break;
            }
            return result;
        }

        protected bool HasData()
        {
            bool hasZone = _zone != null;
            if (!hasZone)
                Debug.LogError("未配置碰撞区域数据");
            bool hasSequence = _sequence != null;
            if (!hasSequence)
                Debug.LogError("未配置序列信息");
            bool hasTarget = _target != null;
            if (!hasTarget)
                Debug.LogError("未配置目标对象");

            return hasZone && hasSequence && hasTarget;
        }
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();
            _target = transform;
            _targetSnapshot = new TransformSnapshot(_target, false);
        }
        protected override void OnUpdateEvent(float timeSinceTrigger)
        {
            base.OnUpdateEvent(timeSinceTrigger);

            if (!HasData()) return;

            if (CheckHit(_zone) && Sequence.IsPlaying)
                _sequence.Play(_sequence.InverseFrameRate + timeSinceTrigger);
        }

        protected override void OnPause()
        {
            base.OnPause();

            if (!HasData()) return;
            _sequence.Pause();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (!HasData()) return;
            _sequence.Resume();
        }
        protected override void OnStop()
        {
            base.OnStop();
            if (!HasData()) return;
            _sequence.Stop();
            _targetSnapshot.Restore();
        }
        protected override void OnFinish()
        {
            base.OnFinish();
            if (!HasData()) return;
            _sequence.Pause();
        }
    }
}

using UnityEngine;
using System.Collections;
using XmlCfg.Skill;
using System;

namespace Flux
{
    [FEvent("控制器/激活隐藏", isSingle: true)]
    public class FActive : FController
    {
        [SerializeField, HideInInspector]
        [FEventField("是否激活")]
        private bool _enable;

        private bool _iniState;
        private GameObject _targetObj;    

        protected override void OnInit()
        {
            base.OnInit();
            _targetObj = FUtility.FindGameObject(_path);
            if (_targetObj != null)
                _iniState = _targetObj.activeSelf;
        }

        protected override void OnTrigger(float timeSinceTrigger)
        {
            base.OnTrigger(timeSinceTrigger);
            if (_targetObj != null)
                _targetObj.SetActive(_enable);
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (_targetObj != null)
                _targetObj.SetActive(_iniState);
        }
    }
}

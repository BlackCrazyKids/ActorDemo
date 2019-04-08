namespace CS.ActorConfig
{
    using System;
    using XmlCfg.Skill;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities.Editor;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Flux;

    [Serializable]
    public class ModelActionEditor
    {
        public enum ActionState
        {
            New,
            Inherit,
            Override,
        }
        public const int INHERIT_WIDTH = 50;
        public const int ACT_NAME_WIDTH = 100;
        public const int ACT_SRC_WIDTH = 100;
        public const int ACT_CLIP_WIDTH = 100;

        [HideInInspector]
        public readonly GeneralAction ModelAction;
        [HideInInspector]
        public readonly bool IsSkillAction = false;

        private ActorConfigEditor _actorEditor;
     

        [OnInspectorGUI]
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            Color color = Color.green;
            string actState = "新建";
            switch (ActState)
            {
                case ActionState.New:
                    break;
                case ActionState.Inherit:
                    color = Color.yellow;
                    actState = "继承";
                    break;
                case ActionState.Override:
                    color = Color.red;
                    actState = "重写";
                    break;
            }
            GUIHelper.PushColor(color);
            GUILayout.Label(actState, SirenixGUIStyles.LabelCentered, GUILayout.Width(INHERIT_WIDTH));
            GUIHelper.PopColor();

            GUILayout.Label(ActionName, SirenixGUIStyles.LabelCentered, GUILayout.Width(ACT_NAME_WIDTH));
            GUILayout.Label(IsFromOther ? "其他" : "自己", SirenixGUIStyles.LabelCentered, GUILayout.Width(ACT_SRC_WIDTH));

            bool isClipMiss = string.IsNullOrEmpty(ActionClip);
            GUIHelper.PushColor(isClipMiss ? Color.red : Color.green);
            GUILayout.Label(isClipMiss ? "Error" : ActionClip, SirenixGUIStyles.LabelCentered, GUILayout.Width(ACT_CLIP_WIDTH));
            GUIHelper.PopColor();

            GUIHelper.PushGUIPositionOffset(Vector2.up * -4f);
            if (GUILayout.Button("加载", SirenixGUIStyles.ButtonLeft))
            {
                LoadSequence();
            }
            bool canOverride = ActState == ActionState.Inherit;
            if (GUILayout.Button(canOverride? "重写" : "修改", SirenixGUIStyles.ButtonRight))
            {
                Modifier();
            }
            bool isSelected = IsSelected;
            if (isSelected)
                GUIHelper.PushColor(Color.green);
            if (GUILayout.Button("选择", SirenixGUIStyles.Button, GUILayout.Width(40)))
            {
                IsSelected = !IsSelected;
            }
            if (isSelected)
                GUIHelper.PopColor();
            GUIHelper.PopGUIPositionOffset();
            if (ActState != ActionState.Inherit)
            {
                if (SirenixEditorGUI.IconButton(EditorIcons.X))
                {
                    if (ActState == ActionState.Override)
                    {
                        ActState = ActionState.Inherit;
                        var baseEditor = HomeConfig.Instance.GetActorEditor(_actorEditor.BaseName);
                        ModelActionEditor action = null;
                        if (IsSkillAction)
                            action = baseEditor.SkillActions.Find(a => a.ActionName.Equals(ActionName));
                        else
                            action = baseEditor.GeneralActions.Find(a => a.ActionName.Equals(ActionName));
                        OtherModelName = action.OtherModelName;
                        ActionClip = action.ActionClip;
                    }
                    else
                    {
                        if (IsSkillAction)
                            _actorEditor.SkillActions.Remove(this);
                        else
                            _actorEditor.GeneralActions.Remove(this);
                    }
                }
            }
            else
            {
                GUILayout.Label("", GUILayout.Width(18), GUILayout.Height(18));
            }

            EditorGUILayout.EndHorizontal();
        }
        private void LoadSequence()
        {
            HomeConfig.Instance.OpenSequence(_actorEditor, this);
        }
        private void Modifier()
        {
            ActionWindow.Init(_actorEditor, this, false, (a) =>
            {
                if (a.ActState == ActionState.Inherit)
                    a.ActState = ActionState.Override;
                OnCfgChange();
            });
        }

        public ActionState ActState { get; set; }
        public ModelActionEditor() { }
        public ModelActionEditor(ActorConfigEditor actorEditor, GeneralAction modelAction, bool isSkill)
        {
            ModelAction = modelAction;
            IsSkillAction = isSkill;

            _actorEditor = actorEditor;
        }
        public void ResetActorEditor(ActorConfigEditor actorEditor)
        {
            _actorEditor = actorEditor;
            OnCfgChange();
        }

        public override bool Equals(object obj)
        {
            ModelActionEditor other = obj as ModelActionEditor;
            if (other == null) return false;

            bool result = other.IsFromOther.Equals(IsFromOther);
            result &= other.OtherModelName.Equals(OtherModelName);
            result &= other.ActionClip.Equals(ActionClip);

            return result;
        }

        public bool IsFromOther { get { return !string.IsNullOrEmpty(ModelAction.OtherModelName); } }
        [OnValueChanged("OnCfgChange")]
        public string ActionName { get { return string.IsNullOrEmpty(ModelAction.ActionName) ? string.Empty : ModelAction.ActionName; } set { ModelAction.ActionName = value; } }        
        [OnValueChanged("OnCfgChange")]
        public string OtherModelName { get { return string.IsNullOrEmpty(ModelAction.OtherModelName) ? string.Empty : ModelAction.OtherModelName; } set { ModelAction.OtherModelName = value; } }
        [OnValueChanged("OnCfgChange")]
        public string ActionClip { get { return string.IsNullOrEmpty(ModelAction.ActionClip) ? string.Empty : ModelAction.ActionClip; } set { ModelAction.ActionClip = value; } }
        public bool IsSelected { get; set; }

        private void OnCfgChange()
        {
            _actorEditor.OnCfgChange();
        }
    }
}

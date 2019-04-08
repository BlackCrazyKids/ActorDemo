namespace ActorEditor
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using Sirenix.OdinInspector.Editor;
    using System.Collections.Generic;

    internal class ActionSettingWindow : OdinEditorWindow
    {
        public static void Init(ActorEditor actorEditor, ActionEditor action, bool isAddAction, Action<ActionEditor> result = null)
        {
            var window = GetWindow<ActionSettingWindow>(true, "行为配置窗口", true);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 250);
            window.minSize = new Vector2(400, 250);
            window.maxSize = new Vector2(400, 250);

            window._nameStyle = SirenixGUIStyles.BoldTitleCentered;
            window._nameStyle.fontSize = 30;
            window._nameStyle.fontStyle = FontStyle.Bold;
            window._nameStyle.normal.textColor = Color.white;

            window._isAddAction = isAddAction;
            window._selfActorEditor = actorEditor;
            window._action = action;
            window._actionName = action.ActionName;
            window._clipName = action.ActionClip;
            window._otherModelName = action.OtherModelName;
            window._result = result;
            if (!action.IsFromOther)
                window.LoadSelfAction();
            else
                window.LoadOtherAction(action.OtherModelName);
        }

        GUIStyle _nameStyle;
        ActorEditor _otherActorEditor;
        ActorEditor _selfActorEditor;
        Action<ActionEditor> _result;

        ActionEditor _action;
        string _actionName = string.Empty;
        string _clipName = string.Empty;
        string _otherModelName = string.Empty;
        bool _isAddAction = false;
        List<string> _actClipList;
        bool IsFromOther { get { return _action.IsFromOther; } }

        private void LoadSelfAction()
        {
            _otherModelName = string.Empty;
            _otherActorEditor = null;
            _actClipList = _selfActorEditor.GetActionClips();
        }
        private void LoadOtherAction(string name)
        {
            _otherModelName = name;
            _otherActorEditor = HomeConfig.Instance.GetActorEditor(name);
            _actClipList = _otherActorEditor.GetActionClips();
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            if (EditorApplication.isCompiling)
            {
                Debug.Log("[行为窗口]脚本正在编译,所以自动关闭窗口!");
                Close();
                return;
            }

            SirenixEditorGUI.BeginBox();
            {
                SirenixEditorGUI.BeginBoxHeader();
                GUILayout.Label(_selfActorEditor.ModelName, _nameStyle);
                SirenixEditorGUI.EndBoxHeader();
                bool isNew = _action.ActState == ActionEditor.ActionState.New;
                if (!isNew) GUIHelper.PushGUIEnabled(false);
                _actionName = EditorGUILayout.TextField("动作行为", _actionName);
                if (!isNew) GUIHelper.PushGUIEnabled(true);


                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("其他角色", SirenixGUIStyles.BoldLabel, GUILayout.Width(142));
                    if (GUILayout.Button(_otherModelName, EditorStyles.textField))
                    {
                        var list = HomeConfig.Instance.GetAllActorList();
                        list.Remove(_selfActorEditor.ModelName);
                        SimplePopupCreator.ShowDialog(list, (name) => LoadOtherAction(name));
                    }
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                    {
                        LoadSelfAction();
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("动画文件", SirenixGUIStyles.BoldLabel, GUILayout.Width(142));
                    if (GUILayout.Button(_clipName, SirenixGUIStyles.Popup))
                        SimplePopupCreator.ShowDialog(_actClipList, (name) => _clipName = name);
                    if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(20), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                        _clipName = string.Empty;
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("修改行为", GUILayout.Height(50)))
                    {
                        if (_isAddAction)
                        {
                            bool hasSame = false;
                            if (!_action.IsSkillAction)
                                hasSame = _selfActorEditor.GeneralActions.Exists(a => a.ActionName.Equals(_actionName));
                            else
                                hasSame = _selfActorEditor.SkillActions.Exists(a => a.ActionName.Equals(_actionName));

                            if (!hasSame)
                            {
                                _action.ActionName = _actionName;
                                _action.ActionClip = _clipName;
                                _action.OtherModelName = _otherModelName;
                                _selfActorEditor.SetChildAction(_action);
                                if (_result != null) _result(_action);
                                Close();
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("动作配置错误", "动作名重复,请重新配置!!!", "确定");
                            }
                        }
                        else
                        {
                            _action.ActionName = _actionName;
                            _action.ActionClip = _clipName;
                            _action.OtherModelName = _otherModelName;
                            if (_result != null) _result(_action);
                            Close();
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
        protected override void OnDestroy()
        {
            _action = null;
            _selfActorEditor = null;
            _otherActorEditor = null;
            _result = null;
            _actClipList = null;
        }
    }
}
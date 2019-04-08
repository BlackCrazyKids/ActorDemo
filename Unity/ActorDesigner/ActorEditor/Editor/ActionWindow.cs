namespace CS.ActorConfig
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using Sirenix.OdinInspector.Editor;
    using System.Collections.Generic;

    internal class ActionWindow : OdinEditorWindow
    {
        public static void Init(ActorConfigEditor actorEditor, ModelActionEditor modelAction, bool isAddAction, Action<ModelActionEditor> result = null)
        {
            var window = GetWindow<ActionWindow>(true, "行为配置窗口", true);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 250);
            window.minSize = new Vector2(400, 250);
            window.maxSize = new Vector2(400, 250);

            window._nameStyle = SirenixGUIStyles.BoldTitleCentered;
            window._nameStyle.fontSize = 30;
            window._nameStyle.fontStyle = FontStyle.Bold;
            window._nameStyle.normal.textColor = Color.white;

            window._isAddAction = isAddAction;
            window._selfActorEditor = actorEditor;
            window._modelAction = modelAction;
            window._actionName = modelAction.ActionName;
            window._clipName = modelAction.ActionClip;
            window._otherModelName = modelAction.OtherModelName;
            window._result = result;
            if (!modelAction.IsFromOther)
                window.LoadSelfAction();
            else
                window.LoadOtherAction(modelAction.OtherModelName);
        }

        GUIStyle _nameStyle;
        ActorConfigEditor _otherActorEditor;
        ActorConfigEditor _selfActorEditor;
        Action<ModelActionEditor> _result;

        ModelActionEditor _modelAction;
        string _actionName = string.Empty;
        string _clipName = string.Empty;
        string _otherModelName = string.Empty;
        bool _isAddAction = false;
        List<string> _actClipList;
        bool IsFromOther { get { return _modelAction.IsFromOther; } }

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
                bool isNew = _modelAction.ActState == ModelActionEditor.ActionState.New;
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
                            if (!_modelAction.IsSkillAction)
                                hasSame = _selfActorEditor.GeneralActions.Exists(a => a.ActionName.Equals(_actionName));
                            else
                                hasSame = _selfActorEditor.SkillActions.Exists(a => a.ActionName.Equals(_actionName));

                            if (!hasSame)
                            {
                                _modelAction.ActionName = _actionName;
                                _modelAction.ActionClip = _clipName;
                                _modelAction.OtherModelName = _otherModelName;
                                _selfActorEditor.SetChildAction(_modelAction);
                                if (_result != null) _result(_modelAction);
                                Close();
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("动作配置错误", "动作名重复,请重新配置!!!", "确定");
                            }
                        }
                        else
                        {
                            _modelAction.ActionName = _actionName;
                            _modelAction.ActionClip = _clipName;
                            _modelAction.OtherModelName = _otherModelName;
                            if (_result != null) _result(_modelAction);
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
            _modelAction = null;
            _selfActorEditor = null;
            _otherActorEditor = null;
            _result = null;
            _actClipList = null;
        }
    }
}
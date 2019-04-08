namespace CS.ActorConfig
{
    using System;
    using System.IO;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using Sirenix.OdinInspector.Editor;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using Sirenix.Utilities.Editor;
    using XmlCfg.Skill;
    using Cfg.Character;
    using Cfg;

    [Serializable]
    public class ActorConfigEditor
    {
        private static readonly List<string> _emptyClips = new List<string>();

        private ActorConfig _actorCfg;
        private string _path;
        private Model _model;
        private List<string> _clips;
        private Dictionary<string, ActorConfigEditor> _childActor = new Dictionary<string, ActorConfigEditor>();
        private bool _clearAllSelected = false;

        public bool IsDirty { get; private set; }
        public string CharacterPath { get { return HomeConfig.Instance.GetCharacterPath(_model.ModelPath); } }
        public string AvatarPath { get { return HomeConfig.Instance.GetAvatarPath(_model.AvatarPath); } }
        public float ModelScale { get { return _model.ModelScale; } }
        public string Path { get { return _path; } }
        public string MenuItemName { get { return string.Format("{0}/{1}", ActionHomeConfig.MenuItems[GroupType], ModelName); } }
        public GroupType GroupType
        {
            get
            {
                if (_model == null)
                    return GroupType.None;
                return (GroupType)_model.GroupType;
            }
            //set
            //{
            //    ActorCfgWindow window = ActorCfgWindow.GetWindow<ActorCfgWindow>();
            //    OdinMenuItem item = window.MenuTree.Selection.FirstOrDefault();
            //    if (item != null)
            //    {
            //        ActorConfigEditor model = item.Value as ActorConfigEditor;
            //        HomeConfigPreview.Instance.RemoveActor(model);
            //        item.Parent.ChildMenuItems.Remove(item);

            //        _actorCfg.GroupType = value;
            //        HomeConfigPreview.Instance.AddActor(model);
            //        var group = window.MenuTree.GetMenuItem(Group);
            //        group.ChildMenuItems.Add(item);
            //        item.MenuTree.Selection.Clear();
            //        item.Select();
            //        item.MenuTree.UpdateMenuTree();
            //        item.MenuTree.DrawMenuTree();
            //    }
            //}
        }
        public string ModelName { get { return _actorCfg == null ? "" : _actorCfg.ModelName; } set { } }
        [OnValueChanged("OnCfgChange", true)]
        public string BaseName { get { return _actorCfg == null ? "" : _actorCfg.BaseModelName; } set { } }
        public string Group
        {
            get
            {
                return ActionHomeConfig.MenuItems[(GroupType)_model.GroupType];
            }
        }


        public ActorConfigEditor() { }
        public ActorConfigEditor(string path, ActorConfig cfg = null)
        {
            _path = path;
            _actorCfg = cfg != null ? cfg : new ActorConfig();
            if (File.Exists(path))
                _actorCfg.LoadAConfig(path);
            if (CfgManager.Model.ContainsKey(_actorCfg.ModelName))
                _model = CfgManager.Model[_actorCfg.ModelName];
            if (_actorCfg != null)
            {
                foreach (var item in _actorCfg.GeneralActions)
                {
                    var action = new ModelActionEditor(this, item.Value, false);
                    action.ActState = ModelActionEditor.ActionState.New;
                    GeneralActions.Add(action);
                }
                foreach (var item in _actorCfg.SkillActions)
                {
                    var action = new ModelActionEditor(this, item.Value, true);
                    action.ActState = ModelActionEditor.ActionState.New;
                    SkillActions.Add(action);
                }
            }
        }
        public void InitBaseModelAction()
        {
            string name = BaseName;
            if (string.IsNullOrEmpty(name))
            {
                RemoveAllBaseAction();
                return;
            }

            //--配置基类模型行为
            var baseModel = HomeConfig.Instance.GetActorEditor(name);
            baseModel.AddChildActor(this);
        }
        public List<string> GetActionClips()
        {
            var clips = HomeConfig.Instance.AllCharacterClips;
            string assetName = _model.ModelPath;
            if (clips.ContainsKey(assetName))
            {
                if (_clips == null)
                    _clips = new List<string>(clips[assetName].Keys);
                return _clips;
            }
            else Debug.LogErrorFormat("[获取Clips]{0} 模型资源不存在!", assetName);
            return _emptyClips;
        }
        /// <summary>
        /// 重置旋转状态
        /// </summary>
        public void ReselectedState()
        {
            foreach (var item in GeneralActions)
                item.IsSelected = false;
            foreach (var item in SkillActions)
                item.IsSelected = false;
            _clearAllSelected = true;
        }
        public void SetChildAction(ModelActionEditor action)
        {
            foreach (var item in _childActor)
            {
                ActorConfigEditor actor = item.Value;
                List<ModelActionEditor> list = null;
                if (action.IsSkillAction)
                    list = actor.SkillActions;
                else
                    list = actor.GeneralActions;

                int index = list.FindIndex(s => s.ActionName.Equals(action.ActionName));
                if (index == -1)
                {
                    ModelActionEditor add = Clipboard.Paste<ModelActionEditor>();
                    add.ActState = ModelActionEditor.ActionState.New;
                    list.Add(add);
                    continue;
                }

                if (list[index].ActState == ModelActionEditor.ActionState.Override)
                    continue;

                Clipboard.Copy(action, CopyModes.DeepCopy);
                ModelActionEditor temp = Clipboard.Paste<ModelActionEditor>();
                temp.ResetActorEditor(actor);
                list[index] = temp;
            }
        }

        Dictionary<string, ModelActionEditor> GetGeneralActionDict()
        {
            var pairs = new Dictionary<string, ModelActionEditor>();
            foreach (var item in GeneralActions)
            {
                pairs.Add(item.ActionName, item);
            }
            return pairs;
        }
        Dictionary<string, ModelActionEditor> GetSkillActionDict()
        {
            var pairs = new Dictionary<string, ModelActionEditor>();
            foreach (var item in SkillActions)
            {
                pairs.Add(item.ActionName, item);
            }
            return pairs;
        }
        void AddChildActor(ActorConfigEditor childCfg)
        {
            var modelDict = childCfg.GetGeneralActionDict();
            foreach (var item in GeneralActions)
            {
                if (modelDict.ContainsKey(item.ActionName))
                {
                    modelDict[item.ActionName].ActState = ModelActionEditor.ActionState.Override;
                }
                else
                {
                    Clipboard.Copy(item, CopyModes.DeepCopy);
                    ModelActionEditor action = Clipboard.Paste<ModelActionEditor>();
                    action.ActState = ModelActionEditor.ActionState.Inherit;
                    childCfg.GeneralActions.Add(action);
                    action.ResetActorEditor(childCfg);
                }
            }
            var skillDict = childCfg.GetSkillActionDict();
            foreach (var item in SkillActions)
            {
                if (skillDict.ContainsKey(item.ActionName))
                {
                    skillDict[item.ActionName].ActState = ModelActionEditor.ActionState.Override;
                }
                else
                {
                    Clipboard.Copy(item, CopyModes.DeepCopy);
                    ModelActionEditor action = Clipboard.Paste<ModelActionEditor>();
                    action.ActState = ModelActionEditor.ActionState.Inherit;
                    childCfg.SkillActions.Add(action);
                    action.ResetActorEditor(childCfg);
                }
            }
            if (!_childActor.ContainsKey(childCfg.ModelName))
                _childActor.Add(childCfg.ModelName, childCfg);
        }
        void RemoveChildActor(ActorConfigEditor childCfg)
        {
            childCfg.RemoveAllBaseAction();
            if (_childActor.ContainsKey(childCfg.ModelName))
                _childActor.Remove(childCfg.ModelName);
        }
        void RemoveAllBaseAction()
        {
            var array = GeneralActions.ToArray();
            GeneralActions.Clear();
            foreach (var item in array)
            {
                switch (item.ActState)
                {
                    case ModelActionEditor.ActionState.New:
                        GeneralActions.Add(item);
                        break;
                    case ModelActionEditor.ActionState.Inherit:
                        break;
                    case ModelActionEditor.ActionState.Override:
                        item.ActState = ModelActionEditor.ActionState.New;
                        GeneralActions.Add(item);
                        break;
                    default:
                        break;
                }
            }
            array = SkillActions.ToArray();
            SkillActions.Clear();
            foreach (var item in array)
            {
                switch (item.ActState)
                {
                    case ModelActionEditor.ActionState.New:
                        SkillActions.Add(item);
                        break;
                    case ModelActionEditor.ActionState.Inherit:
                        break;
                    case ModelActionEditor.ActionState.Override:
                        item.ActState = ModelActionEditor.ActionState.New;
                        SkillActions.Add(item);
                        break;
                    default:
                        break;
                }
            }
        }


        #region 界面设计       
        [OnInspectorGUI, PropertyOrder(-100)]
        protected void OnInspectorGUI()
        {
            SirenixEditorGUI.BeginBox();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("模型名称:", ModelName, SirenixGUIStyles.MessageBox);
                    EditorGUILayout.LabelField("继承模型:", BaseName, SirenixGUIStyles.MessageBox);
                    EditorGUILayout.EndVertical();
                    if (GUILayout.Button("更换继承", GUILayout.Height(EditorGUIUtility.singleLineHeight * 2 + 10)))
                        ModifyBaseName();
                }
                EditorGUILayout.EndHorizontal();
            }
            SirenixEditorGUI.EndBox();
        }

        [ButtonGroup("Button"), Button("复制", ButtonSizes.Large)]
        private void CopyActions()
        {
            List<ModelActionEditor> copy = new List<ModelActionEditor>();
            foreach (var item in GeneralActions)
                if (item.IsSelected) copy.Add(item);
            foreach (var item in SkillActions)
                if (item.IsSelected) copy.Add(item);
            Clipboard.Copy(copy, CopyModes.DeepCopy);
        }
        [ButtonGroup("Button"), Button("粘贴", ButtonSizes.Large)]
        private void PasteActions()
        {
            var generalDict = GetGeneralActionDict();
            var skillDict = GetSkillActionDict();
            List<ModelActionEditor> paste = new List<ModelActionEditor>();
            if (Clipboard.TryPaste(out paste))
            {
                foreach (var item in paste)
                {
                    if (skillDict.ContainsKey(item.ActionName)
                        || generalDict.ContainsKey(item.ActionName))
                    {
                        Debug.LogFormat("<color=orange>[粘贴行为]{0} 行为重复!</color>", item.ActionName);
                        continue;
                    }

                    if (item.IsSkillAction)
                        SkillActions.Add(item);
                    else
                        GeneralActions.Add(item);
                    item.ResetActorEditor(this);
                    item.IsSelected = false;
                }
            }
            else
            {
                EditorUtility.DisplayDialog("粘贴异常", "行为数据粘贴失败,或者无数据可张贴~~", "确定");
            }
        }
        [ButtonGroup("Button"), Button("保存文件", ButtonSizes.Large)]
        public void Save(bool resave = false)
        {
            if (!IsDirty && !resave) return;
            IsDirty = false;

            _actorCfg.GeneralActions.Clear();
            _actorCfg.SkillActions.Clear();
            foreach (var item in GeneralActions)
            {
                if (item.ActState == ModelActionEditor.ActionState.Inherit) continue;
                var action = item.ModelAction;
                _actorCfg.GeneralActions.Add(action.ActionName, action);
            }
            foreach (var item in SkillActions)
            {
                if (item.ActState == ModelActionEditor.ActionState.Inherit) continue;
                var action = item.ModelAction as SkillAction;
                _actorCfg.SkillActions.Add(action.ActionName, action);
            }
            _actorCfg.SaveAConfig(_path);
        }
        [ButtonGroup("Button"), Button("删除文件", ButtonSizes.Large)]
        public void Delete()
        {
            if (EditorUtility.DisplayDialog("删除操作", "确定要删除文件 -> " + ModelName, "确定", "取消"))
            {
                if (File.Exists(_path))
                    File.Delete(_path);

                ActorCfgWindow window = ActorCfgWindow.GetWindow<ActorCfgWindow>();
                OdinMenuItem item = window.MenuTree.Selection.FirstOrDefault();
                if (item != null && item.Value != null)
                {
                    ActorConfigEditor model = item.Value as ActorConfigEditor;
                    HomeConfig.Instance.RemoveActor(model);
                    item.Parent.ChildMenuItems.Remove(item);
                    item.MenuTree.Selection.Clear();
                    item.Parent.Select();
                    item.MenuTree.UpdateMenuTree();
                    item.MenuTree.DrawMenuTree();
                }
            }
        }

        [Title("普通动作列表", bold: true), OnInspectorGUI, PropertyOrder(10)]
        private void OnModelActionsHeader() { OnHeaderGUI(); }

        [LabelText(" "), Space(-5f), PropertyOrder(15)]
        [ListDrawerSettings(OnTitleBarGUI = "OnGUIGeneralAction", CustomAddFunction = "CreateGeneralAction", HideRemoveButton = true)]
        public List<ModelActionEditor> GeneralActions = new List<ModelActionEditor>();

        [Title("技能动作列表", bold: true), OnInspectorGUI, PropertyOrder(20)]
        private void OnSkillActionsHeader() { OnHeaderGUI(); }

        [LabelText(" "), Space(-5f), PropertyOrder(25)]
        [ListDrawerSettings(OnTitleBarGUI = "OnGUISkilllAction", CustomAddFunction = "CreateSkillAction", HideRemoveButton = true)]
        public List<ModelActionEditor> SkillActions = new List<ModelActionEditor>();


        private void OnHeaderGUI()
        {
            SirenixEditorGUI.BeginHorizontalToolbar(SirenixGUIStyles.BoxHeaderStyle);
            GUILayout.Label(" ", GUILayout.Width(17f));
            GUILayout.Label("继承", SirenixGUIStyles.LabelCentered, GUILayout.Width(ModelActionEditor.INHERIT_WIDTH));
            GUILayout.Label("动作名称", SirenixGUIStyles.LabelCentered, GUILayout.Width(ModelActionEditor.ACT_NAME_WIDTH));
            GUILayout.Label("动画来源", SirenixGUIStyles.LabelCentered, GUILayout.Width(ModelActionEditor.ACT_SRC_WIDTH));
            GUILayout.Label("动画文件", SirenixGUIStyles.LabelCentered, GUILayout.Width(ModelActionEditor.ACT_CLIP_WIDTH));
            GUILayout.Label("操作", SirenixGUIStyles.LabelCentered, GUILayout.ExpandWidth(true));
            SirenixEditorGUI.EndHorizontalToolbar();
        }
        private void ModifyModelName()
        {
            var models = new List<string>(CfgManager.Model.Keys);
            models.Remove(ModelName);
            SimplePopupCreator.ShowDialog(new List<string>(models), (name) =>
            {
                _actorCfg.ModelName = name;

                ActorCfgWindow window = ActorCfgWindow.GetWindow<ActorCfgWindow>();
                OdinMenuItem item = window.MenuTree.Selection.FirstOrDefault();
                item.Name = name;
                item.SearchString = name;
            });
        }
        private void ModifyBaseName()
        {
            List<string> models = HomeConfig.Instance.GetAllActorList();
            string none = "-----None-----";
            models.Insert(0, none);
            models.Remove(ModelName);
            SimplePopupCreator.ShowDialog(new List<string>(models), (name) =>
            {
                if (name.Equals(none)) name = "";
                string old = _actorCfg.BaseModelName;
                _actorCfg.BaseModelName = name;
                bool oldEmpty = string.IsNullOrEmpty(old);
                bool newEmpty = string.IsNullOrEmpty(name);
                if (!oldEmpty)
                {
                    var oldBase = HomeConfig.Instance.GetActorEditor(old);
                    if (!newEmpty && !old.Equals(name))
                    {//--修改继承
                        oldBase.RemoveChildActor(this);
                        var newBase = HomeConfig.Instance.GetActorEditor(name);
                        newBase.AddChildActor(this);
                    }
                    else if (newEmpty)
                    { //--移除继承
                        oldBase.RemoveChildActor(this);
                    }
                }
                else if (!newEmpty)
                {
                    //--添加继承
                    var newBase = HomeConfig.Instance.GetActorEditor(name);
                    newBase.AddChildActor(this);
                }
            });
        }

        bool _isSelected_Model = false;
        bool _isSelected_Skill = false;
        private void CreateGeneralAction()
        {
            var action = new ModelActionEditor(this, new GeneralAction(), false);
            action.ActState = ModelActionEditor.ActionState.New;
            ActionWindow.Init(this, action, true, (act) =>
            {
                GeneralActions.Add(act);
                OnCfgChange();
            });
        }
        private void CreateSkillAction()
        {
            var action = new ModelActionEditor(this, new SkillAction(), true);
            action.ActState = ModelActionEditor.ActionState.New;
            ActionWindow.Init(this, action, true, (act) =>
            {
                SkillActions.Add(act);
                OnCfgChange();
            });
        }
        private void OnGUIGeneralAction()
        {
            OnTitleBarGUI(GeneralActions, ref _isSelected_Model);
        }
        private void OnGUISkilllAction()
        {
            OnTitleBarGUI(SkillActions, ref _isSelected_Skill);
        }
        private void OnTitleBarGUI(List<ModelActionEditor> actionEditors, ref bool state)
        {
            EditorIcon icon = state ? EditorIcons.X : EditorIcons.Checkmark;
            if (SirenixEditorGUI.ToolbarButton(icon))
            {
                state = !state;
                foreach (var item in actionEditors)
                    item.IsSelected = state;
            }
            if (_clearAllSelected)
            {
                _clearAllSelected = false;
                state = false;
            }
        }
        public void OnCfgChange()
        {
            if (!HomeConfig.Instance.IsInit) return;

            IsDirty = true;
        }
        #endregion

    }
}

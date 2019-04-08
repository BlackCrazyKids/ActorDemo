namespace ActorEditor
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Sirenix.Utilities;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities.Editor;
    using Cfg.Character;
    using Cfg;
    using Vec3 = UnityEngine.Vector3;
    using GameEditor;

    [GlobalConfig("ActorEditor/Editor", UseAsset = true)]
    internal class ActorConfig : GlobalConfig<ActorConfig>
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public static readonly Dictionary<GroupType, string> MenuItems = new Dictionary<GroupType, string>()
        {
            {GroupType.None, "主页" },
            {GroupType.Base, "基础类型" },
            {GroupType.Player, "玩家" },
            {GroupType.Monster, "怪物" },
            {GroupType.NPC, "NPC" },
        };
        public static string[] MenuItemNames = { "主页", "基础类型", "玩家", "怪物", "NPC" };




        [FolderPath(RequireExistingPath = true), BoxGroup("Config", showLabel: false)]
        [LabelText("Xml存储目录"), PropertyOrder(-100)]
        public string ActionConfigPath;
        [FolderPath(RequireExistingPath = true), BoxGroup("Config", showLabel: false)]
        [LabelText("Csv存储目录"), PropertyOrder(-99)]
        public string ConfigRelativeDir = "";
        [FolderPath(RequireExistingPath = true), BoxGroup("Config", showLabel: false)]
        [LabelText("角色资源目录"), PropertyOrder(-98)]
        public string CharacterRelativeDir = "";
        [FolderPath(RequireExistingPath = true), BoxGroup("Config", showLabel: false)]
        [LabelText("Avatar资源目录"), PropertyOrder(-98)]
        public string AvatarRelativeDir = "";
        [FolderPath(RequireExistingPath = true), BoxGroup("Config", showLabel: false)]
        [LabelText("特效资源目录"), PropertyOrder(-98)]
        public string EffectRelativeDir = "";
        [FolderPath(RequireExistingPath = true), BoxGroup("Config", showLabel: false)]
        [LabelText("碰撞体资源目录"), PropertyOrder(-98)]
        public string ColliderRelativeDir = "";
    }

    [Serializable]
    internal class HomeConfig
    {
        private static HomeConfig _instance;
        public static HomeConfig Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HomeConfig();
                return _instance;
            }
        }
        private ActorConfig _config;

        #region 界面设计        
        [FolderPath(RequireExistingPath = true), BoxGroup("Config", showLabel: false)]
        [ShowInInspector, ReadOnly, LabelText("Xml存储目录"), PropertyOrder(-100)]
        private string ActionConfigPath { get { return string.Format("{0}/../{1}/", Application.dataPath, _config.ActionConfigPath); } }
        [FolderPath(RequireExistingPath = true), BoxGroup("Config", showLabel: false)]
        [ShowInInspector, ReadOnly, LabelText("Csv存储目录"), PropertyOrder(-99)]
        private string ConfigDir { get { return string.Format("{0}/../{1}/", Application.dataPath, _config.ConfigRelativeDir); } }
        [ButtonGroup("Config/Btns")]
        [Button("加载所有动作", ButtonSizes.Large)]
        public void LoadAll()
        {
            ClearAll();

            _config = ActorConfig.Instance;
            CfgManager.ConfigDir = ConfigDir;
            CfgManager.LoadAll();

            //--初始化资源路径
            EUtil.GetAssetsInSubFolderRecursively(_config.CharacterRelativeDir, "*.prefab", ref AllCharacters);
            EUtil.GetAssetsInSubFolderRecursively(_config.AvatarRelativeDir, "*.prefab", ref AllAvatars);
            EUtil.GetAssetsInSubFolderRecursively(_config.EffectRelativeDir, "*.prefab", ref AllEffects);
            foreach (var item in AllCharacters)
            {
                string searchDir = item.Value.Substring(0, item.Value.LastIndexOf(@"/prefab")) + "/clips";
                var selfClips = new Dictionary<string, string>();
                EUtil.GetAssetsInSubFolderRecursively(searchDir, "*.anim", ref selfClips);
                AllCharacterClips[item.Key] = selfClips;
            }

            //--加载行为配置
            foreach (var model in CfgManager.Model)
            {
                var modelName = model.Key;
                string path = string.Format("{0}/{1}.xml", ActionConfigPath, modelName);
                if (!File.Exists(path))
                {
                    var config = new XmlCfg.Skill.ActorConfig() { ModelName = modelName };
                    string savePath = string.Format("{0}/{1}.xml", ActorConfig.Instance.ActionConfigPath, modelName);
                    var actor = new ActorEditor(savePath, config);
                    actor.Save(true);
                    Debug.LogFormat("<color=orange>新建Actor - {0}</color>", modelName);
                }
            }

            string[] files = Directory.GetFiles(ActionConfigPath, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var path in files)
            {
                var actor = new ActorEditor(path);
                AddActor(actor);
            }
            foreach (var item in _modelDict)
                item.Value.InitBaseModelAction();

            var window = ActorEditorWindow.GetWindow<ActorEditorWindow>();
            window.RefreshTree();
            IsInit = true;
            Debug.Log("加载所有动作 完毕!");
        }
        [ButtonGroup("Config/Btns")]
        [Button("保存所有配置", ButtonSizes.Large)]
        public void SaveAll()
        {
            foreach (var group in ModelGroupDict)
            {
                float count = 0;
                foreach (var cfg in group.Value)
                {
                    try
                    {
                        if (group.Key != GroupType.None)
                            cfg.Save();
                    }
                    catch (Exception)
                    {
                        Debug.LogErrorFormat("{0} 数据异常,无法正常存储!", cfg.ModelName);
                    }

                    EditorUtility.DisplayProgressBar("导出所有配置>" + cfg.Group, cfg.Path, count / group.Value.Count);
                }
            }
            EditorUtility.ClearProgressBar();
            Debug.Log("所有配置保存完毕~~");
        }



        [Title("辅助功能区"), OnInspectorGUI, PropertyOrder(0)]
        void OnGUI_Func()
        {
            EditorGUILayout.BeginHorizontal();
            GUIHelper.PushGUIPositionOffset(UnityEngine.Vector2.up * -2);
            if (GUILayout.Button("设置受击对象", GUILayout.Width(150)))
            {
                SimplePopupCreator.ShowDialog(new List<string>(CfgManager.Model.Keys), (name) =>
                {
                    _targetDisplayName = name;
                    ActorEditor actor = GetActorEditor(name);
                });
            }
            GUIHelper.PopGUIPositionOffset();
            EditorGUILayout.LabelField(_targetDisplayName, EditorStyles.textArea);
            EditorGUILayout.EndHorizontal();
        }

        private string _targetDisplayName = "";
        #endregion


        /// <summary>
        /// 剪切板
        /// </summary>
        private List<ActionEditor> _clipboard = new List<ActionEditor>();
        /// <summary>
        /// 所有模型数据 key-模型名称 value-模型编辑器
        /// </summary>
        private Dictionary<string, ActorEditor> _modelDict = new Dictionary<string, ActorEditor>();
        /// <summary>
        /// 模型分组信息 key-分组类型 value-模型行为编辑器
        /// </summary>
        private Dictionary<GroupType, List<ActorEditor>> _modelGroupDict = new Dictionary<GroupType, List<ActorEditor>>();
        private Dictionary<string, string> AllCharacters = new Dictionary<string, string>();
        private Dictionary<string, string> AllAvatars = new Dictionary<string, string>();
        private Dictionary<string, string> AllEffects = new Dictionary<string, string>();

        public bool IsInit { get; private set; }
        public Dictionary<GroupType, List<ActorEditor>> ModelGroupDict
        {
            get
            {
                if (_config == null)
                    LoadAll();
                return _modelGroupDict;
            }
        }
        public Dictionary<string, Dictionary<string, string>> AllCharacterClips = new Dictionary<string, Dictionary<string, string>>();

        public void AddActor(ActorEditor model)
        {
            if (ModelGroupDict.ContainsKey(model.GroupType))
                ModelGroupDict[model.GroupType].Add(model);
            else
            {
                ModelGroupDict.Add(model.GroupType, new List<ActorEditor>());
                ModelGroupDict[model.GroupType].Add(model);
            }
            if (!_modelDict.ContainsKey(model.ModelName))
                _modelDict.Add(model.ModelName, model);
        }
        public void RemoveActor(ActorEditor model)
        {
            if (!ModelGroupDict.ContainsKey(model.GroupType))
            {
                Debug.LogError("未定义组类型 " + model.GroupType);
                return;
            }
            if (!ModelGroupDict[model.GroupType].Remove(model))
                Debug.LogErrorFormat("{0} 无法从分组中移除", model.MenuItemName);
            if (_modelDict.ContainsKey(model.ModelName))
                _modelDict.Remove(model.ModelName);
        }
        public ActorEditor GetActorEditor(string modelName)
        {
            if (_modelDict.ContainsKey(modelName))
                return _modelDict[modelName];
            Debug.LogErrorFormat("[读取配置]{0} 模型配置不存在!", modelName);
            return null;
        }
        public List<string> GetAllActorList()
        {
            return new List<string>(_modelDict.Keys);
        }
        public void Destroy()
        {
            //保存所有配置
            SaveAll();
            ClearAll();
            _instance = null;
        }
        private void ClearAll()
        {
            IsInit = false;
            _modelGroupDict.Clear();
            CfgManager.Clear();
            _clipboard.Clear();
            _modelDict.Clear();
            AllCharacters.Clear();
            AllAvatars.Clear();
            AllCharacterClips.Clear();
            AllEffects.Clear();
            Resources.UnloadUnusedAssets();
        }
    }
}

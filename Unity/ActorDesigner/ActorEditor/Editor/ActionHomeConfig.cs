namespace CS.ActorConfig
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Sirenix.Utilities;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities.Editor;
    using XmlCfg.Skill;
    using Cfg.Character;
    using Cfg;
    using Vec3 = UnityEngine.Vector3;
    using Flux;
    using GameEditor;

    [GlobalConfig("ActorDesigner/ModelEditor/Editor", UseAsset = true)]
    internal class ActionHomeConfig : GlobalConfig<ActionHomeConfig>
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
        private ActionHomeConfig _config;

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

            _config = ActionHomeConfig.Instance;
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

            //--初始化资源
            SequenceRoot = ActorHelper.CreateGameObject("---行为序列---");
            RuntimeAssetRoot = ActorHelper.CreateGameObject("---运行时资源---");

            CharacterDir = ActorHelper.CreateGameObject("Characters", "/---运行时资源---/");
            EffectDir = ActorHelper.CreateGameObject("Effects", "/---运行时资源---/");
            OtherDir = ActorHelper.CreateGameObject("Others", "/---运行时资源---/");

            //--加载行为配置
            foreach (var model in CfgManager.Model)
            {
                var modelName = model.Key;
                string path = string.Format("{0}/{1}.xml", ActionConfigPath, modelName);
                if (!File.Exists(path))
                {
                    var config = new ActorConfig() { ModelName = modelName };
                    string savePath = string.Format("{0}/{1}.xml", ActionHomeConfig.Instance.ActionConfigPath, modelName);
                    var actor = new ActorConfigEditor(savePath, config);
                    actor.Save(true);
                    Debug.LogFormat("<color=orange>新建Actor - {0}</color>", modelName);
                }
            }

            string[] files = Directory.GetFiles(ActionConfigPath, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var path in files)
            {
                var actor = new ActorConfigEditor(path);
                AddActor(actor);
            }
            foreach (var item in _modelDict)
                item.Value.InitBaseModelAction();

            var window = ActorCfgWindow.GetWindow<ActorCfgWindow>();
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
                    ActorConfigEditor actor = GetActorEditor(name);
                    LoadTarget(actor);
                });
            }
            GUIHelper.PopGUIPositionOffset();
            EditorGUILayout.LabelField(_targetDisplayName, EditorStyles.textArea);
            EditorGUILayout.EndHorizontal();
        }

        private string _targetDisplayName = "";
        [HorizontalGroup("Separator", Order = 100)]
        [BoxGroup("Separator/Left", ShowLabel = false), SerializeField]
        [LabelText("显示打击框"), OnValueChanged("OnShowHitBox")]
        public bool _showHitBox = false;
        [BoxGroup("Separator/Left", ShowLabel = false), SerializeField]
        [LabelText("显示碰撞体"), OnValueChanged("OnShowCollider")]
        private bool _showCollider = false;
        [BoxGroup("Separator/Right", ShowLabel = false), SerializeField]
        [LabelText("显示攻击范围"), OnValueChanged("OnShowAttackRange")]
        private bool _showAttackRange = false;
        [BoxGroup("Separator/Right", ShowLabel = false), SerializeField]
        [LabelText("显示受击对象"), OnValueChanged("OnShowTarget")]
        private bool _showTarget = false;

        [Button("LoadAsset", ButtonSizes.Large)]
        void LoadAssetByPath()
        {
            _targetCollider = LoadCollider(_charColliderName);
            var collider = PrefabUtility.InstantiatePrefab(_targetCollider) as GameObject;
            collider.transform.parent = RuntimeAssetRoot.transform;
            Debug.LogError(collider.name);
        }

        private void OnShowHitBox()
        {
            if (Self != null)
            {
                var range = Self.GetComponentInChildren<SelfRange>();
                range.SetRangeActive(_showHitBox);
            }
            if (Target != null)
            {
                var range = Target.GetComponentInChildren<SelfRange>();
                range.SetRangeActive(_showHitBox);
            }
        }
        private void OnShowCollider()
        {
            //if (_targetCollider != null)
            //    _targetCollider.SetActive(_showCollider);
        }
        private void OnShowAttackRange()
        {
            if (_attackRangeOb != null)
                _attackRangeOb.SetActive(_showAttackRange);
        }
        private void OnShowTarget()
        {
            if (Target != null)
                Target.gameObject.SetActive(_showTarget);
        }

        #endregion


        /// <summary>
        /// 剪切板
        /// </summary>
        private List<ModelActionEditor> _clipboard = new List<ModelActionEditor>();
        /// <summary>
        /// 所有模型数据 key-模型名称 value-模型编辑器
        /// </summary>
        private Dictionary<string, ActorConfigEditor> _modelDict = new Dictionary<string, ActorConfigEditor>();
        /// <summary>
        /// 模型分组信息 key-分组类型 value-模型行为编辑器
        /// </summary>
        private Dictionary<GroupType, List<ActorConfigEditor>> _modelGroupDict = new Dictionary<GroupType, List<ActorConfigEditor>>();
        private Dictionary<string, string> AllCharacters = new Dictionary<string, string>();
        private Dictionary<string, string> AllAvatars = new Dictionary<string, string>();
        private Dictionary<string, string> AllEffects = new Dictionary<string, string>();

        public bool IsInit { get; private set; }
        public Dictionary<GroupType, List<ActorConfigEditor>> ModelGroupDict
        {
            get
            {
                if (_config == null)
                    LoadAll();
                return _modelGroupDict;
            }
        }
        public Dictionary<string, Dictionary<string, string>> AllCharacterClips = new Dictionary<string, Dictionary<string, string>>();

        public void AddActor(ActorConfigEditor model)
        {
            if (ModelGroupDict.ContainsKey(model.GroupType))
                ModelGroupDict[model.GroupType].Add(model);
            else
            {
                ModelGroupDict.Add(model.GroupType, new List<ActorConfigEditor>());
                ModelGroupDict[model.GroupType].Add(model);
            }
            if (!_modelDict.ContainsKey(model.ModelName))
                _modelDict.Add(model.ModelName, model);
        }
        public void RemoveActor(ActorConfigEditor model)
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
        public ActorConfigEditor GetActorEditor(string modelName)
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
            UnityEngine.Object.DestroyImmediate(SequenceRoot);
            UnityEngine.Object.DestroyImmediate(RuntimeAssetRoot);
            Resources.UnloadUnusedAssets();
        }



        #region 辅助功能区
        public GameObject SequenceRoot { get; private set; }
        public GameObject RuntimeAssetRoot { get; private set; }
        public GameObject CharacterDir { get; private set; }
        public GameObject EffectDir { get; private set; }
        public GameObject OtherDir { get; private set; }
        public GameObject Self { get; private set; }
        public GameObject Target { get; set; }
        public bool ShowHitBox { get { return _showHitBox; } }
        public bool ShowCollider { get { return _showCollider; } }
        public bool ShowAttackRange { get { return _showAttackRange; } }
        public bool ShowTarget { get { return _showTarget; } }
        public float AttackRange
        {
            get { return _attackRange; }
            set
            {
                _attackRange = value;
                LoadAttackRange();
            }
        }


        private string _charColliderName = "cylinder360";
        private string _attackRangeObName = "攻击区域";
        private GameObject _targetCollider;
        private FSequence _sequence;
        private GameObject _attackRangeOb;
        private float _attackRange = 1;
        private List<GameObject> _hitBoxs = new List<GameObject>();


        public void OpenSequence(ActorConfigEditor actor, ModelActionEditor action)
        {
            LoadSelf(actor);
            string targetName = _targetDisplayName;
            if (string.IsNullOrEmpty(targetName))
                targetName = actor.ModelName;
            LoadTarget(actor);
            //_sequence = FSequence.CreateSequence(SequenceRoot);


            //FluxEditor.FSequenceEditorWindow.Open(_sequence);
        }
        public void CloseSequence()
        {
            var seq = SequenceRoot.GetComponent<FSequence>();
            UnityEngine.Object.DestroyImmediate(seq);
            foreach (var item in _hitBoxs)
                UnityEngine.Object.DestroyImmediate(item);
            _hitBoxs.Clear();
        }


        //-- 加载角色行为时加载资源
        private void LoadSelf(ActorConfigEditor actor)
        {
            string modelName = CfgManager.Model[actor.ModelName].ModelPath;
            if (Self != null && Self.name.Equals(modelName))
                return;

            if (Self != null)
                UnityEngine.Object.DestroyImmediate(Self);
            Self = ActorHelper.Instantiate(actor);
            Self.transform.parent = CharacterDir.transform;
            Self.transform.position = Vec3.forward * 10;
            Self.transform.forward = -Vec3.forward;
            //if (_showCollider)
            //{
            //    var ctrl = Self.GetComponent<CharacterController>();
            //    if (ctrl != null)
            //    {
            //        var collider = LoadCollider(_charColliderName);
            //        collider.transform.localScale = Vec3.one * ctrl.radius;
            //    }
            //}
        }
        private void LoadTarget(ActorConfigEditor actor)
        {
            string modelName = CfgManager.Model[actor.ModelName].ModelPath;
            if (Target != null && Target.name.Equals(modelName))
                return;

            if (Target != null)
                UnityEngine.Object.DestroyImmediate(Target);
            Target = ActorHelper.Instantiate(actor);
            Target.transform.parent = CharacterDir.transform;
            Target.transform.position = Vec3.forward * -10;
            Target.transform.forward = Vec3.forward;
            var cap = Target.GetComponent<CapsuleCollider>();
            if (cap != null)
            {
                var collider = LoadCollider(_charColliderName, Target);
                collider.SetActive(_showCollider);
                float size = cap.radius;
                collider.transform.localScale = Vec3.one * size;
            }
            else
            {
                Debug.LogErrorFormat("{0} 角色没有渲染组件!", Target.name);
            }
        }
        private void LoadAttackRange()
        {
            var attackRange = Self.transform.Find(_attackRangeObName);
            GizmosCircle circle = null;
            if (attackRange == null)
            {
                _attackRangeOb = ActorHelper.CreateGameObject(_attackRangeObName);
                _attackRangeOb.transform.parent = Self.transform;
                circle = _attackRangeOb.AddComponent<GizmosCircle>();
            }
            else
            {
                circle = _attackRangeOb.GetComponent<GizmosCircle>();
            }
            circle.m_Radius = AttackRange;
            _attackRangeOb.SetActive(_showAttackRange);
        }

        public GameObject LoadCollider(string collider, GameObject parent = null)
        {
            string path = string.Format("{0}/{1}.fbx", _config.ColliderRelativeDir, collider);
            GameObject col = ActorHelper.Instantiate(path, parent);
            col.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
            return col;
        }
        public string GetCharacterPath(string name)
        {
            string path = string.Empty;
            AllCharacters.TryGetValue(name, out path);
            return path;
        }
        public string GetAvatarPath(string name)
        {
            string path = string.Empty;
            AllAvatars.TryGetValue(name, out path);
            return path;
        }
        public string GetEffectPath(string name)
        {
            string path = string.Empty;
            AllEffects.TryGetValue(name, out path);
            return path;
        }
        #endregion





        public void CheckAll()
        {
            foreach (var item in _modelDict)
            {
                Debug.LogErrorFormat("{0} : Is dirty>{1}", item.Key, item.Value.IsDirty);
            }
        }
        public void UpdateClipbord(List<ModelActionEditor> editors)
        {
            _clipboard = editors;
        }
        /// <summary>
        /// 创建角色动作行为配置文本
        /// 每个角色只能有一套配置
        /// </summary>
        /// <returns></returns>
        private void Create()
        {
            ActorConfigEditor model = null;
            var models = CfgManager.Model.Keys;
            SimplePopupCreator.ShowDialog(new List<string>(models), (name) =>
            {
                var config = new ActorConfig() { ModelName = name };
                string path = string.Format("{0}/{1}.xml", ActionHomeConfig.Instance.ActionConfigPath, name);
                model = new ActorConfigEditor(path, config);
                model.Save();
            });
        }
    }
}

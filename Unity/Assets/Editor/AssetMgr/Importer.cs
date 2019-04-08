namespace AssetMgr
{
    using UnityEditor;
    using UnityEngine;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities.Editor;
    using Sirenix.Utilities;
    using System.Collections.Generic;
    using System.Linq;

    public class Importer : OdinMenuEditorWindow
    {
        [MenuItem("Tools/AssetMgr/Importer Open", false, 1)]
        private static void OdinOpen()
        {
            Instance = GetWindow<Importer>("资源导入设置");
            Instance.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            TextureImpoterCfg.LoadInstanceIfAssetExists();
            Instance._import = new GUIContent(EditorIcons.Refresh.Raw);
            Instance._importAll = new GUIContent("导入当前配置");
        }
        [MenuItem("Tools/AssetMgr/Importer Close", false, 1)]
        private static void OdinClose()
        {
            Instance = GetWindow<Importer>("资源导入设置");
            Instance.Close();
        }

        public static Importer Instance;

        public void RefreshTree()
        {
            OdinMenuTree MenuTree = Instance.MenuTree;
            var items = MenuTree.MenuItems;
            for (int i = 0; i < items.Count; i++)
            {
                items[i].ChildMenuItems.Clear();
            }
            SetMenuTree(MenuTree);
            MenuTree.UpdateMenuTree();
        }

        readonly HashSet<string> settings = new HashSet<string>() { TAB_TEXTURE, TAB_MODEL, TAB_AUDIO };
        const string TAB_TEXTURE = "#贴图配置";
        const string TAB_MODEL = "#模型配置";
        const string TAB_AUDIO = "#音效配置";

        GUIContent _import;
        GUIContent _importAll;
        TextureSetting _lastObj;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (Instance == null)
                Instance = this;
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(false);
            tree.Config.DrawSearchToolbar = true;

            SetMenuTree(tree);

            EditorUtility.ClearProgressBar();
            return tree;
        }

        public void SetMenuTree(OdinMenuTree tree)
        {
            TextureImpoterCfg.Instance.Load();
            var texImporterView = new TextureImpoterView();
            tree.Add(TAB_TEXTURE, texImporterView, EditorIcons.SettingsCog);
            var dict = AMTool.GetDictionary(TextureImpoterCfg.Instance.TextureSettings);
            foreach (var item in dict)
            {
                string path = item.Key.Replace(Application.dataPath, TAB_TEXTURE);
                tree.Add(path, item.Value);
            }

            tree.Selection.SelectionChanged -= OnSelectionItemChange;
            tree.Selection.SelectionChanged += OnSelectionItemChange;
        }

        private void OnSelectionItemChange(SelectionChangedType type)
        {
            if (MenuTree == null) return;

            switch (type)
            {
                case SelectionChangedType.ItemAdded:
                    var setting = MenuTree.Selection[0].ObjectInstance as TextureSetting;
                    if (setting == null) return;
                    _lastObj = setting;
                    break;
                case SelectionChangedType.ItemRemoved:
                    break;
                case SelectionChangedType.SelectionCleared:
                    if (_lastObj == null) return;

                    _lastObj.SaveCfg2Children();
                    break;
                default:
                    break;
            }
        }

        private void ClearTree()
        {
            var items = MenuTree.MenuItems;
            for (int i = 0; i < items.Count; i++)
            {
                items[i].ChildMenuItems.Clear();
            }
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            if (selected == null) return;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (!settings.Contains(selected.SmartName))
                {
                    if (SirenixEditorGUI.ToolbarButton(_import))
                    {
                        (selected.ObjectInstance as DirectorySetting<TextureSetting>).DoHandle();
                        EditorUtility.ClearProgressBar();
                        Debug.LogFormat("[重新导入配置]文件夹{0}纹理配置导入 OK~", selected.SmartName);
                    }
                }
                if (selected.SmartName.Equals(TAB_TEXTURE))
                {
                    if (SirenixEditorGUI.ToolbarButton(_import))
                    {
                        TextureImpoterCfg.Instance.ReimportAll();
                        Debug.Log("[重新导入配置]各种纹理配置导入 OK~");
                    }
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EditorUtility.SetDirty(TextureImpoterCfg.Instance);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            EditorUtility.UnloadUnusedAssetsImmediate();

            Clipboard.Clear();
        }

    }
}

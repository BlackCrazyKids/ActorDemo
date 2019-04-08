namespace CS.ActorConfig
{
    using UnityEngine;
    using UnityEditor;
    using System.Linq;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using Sirenix.OdinInspector.Editor;
    using Cfg.Character;
    using System.Collections.Generic;

    internal class ActorCfgWindow : OdinMenuEditorWindow
    {
        [MenuItem("Window/Action Designer/Load Action")]
        private static void Open()
        {
            var window = GetWindow<ActorCfgWindow>("模型配置窗口");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.minSize = new Vector2(400, 500);
            ActionHomeConfig.LoadInstanceIfAssetExists();
            HomeConfig.Instance.LoadAll();            
        }


        Rect _windowRect = new Rect(100, 100, 200, 200);
        ActorConfigEditor _actor;
        List<OdinMenuItem> _currentItems = new List<OdinMenuItem>();

        public void RefreshTree()
        {
            if (MenuTree == null) return;
            foreach (var group in HomeConfig.Instance.ModelGroupDict)
            {
                foreach (var item in group.Value)
                {
                    MenuTree.Add(item.MenuItemName, item);
                }
            }
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false);
            tree.Config.DrawSearchToolbar = true;
            foreach (var item in ActionHomeConfig.MenuItems)
            {
                if (item.Key != GroupType.None)
                    tree.Add(item.Value, null, EditorIcons.Table);
                else
                    tree.Add("主页", HomeConfig.Instance, EditorIcons.House);
            }
            foreach (var group in HomeConfig.Instance.ModelGroupDict)
            {
                foreach (var item in group.Value)
                {
                    tree.Add(item.MenuItemName, item);
                }
            }

            tree.Selection.SelectionChanged += OnMenuItemChange;
            return tree;
        }

        protected void OnMenuItemChange(SelectionChangedType state)
        {
            foreach (var item in _currentItems)
            {
                if (item.Value is ActorConfigEditor)
                    (item.Value as ActorConfigEditor).ReselectedState();
            }
            _currentItems.Clear();
            _currentItems.AddRange(MenuTree.Selection);
        }

        protected override void OnBeginDrawEditors()
        {
            if (MenuTree == null) return;

            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    var old = GUI.color;
                    GUI.color = Color.cyan;
                    GUILayout.Label(selected.Name, SirenixGUIStyles.BoldLabel);
                    GUI.color = old;
                }
                if (SirenixEditorGUI.ToolbarButton("检查"))
                {
                    HomeConfig.Instance.CheckAll();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            HomeConfig.Instance.Destroy();

            Clipboard.Clear();
            EditorUtility.UnloadUnusedAssetsImmediate(true);
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
           
            Debug.Log("[模型配置窗口]关闭~~");
        }

    }
}
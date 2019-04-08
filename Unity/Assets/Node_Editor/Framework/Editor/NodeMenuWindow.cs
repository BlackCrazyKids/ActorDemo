using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NodeEditorFramework
{
    public abstract class NodeMenuWindow : EditorWindow
    {
        [SerializeField, HideInInspector]
        private OdinMenuTreeDrawingConfig menuTreeConfig;

        private OdinMenuTreeDrawingConfig MenuTreeConfig
        {
            get
            {
                menuTreeConfig = menuTreeConfig ?? new OdinMenuTreeDrawingConfig()
                {
                    DrawScrollView = true,
                    DrawSearchToolbar = false,
                    AutoHandleKeyboardNavigation = false
                };

                return menuTreeConfig;
            }
        }

        [SerializeField, HideInInspector]
        private float menuWidth = 180;

        [NonSerialized]
        private OdinMenuTree menuTree;

        [SerializeField, HideInInspector]
        private List<string> selectedItems = new List<string>();

        [SerializeField, HideInInspector]
        private bool resizableMenuWidth = true;

        private void ProjectWindowChanged()
        {
            
        }

        /// <summary>
        /// Called when the window is destroyed. Remember to call base.OnDestroy();
        /// </summary>
        protected virtual void OnDestroy()
        {
//            if (UnityEditorEventUtility.HasOnProjectChanged)
//            {
//                UnityEditorEventUtility.OnProjectChanged -= ProjectWindowChanged;
//                UnityEditorEventUtility.OnProjectChanged -= ProjectWindowChanged;
//            }
//            else
//            {
//#pragma warning disable 0618
//                EditorApplication.projectWindowChanged -= ProjectWindowChanged;
//                EditorApplication.projectWindowChanged -= ProjectWindowChanged;
//#pragma warning restore 0618
//            }
        }

        /// <summary>
        /// Builds the menu tree.
        /// </summary>
        protected abstract OdinMenuTree BuildMenuTree();

        /// <summary>
        /// Gets or sets the width of the menu.
        /// </summary>
        public virtual float MenuWidth
        {
            get { return this.menuWidth; }
            set { this.menuWidth = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the menu is resizable.
        /// </summary>
        public virtual bool ResizableMenuWidth
        {
            get { return this.resizableMenuWidth; }
            set { this.resizableMenuWidth = value; }
        }

        /// <summary>
        /// Gets the menu tree.
        /// </summary>
        public OdinMenuTree MenuTree
        {
            get { return this.menuTree; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw the menu search bar.
        /// </summary>
        public bool DrawMenuSearchBar
        {
            get { return this.MenuTreeConfig.DrawSearchToolbar; }
            set { this.MenuTreeConfig.DrawSearchToolbar = value; }
        }

        /// <summary>
        /// Gets or sets the custom search function.
        /// </summary>
        public Func<OdinMenuItem, bool> CustomSearchFunction
        {
            get { return this.MenuTreeConfig.SearchFunction; }
            set { this.MenuTreeConfig.SearchFunction = value; }
        }

        /// <summary>
        /// Forces the menu tree rebuild.
        /// </summary>
        protected void ForceMenuTreeRebuild()
        {
            this.menuTree = this.BuildMenuTree();

            if (this.selectedItems.Count == 0 && this.menuTree.Selection.Count == 0)
            {
                // Select first item if nothing was specified by the user in BuildMenuTree
                var firstMenu = this.menuTree.EnumerateTree().FirstOrDefault(x => x.Value != null);
                if (firstMenu != null)
                {
                    firstMenu.GetParentMenuItemsRecursive(false).ForEach(x => x.Toggled = true);
                    firstMenu.Select();
                }
            }
            else if (this.menuTree.Selection.Count == 0 && this.selectedItems.Count > 0)
            {
                // Select whatever was selected before.
                foreach (var item in this.menuTree.EnumerateTree())
                {
                    if (this.selectedItems.Contains(item.GetFullPath()))
                    {
                        item.Select(true);
                    }
                }
            }

            this.menuTree.Selection.SelectionChanged += this.OnSelectionChanged;
        }

        private void OnSelectionChanged(SelectionChangedType type)
        {
            this.Repaint();
            GUIHelper.RemoveFocusControl();
            this.selectedItems = this.menuTree.Selection.Select(x => x.GetFullPath()).ToList();
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// The method that draws the menu.
        /// </summary>
        protected virtual void DrawMenu()
        {
            if (this.menuTree == null)
            {
                return;
            }

            this.menuTree.DrawMenuTree();
        }


        /// <summary>
        /// Draws the Odin Editor Window.
        /// </summary>
        protected virtual void OnGUI()
        {
            if (Event.current.type == EventType.Layout)
            {
                bool setActive = this.menuTree == null;
                if (this.menuTree == null)
                {
                    this.ForceMenuTreeRebuild();
                    if (setActive)
                    {
                        OdinMenuTree.ActiveMenuTree = this.menuTree;
                    }

//                    if (UnityEditorEventUtility.HasOnProjectChanged)
//                    {
//                        UnityEditorEventUtility.OnProjectChanged -= ProjectWindowChanged;
//                        UnityEditorEventUtility.OnProjectChanged += ProjectWindowChanged;
//                    }
//                    else
//                    {
//#pragma warning disable 0618
//                        EditorApplication.projectWindowChanged -= ProjectWindowChanged;
//                        EditorApplication.projectWindowChanged += ProjectWindowChanged;
//#pragma warning restore 0618
//                    }
                }
            }

            Rect menuBorderRect;

            GUILayout.BeginVertical(GUILayoutOptions.Width(this.MenuWidth).ExpandHeight());
            {
                var rect = GUIHelper.GetCurrentLayoutRect();

                EditorGUI.DrawRect(rect, SirenixGUIStyles.MenuBackgroundColor);
                menuBorderRect = rect;
                menuBorderRect.xMin = rect.xMax - 4;
                menuBorderRect.xMax += 4;

                if (this.ResizableMenuWidth)
                {
                    EditorGUIUtility.AddCursorRect(menuBorderRect, MouseCursor.ResizeHorizontal);
                    this.MenuWidth += SirenixEditorGUI.SlideRect(menuBorderRect).x;
                }

                this.DrawMenu();
            }
            GUILayout.EndVertical();

            EditorGUI.DrawRect(menuBorderRect.AlignCenter(1), SirenixGUIStyles.BorderColor);

            if (this.menuTree != null)
            {
                this.menuTree.HandleKeybaordMenuNavigation();

                // TODO: Handle scroll to selected menu items...
                // this.menuTree.Selection.Last() is the latest selected item.
            }
        }
    }
}
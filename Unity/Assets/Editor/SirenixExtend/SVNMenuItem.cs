using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Sirenix.OdinInspector.Editor
{
    public class SVNMenuItem : OdinMenuItem
    {
        public GenericMenu PopMenu { get { return _popMenu; } }
        private GenericMenu _popMenu;
        public SVNMenuItem(OdinMenuTree tree, string name, IList value) : base(tree, name, value)
        {
            _popMenu = new GenericMenu();
            OnRightClick += OnRightClickEvent;
        }
        public SVNMenuItem(OdinMenuTree tree, string name, object value) : base(tree, name, value)
        {
            _popMenu = new GenericMenu();
            OnRightClick += OnRightClickEvent;
        }

        void OnRightClickEvent(OdinMenuItem item)
        {
            if (MenuTree.Selection.Count == 0) return;

            List<string> paths = new List<string>();
            for (int i = 0; i < MenuTree.Selection.Count; i++)
                paths.Add(MenuTree.Selection[i].Value as string);

            SVNTool.DrawGenericMenu(ref _popMenu, paths);
            _popMenu.DropDown(item.LabelRect);
        }
    }
}
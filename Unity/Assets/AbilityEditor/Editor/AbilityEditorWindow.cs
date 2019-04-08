using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AbilityEditor
{
    public class AbilityEditorWindow : NodeMenuWindow
    {
        [MenuItem("Node Editor/" + TITLE, false, 1)]
        private static AbilityEditorWindow OpenActorEditor()
        {
            _ins = GetWindow<AbilityEditorWindow>("资源导入设置");
            _ins.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);

            Texture iconTexture = ResourceManager.LoadTexture(EditorGUIUtility.isProSkin ? "Textures/Icon_Dark.png" : "Textures/Icon_Light.png");
            _ins.titleContent = new GUIContent(TITLE, iconTexture);
            return _ins;
        }
        [MenuItem("Node Editor/Close " + TITLE, false, 2)]
        private static void CloseActorEditor()
        {
            if (_ins != null)
                _ins.Close();
        }

        [MenuItem("Node Editor/Check Path Data")]
        private static void CheckPathData()
        {
            string path = ResourceManager.ActorEditorPath;
            string[] assets = Directory.GetFiles(path, "*.asset");
            for (int i = 0; i < assets.Length; i++)
            {
                var asset = ResourceManager.LoadResource<NodeCanvas>(assets[i]);
                string newPath = ResourceManager.PreparePath(assets[i]);
                asset.UpdateSource(newPath);
                EditorUtility.DisplayProgressBar(TITLE, asset.name, (i + 1f) / assets.Length);
            }
            EditorUtility.ClearProgressBar();
        }

        public static AbilityEditorWindow Ins { get { return _ins; } }
        private static AbilityEditorWindow _ins;

        public const string TITLE = "Ability Editor";

        // Canvas cache
        private NodeEditorUserCache _canvasCache;
        private AbilityEditorInterface _editorInterface;
        private float _menuWith;
        private Rect CanvasWindowRect { get { return new Rect(MenuWidth, _editorInterface.toolbarHeight, position.width - MenuWidth - 2, position.height - _editorInterface.toolbarHeight); } }

        #region GUI
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.Config.DrawSearchToolbar = true;
            tree.Config.SearchToolbarHeight = (int)_editorInterface.toolbarHeight + 2;

            string path = ResourceManager.ActorEditorPath;
            string[] assets = Directory.GetFiles(path, "*.asset");
            for (int i = 0; i < assets.Length; i++)
            {
                var asset = ResourceManager.LoadResource<NodeCanvas>(assets[i]);
                SVNMenuItem item = new SVNMenuItem(tree, asset.name, asset.savePath);
                tree.AddMenuItemAtPath("", item);
                EditorUtility.DisplayProgressBar(TITLE, asset.name, (i + 1f) / assets.Length);
            }
            EditorUtility.ClearProgressBar();
            tree.SortMenuItemsByName();

            tree.Selection.SelectionConfirmed += SelectionConfirmed;

            return tree;
        }
        
        void SelectionConfirmed(OdinMenuTreeSelection items)
        {
            if (items != null && items.Count > 0)
            {
                if (_editorInterface != null)
                    _editorInterface.CheckCanvasState();

                var assetPath = items[0].Value as string;
                _ins._canvasCache.LoadNodeCanvas(assetPath);
            }
        }

        protected override void OnGUI()
        {
            base.OnGUI();

            // Initiation
            NodeEditor.checkInit(true);
            if (NodeEditor.InitiationError)
            {
                GUILayout.Label("Node Editor Initiation failed! Check console for more information!");
                return;
            }
            AssureSetup();

            // ROOT: Start Overlay GUI for popups
            OverlayGUI.StartOverlayGUI("ActorEditorWindow");

            // Begin Node Editor GUI and set canvas rect
            NodeEditorGUI.StartNodeGUI(true);
            _canvasCache.editorState.canvasRect = CanvasWindowRect;

            try
            {
                // Perform drawing with error-handling
                NodeEditor.DrawCanvas(_canvasCache.nodeCanvas, _canvasCache.editorState);
            }
            catch (UnityException e)
            { // On exceptions in drawing flush the canvas to avoid locking the UI
                _canvasCache.NewNodeCanvas();
                NodeEditor.ReInit(true);
                Debug.LogError("Unloaded Canvas due to an exception during the drawing phase!");
                Debug.LogException(e);
            }

            // Draw Interface
            _editorInterface.DrawToolbarGUI(new Rect(MenuWidth - 5, 0, Screen.width - MenuWidth + 5, 30));
            _editorInterface.DrawModalPanel();

            // End Node Editor GUI
            NodeEditorGUI.EndNodeGUI();

            // END ROOT: End Overlay GUI and draw popups
            OverlayGUI.EndOverlayGUI();
        }
        #endregion

        #region General 
        private void OnEnable()
        {
            _ins = this;
            NodeEditor.ReInit(false);
            AssureSetup();

            // Subscribe to events
            NodeEditor.ClientRepaints -= Repaint;
            NodeEditor.ClientRepaints += Repaint;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            NodeEditor.ClientRepaints -= Repaint;
            _canvasCache.ClearCacheEvents();
        }

        private void AssureSetup()
        {
            if (_canvasCache == null)
            { // Create cache
                _canvasCache = new NodeEditorUserCache();
            }
            _canvasCache.AssureCanvas();
            if (_editorInterface == null)
            { // Setup editor interface
                _editorInterface = new AbilityEditorInterface();
                _editorInterface.canvasCache = _canvasCache;
                _editorInterface.ShowNotificationAction = ShowNotification;
            }
        }
        #endregion
    }
}
using System;
using UnityEngine;
using UnityEditor;
using NodeEditorFramework.IO;
using GenericMenu = NodeEditorFramework.Utilities.GenericMenu;
using System.IO;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace AbilityEditor
{
    public class AbilityEditorInterface
    {
        public NodeEditorUserCache canvasCache;
        public Action<GUIContent> ShowNotificationAction;

        // GUI
        public string sceneCanvasName = "";
        public float toolbarHeight = 17;

        // Modal Panel
        public bool showModalPanel;
        public Rect modalPanelRect = new Rect(20, 50, 250, 70);
        public Action modalPanelContent;

        // IO Format modal panel
        private ImportExportFormat IOFormat;
        private object[] IOLocationArgs;
        private delegate bool? DefExportLocationGUI(string canvasName, ref object[] locationArgs);
        private delegate bool? DefImportLocationGUI(ref object[] locationArgs);
        private DefImportLocationGUI ImportLocationGUI;
        private DefExportLocationGUI ExportLocationGUI;

        public void ShowNotification(GUIContent message)
        {
            if (ShowNotificationAction != null)
                ShowNotificationAction(message);
        }


        public void CheckCanvasState()
        {
            NodeCanvas canvas = canvasCache.nodeCanvas;
            if (canvas == null) return;

            bool isDirty = canvas.isDirty;
            //while (!isDirty)
            //{
            //    isDirty |= CheckState(canvas);
            //}
            if (isDirty)
            {
                if (EditorUtility.DisplayDialog("提示", canvas.name + "已经被修改,是否保存?", "是", "否"))
                {
                    string path = canvasCache.nodeCanvas.savePath;
                    if (!string.IsNullOrEmpty(path))
                        SaveCanvas();
                    else
                        SaveCanvasAs();
                }
            }
        }
        private bool CheckState(NodeCanvas canvas)
        {
            if (canvas == null) return false;

            bool isDirty = false;
            for (int i = 0; i < canvas.editorStates.Length; i++)
            {
                var state = canvas.editorStates[i];
                isDirty |= state.canvas != null && state.canvas.isDirty;
                if (isDirty) break;
            }

            return isDirty;
        }
        #region GUI

        public void DrawToolbarGUI(Rect rect)
        {
            rect.height = toolbarHeight;
            GUILayout.BeginArea(rect, NodeEditorGUI.toolbar);
            GUILayout.BeginHorizontal();
            float curToolbarHeight = 0;


            if (GUILayout.Button("File", NodeEditorGUI.toolbarDropdown, GUILayout.Width(50)))
            {
                GenericMenu menu = new GenericMenu(!Application.isPlaying);

                // New Canvas filled with canvas types
                NodeCanvasManager.FillCanvasTypeMenu(ref menu, NewNodeCanvas, "新建画布/");
                // Load / Save
#if UNITY_EDITOR
                menu.AddItem(new GUIContent("重置画布"), false, ReloadCanvas);
                menu.AddSeparator("");
                if (canvasCache.nodeCanvas.allowSceneSaveOnly)
                {
                    menu.AddDisabledItem(new GUIContent("Save Canvas"));
                    menu.AddDisabledItem(new GUIContent("Save Canvas As"));
                }
                else
                {
                    menu.AddItem(new GUIContent("保存画布"), false, SaveCanvas);
                    menu.AddItem(new GUIContent("另存画布"), false, SaveCanvasAs);
                }
                menu.AddSeparator("");
#endif
                menu.AddItem(new GUIContent("导入数据[未完成!]"), false, Import);
                menu.AddItem(new GUIContent("导出数据[未完成!]"), false, Export);

                // Show dropdown
                menu.Show(new Vector2(5, toolbarHeight));
            }
            curToolbarHeight = Mathf.Max(curToolbarHeight, GUILayoutUtility.GetLastRect().yMax);

            GUILayout.Space(10);
            string fileName = Path.GetFileNameWithoutExtension(canvasCache.nodeCanvas.savePath);
            GUILayout.Button(new GUIContent(fileName, canvasCache.nodeCanvas.savePath), NodeEditorGUI.toolbarArrow);
            curToolbarHeight = Mathf.Max(curToolbarHeight, GUILayoutUtility.GetLastRect().yMax);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            if (Event.current.type == EventType.Repaint)
                toolbarHeight = curToolbarHeight;
        }

        private void SaveSceneCanvasPanel()
        {
            GUILayout.Label("Save Canvas To Scene");

            GUILayout.BeginHorizontal();
            sceneCanvasName = GUILayout.TextField(sceneCanvasName, GUILayout.ExpandWidth(true));
            bool overwrite = NodeEditorSaveManager.HasSceneSave(sceneCanvasName);
            if (overwrite)
                GUILayout.Label(new GUIContent("!!!", "A canvas with the specified name already exists. It will be overwritten!"), GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
                showModalPanel = false;
            if (GUILayout.Button(new GUIContent(overwrite ? "Overwrite" : "Save", "Save the canvas to the Scene")))
            {
                showModalPanel = false;
                if (!string.IsNullOrEmpty(sceneCanvasName))
                    canvasCache.SaveSceneNodeCanvas(sceneCanvasName);
            }
            GUILayout.EndHorizontal();
        }

        public void DrawModalPanel()
        {
            if (showModalPanel)
            {
                if (modalPanelContent == null)
                    return;
                GUILayout.BeginArea(modalPanelRect, NodeEditorGUI.nodeBox);
                modalPanelContent.Invoke();
                GUILayout.EndArea();
            }
        }

        #endregion

        #region Menu Callbacks

        private void NewNodeCanvas(Type canvasType)
        {
            canvasCache.NewNodeCanvas(canvasType);
        }

#if UNITY_EDITOR
        private void ReloadCanvas()
        {
            string path = canvasCache.nodeCanvas.savePath;
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("SCENE/"))
                    canvasCache.LoadSceneNodeCanvas(path.Substring(6));
                else
                    canvasCache.LoadNodeCanvas(path);
                ShowNotification(new GUIContent("Canvas Reloaded!"));
            }
            else
                ShowNotification(new GUIContent("Cannot reload canvas as it has not been saved yet!"));
        }

        private void SaveCanvas()
        {
            string path = canvasCache.nodeCanvas.savePath;
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("SCENE/"))
                    canvasCache.SaveSceneNodeCanvas(path.Substring(6));
                else
                    canvasCache.SaveNodeCanvas(path);
                ShowNotification(new GUIContent("Canvas Saved!"));
            }
            else
                ShowNotification(new GUIContent("No save location found. Use 'Save As'!"));
        }

        private void SaveCanvasAs()
        {
            string panelPath = ResourceManager.ActorEditorPath;
            if (canvasCache.nodeCanvas != null && !string.IsNullOrEmpty(canvasCache.nodeCanvas.savePath))
                panelPath = canvasCache.nodeCanvas.savePath;
            string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Node Canvas", "Node Canvas", "asset", "", panelPath);
            if (!string.IsNullOrEmpty(path))
                canvasCache.SaveNodeCanvas(path);
        }
#endif
        private void Import()
        {

        }
        private void Export()
        {

        }
        #endregion
    }
}

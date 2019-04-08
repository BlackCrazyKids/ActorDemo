using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using NodeEditorFramework.IO;

#if UNITY_EDITOR
using MenuFunctionData = UnityEditor.GenericMenu.MenuFunction2;
#else
using MenuFunction = NodeEditorFramework.Utilities.OverlayGUI.CustomMenuFunction;
using MenuFunctionData = NodeEditorFramework.Utilities.OverlayGUI.CustomMenuFunctionData;
#endif

namespace NodeEditorFramework
{
    [InitializeOnLoad]
    public class NodeSystemStartup
    {
        static NodeSystemStartup()
        {
            NodeSystemSetting.InitSystem();
        }
    }

    public class NodeSystemSetting : ScriptableObject
    {
        [Title("Base Confing")]
        [SerializeField, ShowInInspector]
        private Dictionary<string, ConnectionPortStyle> portStyles = new Dictionary<string, ConnectionPortStyle>();
        [SerializeField, ShowInInspector]
        private Dictionary<string, ValueConnectionType> valueTypes = new Dictionary<string, ValueConnectionType>();
        [SerializeField, ShowInInspector]
        private Dictionary<Type, NodeCanvasTypeData> canvasDatas = new Dictionary<Type, NodeCanvasTypeData>();
        [SerializeField, ShowInInspector]
        private Dictionary<string, NodeTypeData> nodeDatas = new Dictionary<string, NodeTypeData>();
        [SerializeField, ShowInInspector]
        private Dictionary<string, ConnectionPortDeclaration[]> nodePortDec = new Dictionary<string, ConnectionPortDeclaration[]>();
        [SerializeField, ShowInInspector]
        private Dictionary<string, ImportExportFormat> ioFormats = new Dictionary<string, ImportExportFormat>();

        [SerializeField]
        private List<KeyValuePair<EventHandlerAttribute, Delegate>> eventHandlers = new List<KeyValuePair<EventHandlerAttribute, Delegate>>();
        [SerializeField]
        private List<KeyValuePair<HotkeyAttribute, Delegate>> hotkeyHandlers = new List<KeyValuePair<HotkeyAttribute, Delegate>>();
        [SerializeField]
        private List<KeyValuePair<ContextEntryAttribute, MenuFunctionData>> contextEntries = new List<KeyValuePair<ContextEntryAttribute, MenuFunctionData>>();
        [SerializeField]
        private List<KeyValuePair<ContextFillerAttribute, Delegate>> contextFillers = new List<KeyValuePair<ContextFillerAttribute, Delegate>>();


        public Dictionary<string, ConnectionPortStyle> PortStyles { get { return portStyles; } }
        public Dictionary<string, ValueConnectionType> ValueTypes { get { return ValueTypes; } }
        public Dictionary<Type, NodeCanvasTypeData> CanvasDatas { get { return canvasDatas; } }
        public Dictionary<string, NodeTypeData> NodeDatas { get { return nodeDatas; } }
        public Dictionary<string, ConnectionPortDeclaration[]> NodePortDec { get { return nodePortDec; } }
        public Dictionary<string, ImportExportFormat> IOFormats { get { return ioFormats; } }

        public List<KeyValuePair<EventHandlerAttribute, Delegate>> EventHandlers { get { return eventHandlers; } }
        public List<KeyValuePair<HotkeyAttribute, Delegate>> HotkeyHandlers { get { return hotkeyHandlers; } }
        public List<KeyValuePair<ContextEntryAttribute, MenuFunctionData>> ContextEntries { get { return contextEntries; } }
        public List<KeyValuePair<ContextFillerAttribute, Delegate>> ContextFillers { get { return contextFillers; } }

        public static NodeSystemSetting Inst { get { return _inst; } }

        static NodeSystemSetting _inst;
        const string ASSET_PATH = "Assets/Node_Editor/Framework/Core/NodeSystemSetting.asset";


        bool showEventHandlers = false;
        bool showHotkeyHandlers = false;
        bool showContextEntries = false;
        bool showContextFillers = false;

        [MenuItem("Node Editor/Update System Setting", false, 1000)]
        public static void InitSystem()
        {
            _inst = AssetDatabase.LoadAssetAtPath<NodeSystemSetting>(ASSET_PATH);
            bool hasExist = _inst != null;
            if (!hasExist)
            {
                _inst = CreateInstance<NodeSystemSetting>();
                AssetDatabase.CreateAsset(_inst, ASSET_PATH);
            }

            ConnectionPortStyles.InitSystem(out _inst.portStyles, out _inst.valueTypes);
            NodeTypes.InitSystem(out _inst.nodeDatas);
            NodeCanvasManager.InitSystem(out _inst.canvasDatas);
            ConnectionPortManager.InitSystem(out _inst.nodePortDec);
            ConnectionPortManager.InitSystem(out _inst.nodePortDec);
            ImportExportManager.InitSystem(out _inst.ioFormats);

            NodeEditorInputSystem.InitSystem(out _inst.eventHandlers, out _inst.hotkeyHandlers, out _inst.contextEntries, out _inst.contextFillers);
        }


        [OnInspectorGUI, Title("Handles")]
        void OnDrawGUI()
        {
            DrawListKeyValuePair(eventHandlers, ref showEventHandlers, "EventHandlers", DrawEvent);
            DrawListKeyValuePair(hotkeyHandlers, ref showHotkeyHandlers, "HotkeyHandlers", DrawHotkey);
            DrawListKeyValuePair(contextEntries, ref showContextEntries, "ContextEntries", DrawEntries);
            DrawListKeyValuePair(contextFillers, ref showContextFillers, "ContextFillers", DrawFillers);
        }
        void DrawListKeyValuePair<K, V>(List<KeyValuePair<K, V>> pairs, ref bool state, string label, Action<K> DrawItem)
        {
            SirenixEditorGUI.BeginIndentedVertical(SirenixGUIStyles.PropertyPadding);
            {
                SirenixEditorGUI.BeginHorizontalToolbar();
                {
                    state = SirenixEditorGUI.Foldout(state, label);
                    GUILayout.FlexibleSpace();
                }
                SirenixEditorGUI.EndHorizontalToolbar();

                if (state)
                {
                    SirenixEditorGUI.BeginVerticalList(false);
                    foreach (var item in pairs)
                    {
                        SirenixEditorGUI.BeginListItem(false);
                        EditorGUILayout.BeginHorizontal();
                        DrawItem(item.Key);
                        EditorGUILayout.EndHorizontal();
                        SirenixEditorGUI.EndListItem();
                    }
                    SirenixEditorGUI.EndVerticalList();
                }
            }
            SirenixEditorGUI.EndIndentedVertical();
        }
        void DrawEvent(EventHandlerAttribute evt)
        {
            EditorGUILayout.EnumPopup(evt.handledEvent ?? EventType.Ignore);
            EditorGUILayout.IntField(evt.priority);
        }
        void DrawHotkey(HotkeyAttribute hotkey)
        {
            EditorGUILayout.EnumPopup(hotkey.handledHotKey);
            EditorGUILayout.EnumPopup(hotkey.modifiers ?? EventModifiers.None);
            EditorGUILayout.EnumPopup(hotkey.limitingEventType ?? EventType.Ignore);
            EditorGUILayout.IntField(hotkey.priority);
        }
        void DrawEntries(ContextEntryAttribute entry)
        {
            EditorGUILayout.EnumPopup(entry.contextType);
            EditorGUILayout.LabelField(entry.contextPath);
        }
        void DrawFillers(ContextFillerAttribute filler)
        {
            EditorGUILayout.EnumPopup(filler.contextType);
        }
    }
}


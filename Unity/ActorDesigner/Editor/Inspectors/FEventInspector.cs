using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections.Generic;
using System.Reflection;

using Flux;

namespace FluxEditor
{
    [CustomEditor(typeof(Flux.FEvent), true)]
    [CanEditMultipleObjects]
    public class FEventInspector : Editor
    {
        private const string FRAMERANGE_START_FIELD_ID = "FrameRange.Start";

        private FEvent _evt;

        private bool _allEventsSameType = true;
        private bool _isSingleFrame = false;

        //	protected SerializedProperty _script;
        protected SerializedProperty _triggerOnSkip;
        protected SerializedProperty _singleFrame;
        protected GUIContent _singleFrameUI = new GUIContent("单帧:");
        private List<FieldDraw> _fields = new List<FieldDraw>();

        class FieldDraw
        {
            public GUIContent DisplayName;
            public SerializedProperty Property;

            public bool AddFindBtn;
        }

        protected virtual void OnEnable()
        {
            if (target == null)
            {
                DestroyImmediate(this);
                return;

            }
            _evt = (Flux.FEvent)target;

            Type evtType = _evt.GetType();

            for (int i = 0; i != targets.Length; ++i)
            {
                FEvent evt = (FEvent)targets[i];
                if (evtType != evt.GetType())
                {
                    _allEventsSameType = false;
                    break;
                }
            }

            //		_script = serializedObject.FindProperty("m_Script");
            _singleFrame = serializedObject.FindProperty("_singleFrame");
            if (_allEventsSameType)
            {
                _triggerOnSkip = serializedObject.FindProperty("_triggerOnSkip");
            }

            _fields.Clear();
            var fields = evtType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var f in fields)
            {
                //if (!f.DeclaringType.Equals(evtType)) continue;
                if (!typeof(FEvent).IsAssignableFrom(evtType)) continue;

                var attributes0 = f.GetCustomAttributes(typeof(FEventField), false);
                if (attributes0 != null && attributes0.Length > 0)
                {
                    string alias = (attributes0[0] as FEventField).Name;
                    string tip = (attributes0[0] as FEventField).Tip;
                    SerializedProperty property = serializedObject.FindProperty(f.Name);
                    bool needAddBtn = false;
                    var attributes1 = f.GetCustomAttributes(typeof(FFindPathBtn), false);
                    if (attributes1 != null && attributes1.Length > 0)
                        needAddBtn = true;
                    _fields.Add(new FieldDraw() { DisplayName = new GUIContent(alias, tip), Property = property, AddFindBtn = needAddBtn });
                }
            }

            object[] customAttributes = evtType.GetCustomAttributes(typeof(FEventAttribute), false);
            if (customAttributes.Length > 0)
                _isSingleFrame = ((FEventAttribute)customAttributes[0]).isSingleFrame;
        }


        public override void OnInspectorGUI()
        {
            //		EditorGUILayout.PropertyField( _script );

            if (_allEventsSameType)
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(_triggerOnSkip, new GUIContent("跳过触发器"));
            }
            else
            {
                bool triggerOnSkipMatch = true;

                for (int i = 0; i != targets.Length; ++i)
                {
                    if (((FEvent)targets[i]).TriggerOnSkip != _evt.TriggerOnSkip)
                    {
                        triggerOnSkipMatch = false;
                        break;
                    }
                }

                EditorGUI.BeginChangeCheck();
                bool triggerOnSkip = EditorGUILayout.Toggle("跳过触发器", _evt.TriggerOnSkip, triggerOnSkipMatch ? EditorStyles.toggle : "ToggleMixed");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObjects(targets, " Inspector");
                    for (int i = 0; i != targets.Length; ++i)
                    {
                        FEvent evt = (FEvent)targets[i];
                        evt.TriggerOnSkip = triggerOnSkip;
                    }
                }
            }

            //        EditorGUILayout.IntField( "Instance ID", _evt.GetInstanceID() );

            FrameRange validRange = _evt.Track != null ? _evt.Track.GetValidRange(_evt) : new FrameRange(_evt.Start, _evt.End);
            if (!_isSingleFrame)
            {
                float startFrame = _evt.Start;
                float endFrame = _evt.End;
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("范围");
                GUILayout.Label("开始:", EditorStyles.label);
                GUI.SetNextControlName(FRAMERANGE_START_FIELD_ID);
                startFrame = EditorGUILayout.IntField(_evt.Start);
                GUILayout.Label("结束:", EditorStyles.label);
                endFrame = EditorGUILayout.IntField(_evt.End);
                EditorGUILayout.EndHorizontal();


                if (EditorGUI.EndChangeCheck())
                {
                    bool changedStart = GUI.GetNameOfFocusedControl() == FRAMERANGE_START_FIELD_ID;

                    for (int i = 0; i != targets.Length; ++i)
                    {
                        FEvent evt = (FEvent)targets[i];

                        FrameRange newFrameRange = evt.FrameRange;
                        if (changedStart)
                        {
                            if (startFrame <= newFrameRange.End)
                                newFrameRange.Start = (int)startFrame;
                        }
                        else if (endFrame >= newFrameRange.Start)
                            newFrameRange.End = (int)endFrame;

                        if (newFrameRange.Length >= evt.GetMinLength() && newFrameRange.Length <= evt.GetMaxLength())
                        {
                            FSequenceEditorWindow.instance.GetSequenceEditor().MoveEvent(evt, newFrameRange);
                            FEventEditor.FinishMovingEventEditors();
                            //						FUtility.Resize( evt, newFrameRange );
                        }
                    }
                }

                if (targets.Length == 1)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(EditorGUIUtility.labelWidth);
                    float sliderStartFrame = startFrame;
                    float sliderEndFrame = endFrame;
                    EditorGUILayout.MinMaxSlider(ref sliderStartFrame, ref sliderEndFrame, validRange.Start, validRange.End);
                    EditorGUILayout.EndHorizontal();
                    if (EditorGUI.EndChangeCheck())
                    {
                        FrameRange newFrameRange = new FrameRange((int)sliderStartFrame, (int)sliderEndFrame);
                        if (newFrameRange.Length < _evt.GetMinLength())
                        {
                            if (sliderStartFrame != startFrame) // changed start
                                newFrameRange.Start = newFrameRange.End - _evt.GetMinLength();
                            else
                                newFrameRange.Length = _evt.GetMinLength();
                        }

                        //					FUtility.Resize( _evt, newFrameRange );
                        FSequenceEditorWindow.instance.GetSequenceEditor().MoveEvent(_evt, newFrameRange);
                        FEventEditor.FinishMovingEventEditors();
                    }
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.IntSlider(_singleFrame, validRange.Start + 1, validRange.End, _singleFrameUI);
                if (EditorGUI.EndChangeCheck())
                {
                    _evt.SingleFrame = _singleFrame.intValue;
                    FrameRange newFrameRange = _evt.FrameRange;
                    newFrameRange.Start = _evt.SingleFrame - 1;
                    newFrameRange.End = _evt.SingleFrame;
                    FSequenceEditorWindow.instance.GetSequenceEditor().MoveEvent(_evt, newFrameRange);
                    FEventEditor.FinishMovingEventEditors();
                }
            }


            if (_allEventsSameType)
            {
                serializedObject.ApplyModifiedProperties();
                base.OnInspectorGUI();

                serializedObject.Update();
                foreach (var field in _fields)
                {
                    if (field.AddFindBtn)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(field.Property, field.DisplayName);
                        if (GUILayout.Button("F", GUILayout.Width(30)))
                        {
                            UnityEngine.Object obj = Selection.activeObject;
                            string path = AssetDatabase.GetAssetPath(obj);
                            if (obj != null && string.IsNullOrEmpty(path))
                            {
                                path = GetSceneNodePath((obj as GameObject).transform);
                                field.Property.stringValue = path;
                            }
                            else if (!string.IsNullOrEmpty(path))
                                field.Property.stringValue = path;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                        EditorGUILayout.PropertyField(field.Property, field.DisplayName);
                }
                serializedObject.ApplyModifiedProperties();
            }
        }

        private string GetSceneNodePath(Transform node)
        {
            string path = node.name;
            while (node.parent != null)
            {
                path = string.Format("{0}/{1}", node.parent.name, path);
                node = node.parent;
            }
            return "/" + path;
        }


        public static void OnInspectorGUIGeneric(List<FEvent> evts)
        {
            if (evts.Count == 0)
                return;

            bool triggerOnSkipMatch = true;

            int startFrame = evts[0].Start;
            int endFrame = evts[0].End;

            bool startFrameMatch = true;
            bool endFrameMatch = true;

            for (int i = 1; i < evts.Count; ++i)
            {
                if (evts[i].TriggerOnSkip != evts[0].TriggerOnSkip)
                {
                    triggerOnSkipMatch = false;
                }
                if (evts[i].Start != startFrame)
                {
                    startFrameMatch = false;
                }
                if (evts[i].End != endFrame)
                {
                    endFrameMatch = false;
                }
            }

            EditorGUI.BeginChangeCheck();
            bool triggerOnSkip = EditorGUILayout.Toggle("跳过触发器", evts[0].TriggerOnSkip, triggerOnSkipMatch ? EditorStyles.toggle : "ToggleMixed");
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(evts.ToArray(), "Inspector");
                for (int i = 0; i < evts.Count; ++i)
                {
                    evts[i].TriggerOnSkip = triggerOnSkip;
                    EditorUtility.SetDirty(evts[i]);
                }
            }

            //			FrameRange validRange = _evt.GetTrack().GetValidRange( _evt );

            EditorGUI.BeginChangeCheck();

            //			foreach( GUIStyle style in GUI.skin.customStyles )
            //			{
            ////				if( style.name.ToLower().Contains( "" ) )
            //					Debug.Log( style.name );
            //			}

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("范围");
            GUILayout.Label("开始:", EditorStyles.label);
            GUI.SetNextControlName(FRAMERANGE_START_FIELD_ID);
            startFrame = EditorGUILayout.IntField(startFrame, startFrameMatch ? EditorStyles.numberField : "PR TextField");
            GUILayout.Label("结束:", EditorStyles.label);
            endFrame = EditorGUILayout.IntField(endFrame, endFrameMatch ? EditorStyles.numberField : "PR TextField");
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                //				bool changedStart = GUI.GetNameOfFocusedControl() == FRAMERANGE_START_FIELD_ID;

                //				for( int i = 0; i != targets.Length; ++i )
                //				{
                //					FEvent evt = (FEvent)targets[i];
                //					
                //					FrameRange newFrameRange = evt.FrameRange;
                //					if( changedStart )
                //					{
                //						if( startFrame <= newFrameRange.End )
                //							newFrameRange.Start = (int)startFrame;
                //					}
                //					else if( endFrame >= newFrameRange.Start )
                //						newFrameRange.End = (int)endFrame;
                //					
                //					if( newFrameRange.Length >= evt.GetMinLength() && newFrameRange.Length <= evt.GetMaxLength() )
                //					{
                //						FSequenceEditorWindow.instance.GetSequenceEditor().MoveEvent( evt, newFrameRange );
                //						//						FUtility.Resize( evt, newFrameRange );
                //					}
                //				}
            }
        }
    }
}

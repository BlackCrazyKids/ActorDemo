using Flux;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FluxEditor
{
    [CustomEditor(typeof(Flux.FHitCheck), true)]
    public class FHitCheckInspector : FEventInspector
    {
        private SerializedProperty _target;
        private SerializedProperty _isDynamic;
        private SerializedProperty _zone;
        private SerializedProperty _sequence;


        private GUIContent _targetUI = new GUIContent("目标对象");
        private GUIContent _isDynamicUI = new GUIContent("是否动态检测");
        private GUIContent _zoneUI = new GUIContent("碰撞区域配置");
        private GUIContent _sequenceUI = new GUIContent("动态序列");

        private List<string> _paths;
        private GUIContent[] _pathNames;
        private int _selectedNodeIndex;
        private Dictionary<string, Transform> _targetDict = new Dictionary<string, Transform>();

        private List<FHitZone> _zones;
        private GUIContent[] _zoneNames;
        private int _selectedZoneIndex;

        private List<FSequence> _sequences;
        private GUIContent[] _sequenceNames;
        private int _selectedSequenceIndex;


        protected override void OnEnable()
        {
            base.OnEnable();

            _isDynamic = serializedObject.FindProperty("_isDynamic");

            _target = serializedObject.FindProperty("_target");
            Transform[] objs = FUtility.FindObjects<Transform>();
            _paths = new List<string>();
            _targetDict.Clear();
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].parent == null)
                    _paths.AddRange(GetTransformPath(objs[i]));
            }
            string path = _paths[_selectedNodeIndex];
            if (_selectedNodeIndex < _paths.Count && _targetDict.ContainsKey(path))
                _target.objectReferenceValue = _targetDict[path];
            _pathNames = new GUIContent[_paths.Count];
            for (int i = 0; i < _paths.Count; i++)
                _pathNames[i] = new GUIContent(_paths[i]);

            _zone = serializedObject.FindProperty("_zone");
            _zones = new List<FHitZone>(FUtility.FindObjects<FHitZone>());
            _zones.Sort(delegate (FHitZone a, FHitZone b) { return a.name.CompareTo(b.name); });
            if (_zone.objectReferenceValue == null && _zones.Count > 0)
            {
                _selectedZoneIndex = 0;
                _zone.objectReferenceValue = _zones[0];
            }
            else
                _selectedZoneIndex = _zones.FindIndex((select) => select.name == _zone.objectReferenceValue.name);
            _zoneNames = new GUIContent[_zones.Count];
            for (int i = 0; i != _zones.Count; ++i)
                _zoneNames[i] = new GUIContent(_zones[i].Text);

            var staticHit = (FHitCheck)target;
            _sequence = serializedObject.FindProperty("_sequence");
            _sequences = new List<FSequence>(FUtility.FindObjects<FSequence>());
            _sequences.Remove(staticHit.Sequence);
            _sequences.Sort(delegate (FSequence x, FSequence y) { return x.name.CompareTo(y.name); });
            if (_sequence.objectReferenceValue == null && _sequences.Count > 0)
            {
                _selectedSequenceIndex = 0;
                _sequence.objectReferenceValue = _sequences[0];
            }
            else
                _selectedSequenceIndex = _sequences.FindIndex((select) => select.name == _sequence.objectReferenceValue.name);
            _sequenceNames = new GUIContent[_sequences.Count];
            for (int i = 0; i != _sequences.Count; ++i)
                _sequenceNames[i] = new GUIContent(_sequences[i].name);

            serializedObject.ApplyModifiedProperties();
        }

        private List<string> GetTransformPath(Transform tran)
        {
            List<string> ls = new List<string>();
            if (tran.childCount == 0)
            {
                Transform node = tran;
                string path = tran.name;
                while (tran.parent != null)
                {
                    path = string.Format("{0}/{1}", tran.parent.name, path);
                    tran = tran.parent;
                }
                ls.Add(path);
                if (!_targetDict.ContainsKey(path))
                    _targetDict.Add(path, node);
            }
            else for (int i = 0; i < tran.childCount; i++)
                    ls.AddRange(GetTransformPath(tran.GetChild(i)));
            return ls;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            int index00 = EditorGUILayout.Popup(_targetUI, _selectedNodeIndex, _pathNames);
            if (EditorGUI.EndChangeCheck())
            {
                _selectedNodeIndex = index00;
                string path = _paths[_selectedNodeIndex];
                if (_targetDict.ContainsKey(path))
                    _target.objectReferenceValue = _targetDict[path];
                else
                    Debug.LogError("场景中不存在目标对象!" + path);
            }

            EditorGUILayout.PropertyField(_isDynamic, _isDynamicUI);

            EditorGUI.BeginChangeCheck();
            int index0 = EditorGUILayout.Popup(_zoneUI, _selectedZoneIndex, _zoneNames);
            if (EditorGUI.EndChangeCheck())
            {
                _selectedZoneIndex = index0;
                _zone.objectReferenceValue = _zones[index0];
            }

            EditorGUI.BeginChangeCheck();
            int index1 = EditorGUILayout.Popup(_sequenceUI, _selectedSequenceIndex, _sequenceNames);
            if (EditorGUI.EndChangeCheck())
            {
                _selectedSequenceIndex = index1;
                _sequence.objectReferenceValue = _sequences[index1];
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

using UnityEngine;
using UnityEditor;
using Flux;
using System.Collections.Generic;

namespace FluxEditor
{
    [CustomEditor(typeof(Flux.FSequenceTrack), true)]
    public class FSequenceTrackInspector : FTrackInspector
    {
        private SerializedProperty _ownerSequence = null;
        private GUIContent _ownerSequenceUI = new GUIContent("序列");

        private List<FSequence> _sequences;
        private GUIContent[] _sequenceNames;
        private int _selectedSequenceIndex;

        public override void OnEnable()
        {
            base.OnEnable();

            if (target == null)
            {
                DestroyImmediate(this);
                return;
            }
            var sequenceTrack = (FSequenceTrack)target;
            _ownerSequence = serializedObject.FindProperty("_ownerSequence");

            _sequences = new List<FSequence>(FUtility.FindObjects<FSequence>());
            _sequences.Remove(sequenceTrack.Sequence);
            _sequences.Sort(delegate (FSequence x, FSequence y) { return x.name.CompareTo(y.name); });
            _selectedSequenceIndex = _sequences.FindIndex((select) => select.name == _ownerSequence.objectReferenceValue.name);

            _sequenceNames = new GUIContent[_sequences.Count];
            for (int i = 0; i != _sequences.Count; ++i)
            {
                _sequenceNames[i] = new GUIContent(_sequences[i].name);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            int index = EditorGUILayout.Popup(_ownerSequenceUI, _selectedSequenceIndex, _sequenceNames);
            if (EditorGUI.EndChangeCheck())
            {
                _selectedSequenceIndex = index;
                _ownerSequence.objectReferenceValue = _sequences[index];
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

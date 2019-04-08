using UnityEngine;
using UnityEditor;

using Flux;

namespace FluxEditor
{
    [CustomEditor(typeof(FVolumeAudioEvent))]
    public class FVolumeAudioEventInspector : FTweenEventInspector
    {
        private FVolumeAudioEvent _audioEvt;
        private SerializedProperty _source;
        private GUIContent _sourceUI = new GUIContent("音频源");
        private AudioSource[] _sources;
        private GUIContent[] _sourceNames;
        private int _selectedIndex;

        protected override void OnEnable()
        {
            base.OnEnable();

            _audioEvt = (FVolumeAudioEvent)target;
            _source = serializedObject.FindProperty("_source");

            _sources = FUtility.FindObjects<AudioSource>();
            _sourceNames = new GUIContent[_sources.Length];
            for (int i = 0; i < _sources.Length; i++)
                _sourceNames[i] = new GUIContent(_sources[i].name);
            if (_sources.Length > 0)
                _source.objectReferenceValue = _sources[_selectedIndex];

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            int index = EditorGUILayout.Popup(_sourceUI, _selectedIndex, _sourceNames);
            if (EditorGUI.EndChangeCheck())
            {
                _selectedIndex = index;
                _source.objectReferenceValue = _sources[index];
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

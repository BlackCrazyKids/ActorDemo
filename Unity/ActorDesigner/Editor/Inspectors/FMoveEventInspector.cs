using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

//using UnityEditorInternal;

using System.Collections.Generic;

using Flux;
using FluxEditor;

namespace FluxEditor
{
    [CustomEditor(typeof(FMove))]
    public class FMoveEventInspector : FEventInspector
    {
        private SerializedProperty _moveType;
        private SerializedProperty _relateType;

        private int _selectedMoveIndex;
        private int _selectedRelateIndex;
        private GUIContent _moveTypeUI = new GUIContent("位移类型");
        //private GUIContent _relateTypeUI = new GUIContent("相对类型");
        private GUIContent[] _moveTypes = new GUIContent[]
        {
            new GUIContent("向目标移动"),
            new GUIContent("按当前方向移动"),
        };
        //private GUIContent[] _relateTypes = new GUIContent[]
        //{
        //    new GUIContent("相对于自己"),
        //    new GUIContent("相对于目标"),
        //};
        protected override void OnEnable()
        {
            base.OnEnable();

            _moveType = serializedObject.FindProperty("_moveType");
            //_relateType = serializedObject.FindProperty("_relateType");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            int index = EditorGUILayout.Popup(_moveTypeUI, _selectedMoveIndex, _moveTypes);
            if (EditorGUI.EndChangeCheck())
            {
                _moveType.enumValueIndex = index;
                _selectedMoveIndex = index;
            }

            //EditorGUI.BeginChangeCheck();
            //index = EditorGUILayout.Popup(_relateTypeUI, _selectedRelateIndex, _relateTypes);
            //if (EditorGUI.EndChangeCheck())
            //{
            //    _relateType.enumValueIndex = index;
            //    _selectedRelateIndex = index;
            //}
        }
    }
}

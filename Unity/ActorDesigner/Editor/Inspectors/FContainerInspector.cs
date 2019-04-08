using UnityEngine;
using UnityEditor;

using System;

using Flux;

namespace FluxEditor
{
	[CustomEditor(typeof(FContainer), true)]
	public class FContainerInspector : Editor {

		FContainer _container = null;

        private SerializedProperty _containerNo;
        private SerializedProperty _color;
        private GUIContent _containerNoUI = new GUIContent("编号");
        private GUIContent _colorUI = new GUIContent("颜色");

		void OnEnable()
		{
			_container = (FContainer)target;
            _containerNo = serializedObject.FindProperty("ContainerNo");
            _color = serializedObject.FindProperty("_color");
        }

		public override void OnInspectorGUI ()
		{
            EditorGUI.BeginChangeCheck();
            string name = EditorGUILayout.TextField("名称", _container.gameObject.name);
            EditorGUILayout.PropertyField(_containerNo, _containerNoUI);
            EditorGUILayout.PropertyField(_color, _colorUI);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_container.gameObject, "change name");
                _container.gameObject.name = name;
                EditorUtility.SetDirty(_container.gameObject);
            }
            base.DrawDefaultInspector();
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Inspector
{
    [CustomEditor(typeof(Gray), true)]
    public class GrayInspector : Editor
    {
        private Gray gray;
        private SerializedProperty isGray;
        private void OnEnable()
        {
            gray = (Gray)target;
            isGray = serializedObject.FindProperty("isGray");
            gray.Init();
        }
        private void OnDisable()
        {
            gray.Clear();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (EditorGUILayout.Toggle(isGray.displayName, isGray.boolValue))
                gray.EnableGray();
            else
                gray.DisableGray();
        }
    }
}
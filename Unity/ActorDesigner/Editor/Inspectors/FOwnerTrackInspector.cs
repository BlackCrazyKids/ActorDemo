using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

//using UnityEditorInternal;

using System.Collections.Generic;

using Flux;
using FluxEditor;

namespace FluxEditor
{
    public abstract class FOwnerTrackInspector : FTrackInspector
    {
        private SerializedProperty _owner = null;
        private GUIContent _ownerUI;

        protected virtual string OwnerName { get { return "角色"; } }

        public override void OnEnable()
        {
            base.OnEnable();

            if (target == null)
            {
                DestroyImmediate(this);
                return;
            }
            _owner = serializedObject.FindProperty("_owner");
            _ownerUI = new GUIContent(OwnerName);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_owner, _ownerUI);
            if (EditorGUI.EndChangeCheck())
                OnChangeCharacter();
        }

        protected virtual void OnChangeCharacter() { }
    }
}

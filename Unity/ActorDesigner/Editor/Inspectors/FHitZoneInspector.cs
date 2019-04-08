using System;
using UnityEditor;
using UnityEngine;
using XmlCfg.Skill;

namespace FluxEditor
{
    [CustomEditor(typeof(Flux.FHitZone), true)]
    public class FHitZoneInspector : FEventInspector
    {
        private SerializedProperty _sharpType;
        private SerializedProperty _scale;
        private SerializedProperty _radius;
        private SerializedProperty _height;
        private SerializedProperty _angle;

        private GUIContent _scaleUI = new GUIContent("缩放");
        private GUIContent _radiusUI = new GUIContent("半径");
        private GUIContent _heightUI = new GUIContent("高度");
        private GUIContent _angleUI = new GUIContent("角度");

        protected override void OnEnable()
        {
            base.OnEnable();

            _sharpType = serializedObject.FindProperty("_sharpType");
            _scale = serializedObject.FindProperty("_scale");
            _radius = serializedObject.FindProperty("_radius");
            _height = serializedObject.FindProperty("_height");
            _angle = serializedObject.FindProperty("_angle");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            _sharpType.enumValueIndex = EditorGUILayout.Popup("碰撞体形状", _sharpType.enumValueIndex, _sharpType.enumDisplayNames);
            switch ((HitSharpType)_sharpType.enumValueIndex)
            {
                case HitSharpType.Cube:
                    EditorGUILayout.PropertyField(_scale, _scaleUI);
                    break;
                case HitSharpType.Sphere:
                    EditorGUILayout.PropertyField(_radius, _radiusUI);
                    break;
                case HitSharpType.Cylinder:
                    EditorGUILayout.PropertyField(_radius, _radiusUI);
                    EditorGUILayout.PropertyField(_height, _heightUI);
                    EditorGUILayout.PropertyField(_angle, _angleUI);
                    break;
                default:
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

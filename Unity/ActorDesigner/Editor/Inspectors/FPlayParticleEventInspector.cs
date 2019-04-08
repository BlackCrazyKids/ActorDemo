using UnityEngine;
using UnityEditor;

using Flux;

namespace FluxEditor
{
    [CustomEditor(typeof(FPlayParticleEvent))]
    public class FPlayParticleEventInspector : FEventInspector
    {

        private FPlayParticleEvent _playParticleEvent = null;

        private SerializedProperty _randomSeed = null;
        private SerializedProperty _particleSystem = null;

        private GUIContent _randomSeedUI;
        private GUIContent _particleSystemUI;

        protected override void OnEnable()
        {
            base.OnEnable();
            _playParticleEvent = (FPlayParticleEvent)target;

            _randomSeed = serializedObject.FindProperty("_randomSeed");
            _particleSystem = serializedObject.FindProperty("_particleSystem");


            _randomSeedUI = new GUIContent("随机种子");
            _particleSystemUI = new GUIContent("粒子对象");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_randomSeed, _randomSeedUI);
            if (GUILayout.Button(new GUIContent("R", "随机获得一个种子"), GUILayout.Width(25)))
                _randomSeed.intValue = Random.Range(0, int.MaxValue);
            EditorGUILayout.EndHorizontal();

            if (_randomSeed.intValue == 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("A random seed of 0 means that it always randomizes the particle system, which breaks backwards play of particle systems", MessageType.Warning);
                if (GUILayout.Button("Fix", GUILayout.Width(40), GUILayout.Height(40)))
                    _randomSeed.intValue = Random.Range(0, int.MaxValue);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.PropertyField(_particleSystem, _particleSystemUI);            
            serializedObject.ApplyModifiedProperties();
            ParticleSystem particleSystem = _particleSystem.objectReferenceValue as ParticleSystem;

            if (particleSystem == null)
            {
                EditorGUILayout.HelpBox("粒子事件中未添加粒子对象", MessageType.Warning);
                return;
            }

            if (_playParticleEvent.LengthTime > particleSystem.main.duration || _playParticleEvent.LengthTime > particleSystem.main.startLifetime.constant)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("粒子的持续时间/开始时间小于事件长度,这将与游戏运行时结果不一致", MessageType.Warning);
                if (GUILayout.Button("Fix", GUILayout.Width(40), GUILayout.Height(40)))
                {
                    int selectedOption = EditorUtility.DisplayDialogComplex("修正粒子事件?", "不幸的是,Unity的粒子系统并不完全支持向后播放." +
                        "你想怎么纠正这个?" +
                        "\n - 增加粒子持续时间：使粒子持续时间与事件的长度相同" +
                        "\n - 收缩事件：将事件大小调整为粒子系统的长度", "增加粒子持续时间", "取消", "收缩事件");
                    switch (selectedOption)
                    {
                        case 0: // Increase duration
                            SerializedObject particleSystemSO = new SerializedObject(particleSystem);
                            particleSystemSO.FindProperty("lengthInSec").floatValue = _playParticleEvent.LengthTime;
                            particleSystemSO.FindProperty("InitialModule.startLifetime.scalar").floatValue = _playParticleEvent.LengthTime;
                            particleSystemSO.ApplyModifiedProperties();
                            break;
                        case 1: // Cancel
                            break;
                        case 2: // Shrink event
                            FUtility.Rescale(_playParticleEvent,
                                new FrameRange(_playParticleEvent.FrameRange.Start,
                                    Mathf.RoundToInt(Mathf.Min(particleSystem.main.duration, particleSystem.main.startLifetime.constant) * _playParticleEvent.Sequence.FrameRate)));
                            break;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
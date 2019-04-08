using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

//using UnityEditorInternal;

using System.Collections.Generic;

using Flux;
using FluxEditor;

namespace FluxEditor
{
    [CustomEditor(typeof(FAnimationTrack))]
    public class FAnimationTrackInspector : FTrackInspector
    {

        //		private const string ADVANCE_TRIGGER = "FAdvanceTrigger";
        private const string FLUX_STATE_MACHINE_NAME = "FluxStateMachine";

        private FAnimationTrack _animTrack = null;

        private SerializedProperty _owner = null;
        private SerializedProperty _layerName = null;
        private SerializedProperty _layerId = null;
        private GUIContent _ownerUI;

        public override void OnEnable()
        {
            base.OnEnable();

            if (target == null)
            {
                DestroyImmediate(this);
                return;
            }
            _animTrack = (FAnimationTrack)target;
            _owner = serializedObject.FindProperty("_owner");
            _layerName = serializedObject.FindProperty("_layerName");
            _layerId = serializedObject.FindProperty("_layerId");

            _ownerUI = new GUIContent("角色");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_owner, _ownerUI);
            if (EditorGUI.EndChangeCheck())
            {
                _animTrack.ClearCache();
                _animTrack.AnimatorController = null;
                serializedObject.ApplyModifiedProperties();
                SetAnimator(_animTrack);
            }
        }

        public void UpdateLayer(AnimatorControllerLayer layer)
        {
            if (layer == null)
            {
                _layerName.stringValue = null;
                _layerId.intValue = -1;
            }
            else
            {
                AnimatorController controller = (AnimatorController)_animTrack.AnimatorController;
                for (int i = 0; i != controller.layers.Length; ++i)
                {
                    if (controller.layers[i].name == layer.name)
                    {
                        _layerName.stringValue = layer.name;
                        _layerId.intValue = i;
                        return;
                    }
                }
                // shouldn't get here..
                Debug.LogError("Trying to set a layer that doesn't belong to the controller");
                UpdateLayer(null);
            }
        }

        public static void SetAnimator(FAnimationTrack animTrack)
        {
            if (animTrack.Owner.GetComponent<Animator>() == null)
            {
                Animator animator = animTrack.Owner.gameObject.AddComponent<Animator>();
                Undo.RegisterCreatedObjectUndo(animator, string.Empty);
            }
            if (animTrack.AnimatorController == null)
            {
                var temp = FUtility.GetFluxAssets<UnityEditor.Animations.AnimatorController>("EditorAnimatorController.controller");
                var animatorController = Instantiate(temp);
                animTrack.LayerId = 0;
                animTrack.LayerName = "Base Layer";
                var layers = animatorController.layers;
                layers[animTrack.LayerId].name = animTrack.LayerName;
                layers[animTrack.LayerId].stateMachine.name = animTrack.LayerName;
                animatorController.layers = layers;
                animTrack.AnimatorController = animatorController;
                FAnimationTrackInspector.RebuildStateMachine(animTrack);
            }
        }
        public static void RebuildStateMachine(FAnimationTrack track)
        {
            if (track.AnimatorController == null || track.LayerId == -1)
                return;

            bool isPreviewing = track.HasCache;

            if (isPreviewing)
                track.ClearCache();

            Animator animator = track.Owner.GetComponent<Animator>();
            animator.runtimeAnimatorController = null;

            AnimatorController controller = (AnimatorController)track.AnimatorController;

            track.UpdateEventIds();

            AnimatorControllerLayer layer = FindLayer(controller, track.LayerName);

            if (layer == null)
            {
                controller.AddLayer(track.LayerName);
                layer = FindLayer(controller, track.LayerName);
            }

            AnimatorStateMachine fluxSM = FindStateMachine(layer.stateMachine, FLUX_STATE_MACHINE_NAME);

            if (fluxSM == null)
            {
                fluxSM = layer.stateMachine.AddStateMachine(FLUX_STATE_MACHINE_NAME);

                ChildAnimatorStateMachine[] stateMachines = layer.stateMachine.stateMachines;

                for (int i = 0; i != stateMachines.Length; ++i)
                {
                    if (stateMachines[i].stateMachine == fluxSM)
                    {
                        stateMachines[i].position = layer.stateMachine.entryPosition + new Vector3(300, 0, 0);
                        break;
                    }
                }
                layer.stateMachine.stateMachines = stateMachines;
            }

            List<FEvent> events = track.Events;

            if (fluxSM.states.Length > events.Count)
            {
                for (int i = events.Count; i < fluxSM.states.Length; ++i)
                    fluxSM.RemoveState(fluxSM.states[i].state);
            }
            else if (fluxSM.states.Length < events.Count)
            {
                for (int i = fluxSM.states.Length; i < events.Count; ++i)
                    fluxSM.AddState(i.ToString());
            }

            AnimatorState lastState = null;
            Vector3 pos = fluxSM.entryPosition + new Vector3(300, 0, 0);

            Vector3 statePosDelta = new Vector3(0, 80, 0);

            ChildAnimatorState[] states = fluxSM.states;

            for (int i = 0; i != events.Count; ++i)
            {
                FPlayAnimationEvent animEvent = (FPlayAnimationEvent)events[i];

                if (animEvent._animationClip == null) // dump events without animations
                    continue;

                states[i].position = pos;

                AnimatorState state = states[i].state;

                state.name = i.ToString();

                pos += statePosDelta;

                state.motion = animEvent._animationClip;

                animEvent._stateHash = Animator.StringToHash(state.name);

                if (lastState)
                {
                    AnimatorStateTransition[] lastStateTransitions = lastState.transitions;

                    if (animEvent.IsBlending())
                    {
                        AnimatorStateTransition transition = null;

                        for (int j = lastStateTransitions.Length - 1; j > 0; --j)
                        {
                            lastState.RemoveTransition(lastStateTransitions[j]);
                        }

                        transition = lastStateTransitions.Length == 1 ? lastStateTransitions[0] : lastState.AddTransition(state);

                        transition.offset = (animEvent._startOffset / animEvent._animationClip.frameRate) / animEvent._animationClip.length;

                        FPlayAnimationEvent prevAnimEvent = (FPlayAnimationEvent)events[i - 1];

                        int offsetFrame = Mathf.RoundToInt(transition.offset * animEvent._animationClip.length * animEvent._animationClip.frameRate);
                        animEvent._startOffset = Mathf.Clamp(offsetFrame, 0, animEvent.GetMaxStartOffset());
                        transition.offset = (animEvent._startOffset / animEvent._animationClip.frameRate) / animEvent._animationClip.length;
                        EditorUtility.SetDirty(animEvent);

                        float blendSeconds = animEvent._blendLength / prevAnimEvent._animationClip.frameRate;
                        transition.duration = blendSeconds;

                        transition.hasExitTime = true;
                        float p = blendSeconds / prevAnimEvent._animationClip.length;
                        transition.exitTime = p > 1f ? 1f : 1f - Mathf.Clamp01(blendSeconds / prevAnimEvent._animationClip.length);

                        for (int j = transition.conditions.Length - 1; j >= 0; --j)
                        {
                            transition.RemoveCondition(transition.conditions[j]);
                        }

                        //						AnimatorCondition condition = transition.conditions[0];
                        //						condition.threshold = 0;
                    }
                    else // animations not blending, needs hack for animation previewing
                    {
                        for (int j = lastStateTransitions.Length - 1; j >= 0; --j)
                        {
                            lastState.RemoveTransition(lastStateTransitions[j]);
                        }
                    }
                }

                lastState = state;
            }

            fluxSM.states = states;

            if (fluxSM.states.Length > 0)
                layer.stateMachine.defaultState = fluxSM.states[0].state;

            animator.runtimeAnimatorController = controller;

            if (isPreviewing)
                track.CreateCache();
        }

        public static AnimatorStateTransition GetTransitionTo(FPlayAnimationEvent animEvt)
        {
            FAnimationTrack animTrack = (FAnimationTrack)animEvt.Track;

            if (animTrack.AnimatorController == null)
                return null;

            AnimatorController controller = (AnimatorController)animTrack.AnimatorController;

            AnimatorState animState = null;

            AnimatorControllerLayer layer = FindLayer(controller, ((FAnimationTrack)animEvt.Track).LayerName);

            if (layer == null)
                return null;

            if (layer.stateMachine.stateMachines.Length == 0)
                return null;

            ChildAnimatorStateMachine fluxSM = layer.stateMachine.stateMachines[0];

            AnimatorStateMachine stateMachine = fluxSM.stateMachine;

            for (int i = 0; i != stateMachine.states.Length; ++i)
            {
                if (stateMachine.states[i].state.nameHash == animEvt._stateHash)
                {
                    animState = stateMachine.states[i].state;
                    break;
                }
            }

            if (animState == null)
            {
                //				Debug.LogError("Couldn't find state " + animEvt._animationClip );
                return null;
            }

            for (int i = 0; i != stateMachine.states.Length; ++i)
            {
                AnimatorState state = stateMachine.states[i].state;

                AnimatorStateTransition[] transitions = state.transitions;
                for (int j = 0; j != transitions.Length; ++j)
                {
                    if (transitions[j].destinationState == animState)
                    {
                        return transitions[j];
                    }
                }
            }

            return null;
        }

        private static AnimatorControllerParameter FindParameter(AnimatorController controller, string paramName)
        {
            foreach (AnimatorControllerParameter p in controller.parameters)
            {
                if (p.name == paramName)
                    return p;
            }

            return null;
        }

        private static AnimatorControllerLayer FindLayer(AnimatorController controller, string layerName)
        {
            foreach (AnimatorControllerLayer layer in controller.layers)
            {
                if (layer.name == layerName)
                    return layer;
            }

            return null;
        }

        private static AnimatorStateMachine FindStateMachine(AnimatorStateMachine stateMachine, string stateMachineName)
        {
            foreach (ChildAnimatorStateMachine sm in stateMachine.stateMachines)
            {
                if (sm.stateMachine.name == stateMachineName)
                    return sm.stateMachine;
            }

            return null;
        }
    }
}

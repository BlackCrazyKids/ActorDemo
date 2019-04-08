using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
//using UnityEditorInternal;

using System;
using System.Collections.Generic;

using Flux;

namespace FluxEditor
{
    [FEditor(typeof(FPlayAnimationEvent))]
    public class FAnimationEventEditor : FEventEditor
    {

        //        [SerializeField]
        protected FPlayAnimationEvent AnimEvt { get { return (FPlayAnimationEvent)Obj; } }
        //        [SerializeField]
        protected FAnimationTrack AnimTrack { get { return (FAnimationTrack)Owner.Obj; } }

        protected AnimatorState _animState;

        protected AnimatorStateTransition _transitionToState;

        private SerializedObject _animEvtSO;
        private SerializedProperty _blendLength;
        private SerializedProperty _startOffset;

        private SerializedObject _transitionSO;
        private SerializedProperty _transitionExitTime;
        private SerializedProperty _transitionDuration;
        private SerializedProperty _transitionOffset;

        private static int _mouseDown = int.MinValue;

        public override void OnDelete()
        {
            FAnimationEventInspector.CheckDeleteAnimation(AnimEvt);
        }

        private void UpdateEventFromController()
        {
            bool isBlending = AnimEvt.IsBlending();

            if (isBlending)
            {
                if (_transitionToState == null)
                {
                    _transitionToState = FAnimationTrackInspector.GetTransitionTo(AnimEvt);

                    if (_transitionToState == null || _transitionToState.conditions.Length > 0)
                    {
                        FAnimationTrackInspector.RebuildStateMachine((FAnimationTrack)TrackEditor.Track);

                        _transitionToState = FAnimationTrackInspector.GetTransitionTo(AnimEvt);
                    }
                }

                if (_transitionSO == null)
                {
                    if (_transitionToState != null)
                    {
                        _transitionSO = new SerializedObject(_transitionToState);
                        _transitionExitTime = _transitionSO.FindProperty("m_ExitTime");
                        _transitionDuration = _transitionSO.FindProperty("m_TransitionDuration");
                        _transitionOffset = _transitionSO.FindProperty("m_TransitionOffset");
                    }
                }
                else if (_transitionSO.targetObject == null)
                {
                    _transitionExitTime = null;
                    _transitionDuration = null;
                    _transitionOffset = null;
                    _transitionSO = null;
                }
            }
            else
            {
                if (_transitionToState != null || _transitionSO != null)
                {
                    _transitionToState = null;
                    _transitionSO = null;
                    _transitionExitTime = null;
                    _transitionOffset = null;
                    _transitionDuration = null;
                }
            }

            if (_transitionSO != null)
            {
                _transitionSO.Update();
                _animEvtSO.Update();

                FPlayAnimationEvent prevAnimEvt = (FPlayAnimationEvent)AnimTrack.Events[AnimEvt.GetId() - 1];
                AnimationClip prevAnimEvtClip = prevAnimEvt._animationClip;
                if (prevAnimEvtClip != null)
                {
                    float blendSeconds = _blendLength.intValue / prevAnimEvtClip.frameRate;

                    if (!Mathf.Approximately(blendSeconds, _transitionDuration.floatValue))
                    {
                        _transitionDuration.floatValue = blendSeconds;
                        float p = blendSeconds / prevAnimEvtClip.length;
                        _transitionExitTime.floatValue = p > 1f ? 1f : 1f - Mathf.Clamp01(blendSeconds / prevAnimEvtClip.length);
                        _animEvtSO.ApplyModifiedProperties();
                    }

                    float startOffsetNorm = _startOffset.intValue / AnimEvt._animationClip.frameRate / AnimEvt._animationClip.length;

                    if (!Mathf.Approximately(startOffsetNorm, _transitionOffset.floatValue))
                    {
                        _transitionOffset.floatValue = startOffsetNorm;
                        _animEvtSO.ApplyModifiedProperties();
                    }
                }
                _transitionSO.ApplyModifiedProperties();
            }
        }

        protected override void RenderEvent(FrameRange viewRange, FrameRange validKeyframeRange)
        {
            if (_animEvtSO == null)
            {
                _animEvtSO = new SerializedObject(AnimEvt);
                _blendLength = _animEvtSO.FindProperty("_blendLength");
                _startOffset = _animEvtSO.FindProperty("_startOffset");
            }

            UpdateEventFromController();

            _animEvtSO.Update();

            FAnimationTrackEditor animTrackEditor = (FAnimationTrackEditor)TrackEditor;

            Rect transitionOffsetRect = _eventRect;

            int startOffsetHandleId = EditorGUIUtility.GetControlID(FocusType.Passive);
            int transitionHandleId = EditorGUIUtility.GetControlID(FocusType.Passive);

            bool isBlending = AnimEvt.IsBlending();
            bool isAnimEditable = Flux.FUtility.IsAnimationEditable(AnimEvt._animationClip);

            if (isBlending)
            {
                transitionOffsetRect.xMin = Rect.xMin + SequenceEditor.GetXForFrame(AnimEvt.Start + AnimEvt._blendLength) - 3;
                transitionOffsetRect.width = 6;
                transitionOffsetRect.yMin = transitionOffsetRect.yMax - 8;
            }

            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (EditorGUIUtility.hotControl == 0 && Event.current.alt && !isAnimEditable)
                    {
                        if (isBlending && transitionOffsetRect.Contains(Event.current.mousePosition))
                        {
                            EditorGUIUtility.hotControl = transitionHandleId;

                            AnimatorWindowProxy.OpenAnimatorWindowWithAnimatorController((AnimatorController)AnimTrack.AnimatorController);

                            if (Selection.activeObject != _transitionToState)
                                Selection.activeObject = _transitionToState;

                            Event.current.Use();
                        }
                        else if (_eventRect.Contains(Event.current.mousePosition))
                        {
                            _mouseDown = SequenceEditor.GetFrameForX(Event.current.mousePosition.x) - AnimEvt.Start;

                            EditorGUIUtility.hotControl = startOffsetHandleId;

                            Event.current.Use();
                        }
                    }
                    break;

                case EventType.Ignore:
                case EventType.MouseUp:
                    if (EditorGUIUtility.hotControl == transitionHandleId
                       || EditorGUIUtility.hotControl == startOffsetHandleId)
                    {
                        EditorGUIUtility.hotControl = 0;
                        Event.current.Use();
                    }
                    break;

                case EventType.MouseDrag:
                    if (EditorGUIUtility.hotControl == transitionHandleId)
                    {
                        int mouseDragPos = Mathf.Clamp(SequenceEditor.GetFrameForX(Event.current.mousePosition.x - Rect.x) - AnimEvt.Start, 0, AnimEvt.Length);

                        if (_blendLength.intValue != mouseDragPos)
                        {
                            _blendLength.intValue = mouseDragPos;

                            FPlayAnimationEvent prevAnimEvt = (FPlayAnimationEvent)animTrackEditor.Track.GetEvent(AnimEvt.GetId() - 1);

                            float blendSeconds = _blendLength.intValue / prevAnimEvt._animationClip.frameRate;
                            if (_transitionDuration != null)
                            {
                                _transitionDuration.floatValue = blendSeconds;
                                float p = blendSeconds / prevAnimEvt._animationClip.length;
                                _transitionExitTime.floatValue = p > 1f ? 1f : 1f - Mathf.Clamp01(blendSeconds / prevAnimEvt._animationClip.length);
                            }
                            Undo.RecordObject(this, "Animation Blending");
                        }
                        Event.current.Use();
                    }
                    else if (EditorGUIUtility.hotControl == startOffsetHandleId)
                    {
                        int mouseDragPos = Mathf.Clamp(SequenceEditor.GetFrameForX(Event.current.mousePosition.x - Rect.x) - AnimEvt.Start, 0, AnimEvt.Length);

                        int delta = _mouseDown - mouseDragPos;

                        _mouseDown = mouseDragPos;

                        _startOffset.intValue = Mathf.Clamp(_startOffset.intValue + delta, 0, AnimEvt._animationClip.isLooping ? AnimEvt.Length : Mathf.RoundToInt(AnimEvt._animationClip.length * AnimEvt._animationClip.frameRate) - AnimEvt.Length);

                        if (_transitionOffset != null)
                            _transitionOffset.floatValue = (_startOffset.intValue / AnimEvt._animationClip.frameRate) / AnimEvt._animationClip.length;

                        Undo.RecordObject(this, "Animation Offset");

                        Event.current.Use();
                    }
                    break;
            }

            _animEvtSO.ApplyModifiedProperties();
            if (_transitionSO != null)
                _transitionSO.ApplyModifiedProperties();


            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                    if (_eventRect.Contains(Event.current.mousePosition))
                    {
                        int numAnimationsDragged = FAnimationEventInspector.NumAnimationsDragAndDrop(Evt.Sequence.FrameRate);
                        DragAndDrop.visualMode = numAnimationsDragged > 0 ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;
                        Event.current.Use();
                    }
                    break;
                case EventType.DragPerform:
                    if (_eventRect.Contains(Event.current.mousePosition))
                    {
                        AnimationClip animationClipDragged = FAnimationEventInspector.GetAnimationClipDragAndDrop(Evt.Sequence.FrameRate);
                        if (animationClipDragged)
                        {
                            int animFrameLength = Mathf.RoundToInt(animationClipDragged.length * animationClipDragged.frameRate);

                            FAnimationEventInspector.SetAnimationClip(AnimEvt, animationClipDragged);

                            FrameRange maxRange = AnimEvt.GetMaxFrameRange();

                            SequenceEditor.MoveEvent(AnimEvt, new FrameRange(AnimEvt.Start, Mathf.Min(AnimEvt.Start + animFrameLength, maxRange.End)));

                            DragAndDrop.AcceptDrag();
                            Event.current.Use();
                        }
                        else
                        {
                            Event.current.Use();
                        }
                    }
                    break;
            }

            //            FrameRange currentRange = Evt.FrameRange;

            base.RenderEvent(viewRange, validKeyframeRange);

            //            if( isAnimEditable && currentRange.Length != Evt.FrameRange.Length )
            //            {
            //                FAnimationEventInspector.ScaleAnimationClip( AnimEvt._animationClip, Evt.FrameRange );
            //            }

            if (Event.current.type == EventType.Repaint)
            {
                if (isBlending && !isAnimEditable && viewRange.Contains(AnimEvt.Start + AnimEvt._blendLength))
                {
                    GUISkin skin = FUtility.GetFluxSkin();

                    GUIStyle transitionOffsetStyle = skin.GetStyle("BlendOffset");

                    Texture2D t = FUtility.GetFluxAssets<Texture2D>("Blender.png");

                    float offset = SequenceEditor.PixelsPerFrame * AnimEvt._startOffset;
                    float x = _eventRect.xMin + offset;
                    float maxWidth = SequenceEditor.PixelsPerFrame * AnimEvt.Length - offset;
                    float width = SequenceEditor.PixelsPerFrame * AnimEvt._blendLength;
                    if (width > maxWidth)
                        width = maxWidth;
                    Rect r = new Rect(x, _eventRect.yMin + 1, width, _eventRect.height - 2);

                    Color guiColor = GUI.color;

                    Color c = new Color(1f, 1f, 1f, 0.5f);
                    c.a *= guiColor.a;
                    GUI.color = c;

                    GUI.DrawTexture(r, t);

                    if (Event.current.alt)
                        GUI.color = Color.black;

                    GUI.color = guiColor;
                }

                if (EditorGUIUtility.hotControl == transitionHandleId)
                {
                    Rect transitionOffsetTextRect = transitionOffsetRect;
                    transitionOffsetTextRect.y -= 16;
                    transitionOffsetTextRect.height = 20;
                    transitionOffsetTextRect.width += 50;
                    GUI.Label(transitionOffsetTextRect, AnimEvt._blendLength.ToString(), EditorStyles.label);
                }

                if (EditorGUIUtility.hotControl == startOffsetHandleId)
                {
                    Rect startOffsetTextRect = _eventRect;
                    GUI.Label(startOffsetTextRect, AnimEvt._startOffset.ToString(), EditorStyles.label);
                }
            }
        }

        public override void OnEventFinishedMoving(FrameRange oldFrameRange)
        {
            ////---动画帧数据不做缩放操作
            //if (oldFrameRange.Length != AnimEvt.FrameRange.Length && Flux.FUtility.IsAnimationEditable(AnimEvt._animationClip))
            //{
            //    FAnimationEventInspector.ScaleAnimationClip(AnimEvt._animationClip, AnimEvt.FrameRange);
            //}
        }

    }
}

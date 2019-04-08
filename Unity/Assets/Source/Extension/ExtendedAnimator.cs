using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendedAnimator
{
    public static float Bake(this Animator animator, string action)
    {
        if(!string.IsNullOrEmpty(action))
        {
            animator.Rebind();
            animator.StopPlayback();
            animator.recorderStartTime = 0;
            // 这里可以在指定的时间触发新的动画状态
            animator.Play(action);

            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            //const float frameRate = 30f;
            //int frameCount = (int)((Duration * frameRate) + 2);
            // 开始记录指定的帧数
            animator.StartRecording(0);
            
            while (info.normalizedTime < 1f)
            {
                // 记录每一帧
                animator.Update(1.0f / 30);
                info = animator.GetCurrentAnimatorStateInfo(0);
                //Debug.Log(info.length);
                //Debug.Log(info.normalizedTime);
                if (info.length <= 0)
                {
                    return 0;
                }
            }
            // 完成记录
            animator.StopRecording();
            // 开启回放模式
            animator.StartPlayback();
            return info.length;
        }

        return 0;
    }

    public static float BakeCurrent(this Animator animator)
    {
        animator.Rebind();
        animator.StopPlayback();
        animator.recorderStartTime = 0;
        // 这里可以在指定的时间触发新的动画状态
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        //const float frameRate = 30f;
        //int frameCount = (int)((Duration * frameRate) + 2);
        // 开始记录指定的帧数
        animator.StartRecording(0);
        while (info.normalizedTime < 1f)
        {
            // 记录每一帧
            animator.Update(1.0f / 30);
            info = animator.GetCurrentAnimatorStateInfo(0);
            //Debug.Log(info.length);
            //Debug.Log(info.normalizedTime);
        }
        // 完成记录
        animator.StopRecording();
        // 开启回放模式
        animator.StartPlayback();
        return info.length;
    }

    //public static UnityEditorInternal.State GetDefaultState(this Animator animator)
    //{
    //    // Get a reference to the Animator Controller:
    //    UnityEditorInternal.AnimatorController ac = animator.runtimeAnimatorController as UnityEditorInternal.AnimatorController;
    //    if(ac != null)
    //    {
    //        UnityEditorInternal.AnimatorControllerLayer layer = ac.GetLayer(0);
    //        UnityEditorInternal.StateMachine sm = layer.stateMachine;
    //        return sm.defaultState;
    //    }
    //    return null;
        
    //}

    //public static string GetDefaultStateName(this Animator animator)
    //{
    //    UnityEditorInternal.State state = animator.GetDefaultState();

    //    if (state != null)
    //    {
    //        return state.name;
    //    }
    //    return string.Empty;
    //}


    public static float GetCurrentStateLength(this Animator animator)
    {
        if (animator != null)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            return info.length;
            
        }
        return 0;
    }

    public static void Sample(this Animator animator, float time)
    {
        if (animator != null)
        {
            animator.playbackTime = time;
            animator.Update(0);
        }
    }
   

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game;

[System.Serializable]
public class AnimationData
{
    public AnimationClip clip;
    public string clipName;
    public float speed = 1;
    public float transitionDuration = -1;
    public WrapMode wrapMode;
    public bool applyRootMotion;
    [HideInInspector] public int timesPlayed = 0;
    [HideInInspector] public float secondsPlayed = 0;
    [HideInInspector] public float length = 0;
    [HideInInspector] public int stateHash;
    [HideInInspector] public string stateName;

    public float NormalizedTime
    {
        get { return length == 0 ? 0 : secondsPlayed / length; }
    }

    public void Reset()
    {
        clip = null;
        clipName = string.Empty;
        speed = 1;
        transitionDuration = -1;
        applyRootMotion = false;
        timesPlayed = 0;
        secondsPlayed = 0;
        length = 0;
        stateHash = 0;
        stateName = string.Empty;
    }
}

[RequireComponent(typeof(Animator))]
public class MecanimControl : MonoBehaviour
{
    #region ControllerPool
    static Game.SimplePool<AnimatorOverrideController> controllerPool = new Game.SimplePool<AnimatorOverrideController>(50);
    static AnimatorOverrideController GetController()
    {
        AnimatorOverrideController overrideController = controllerPool.Get();
        if (overrideController == null)
        {
            //Util.LogColor("cyan", "new Controller");
            var controller1 = (RuntimeAnimatorController)Resources.Load("controller1");
            overrideController = new AnimatorOverrideController();
            overrideController.runtimeAnimatorController = controller1;
        }
        else
        {
            //Util.LogColor("yellow", string.Format("get Controller,count:{0}", controllerPool.Count));
        }
        return overrideController;
    }

    static void PutController(AnimatorOverrideController overrideController)
    {
        if (overrideController != null)
        {
            overrideController["State1"] = null;
            overrideController["State2"] = null;
            overrideController["State3"] = null;
            controllerPool.Release(overrideController);
            Destroy(overrideController);
        }
    }

    public static void ClearControllerPool()
    {
        AnimatorOverrideController controller = null;
        while ((controller = controllerPool.Get()) != null)
        {
            Destroy(controller);
            //Util.LogColor("red", string.Format("clear ControllerPool,count:{0}", controllerPool.Count));
        }
    }
    #endregion

    public Dictionary<string, AnimationData> animations = new Dictionary<string, AnimationData>();

    public bool debugMode = false;
    public bool alwaysPlay = false;
    public bool playOnStart = true;
    public bool overrideRootMotion = false;
    public float defaultTransitionDuration = 0f;
    public WrapMode defaultWrapMode = WrapMode.Default;


    public AnimationData defaultAnimationData = new AnimationData(); //State3
    private AnimationData currentAnimationData; //State
    private bool currentMirror = false;
    private float currentBlendingTime = 0f;

    private float globalSpeed = 1;
    public float speed
    {
        get
        {
            return globalSpeed;
        }
        set
        {
            globalSpeed = value;
            if (currentAnimationData != null)
            {
                animator.speed = globalSpeed * currentAnimationData.speed;
            }
            else
            {
                animator.speed = globalSpeed;
            }
        }
    }


    [SerializeField]
    private string _modelName = "";

    public string modelName
    {
        get { return _modelName; }
        set { _modelName = value; }
    }


    private Animator animator;
    private AnimatorOverrideController cachedController = null; //切换动作状态时，省去新增Controller

    private RuntimeAnimatorController controller1;



    public delegate void AnimEvent(AnimationData animationData);
    public static event AnimEvent OnAnimationBegin;
    public static event AnimEvent OnAnimationEnd;
    public static event AnimEvent OnAnimationLoop;

    public void Init()
    {
        ////Util.LogColor("red", "MecanimControl Start");

        animator = gameObject.GetComponent<Animator>();
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        controller1 = (RuntimeAnimatorController)Resources.Load("controller1");

        animator.runtimeAnimatorController = GetController();
        cachedController = GetController();


        foreach (var pair in animations)
        {
            AnimationData animData = pair.Value;
            if (animData.wrapMode == WrapMode.Default) animData.wrapMode = defaultWrapMode;
            animData.clip.wrapMode = animData.wrapMode;
        }

        if (defaultAnimationData.clip == null && animations.Count > 0)
        {
            var e = animations.GetEnumerator();
            if (e.MoveNext())
            {
                var animData = e.Current.Value;
                SetDefaultClip(animData.clip, "Default", animData.speed, animData.wrapMode);
            }
        }

        if (defaultAnimationData.clip != null)
        {

            SetOverrideController(defaultAnimationData, defaultAnimationData, currentMirror);
            currentAnimationData = defaultAnimationData;

            if (playOnStart)
            {
                AnimatorPlay("State2", 0, 0);
                if (overrideRootMotion) animator.applyRootMotion = currentAnimationData.applyRootMotion;
                SetSpeed(currentAnimationData.speed);
            }
        }
    }

    void OnEnable()
    {
        if (currentAnimationData != null)
        {
            Play(currentAnimationData, currentBlendingTime, currentAnimationData.NormalizedTime, currentMirror);
        }
    }

    void OnDestroy()
    {
        ClearClips();
        if (animator != null)
        {
            PutController(animator.runtimeAnimatorController as AnimatorOverrideController);
            animator.runtimeAnimatorController = null;
            PutController(cachedController);
            cachedController = null;
        }
    }

    //void FixedUpdate(){
    //WrapMode emulator
    /*
    if (currentAnimationData == null || currentAnimationData.clip == null) return;
    var normalizedTime = GetNormalizedTime();
    Debug.Log("normalizedTime" + normalizedTime + "Mathf.FloorToInt(normalizedTime)" + Mathf.FloorToInt(normalizedTime));
    var timesPlayed = Mathf.FloorToInt(normalizedTime);
    if (currentAnimationData.timesPlayed > timesPlayed)
    {
        currentAnimationData.timesPlayed = timesPlayed;
    }
    else if (currentAnimationData.timesPlayed < timesPlayed)
    {
        if (currentAnimationData.clip.wrapMode == WrapMode.Loop || currentAnimationData.clip.wrapMode == WrapMode.PingPong)
        {
            if (MecanimControl.OnAnimationLoop != null) MecanimControl.OnAnimationLoop(currentAnimationData);
            currentAnimationData.timesPlayed++;

            var position = normalizedTime - timesPlayed;
            if (currentAnimationData.clip.wrapMode == WrapMode.Loop)
            {
                //SetCurrentClipPosition(position);
                currentAnimationData.secondsPlayed = normalizedTime * currentAnimationData.length;
            }

            if (currentAnimationData.clip.wrapMode == WrapMode.PingPong)
            {
                SetSpeed(currentAnimationData.clipName, -currentAnimationData.speed);
                SetCurrentClipPosition(position);
            }

        }
        else if (currentAnimationData.timesPlayed == 0)
        {
            if (MecanimControl.OnAnimationEnd != null) MecanimControl.OnAnimationEnd(currentAnimationData);
            currentAnimationData.timesPlayed = 1;

            if (currentAnimationData.clip.wrapMode == WrapMode.Once && alwaysPlay)
            {
                Play(defaultAnimation, currentMirror);
            }
            else if (!alwaysPlay)
            {
                animator.speed = 0;
            }
        }
    }
    else
    {
        //currentAnimationData.secondsPlayed += Time.deltaTime * animator.speed * Time.timeScale;
        currentAnimationData.secondsPlayed += (Time.fixedDeltaTime * animator.speed);
        //if (currentAnimationData.secondsPlayed > currentAnimationData.length) 
        //	currentAnimationData.secondsPlayed = currentAnimationData.length;
    }

    */
    //}

    //void OnGUI()
    //{
    //    //Toggle debug mode to see the live data in action
    //    if (debugMode)
    //    {
    //        GUI.Box(new Rect(Screen.width - 340, 40, 340, 400), "Animation Data");
    //        GUI.BeginGroup(new Rect(Screen.width - 330, 60, 400, 400));
    //        {

    //            AnimatorClipInfo[] animationInfoArray = animator.GetCurrentAnimatorClipInfo(0);
    //            foreach (AnimatorClipInfo animationInfo in animationInfoArray)
    //            {
    //                AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
    //                GUILayout.Label(animationInfo.clip.name);
    //                GUILayout.Label("-Wrap Mode: " + animationInfo.clip.wrapMode);
    //                GUILayout.Label("-Is Playing: " + IsPlaying(animationInfo.clip.name));
    //                GUILayout.Label("-Blend Weight: " + animationInfo.weight);
    //                GUILayout.Label("-Normalized Time: " + animatorStateInfo.normalizedTime);
    //                GUILayout.Label("-Length: " + animationInfo.clip.length);
    //                GUILayout.Label("----");
    //            }

    //            GUILayout.Label("--Current Animation Data--");
    //            GUILayout.Label("-Current Clip Name: " + currentAnimationData.clipName);
    //            GUILayout.Label("-Current Speed: " + GetSpeed().ToString());
    //            GUILayout.Label("-Times Played: " + currentAnimationData.timesPlayed);
    //            GUILayout.Label("-Seconds Played: " + currentAnimationData.secondsPlayed);
    //            GUILayout.Label("-Emul. Normalized: " + (currentAnimationData.secondsPlayed / currentAnimationData.length));
    //            GUILayout.Label("-Lengh: " + currentAnimationData.length);

    //            GUILayout.Label("-GetNormalizedTime(): " + GetNormalizedTime());
    //        }
    //        GUI.EndGroup();
    //    }
    //}

    public void ClearClips()
    {
        foreach (var name in animations.Keys)
        {
            Game.AnimationClipPool.RemoveClipRef(name);
        }
        animations.Clear();
        defaultAnimationData.Reset();
    }

    public void Copy(MecanimControl original)
    {
        ClearClips();
        foreach (var pair in original.animations)
        {
            var data = pair.Value;
            AddClip(data.clip, data.clipName, data.speed, data.wrapMode);
        }
        Init();
    }

    public void AnimatorPlay(string statename, int layer, float normalizedTime)
    {
        //Util.LogColor("green",Time.time+ modelName + "AnimatorPlay:" + statename+ "normalizedTime:"+ normalizedTime);
        animator.Play(statename, layer, normalizedTime);
    }


    // MECANIM CONTROL METHODS
    public void RemoveClip(string name)
    {
        if (animations.ContainsKey(name))
        {
            animations.Remove(name);
            Game.AnimationClipPool.RemoveClipRef(name);
        }
        if (name == defaultAnimationData.clipName)
        {
            defaultAnimationData.Reset();
        }
    }



    public void SetDefaultClip(AnimationClip clip, string name, float speed, WrapMode wrapMode)
    {
        defaultAnimationData.clip = clip;// (AnimationClip) Instantiate(clip);
        defaultAnimationData.clip.wrapMode = wrapMode;
        defaultAnimationData.clipName = name;
        defaultAnimationData.speed = speed;
        defaultAnimationData.transitionDuration = -1;
        defaultAnimationData.wrapMode = wrapMode;
    }

    public void AddClip(AnimationClip clip, string newName)
    {
        AddClip(clip, newName, 1, defaultWrapMode);
    }

    public void AddClip(AnimationClip clip, string newName, float speed, WrapMode wrapMode)
    {
        ////Util.LogColor("green", "AddClip:" + newName+ wrapMode);
        if (GetAnimationData(newName) != null)
        {
            Debug.LogError("An animation with the name '" + newName + "' already exists.");
            return;
        }
        AnimationData animData = new AnimationData();
        animData.clip = clip; //Instantiate(clip);
        if (wrapMode == WrapMode.Default) wrapMode = defaultWrapMode;
        animData.clip.wrapMode = wrapMode;
        animData.clip.name = newName;
        animData.clipName = newName;
        animData.speed = speed;
        animData.length = clip.length;
        animData.wrapMode = wrapMode;
        animations[newName] = animData;
        Game.AnimationClipPool.AddClipRef(newName);
    }

    public void SetOverrideController(AnimationData currentAnimationData, AnimationData targetAnimationData, bool mirror)
    {
        //Util.LogColor("cyan", string.Format("SetOverrideController currentAnimationData:{0} targetAnimationData:{1}", currentAnimationData.clipName, targetAnimationData.clipName));
        if (targetAnimationData == null || targetAnimationData.clip == null || currentAnimationData == null || currentAnimationData.clip == null)
        {
            return;
        }
        this.currentMirror = mirror;

        AnimatorOverrideController overrideController = cachedController;
        overrideController["State1"] = currentAnimationData.clip;
        overrideController["State2"] = targetAnimationData.clip;
        if (defaultAnimationData != null && defaultAnimationData.clip != null)
        {
            overrideController["State3"] = defaultAnimationData.clip;
        }
        cachedController = animator.runtimeAnimatorController as AnimatorOverrideController;
        animator.runtimeAnimatorController = overrideController;
        animator.Update(0);
    }

    private void _playAnimation(AnimationData targetAnimationData, float blendingTime, float normalizedTime, bool mirror)
    {
        //The overrite machine. Creates an overrideController, replace its core animations and restate it back in
        ////("cyan", "_playAnimation:"+targetAnimationData.clipName);
        if (targetAnimationData == null || targetAnimationData.clip == null)
            return;
        if (currentAnimationData == null)
        {
            Debug.LogError("_playAnimation :currentAnimationData == null!");
            return;
        }

        if (currentAnimationData.clipName != targetAnimationData.clipName)
        {
            SetOverrideController(currentAnimationData, targetAnimationData, mirror);
        }

        float newAnimatorSpeed = Mathf.Abs(targetAnimationData.speed) * speed;
        float currentNormalizedTime = GetNormalizedTime();

        if (blendingTime == -1) blendingTime = currentAnimationData.transitionDuration;
        if (blendingTime == -1) blendingTime = defaultTransitionDuration;
        currentBlendingTime = blendingTime;

        if (blendingTime <= 0 || currentAnimationData.clip == targetAnimationData.clip)
        {
            //Util.LogColor("red", modelName + "play:" + targetAnimationData.clipName);
            //animator.Rebind();
            AnimatorPlay("State2", 0, normalizedTime);

        }
        else
        {
            //Util.LogColor("cyan", modelName + "crossfade:" + targetAnimationData.clipName+ "blendingTime:"+ blendingTime);
            currentAnimationData.stateName = "State1";
            SetCurrentClipPosition(currentNormalizedTime);
            animator.Update(0);
            animator.CrossFade("State2", blendingTime / newAnimatorSpeed);

        }


        targetAnimationData.timesPlayed = 0;
        targetAnimationData.secondsPlayed = (normalizedTime * targetAnimationData.clip.length) / newAnimatorSpeed;
        targetAnimationData.length = targetAnimationData.clip.length;

        if (overrideRootMotion) animator.applyRootMotion = targetAnimationData.applyRootMotion;
        SetSpeed(targetAnimationData.speed);

        currentAnimationData = targetAnimationData;
        currentAnimationData.stateName = "State2";
        //currentAnimationData.stateHash = state2Hash;

        if (MecanimControl.OnAnimationBegin != null) MecanimControl.OnAnimationBegin(currentAnimationData);
    }


    #region Operate Current Clip


    public string GetCurrentClipName()
    {
        return currentAnimationData.clipName;
    }

    public AnimationData GetCurrentAnimationData()
    {
        return currentAnimationData;
    }

    public int GetCurrentClipPlayCount()
    {
        return currentAnimationData.timesPlayed;
    }

    public float GetCurrentClipTime()
    {
        return currentAnimationData.secondsPlayed;
    }

    public float GetCurrentClipLength()
    {
        return currentAnimationData.length;
    }

    public void SetCurrentClipPosition(float normalizedTime)
    {
        SetCurrentClipPosition(normalizedTime, false);
    }

    public void SetCurrentClipPosition(float normalizedTime, bool pause)
    {
        AnimatorPlay(currentAnimationData.stateName, 0, normalizedTime);
        currentAnimationData.secondsPlayed = normalizedTime * currentAnimationData.length;
        if (pause) Pause();
    }

    public float GetCurrentClipPosition()
    {
        if (currentAnimationData == null || currentAnimationData.length == 0)
        {
            return 0f;
        }
        return currentAnimationData.secondsPlayed / currentAnimationData.length;
    }

    public float GetNormalizedTime()
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return animatorStateInfo.normalizedTime;
    }


    public void Play()
    {
        animator.speed = Mathf.Abs(currentAnimationData.speed) * globalSpeed;
    }

    public void Stop()
    {
        Play(defaultAnimationData.clipName, defaultTransitionDuration, 0, currentMirror);
    }

    public void Pause()
    {
        animator.speed = 0;
    }

    public void SetSpeed(float speed)
    {
        animator.speed = Mathf.Abs(speed) * globalSpeed;
    }

    public float GetSpeed()
    {
        return animator.speed;
    }


    public void RestoreSpeed()
    {
        //SetCurrentClipPosition(GetCurrentClipPosition());
        SetSpeed(currentAnimationData.speed);
    }

    public void Rewind()
    {
        SetSpeed(-currentAnimationData.speed);
    }

    public void SetWrapMode(WrapMode wrapMode)
    {
        defaultWrapMode = wrapMode;
    }


    public bool GetMirror()
    {
        return currentMirror;
    }

    public void SetMirror(bool toggle)
    {
        SetMirror(toggle, 0, false);
    }

    public void SetMirror(bool toggle, float blendingTime)
    {
        SetMirror(toggle, blendingTime, false);
    }

    public void SetMirror(bool toggle, float blendingTime, bool forceMirror)
    {
        if (currentMirror == toggle && !forceMirror) return;

        if (blendingTime == 0) blendingTime = defaultTransitionDuration;
        Play(currentAnimationData, blendingTime, GetCurrentClipPosition(), toggle);
    }

    #endregion



    #region Operate By AnimationData

    public AnimationData GetAnimationData(string clipName)
    {
        if (animations.ContainsKey(clipName))
        {
            return animations[clipName];
        }
        if (clipName == defaultAnimationData.clipName) return defaultAnimationData;
        return null;
    }

    public void CrossFade(AnimationData animationData, float blendingTime, float normalizedTime, bool mirror)
    {
        Play(animationData, blendingTime, normalizedTime, mirror);
    }

    public void Play(AnimationData animationData, bool mirror)
    {
        Play(animationData, animationData.transitionDuration, 0, mirror);
    }

    public void Play(AnimationData animationData)
    {
        Play(animationData, animationData.transitionDuration, 0, currentMirror);
    }

    public void Play(AnimationData animationData, float blendingTime, float normalizedTime, bool mirror)
    {
        _playAnimation(animationData, blendingTime, normalizedTime, mirror);
    }

    public bool IsPlaying(AnimationData animData)
    {
        return IsPlaying(animData, 0);
    }

    public bool IsPlaying(AnimationData animData, float weight)
    {
        if (animData == null) return false;
        if (currentAnimationData == null) return false;
        //if (currentAnimationData == animData && animData.wrapMode == WrapMode.Once && animData.timesPlayed > 0) return false;
        //if (currentAnimationData == animData) return true;
        //AnimatorClipInfo[] animationInfoArray = animator.GetCurrentAnimatorClipInfo(0);
        //for (var i = 0;i<animationInfoArray.Length;i++)
        //{
        //    AnimatorClipInfo animationInfo = animationInfoArray[i];
        //    if (animData.clip == animationInfo.clip && animationInfo.weight >= weight) return true;
        //}
        if (currentAnimationData == animData)
        {
            return animData.wrapMode == WrapMode.Loop || GetNormalizedTime() < 1;
        }
        return false;
    }

    public bool IsPlayingSkill(AnimationData animData, float loop)
    {
        return IsPlaying(animData) && GetNormalizedTime() < loop;
    }

    public void SetWrapMode(AnimationData animationData, WrapMode wrapMode)
    {
        animationData.wrapMode = wrapMode;
        animationData.clip.wrapMode = wrapMode;
    }

    #endregion


    #region Operate By String
    public void CrossFade(string clipName, float blendingTime)
    {
        Play(clipName, blendingTime, 0, currentMirror);
    }

    public void CrossFade(string clipName, float blendingTime, float normalizedTime, bool mirror)
    {
        Play(clipName, blendingTime, normalizedTime, mirror);
    }

    public void Play(string clipName, bool mirror)
    {
        Play(clipName, 0, 0, mirror);
    }

    public void Play(string clipName)
    {
        Play(clipName, 0, 0, currentMirror);
    }


    public void Play(string clipName, float blendingTime, float normalizedTime, bool mirror)
    {
        var animData = GetAnimationData(clipName);
        if (animData == null)
        {
            Debug.LogError(string.Format("model【{0}】 does not have animation state {1},please check!", modelName, clipName));
            return;
        }
        else if (animData.clip == null)
        {
            Debug.LogError(string.Format("model【{0}】 animation clip【{1}】 is null ,please check!", modelName, clipName));
            return;
        }
        Play(animData, blendingTime, normalizedTime, mirror);
    }






    public bool IsPlaying(string clipName)
    {
        return IsPlaying(GetAnimationData(clipName));
    }

    public bool IsPlaying(string clipName, float weight)
    {
        return IsPlaying(GetAnimationData(clipName), weight);
    }

    public bool IsPlayingSkill(string clipName, float loop)
    {
        return IsPlayingSkill(GetAnimationData(clipName), loop);
    }
    public void SetSpeed(string clipName, float speed)
    {
        AnimationData animData = GetAnimationData(clipName);
        //  if (animData.speed == speed && animator.speed == Mathf.Abs(speed)) return;
        animData.speed = speed;
        if (IsPlaying(clipName))
        {
            SetSpeed(speed);
        }
    }

    public float GetSpeed(string clipName)
    {
        AnimationData animData = GetAnimationData(clipName);
        return animData.speed;
    }

    public void SetWrapMode(string clipName, WrapMode wrapMode)
    {
        AnimationData animData = GetAnimationData(clipName);
        animData.wrapMode = wrapMode;
        animData.clip.wrapMode = wrapMode;
    }



    #endregion


    /*****************************************************************************************
    *** 下面为扩展
    *****************************************************************************************/

    #region Operate By HashCode

    //public bool IsPlaying(int hashclip)
    //{
    //    return IsPlaying(LuaHelper.hash_name_map[hashclip]);
    //}

    public void CrossFade(int hashclip, float blendingTime)
    {
        CrossFade(LuaHelper.hash_name_map[hashclip], blendingTime, 0, currentMirror);
    }

    public void CrossFade(int hashclip, float blendingTime, float normalizedTime, bool mirror)
    {
        CrossFade(LuaHelper.hash_name_map[hashclip], blendingTime, normalizedTime, mirror);
    }


    public void Play(int hashclip, float blendingTime, float normalizedTime, bool mirror)
    {
        Play(LuaHelper.hash_name_map[hashclip], blendingTime, normalizedTime, mirror);
    }

    public void Play(int hashclip, bool mirror)
    {
        Play(LuaHelper.hash_name_map[hashclip], 0, 0, mirror);
    }


    public void Play(int hashclip)
    {
        Play(LuaHelper.hash_name_map[hashclip], 0, 0, currentMirror);
    }

    public bool IsPlaying(int hashclip)
    {
        return IsPlaying(LuaHelper.hash_name_map[hashclip]);
    }


    public bool IsPlaying(int hashclip, float weight)
    {
        return IsPlaying(LuaHelper.hash_name_map[hashclip], weight);
    }

    public bool IsPlayingSkill(int hashclip, float loop)
    {
        return IsPlayingSkill(LuaHelper.hash_name_map[hashclip], loop);
    }

    public void SetSpeed(int hashclip, float speed)
    {
        SetSpeed(LuaHelper.hash_name_map[hashclip], speed);
    }

    public float GetSpeed(int hashclip)
    {
        return GetSpeed(LuaHelper.hash_name_map[hashclip]);
    }

    public void SetWrapMode(int hashclip, WrapMode wrapMode)
    {
        SetWrapMode(LuaHelper.hash_name_map[hashclip], wrapMode);
    }

    #endregion

    #region Extend For Editor
    public float GetAnimationLength(string name)
    {
        var animData = GetAnimationData(name);
        if (animData != null)
        {
            return animData.length;
        }
        return 0;
    }
    public void Bake(string targetAnimation)
    {
        AnimationData targetAnimationData = GetAnimationData(targetAnimation);

        //The overrite machine. Creates an overrideController, replace its core animations and restate it back in
        if (targetAnimationData == null || targetAnimationData.clip == null) return;

        currentAnimationData = targetAnimationData;
        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        float newAnimatorSpeed = Mathf.Abs(targetAnimationData.speed);
        overrideController.runtimeAnimatorController = controller1;
        overrideController["State1"] = targetAnimationData.clip;
        animator.runtimeAnimatorController = overrideController;
        animator.Bake("State1");
    }

    public void Sample(float time)
    {
        animator.Sample(time);
    }

    #endregion


}

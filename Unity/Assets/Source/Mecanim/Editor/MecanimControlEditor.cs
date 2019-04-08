using UnityEngine;
using UnityEditor;
using Sirenix;

[CustomEditor(typeof(MecanimControl))]
public class MecanimControlEditor : Editor
{
    bool mEndHorizontal = false;
    public bool DrawHeader(string text, bool forceOn)
    {
        string key = text;
        bool minimalistic = !forceOn;
        bool state = EditorPrefs.GetBool(key, true);

        if (!minimalistic) GUILayout.Space(3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        GUI.changed = false;

        if (minimalistic)
        {
            if (state) text = "\u25BC" + (char)0x200a + text;
            else text = "\u25BA" + (char)0x200a + text;

            GUILayout.BeginHorizontal();
            GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
            if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        else
        {
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
        }

        if (GUI.changed) EditorPrefs.SetBool(key, state);

        if (!minimalistic) GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }
    public void BeginContents(bool minimalistic)
    {
        if (!minimalistic)
        {
            mEndHorizontal = true;
            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginHorizontal(EditorStyles.textArea, GUILayout.MinHeight(10f));
        }
        else
        {
            mEndHorizontal = false;
            EditorGUILayout.BeginHorizontal(EditorStyles.textArea, GUILayout.MinHeight(10f));
            GUILayout.Space(10f);
        }
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
    }
    public void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (mEndHorizontal)
        {
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(3f);
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 200f;
        MecanimControl mc = target as MecanimControl;
        GUILayout.Space(6f);

        EditorGUILayout.LabelField("Model Name", mc.modelName);
        mc.debugMode = EditorGUILayout.Toggle("Debug Mode", mc.debugMode);
        mc.alwaysPlay = EditorGUILayout.Toggle("Always Play", mc.alwaysPlay);
        mc.playOnStart = EditorGUILayout.Toggle("Play On Start", mc.playOnStart);
        mc.overrideRootMotion = EditorGUILayout.Toggle("Override Root Motion", mc.overrideRootMotion);
        mc.defaultTransitionDuration = EditorGUILayout.FloatField("Default Transition Duration", mc.defaultTransitionDuration);
        mc.defaultWrapMode = (WrapMode)EditorGUILayout.EnumPopup("Default Wrap Mode", mc.defaultWrapMode);


        if (DrawHeader("Default Animation", true))
        {
            BeginContents(false);
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Clip", GUILayout.MinWidth(150), GUILayout.Height(50));
            EditorGUILayout.LabelField("Name", GUILayout.MinWidth(150), GUILayout.Height(50));
            EditorGUILayout.LabelField("Speed", GUILayout.Width(50), GUILayout.Height(50));
            EditorGUILayout.LabelField("Transition\nDuration", GUILayout.Width(60), GUILayout.Height(50));
            EditorGUILayout.LabelField("Wrap\nMode", GUILayout.Width(50), GUILayout.Height(50));
            EditorGUILayout.LabelField("Apply\nRoot\nMotion", GUILayout.Width(50), GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            var clip = mc.defaultAnimationData.clip;
            clip = EditorGUILayout.ObjectField(clip, typeof(AnimationClip), false, GUILayout.MinWidth(150)) as AnimationClip;
            mc.defaultAnimationData.clipName = EditorGUILayout.TextField(mc.defaultAnimationData.clipName, GUILayout.MinWidth(150));
            mc.defaultAnimationData.speed = EditorGUILayout.FloatField(mc.defaultAnimationData.speed, GUILayout.Width(50));
            mc.defaultAnimationData.transitionDuration = EditorGUILayout.FloatField(mc.defaultAnimationData.transitionDuration, GUILayout.Width(50));
            mc.defaultAnimationData.wrapMode = (WrapMode)EditorGUILayout.EnumPopup(mc.defaultAnimationData.wrapMode, GUILayout.Width(50));
            mc.defaultAnimationData.applyRootMotion = EditorGUILayout.Toggle(mc.defaultAnimationData.applyRootMotion, GUILayout.Width(50));
            if (clip != mc.defaultAnimationData.clip)
            {
                mc.defaultAnimationData.clip = clip;
                mc.defaultAnimationData.clipName = clip.name;
                mc.defaultAnimationData.speed = 1;
                mc.defaultAnimationData.transitionDuration = -1;
                mc.defaultAnimationData.wrapMode = clip.wrapMode;
                mc.defaultAnimationData.applyRootMotion = false;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EndContents();
        }

        if (DrawHeader("Animations", true))
        {
            BeginContents(false);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Clip", GUILayout.MinWidth(150), GUILayout.Height(50));
            EditorGUILayout.LabelField("Name", GUILayout.MinWidth(150), GUILayout.Height(50));
            EditorGUILayout.LabelField("Speed", GUILayout.Width(50), GUILayout.Height(50));
            EditorGUILayout.LabelField("Transition\nDuration", GUILayout.Width(60), GUILayout.Height(50));
            EditorGUILayout.LabelField("Wrap\nMode", GUILayout.Width(50), GUILayout.Height(50));
            EditorGUILayout.LabelField("Apply\nRoot\nMotion", GUILayout.Width(50), GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            foreach (var pairs in mc.animations)
            {
                var animData = pairs.Value;
                EditorGUILayout.BeginHorizontal();
                var clip = animData.clip;
                clip = EditorGUILayout.ObjectField(clip, typeof(AnimationClip), false, GUILayout.MinWidth(150)) as AnimationClip;
                animData.clipName = EditorGUILayout.TextField(animData.clipName, GUILayout.MinWidth(150));
                animData.speed = EditorGUILayout.FloatField(animData.speed, GUILayout.Width(50));
                animData.transitionDuration = EditorGUILayout.FloatField(animData.transitionDuration, GUILayout.Width(50));
                animData.wrapMode = (WrapMode)EditorGUILayout.EnumPopup(animData.wrapMode, GUILayout.Width(50));
                animData.applyRootMotion = EditorGUILayout.Toggle(animData.applyRootMotion, GUILayout.Width(50));

                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();

            EndContents();
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Timers;
using System.Diagnostics;
using UDebug = UnityEngine.Debug;
using UnityEditor;

public class XProfile 
{
    //[MenuItem("Assets/LoadScript")]
    //static void LoadScript()
    //{
    //    string path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
    //    var mono = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
    //    UDebug.LogError(mono.GetClass());
    //    UDebug.LogError(mono);
    //}

    static Stopwatch stopwatch = new Stopwatch();
    public static void Begin(string name)
    {
        stopwatch.Start();

        Profiler.BeginSample(name);
    }

    public static void End(bool isReset = false)
    {
        stopwatch.Stop();
        UDebug.LogError(stopwatch.ElapsedMilliseconds);
        if (isReset)
            stopwatch.Reset();
        Profiler.EndSample();
    }

}

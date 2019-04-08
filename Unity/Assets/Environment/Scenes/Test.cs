using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.IO;

public class Test : MonoBehaviour
{
    public static void LoadAtlas()
    {
        Debug.Log("-------------开始测试-------------");
        string atlasPath = string.Format("{0}/Interface/", Application.dataPath);
        List<string> fs = new List<string>(Directory.GetFiles(atlasPath, "*_fui.bytes", SearchOption.AllDirectories));
        for (int i = 0; i < fs.Count; i++)
        {
            string package = fs[i].Replace(Application.dataPath, "Assets").Replace("_fui.bytes", "");
#if UNITY_EDITOR
            UIPackage.AddPackage(package);
#endif
        }

#if UNITY_EDITOR
        //object a = UIPackage.GetItemAssetByURL("ui://Audio/BtnAudio");
        string p = "Assets/FairyGUI/Examples/Resources/UI/Basics_gojg7u.wav";
        var clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(p);
        UIConfig.buttonSound = new NAudioClip(clip);
#endif
    }



    void Awake()
    {
        LoadAtlas();
    }

    void Start()
    {
//        GButton button = GetComponent<UIPanel>().ui.asButton;
//        Debug.Log(button.baseUserData);

//        var buffer = button.packageItem.rawData;
//        buffer.Seek(0, 6);
//        var mode = (ButtonMode)buffer.ReadByte();
//        string str = buffer.ReadS();
//        if (str != null)
//        {
//            var sound = UIPackage.GetItemAssetByURL(str) as NAudioClip;
//#if UNITY_EDITOR
//            Debug.Log(UnityEditor.AssetDatabase.GetAssetPath(sound.nativeClip));
//#endif
//        }
    }
}

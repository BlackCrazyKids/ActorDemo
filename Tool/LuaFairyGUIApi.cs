using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LuaFairyGUIApi : LuaApiGen
{
    static string fairyGUIDir = Application.dataPath + "/Interface/GUI";
    static void GenSelectedFairyGUIApi()
    {

    }
    static void GenFairyGUIApi()
    {
        List<Info> infos = new List<Info>();
        string[] fs = Directory.GetFiles(fairyGUIDir);
        for (int i = 0; i < fs.Length; i++)
        {
            string path = EUtil.FilePath2UnityPath(fs[i]);
            infos.AddRange(CreateInfo(path));
        }
        //GenLuaApi("FairyGUIApi",)
    }
    static List<Info> CreateInfo(string path)
    {
        List<Info> infos = new List<Info>();
        var pkg = UIPackage.AddPackage(path);
        var items = pkg.GetItems();
        foreach (var item in items)
        {
            GComponent gcomp = pkg.CreateObject(item.name).asCom;
            CollectUIElements(gcomp, infos);
        }
        return infos;
    }
    static void CollectUIElements(GComponent comp, List<Info> infos)
    {

    }
}

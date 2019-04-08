using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 仅作Texture资源加载
/// </summary>
public class MyLoader : GLoader
{
    protected override void LoadExternal()
    {
        int index = url.LastIndexOf('/') + 1;
        string path = url.Insert(index, "t_") + ".bundle";
        Game.Client.ResMgr.AddTask(path, (tex) =>
        {
            Texture content = tex as Texture;
            if (content != null)
            {
                texture = new NTexture(content);
                onExternalLoadSuccess(texture);
            }
            else
            {
                Debug.LogErrorFormat("[MyLoader]Texture资源无法正常加载,url:{0}", url);
                onExternalLoadFailed();
            }
        });
    }
    protected override void FreeExternal(NTexture texture)
    {
        texture.Dispose();
    }
}

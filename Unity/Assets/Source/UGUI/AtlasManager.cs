using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Game
{
    /// <summary>
    /// 图集管理器
    /// 注:如果图集区常驻内存和不常驻,则需对不常驻内存图集进行销毁处理
    /// </summary>
    public class AtlasManager : Manager
    {
        private Dictionary<string, SpriteAtlas> _uiAtlas = new Dictionary<string, SpriteAtlas>();

        public override void Init()
        {
            SpriteAtlasManager.atlasRequested += RequestAtlas;
        }
        public override void Dispose()
        {
            SpriteAtlasManager.atlasRequested -= RequestAtlas;
        }
        public void GetSpriteAtlas(string name, Action<SpriteAtlas> callback)
        {
            if (_uiAtlas.ContainsKey(name))
                callback(_uiAtlas[name]);
            else
                Client.ResMgr.AddTask(string.Format("ui/atlas/{0}.bundle", name), (obj) =>
                {
                    SpriteAtlas atlas = (SpriteAtlas)obj;
                    if (atlas == null)
                        Debug.LogErrorFormat("{0}图集资源无法正常加载!", name);
                    else
                        callback(atlas);
                }, (int)ResourceLoadType.LoadBundleFromFile);
        }
        void RequestAtlas(string name, Action<SpriteAtlas> callback)
        {
            if (_uiAtlas.ContainsKey(name))
                callback(_uiAtlas[name]);
            else
            {
                Client.ResMgr.AddTask(string.Format("ui/atlas/{0}.bundle", name), (obj) =>
                {
                    SpriteAtlas sa = (SpriteAtlas)obj;
                    if (sa != null)
                        callback(sa);
                    else
                        Debug.LogErrorFormat("{0}图集资源无法正常加载!", name);
                }, (int)ResourceLoadType.LoadBundleFromFile);
            }
        }
    }
}
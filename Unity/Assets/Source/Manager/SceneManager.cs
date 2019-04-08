using System;
using UnityEngine;
using System.Collections;

namespace Game
{
    public class SceneManager : IManager
    {
        public string SceneName { get { return _sceneName; } }
        public AsyncOperation AsyncOpt { get { return _asyncOpt; } }



        string _sceneName;
        AssetBundle _sceneBundle;
        AsyncOperation _asyncOpt;
        Action<bool> _onSceneLoadFinish;

        //---3D场景地形管理
        //TODO
  

        public void Init() { _asyncOpt = null; }
        public void Release()
        {
            System.GC.Collect();
            Resources.UnloadUnusedAssets();
        }
        public void Dispose()
        {
            _sceneBundle = null;
            _asyncOpt = null;
            _onSceneLoadFinish = null;

            Release();
        }

        /// <summary>
        /// 注册场景加载完成事件
        /// </summary>
        public void RegisteOnSceneLoadFinish(Action<bool> action)
        {
            _onSceneLoadFinish = (bool result) =>
            {
                if (_sceneBundle != null)
                {
                    _sceneBundle.Unload(false);
                }
                if (action != null)
                    action(result);                  
            };
        }
        /// <summary>
        /// 变更场景
        /// </summary>
        public void ChangeMap(string sceneName)
        {
            _sceneName = sceneName;

            string sceneFile = string.Format("scene/s_{0}.bundle", sceneName);
            Client.ResMgr.AddTask(sceneFile, LoadFinishedEventHandler,
                    (int)(ResourceLoadType.LoadBundleFromFile) | (int)(ResourceLoadType.ReturnAssetBundle));
            Release();
        }

        private void LoadFinishedEventHandler(UnityEngine.Object obj)
        {

            _sceneBundle = obj as AssetBundle;
            if (_sceneBundle != null)
            {
                Release();
                Client.Ins.StartCoroutine(LoadAsync());
            }
            else
            {
                if (_onSceneLoadFinish != null)
                    _onSceneLoadFinish(false);
            }
        }
        private IEnumerator LoadAsync()
        {
            _asyncOpt = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName);
            _asyncOpt.allowSceneActivation = true;
            yield return _asyncOpt;

            if (_asyncOpt.isDone && _onSceneLoadFinish != null)
                _onSceneLoadFinish(true);
        }
    }
}

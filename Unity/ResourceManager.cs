using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using Object = UnityEngine.Object;

public class ManagedResource : MonoBehaviour
{
    public string bundlename;
    public void OnDestroy()
    {
        Game.Manager.ResMgr.RemoveRefCount(bundlename);
    }
}

namespace Game
{
    public enum ResourceLoadType
    {
        Default = 0,                          // 资源已加载,直接取资源
        Persistent = 1 << 0,                     // 永驻内存的资源
        Cache = 1 << 1,                     // Asset需要缓存

        UnLoad = 1 << 4,                     // 利用www加载并且处理后是否立即unload ab,如不卸载,则在指定时间后清理

        // 加载方式
        LoadBundleFromFile = 1 << 6,                     // 利用AssetBundle.LoadFromFile加载
        LoadBundleFromWWW = 1 << 7,                     // 利用WWW 异步加载 AssetBundle
        ReturnAssetBundle = 1 << 8,                     // 返回scene AssetBundle,默认unload ab
    }
    class CacheObject
    {
        public Object obj;
        public float lastUseTime;
    }
    /// <summary>
    /// 资源指AB包资源
    /// </summary>
    class AssetLoadTask
    {
        public uint id;
        public List<uint> parentTaskIds;
        public int loadType;
        public string path;
        public Action<Object> actions;
        public List<string> dependencies;
        public int loadedDependenciesCount = 0;
        public void Reset()
        {
            id = 0;
            parentTaskIds = null;
            path = string.Empty;
            actions = null;
            dependencies = null;
            loadedDependenciesCount = 0;
        }
    }
    /// <summary>
    /// AssetBundle加载完毕,后续处理
    /// </summary>
    class CompleteTask
    {
        public AssetLoadTask task;
        public AssetBundle bundle;
    }

    public class ResourceManager : IManager
    {
        public ResourceManager() { }

        //资源存储策略
        private Dictionary<string, Object> _persistantObjects = new Dictionary<string, Object>();       //key-依赖路径
        private Dictionary<string, CacheObject> _cacheObjects = new Dictionary<string, CacheObject>();

        //加载任务
        private Dictionary<string, AssetLoadTask> _loadingFiles = new Dictionary<string, AssetLoadTask>();//key-文件名
        private Dictionary<uint, AssetLoadTask> _loadingTasks = new Dictionary<uint, AssetLoadTask>();  //key-任务ID
        private SimplePool<AssetLoadTask> _assetLoadTaskPool = new SimplePool<AssetLoadTask>();        //加载资源的相关信息
        private Queue<AssetLoadTask> _delayAssetLoadTask = new Queue<AssetLoadTask>();
        private SimplePool<CompleteTask> _loadTaskPool = new SimplePool<CompleteTask>();
        private Queue<CompleteTask> _delayLoadTask = new Queue<CompleteTask>();

        //所有资源的依赖信息,由OnlyRead目录和ReadWrite目录组合
        private Dictionary<string, string[]> _assetDependencies = new Dictionary<string, string[]>();

        private bool _canStartCleanupMemeory = true;
        private const float _cleanupMemInterval = 180;
        private const float _cleanupCacheInterval = 120;
        private const float _cleanupBundleInterval = 30;

        private string _preloadListPath = "config/preloadlist.txt";
        private List<string> _preloadList = new List<string>();
        private int _currentPreLoadCount = 0;

        //加载的AB包信息
        private Dictionary<string, AssetBundle> _assetBundlesObj = new Dictionary<string, AssetBundle>();  //key-依赖路径
        private Dictionary<string, int> _refCount = new Dictionary<string, int>();
        private Dictionary<string, float> _refDelTime = new Dictionary<string, float>();

        private int _currentTaskCount = 0;
        private const int _defaultMaxTaskCount = 10;

        private float _cleanupMemoryLastTime;
        private float _cleanupCacheObjectLastTime;
        private float _cleanupBundlesLastTime;

        private static uint _nextTaskId;

        public int MaxTaskCount { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            int memorySize = 1024 * 1024; //内存大小1GB
            if (memorySize <= 1024 * 1024)
            {
                MaxTaskCount = 5;
            }

            _assetLoadTaskPool.AutoCreate = () => new AssetLoadTask();
            _loadTaskPool.AutoCreate = () => new CompleteTask();

            byte[] stream = null;
            string path = "";
#if UNITY_EDITOR || UNITY_EDITOR_WIN
            path = GetResPath("GamePlayer");
#elif UNITY_ANDROID
            path = GetResPath("Android");
#elif UNITY_IOS
            path = GetResPath("IOS");
#endif
            if (!File.Exists(path)) return;
            stream = File.ReadAllBytes(path);
            var assetbundle = AssetBundle.LoadFromMemory(stream);
            AssetBundleManifest manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] assets = manifest.GetAllAssetBundles();
            for (int i = 0; i < assets.Length; i++)
            {
                string[] dependencies = manifest.GetAllDependencies(assets[i]);
                _assetDependencies.Add(assets[i], dependencies);
            }

            PreLoadResource();
        }

        /*
            UI部分,分成两部分.
            1.des界面预制描述内容
            2.res界面美术资源图集
            作为图集资源,des和res合并打成一个包
            作为UI界面,则只打des包文件
        */
        private void PreLoadResource()
        {
            _preloadList.Clear();
            string dataPath = GetResPath(_preloadListPath);

            if (!File.Exists(dataPath)) return;

            foreach (string p in File.ReadAllLines(dataPath))
            {
                if (!string.IsNullOrEmpty(p))
                    _preloadList.Add(p);
            }

            foreach (string path in _preloadList)
            {
                //if (path.Contains("_atlas0!a") && Application.platform != UnityEngine.RuntimePlatform.Android)
                //    currentPreLoadCount++;
                //else
                AddTask(path, PreLoadEventHandler, (int)(ResourceLoadType.LoadBundleFromFile
                    | ResourceLoadType.Persistent | ResourceLoadType.ReturnAssetBundle));
            }
        }
        private void PreLoadEventHandler(UnityEngine.Object obj)
        {
            _currentPreLoadCount++;
            if (!IsPreLoadDone)
            {
                AssetBundle ab = obj as AssetBundle;
                var pkg = UIPackage.AddPackage(ab);
                pkg.LoadAllAssets();
                Main.Instance.StartCoroutine(AsyncUnload(ab));
            }
        }
        /// <summary>
        /// 判断预加载资源是否已经加载结束
        /// </summary>
        public bool IsPreLoadDone { get { return _currentPreLoadCount >= _preloadList.Count; } }
        IEnumerator AsyncUnload(AssetBundle ab)
        {
            yield return new WaitForEndOfFrame();
            ab.Unload(false);
        }


        public bool IsLoading(uint taskId)
        {
            return _loadingTasks.ContainsKey(taskId);
        }
        public void RemoveTask(uint taskId, Action<Object> action)
        {
            if (IsLoading(taskId))
            {
                AssetLoadTask oldTask = null;
                if (_loadingTasks.TryGetValue(taskId, out oldTask))
                {
                    if (null != action)
                    {
                        oldTask.actions -= action;
                    }
                }
            }
        }

        public void AddRefCount(string bundlename)
        {
            string[] dependencies = _assetDependencies[bundlename];
            if (dependencies != null && dependencies.Length > 0)
            {
                for (int i = 0; i < dependencies.Length; i++)
                {
                    string depname = dependencies[i];
                    if (!_persistantObjects.ContainsKey(depname))
                    {
                        if (!_refCount.ContainsKey(depname))
                        {
                            _refCount[depname] = 0;
                        }
                        _refCount[depname]++;
                    }
                }
            }
        }
        public void RemoveRefCount(string bundlename)
        {
            string[] dependencies = _assetDependencies[bundlename];
            if (dependencies != null && dependencies.Length > 0)
            {
                for (int i = 0; i < dependencies.Length; i++)
                {
                    string depname = dependencies[i];
                    if (_refCount.ContainsKey(depname))
                    {
                        _refCount[depname]--;
                        if (_refCount[depname] <= 0)
                        {
                            _refDelTime[depname] = Time.realtimeSinceStartup;
                        }
                    }
                }
            }
        }

        public uint AddTask(string file, LuaFunction action, int loadType = (int)ResourceLoadType.LoadBundleFromFile)
        {
            return AddTask(file, o => action.Call(o), loadType, 0);
        }
        public uint AddTask(string file, Action<Object> action, int loadType = (int)ResourceLoadType.LoadBundleFromFile)
        {
            return AddTask(file, action, loadType, 0);
        }
        private uint AddTask(string file, Action<Object> action, int loadType, uint parentTaskId)
        {
            if (string.IsNullOrEmpty(file))
            {
                return 0;
            }
            string fileReplace = file.Replace(@"\", @"/");

            string lowerFile = fileReplace.ToLower();
            Object obj;
            if (_persistantObjects.TryGetValue(lowerFile, out obj))
            {
                action(obj);
                return 0;
            }

            CacheObject cacheObject;
            if (_cacheObjects.TryGetValue(lowerFile, out cacheObject))
            {
                cacheObject.lastUseTime = Time.realtimeSinceStartup;

                action(cacheObject.obj);
                return 0;
            }

            AssetLoadTask oldTask;
            if (_loadingFiles.TryGetValue(lowerFile, out oldTask))
            {//资源加载任务-正在进行中
                if (action != null)
                {
                    oldTask.actions += action;
                }

                if ((loadType & (int)ResourceLoadType.Persistent) > 0)
                {
                    oldTask.loadType |= (int)ResourceLoadType.Persistent;
                }

                if (parentTaskId != 0)
                {
                    if (oldTask.parentTaskIds == null)
                    {
                        oldTask.parentTaskIds = new List<uint>();
                        Debug.LogErrorFormat("resource path {0} type is mixed, dependency resource or not ", oldTask.path);
                    }
                    oldTask.parentTaskIds.Add(parentTaskId);
                }

                return 0;
            }

            //加载资源到内存
            uint id = ++_nextTaskId;
            List<uint> ptList = null;
            if (parentTaskId != 0)
            {
                ptList = new List<uint>();
                ptList.Add(parentTaskId);
            }
            string[] deps = _assetDependencies[lowerFile];
            var task = _assetLoadTaskPool.Get();
            {
                task.id = id;
                task.parentTaskIds = ptList;
                task.path = lowerFile;
                task.loadType = loadType;
                task.actions = action;
                task.dependencies = deps == null ? null : new List<string>(deps);
                task.loadedDependenciesCount = 0;
            };
            _loadingFiles[lowerFile] = task;
            _loadingTasks[id] = task;
            if (_assetBundlesObj.ContainsKey(task.path))
            {//依赖资源已被加载
                AddRefCount(task.path);
            }
            //任务数量达最大时,延迟加载资源[需要排队]
            if (_currentTaskCount < MaxTaskCount)
                DoTask(task);
            else
                _delayAssetLoadTask.Enqueue(task);

            return id;
        }

        private void DoTask(AssetLoadTask task)
        {
            if (task.dependencies == null)
            {
                DoImmediateTask(task);
            }
            else
            {
                if (task.loadedDependenciesCount >= task.dependencies.Count)
                {
                    DoImmediateTask(task);
                }
                else
                {
                    int i = task.loadedDependenciesCount;
                    for (; i < task.dependencies.Count; ++i)
                    {
                        if (_assetBundlesObj.ContainsKey(task.dependencies[i]) || _persistantObjects.ContainsKey(task.dependencies[i]))
                        {
                            task.loadedDependenciesCount += 1;
                            if (task.loadedDependenciesCount >= task.dependencies.Count)
                            {
                                DoImmediateTask(task);
                                return;
                            }
                        }
                        else
                        {
                            AddTask(task.dependencies[i], null, task.loadType, task.id);
                        }
                    }
                }
            }
        }
        private void DoImmediateTask(AssetLoadTask task)
        {
            _currentTaskCount += 1;
            if ((task.loadType & (int)ResourceLoadType.LoadBundleFromWWW) != 0)
            {
                Main.Instance.StartCoroutine(LoadBundleFromWWW(task));
            }
            else if ((task.loadType & (int)ResourceLoadType.LoadBundleFromFile) != 0)
            {
                LoadBundleFromFile(task);
            }
            else
            {
                _currentTaskCount -= 1;
                Debug.LogErrorFormat("Unknown task loadtype:{0} path:{1}", task.loadType, task.path);
            }
        }
        private IEnumerator LoadBundleFromWWW(AssetLoadTask task)
        {
            string path = GetResPath(task.path);
            var www = new WWW(path);
            yield return www;
            if (null != www.error)
                Debug.LogErrorFormat("LoadBundleAsync: {0} failed! www error:{1}", task.path, www.error);
            OnBundleLoaded(task, www.assetBundle);
            www.Dispose();
        }
        private void LoadBundleFromFile(AssetLoadTask task)
        {
            string path = GetResPath(task.path);
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            OnBundleLoaded(task, ab);
        }
        //AssetBundle加载完毕
        private void OnBundleLoaded(AssetLoadTask task, AssetBundle ab)
        {
            _currentTaskCount -= 1;
            Object obj = null;
            if (ab == null)
            {
                Debug.LogErrorFormat("LoadBundle: {0} failed! assetBundle == NULL!", task.path);
                OnAseetsLoaded(task, ab, obj);
            }
            else
            {
                var loadTask = _loadTaskPool.Get();
                loadTask.task = task;
                loadTask.bundle = ab;
                _delayLoadTask.Enqueue(loadTask);
            }
        }
        public void Update()
        {
            CleanupCacheObjetc();
            CleanupMemoryInterval();
            CleanupBundlesInterval();
            //执行延迟加载任务
            DoDelayTasks();
        }
        private void DoDelayTasks()
        {
            if (_delayAssetLoadTask.Count > 0)
            {
                while (_delayAssetLoadTask.Count > 0 && _currentTaskCount < MaxTaskCount)
                {
                    AssetLoadTask task = _delayAssetLoadTask.Dequeue();
                    DoTask(task);
                }
            }
            if (_delayLoadTask.Count > 0)
            {
                var maxLoadTime = 0.02f;
                var startTime = Time.realtimeSinceStartup;
                while (_delayLoadTask.Count > 0 && Time.realtimeSinceStartup - startTime < maxLoadTime)
                {
                    CompleteTask loadTask = _delayLoadTask.Dequeue();
                    var task = loadTask.task;
                    var bundle = loadTask.bundle;

                    Object obj = null;
                    if (bundle != null)
                    {
                        if (!bundle.isStreamedSceneAssetBundle)
                        {
                            var objs = bundle.LoadAllAssets();
                            if (objs.Length > 0)
                                obj = objs[0];
                            if (obj == null)
                            {
                                Debug.LogErrorFormat("LoadBundle: {0} ! No Assets in Bundle!", task.path);
                            }
                        }
                    }
                    OnAseetsLoaded(task, bundle, obj);
                    _loadTaskPool.Release(loadTask);
                }
            }
        }
        //Asset加载完毕,可能是依赖资源,也可能是主资源
        private void OnAseetsLoaded(AssetLoadTask task, AssetBundle ab, Object obj)
        {
            string[] dependencies = _assetDependencies[task.path];
            if (dependencies == null || dependencies.Length == 0)
            {
                RemoveRefCount(task.path);
            }

            _loadingFiles.Remove(task.path);
            _loadingTasks.Remove(task.id);

            //主资源加载完毕
            if (task.actions != null && task.parentTaskIds == null)
            {
                Delegate[] delegates = task.actions.GetInvocationList();
                foreach (var dele in delegates)
                {
                    var action = (Action<Object>)dele;
                    try
                    {
                        if ((task.loadType & (int)ResourceLoadType.ReturnAssetBundle) > 0)
                        {
                            action(ab);
                        }
                        else
                        {
                            action(obj);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat("Load Bundle {0} DoAction Exception: {1}", task.path, e);
                    }
                }
            }

            //缓存加载的资源对象[非AB]
            if (ab != null && task.parentTaskIds == null)
            {
                if ((task.loadType & (int)ResourceLoadType.Persistent) > 0)
                {
                    _persistantObjects[task.path] = obj;
                    if ((task.loadType & (int)ResourceLoadType.UnLoad) > 0)
                    {
                        ab.Unload(false);
                    }
                }
                else
                {
                    if ((task.loadType & (int)ResourceLoadType.Cache) > 0)
                    {
                        var cacheObject = new CacheObject
                        {
                            lastUseTime = Time.realtimeSinceStartup,
                            obj = obj
                        };

                        _cacheObjects[task.path] = cacheObject;
                    }
                    if ((task.loadType & (int)ResourceLoadType.ReturnAssetBundle) == 0)
                    {
                        ab.Unload(false);
                    }
                }
            }

            //依赖资源加载完毕
            if (task.parentTaskIds != null)
            {
                _assetBundlesObj[task.path] = ab;
                for (int i = 0; i < task.parentTaskIds.Count; ++i)
                {
                    uint taskid = task.parentTaskIds[i];
                    AssetLoadTask pt = null;
                    if (_loadingTasks.TryGetValue(taskid, out pt))
                    {
                        pt.loadedDependenciesCount += 1;
                        if (pt.loadedDependenciesCount >= pt.dependencies.Count)
                        {
                            DoTask(pt);
                        }
                    }
                }
            }

            task.Reset();
            _assetLoadTaskPool.Release(task);
        }
        public string GetResPath(string relative)
        {
            relative = Util.StandardlizePath(relative);
            string fullPath = Util.DataPath + relative;
            if (!File.Exists(fullPath))
                fullPath = Util.StreamingPath + relative;
            return fullPath;
        }

        //定时清理功能       
        //-内存清理 
        public void CleanupMemoryInterval()
        {
            if (Time.realtimeSinceStartup > _cleanupMemoryLastTime + _cleanupMemInterval)
                CleanupMemoryNow();
        }
        public void CleanupMemoryNow()
        {
            if (_canStartCleanupMemeory)
            {
                _canStartCleanupMemeory = false;
                _cleanupMemoryLastTime = Time.realtimeSinceStartup;
                Main.Instance.StartCoroutine(CleanupMemoryAsync());
            }
        }
        private IEnumerator CleanupMemoryAsync()
        {
            yield return Resources.UnloadUnusedAssets();
            GC.Collect();
            _canStartCleanupMemeory = true;
            _cleanupMemoryLastTime = Time.realtimeSinceStartup;
        }
        //-缓存包清理
        private void CleanupCacheObjetc()
        {
            if (_cacheObjects.Count <= 0) return;
            if (!(Time.realtimeSinceStartup > _cleanupCacheObjectLastTime + 10)) return;

            var now = _cleanupCacheObjectLastTime = Time.realtimeSinceStartup;

            var tempList = new List<string>();
            foreach (var pair in _cacheObjects)
            {
                if (now > pair.Value.lastUseTime + _cleanupCacheInterval)
                {
                    tempList.Add(pair.Key);
                    if (null != pair.Value.obj)
                    {
                        //Debug.LogError(" try to destroy object: " + pair.Key);
                        if (pair.Value.obj is GameObject)
                        {
                            UnityEngine.Object.DestroyImmediate(pair.Value.obj, true);
                        }
                        else
                        {
                            Resources.UnloadAsset(pair.Value.obj);
                        }
                    }
                }
            }
            foreach (var bundle in tempList)
            {
                _cacheObjects.Remove(bundle);
            }
        }
        //-依赖包清理
        public void CleanupBundlesInterval()
        {
            if (Time.realtimeSinceStartup > _cleanupBundlesLastTime + _cleanupBundleInterval)
            {
                CleanupBundlesNow();
            }
        }
        public void CleanupBundlesNow()
        {
            if (_refCount == null || _refDelTime == null || _assetBundlesObj == null)
            {
                return;
            }
            _cleanupBundlesLastTime = Time.realtimeSinceStartup;
            List<string> refCountToRemove = new List<string>();
            foreach (var pairs in _refCount)
            {
                if (pairs.Value <= 0)
                {
                    if (_assetBundlesObj.ContainsKey(pairs.Key) &&
                        _refDelTime.ContainsKey(pairs.Key) &&
                        Time.realtimeSinceStartup - _refDelTime[pairs.Key] > 60)
                    {
                        if (_assetBundlesObj.ContainsKey(pairs.Key))
                        {
                            if (_assetBundlesObj[pairs.Key] != null)
                                _assetBundlesObj[pairs.Key].Unload(false);
                            _assetBundlesObj[pairs.Key] = null;
                            _assetBundlesObj.Remove(pairs.Key);
                        }
                        _refDelTime.Remove(pairs.Key);
                        refCountToRemove.Add(pairs.Key);
                    }
                }
            }
            foreach (var remove in refCountToRemove)
            {
                _refCount.Remove(remove);
            }
        }

        //其他操作        
        public void Dispose()
        {
            foreach (KeyValuePair<string, Object> pair in _persistantObjects)
            {
                if (pair.Value != null)
                {
                    if (pair.Value is GameObject)
                    {
                        UnityEngine.Object.DestroyImmediate(pair.Value, true);
                    }
                    else
                    {
                        Resources.UnloadAsset(pair.Value);
                    }

                }
            }

            _persistantObjects.Clear();

            foreach (var pair in _assetBundlesObj)
            {
                if (pair.Value != null)
                {
                    pair.Value.Unload(true);
                }
            }
            _assetBundlesObj.Clear();

            foreach (var pair in _cacheObjects)
            {
                if (pair.Value != null && pair.Value.obj != null)
                {
                    if (pair.Value.obj is GameObject)
                    {
                        UnityEngine.Object.DestroyImmediate(pair.Value.obj, true);
                    }
                    else
                    {
                        Resources.UnloadAsset(pair.Value.obj);
                    }
                }
            }
            _cacheObjects.Clear();

            LuaWindow.Destroy();
            Debug.Log("~ResourceManager was destroy!");
        }
    }
}

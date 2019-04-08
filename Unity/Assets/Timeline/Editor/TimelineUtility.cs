using System;
using UnityEditor;
using System.Reflection;
using UnityEngine.Timeline;

namespace TimelineEditor
{
    public static class TimelineUtility
    {
        private const string EDITOR_DIR = "Assets/Timeline/";
        private const string RESOURCE_DIR = EDITOR_DIR + "Editor/Resource/";
        private const string DATA_DIR = EDITOR_DIR + "Data/";

        /// <summary>
        /// 编辑器资源加载
        /// </summary>
        /// <param name="name">资源名带扩展</param>
        public static T LoadEditorAsset<T>(string name) where T : UnityEngine.Object
        {
            string path = string.Format("{0}{1}", RESOURCE_DIR, name);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset;

        }
        /// <summary>
        /// 加载数据资源
        /// </summary>
        /// <param name="name">资源名带扩展</param>
        public static T LoadDataAsset<T>(string name) where T : UnityEngine.Object
        {
            string path = string.Format("{0}{1}", DATA_DIR, name);
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset;
        }

        /// <summary>
        /// 直接使用本地TimelineAsset打开TimelineWindow
        /// </summary>
        public static void OpenTimelineEditor(TimelineAsset timeline)
        {
            SetCurrentTimeline(timeline);
        }
        public static void SetCurrentTimeline(TimelineAsset timeline)
        {
            Assembly assembly = typeof(UnityEditor.Timeline.TimelineEditor).Assembly;
            Type timelineWindow = assembly.GetType("UnityEditor.Timeline.TimelineWindow");
            var window = EditorWindow.GetWindow(timelineWindow);
            var setCurrentTimeline = timelineWindow.GetMethod("SetCurrentTimeline", new Type[] { typeof(TimelineAsset) });
            setCurrentTimeline.Invoke(window, new object[] { timeline });
        }


      
    }
}


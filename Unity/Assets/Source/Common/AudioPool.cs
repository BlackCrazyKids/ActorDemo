
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class AudioObject {
        public static float CacheTime = 10;
        public static float IntervalTime = 0.2f;
        public float lastusetime;
        public string path;
        public AudioClip clip;
        public AssetBundle ab;

        public bool CanPlay() {
            return Time.time - lastusetime > IntervalTime;
        }

        public bool CanRelease() {
            return Time.time - lastusetime > clip.length + CacheTime;
        }
        public void Release() {
            //Util.LogColor("red", "AudioPool:Release " + path + Time.time);
            //Object.DestroyImmediate(clip,true);
            Resources.UnloadAsset(clip);
            ab.Unload(false);
        }
    }
    public class AudioPool
    {
        static readonly int MaxPoolCount = 10;
        static Dictionary<string, AudioObject> Pool = new Dictionary<string, AudioObject>();
        static float _cleanupLastTime = 0;

        public static string GetStandardlize(string path)
        {
            return path.Replace(@"\", @"/").ToLower();
        }

        public static bool Contains(string ipath)
        {
            var path = GetStandardlize(ipath);
            return Pool.ContainsKey(path);
        }

        public static bool CanPlay(string ipath)
        {
            var path = GetStandardlize(ipath);
            if (!Pool.ContainsKey(path)) {
                return true;
            }
            AudioObject ao = Pool.Get(path);
            return ao.CanPlay();
        }
        public static AudioClip Get(string ipath) {
            var path = GetStandardlize(ipath);
            if (Pool.ContainsKey(path)) {
                Pool[path].lastusetime = Time.time;
                return Pool[path].clip;
            }
            return null;
        }

        

        public static void Put(string ipath, AudioClip clip,AssetBundle ab) {
            var path = GetStandardlize(ipath);
            if (Pool.ContainsKey(path))
            {
                if (Pool[path].ab != ab) {
                    ab.Unload(false);
                }
                Pool[path].lastusetime = Time.time;
            }
            else {
                AudioObject newclipobject = new AudioObject();
                newclipobject.path = path;
                newclipobject.lastusetime = Time.time;
                newclipobject.clip = clip;
                newclipobject.ab = ab;
                Pool[path] = newclipobject;
                //Util.LogColor("green", "AudioPool:New " + path + Time.time);
            }
        }

        public static void CleanupInterval()
        {
            const float interval = 30;
            if (Time.realtimeSinceStartup > _cleanupLastTime + interval)
            {
                CleanupNow();
            }
        }


        public static void CleanupNow()
        {
            if (Pool.Count > MaxPoolCount)
            {
                var unusedeffectpools = new List<AudioObject>();
                foreach (var pair in Pool)
                {
                    if (pair.Value.CanRelease())
                    {
                        unusedeffectpools.Add(pair.Value);
                    }
                }
                unusedeffectpools.Sort((x, y) => { return x.lastusetime.CompareTo(y.lastusetime); });

                for (int index = 0; index < Mathf.Min(Pool.Count - MaxPoolCount, unusedeffectpools.Count); index++)
                {
                    unusedeffectpools[index].Release();
                    Pool.Remove(unusedeffectpools[index].path);
                }

            }
        }

        public static void ReleaseAll() {
            foreach (var kv in Pool)
            {
                kv.Value.Release();
            }
            Pool.Clear();
        }

		
    }
}

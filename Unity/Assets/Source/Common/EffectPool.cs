/********************************************************************
description: 	
 * 特效缓存池（对象级）
 * 每种特效最多实例化5个特效
 * 也可以以特效池为模板克隆（EffectPool.GetEffectClone），但是返回的特效不由池管理
 * 缓存池最多保留10种特效，多出的无用特效（游戏中已经没有在用的特效）删除
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Game
{
    public class EffectPoolItem
    {
        public static int MaxCount = 5;
        public static float CacheTime = 100;
        public EffectPoolItem(string path, GameObject originaleffect)
        {
            this.path = path;
            this.originaleffect = originaleffect;
            this.originaleffect.SetActive(false);
            this.lastusetime = Time.time;
            GameObject.DontDestroyOnLoad(this.originaleffect);
        }
        public float lastusetime = 0;
        public string path;
        public GameObject originaleffect;
        SimplePool<GameObject> effectpool = new SimplePool<GameObject>(MaxCount);
        HashSet<GameObject> usingeffects = new HashSet<GameObject>();

        public bool IsFull()
        {
            return effectpool.Count + usingeffects.Count >= MaxCount;
        }

        public bool HasEffect()
        {
            return effectpool.Count > 0 || !IsFull();
        }

        public GameObject Clone()
        {
            var effect = Util.Copy(originaleffect);
            GameObject.DontDestroyOnLoad(effect);
            return effect;
        }
        //

        public GameObject Get()
        {
            if (!HasEffect())
            {
                return null;
            }
            var effect = effectpool.Get();
            if (effect == null)
            {
                effect = Clone();
            }
            usingeffects.Add(effect);
            //Util.LogColor("green", "EffectPoolItem:Get " + path+Time.time);
            return effect;
        }

        public void Put(GameObject effect)
        {
            if (usingeffects.Contains(effect))
            {
                effectpool.Release(effect);
                usingeffects.Remove(effect);
                lastusetime = Time.time;
                //Util.LogColor("white", "EffectPoolItem:Put " + path + Time.time);
            }
        }


        public bool IsUnused()
        {
            return usingeffects.Count == 0 && Time.time - lastusetime > CacheTime;
        }

        public void Release()
        {
            var del = effectpool.Get();
            while (del != null)
            {
                Object.Destroy(del);
                del = effectpool.Get();
            }
            Object.Destroy(originaleffect);
            //Util.LogColor("red", "EffectPoolItem:Release " + path + Time.time);
        }


    }
    public class EffectPool
    {
        static int MaxCount = 10;
        static Dictionary<string, EffectPoolItem> CachedEffect = new Dictionary<string, EffectPoolItem>();
        static float _cleanupLastTime = 0;


        public static string GetStandardlize(string path)
        {
            return path.Replace(@"\", @"/").ToLower();
        }
        //是否有特效缓存
        public static bool HasEffectPoolItem(string path)
        {
            return CachedEffect.ContainsKey(GetStandardlize(path));
        }

        public static EffectPoolItem GetEffectPoolItem(string path)
        {
            return CachedEffect[GetStandardlize(path)];
        }

        public static void AddEffectPoolItem(string path, GameObject assetobj)
        {
            if (!HasEffectPoolItem(path))
            {
                var standardlizepath = GetStandardlize(path);
                var effect = Util.Instantiate(assetobj, standardlizepath);
                EffectPoolItem item = new EffectPoolItem(standardlizepath, effect);
                CachedEffect[standardlizepath] = item;
                //Util.LogColor("cyan", "EffectPool:AddCachedEffect " + path + CachedEffect.Count);
            }

        }

        public static void DestroyEffectPoolItem(string path)
        {
            if (HasEffectPoolItem(path))
            {
                var item = GetEffectPoolItem(path);
                item.Release();
                CachedEffect.Remove(item.path);
            }
        }

        public static bool HasEffect(string path)
        {
            return GetEffectPoolItem(path).HasEffect();
        }

        public static GameObject GetEffect(string path)
        {
            return GetEffectPoolItem(path).Get();
        }

        public static void PutEffect(string path, GameObject effect)
        {
            GetEffectPoolItem(path).Put(effect);
        }

        public static GameObject GetEffectClone(string path)
        {
            return GetEffectPoolItem(path).Clone();
        }

        public static void ReleaseAll()
        {
            foreach (var pairs in CachedEffect)
            {
                pairs.Value.Release();
            }
            CachedEffect.Clear();
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
            if (CachedEffect.Count > MaxCount)
            {
                var unusedeffectpools = new List<EffectPoolItem>();
                foreach (var pair in CachedEffect)
                {
                    if (pair.Value.IsUnused())
                    {
                        unusedeffectpools.Add(pair.Value);
                    }
                }
                unusedeffectpools.Sort((x, y) => { return x.lastusetime.CompareTo(y.lastusetime); });

                for (int index = 0; index < Mathf.Min(CachedEffect.Count - MaxCount, unusedeffectpools.Count); index++)
                {
                    DestroyEffectPoolItem(unusedeffectpools[index].path);
                }

            }
        }


    }


}

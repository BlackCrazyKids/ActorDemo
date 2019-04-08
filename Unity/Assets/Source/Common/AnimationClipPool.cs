/********************************************************************
created:	    2016-07-13
author:			zhangjialing
description: 	
Copyright (C) - All Rights Reserved
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class AnimationClipObject
    {
        public static float CacheTime = 10;
        public AnimationClip clip;
        //public AssetBundle ab;
        public int refcount ; //引用计数
        public float lastusetime;

        public AnimationClipObject(AnimationClip clip) {
            this.clip = clip;
            //this.ab = ab;
            this.refcount = 0;
            this.lastusetime = Time.time;
        }

        public AnimationClip GetAnimtionClip()
        {
            lastusetime = Time.time;
            return clip;
        }

        public bool CanRelease() {
            return refcount <=0 && Time.time - lastusetime > CacheTime ;
        }
        public void Release()
        {
            //Util.LogColor("red", "AudioPool:Release " + path + Time.time);
            //Object.DestroyImmediate(clip,true);
            Resources.UnloadAsset(clip);
            //ab.Unload(false);
            clip = null;
            //ab = null;
        }
    }
    public class AnimationClipPool
    {
        static Dictionary<int, AnimationClipObject> Pool = new Dictionary<int, AnimationClipObject>();

        static float _cleanupLastTime = 0;

        public static bool Contains(int hashclip)
        {
            return Pool.ContainsKey(hashclip);
        }

        public static AnimationClip Get(int hashclip)
        {
            if (Pool.ContainsKey(hashclip))
            {
                return Pool[hashclip].GetAnimtionClip();
            }
            return null;
        }

        public static void AddClipRef(int hashclip)
        {
            if (Pool.ContainsKey(hashclip))
            {
                Pool[hashclip].refcount++;
            }
        }

        public static void RemoveClipRef(int hashclip)
        {
            if (Pool.ContainsKey(hashclip))
            {
                Pool[hashclip].refcount--;
            }
        }

        public static void AddClipRef(string clip)
        {
            AddClipRef(Animator.StringToHash(clip));
        }

        public static void RemoveClipRef(string clip)
        {
            RemoveClipRef(Animator.StringToHash(clip));
        }



        public static void Put(int hashclip,AnimationClip clip)
        {
            if (clip == null)
            {
                Debug.LogError("Load Clip Error! clip == null :" + LuaHelper.hash_name_map[hashclip]);
                return;
            }

            if (!Pool.ContainsKey(hashclip))
            {
                Pool[hashclip] = new AnimationClipObject(clip);
            }
            Pool[hashclip].lastusetime = Time.time;
            
        }

        public static void CleanupInterval()
        {
            const float interval = 30;
            if (Time.realtimeSinceStartup > _cleanupLastTime + interval)
            {
                CleanupNow();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                //Util.LogColor("cyan", "PoolCount:" + Pool.Count);
                foreach (var pair in Pool)
                {
                    if (pair.Value.refcount <= 0)
                    {
                        //Util.LogColor("red", LuaHelper.hash_name_map[pair.Key] + " : " + pair.Value.refcount);
                    }
                    else
                    {
                        //Util.LogColor("cyan", LuaHelper.hash_name_map[pair.Key] + " : " + pair.Value.refcount);
                    }

                }
            }
        }


        public static void CleanupNow()
        {
            _cleanupLastTime = Time.realtimeSinceStartup;
            var unusedanimationclips = new List<int>();
            foreach (var pair in Pool)
            {
                if (pair.Value.CanRelease())
                {
                    unusedanimationclips.Add(pair.Key);
                }
            }
            foreach (var hashclip in unusedanimationclips)
            {
                Pool[hashclip].Release();
                //Util.LogColor("red", "AnimationClipPool Remove:" + LuaHelper.hash_name_map[hashclip]);
                Pool.Remove(hashclip);
            }
        }

        public static void ReleaseAll()
        {
            foreach (var kv in Pool)
            {
                kv.Value.Release();
            }
            Pool.Clear();
        }


    }
}
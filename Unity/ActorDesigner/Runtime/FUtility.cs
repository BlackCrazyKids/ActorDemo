using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Flux
{
    /**
	 * @brief Runtime Utility class for Flux
	 */
    public class FUtility
    {

        public const int FLUX_VERSION = 210;
        public const string ResourceName = "-----Resources-----";

        public static bool IsAnimationEditable(AnimationClip clip)
        {
            //return clip == null || ( ((clip.hideFlags & HideFlags.NotEditable) == 0) && !clip.isLooping);
            return false;
        }

        public static void ResizeAnimationCurve(AnimationCurve curve, float newLength)
        {
            float frameRate = 60;

            float oldLength = curve.length == 0 ? 0 : curve.keys[curve.length - 1].time;
            //			float newLength = newLength;

            if (oldLength == 0)
            {
                // handle no curve
                curve.AddKey(0, 1);
                curve.AddKey(newLength, 1);
                return;
            }

            float ratio = newLength / oldLength;
            float inverseRatio = 1f / ratio;

            int start = 0;
            int limit = curve.length;
            int increment = 1;

            if (ratio > 1)
            {
                start = limit - 1;
                limit = -1;
                increment = -1;
            }

            for (int i = start; i != limit; i += increment)
            {
                Keyframe newKeyframe = new Keyframe(Mathf.RoundToInt(curve.keys[i].time * ratio * frameRate) / frameRate, curve.keys[i].value, curve.keys[i].inTangent * inverseRatio, curve.keys[i].outTangent * inverseRatio);
                newKeyframe.tangentMode = curve.keys[i].tangentMode;
                curve.MoveKey(i, newKeyframe);
            }
        }

        /// <summary>
        ///  一般在激活状态下使用
        ///  资源路径均采用相对路径来处理.
        ///  保证场景资源与项目资源相对路径一致.
        /// </summary>
        public static GameObject FindGameObject(string path)
        {
            GameObject resources = GameObject.Find(ResourceName);
            if (resources == null)
                resources = new GameObject(ResourceName);
            GameObject go = GameObject.Find(path);//这里的对象可以是任意节点下的对象
            if (go == null)
            {
#if UNITY_EDITOR
                go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
                if (go == null)
                {
                    Debug.LogError("资源不存在" + path);
                    return null;
                }
              
                go.transform.parent = resources.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
            }
            return go;
        }
    }
}

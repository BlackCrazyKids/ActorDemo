using UnityEngine;
using System.Collections.Generic;

public static class ExtendedTransform
{
	public static Transform GetChildByName(this Transform root, string name)
	{
		foreach(var child in root.WalkTree())
		{
			if(child.name == name)
			{
				return child;
			}
		}

		return null;
	}

	public static T GetChildComponent<T>(this Component com, string name) where T : Component
	{
		return com.transform.GetChildByName (name).GetComponent<T>();
	}
	
	public static IEnumerable<Transform> WalkTree(this Transform root)
	{
		if(null != root)
		{
			yield return root;

			foreach(Transform child in root)
			{
				foreach(var grandson in child.WalkTree())
				{
					yield return grandson;
				}
			}
		}
	}

    /// <summary>
    /// 同步节点Transfrom数据(PRS),包含子节点
    /// </summary>
	public static void SynchronizeShape(this Transform destination, Transform source)
	{
		if(null == source || null == destination)
		{
		    string s = string.Format("source={0}, destination={1}", source, destination);
			UnityEngine.Debug.LogError(s);
			return;
		}

		using(var srcEnumerator	= source.WalkTree().GetEnumerator())
		using(var dstEnumerator	= destination.WalkTree().GetEnumerator())
		{
			srcEnumerator.MoveNext(); 
			dstEnumerator.MoveNext();

			while(srcEnumerator.MoveNext() && dstEnumerator.MoveNext())
			{
				var srcTransform	= srcEnumerator.Current;
				var dstTransform	= dstEnumerator.Current;
				dstTransform.localPosition	= srcTransform.localPosition;
				dstTransform.localRotation	= srcTransform.localRotation;
				dstTransform.localScale		= srcTransform.localScale;
			}
		}
	}
}


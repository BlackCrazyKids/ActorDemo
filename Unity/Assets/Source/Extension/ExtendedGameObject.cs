using UnityEngine;
using System;
using System.Collections.Generic;

public static class ExtendedGameObject
{	
	public static void PrintActiveState(this GameObject gameObject)
	{
		Debug.Log ("The active state of " + gameObject.name + ":" + GetActiveState(gameObject));
	}
	
	public static string GetActiveState(this GameObject gameObject)
	{
		string res = "";
		res += gameObject.GetPath() + ":[" + gameObject.activeSelf + "];";
		foreach(Transform child in gameObject.transform)
		{
			res += GetActiveState(child.gameObject);
		}
		return res;
	}
	
	public static void ForEach_Depth(this GameObject prefab, System.Action<GameObject> action)
	{
		if(null == prefab)
		{
			return;
		}
		
		if( action!=null)
		{
			action(prefab);
		}
		
		foreach( Transform t in prefab.transform)
		{
			ForEach_Depth(t.gameObject,action);
		}
	}

	public static GameObject Clone(this GameObject prefab)
	{
		if(null == prefab)
		{
			return null;
		}

		var cloned					= GameObject.Instantiate(prefab) as GameObject;
		var prefabTransform			= prefab.transform;
		cloned.transform.parent		= prefabTransform.parent;
		// in NGUI, the localScale will be adjusted automatically according to the scale of camera. so restore it.
		cloned.transform.localScale	= prefabTransform.localScale;
		return cloned;
	}

	public static GameObject ReplacedByPrefab(this GameObject go, GameObject prefab)
	{
		if(null == go || null == prefab)
		{
			return null;
		}

		var cloned					= GameObject.Instantiate(prefab) as GameObject;
		var oldTransform			= go.transform;
		cloned.transform.parent		= oldTransform.parent;
		// in NGUI, the localScale will be adjusted automatically according to the scale of camera. so restore it.
		cloned.transform.localScale	= oldTransform.localScale;
		GameObject.Destroy(go);
		return cloned;
	}

	public static void SetActiveRecursely(this GameObject go, bool tag)
	{
		foreach(Transform child in go.transform)
		{
			child.gameObject.SetActiveRecursely(tag);
		}

		go.SetActive(tag);
	}

    public static void SetToLayer(this GameObject go, int layer)
    {
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetToLayer(layer);
        }

        go.layer = layer;
    }

    public static void SetParticleSystemScale(this GameObject go, float scale)
    {
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetParticleSystemScale(scale);
        }

        if (go.GetComponent<ParticleSystem>() != null)
        {
            go.GetComponent<ParticleSystem>().startSize *= scale;
            go.GetComponent<ParticleSystem>().startSpeed *= scale;
        }
    }


    public static void SetParticleSystemVisible(this GameObject go, bool bVisible)
    {
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetParticleSystemVisible(bVisible);
        }

        if (go.GetComponent<ParticleSystem>() != null)
        {
            if (bVisible)
            {
                go.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                go.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

	public static GameObject GetChildByName(this GameObject go, string name)
	{
		if(null != go)
		{
			foreach(Transform child in go.transform)
			{
				if(child.name == name)
				{
					return child.gameObject;
				}

				var grandson	= child.gameObject.GetChildByName(name);
				if(null != grandson)
				{
					return grandson;
				}
			}
		}

		return null;
	}

    public static GameObject GetChildHasName(this GameObject go, string name)
    {
        if (null != go)
        {
            foreach (Transform child in go.transform)
            {
                if (child.name.Contains(name))
                {
                    return child.gameObject;
                }

                var grandson = child.gameObject.GetChildHasName(name);
                if (null != grandson)
                {
                    return grandson;
                }
            }
        }

        return null;
    }

    public static GameObject[] GetDirectChildren(this GameObject go, string[] names)
	{
		if(null != go && null != names)
		{
			var goes	= new GameObject[names.Length];
			for(int i= 0; i< names.Length; ++i)
			{
				var name	= names[i];
				foreach(Transform child in go.transform)
				{
					if(child.name == name)
					{
						goes[i]	= child.gameObject;
						break;
					}
				}
			}

			return goes;
		}

		return null;
	}

    public static GameObject GetRootObject(this GameObject go)
    {
        if (go == null) { return null; }
        var root = go.transform;
        while (root.parent!=null)
        {
            root = root.parent;
        }

        return root.gameObject;
    }

    public static IEnumerable<GameObject> GetDirectChildren(this GameObject go)
	{
		if(null != go)
		{
			foreach(Transform child in go.transform)
			{
				yield return child.gameObject;
			}
		}
	}

    public static void DeleteChildren(this GameObject go)
    {
        if (go != null)
        {
            int count   = go.transform.childCount;
            for (int i = count -1; i >=0; i--)
            {
                GameObject.Destroy(go.transform.GetChild(i).gameObject); 
            }

        }
    }

    public static void DeleteChildrenImmediate(this GameObject go)
    {
        if (go != null)
        {
            int count = go.transform.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(go.transform.GetChild(i).gameObject,true);
            }

        }
    }


    public static string GetPath(this GameObject go)
	{
		GameObject current = go;
		string path = current.name;

		while(null != current.transform.parent)
		{
			current = current.transform.parent.gameObject;
			path = current.name + "/" + path;
		}

		return path;
	}

	public static void SetLayerRecursively(this GameObject go, string layer)
	{
		if(null == go)
			return;

		go.layer = LayerMask.NameToLayer(layer);
		foreach(Transform child in go.transform)
			child.gameObject.SetLayerRecursively(layer);
	}
	
	public static void SetLayerRecursively(this GameObject go, int layer)
	{
		if(null == go)
			return;

		go.layer = layer;
		foreach(Transform child in go.transform)
			child.gameObject.SetLayerRecursively(layer);
        
	}

    public static bool GetActive(this GameObject go)
    {
        return go && go.activeInHierarchy;
    }

	public static IEnumerable<GameObject> WalkTree(this GameObject root)
	{
		if(null != root)
		{
			yield return root;

			foreach(Transform child in root.transform)
			{
				foreach(var grandson in child.gameObject.WalkTree())
				{
					yield return grandson;
				}
			}
		}
	}

	public static void ReplaceMaterials(this GameObject root, Material material)
	{
		foreach(var child in root.WalkTree())
		{
			var childRenderer	= child.GetComponent<Renderer>();
			if(null == childRenderer || null == childRenderer.sharedMaterials)
			{
				continue;
			}

			var materials	= new Material[childRenderer.sharedMaterials.Length];
			for(int i= 0; i< materials.Length; ++i)
			{
				materials[i]= material;
			}

			childRenderer.sharedMaterials	= materials;
		}
	}

	public static void SynchronizeShape(this GameObject destination, GameObject source)
	{
		if(null == source || null == destination)
		{
            string s = string.Format("source={0}, destination={1}", source, destination);
			UnityEngine.Debug.LogError(s);
			return;
		}

		destination.transform.SynchronizeShape(source.transform);
	}

	public static T SetDefaultComponent<T>(this GameObject go) where T : Component
	{
		if(null != go)
		{
			var component	= go.GetComponent<T>();
			if(null == component)
			{
				component	= go.AddComponent<T>();
			}

			return component;
		}

		return null;
	}
	
	public static T GetComponentNull<T>(this Component c, string path) where T : Component
	{
		if(c != null)
		{
			var t = c.transform;
			t = t.Find(path);
			if(t != null)
				return t.GetComponent<T>();
		}
		return null;
	}
	public static T GetComponentNull<T>(this GameObject c, string path) where T : Component
	{
		if(c != null)
		{
			var t = c.transform;
			t = t.Find(path);
			if(t != null)
				return t.GetComponent<T>();
		}
		return null;
	}

    public static T GetComponenetInParents<T>(this GameObject c) where T : Component
    {
        T com = null;
        if (c != null)
        {
            Transform t = c.transform.parent;
            while (t != null)
            {
                com = t.GetComponent<T>();
                if (com != null)
                {
                    return com;
                }
                t = t.parent;
            }
        }
        return null;
    }

    public static bool IsNullOrBeDestroyed(this GameObject go)
    {
        return go == null ||go.transform == null;
    }


}

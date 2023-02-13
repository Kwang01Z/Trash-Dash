using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

/// <summary>
/// This class allows us to create multiple instances of a prefabs and reuse them.
/// It allows us to avoid the cost of destroying and creating objects.
/// </summary>
public class Pooler
{
	protected Stack<GameObject> m_FreeInstances = new Stack<GameObject>();
	protected GameObject m_Original;
	public Pooler(Transform parent, GameObject original, int initialSize)
	{
		m_Original = original;
		m_FreeInstances = new Stack<GameObject>(initialSize);

		for (int i = 0; i < initialSize; ++i)
		{
			GameObject obj = Object.Instantiate(original);
			obj.transform.parent = parent;
			obj.SetActive(false);
			m_FreeInstances.Push(obj);
		}
	}

	public GameObject Get(Transform parent ,Vector3 pos, Quaternion quat)
	{
		GameObject ret = m_FreeInstances.Count > 0 ? m_FreeInstances.Pop() : Object.Instantiate(m_Original);
		ret.SetActive(true);
		ret.transform.parent = parent;
		ret.transform.position = pos;
		ret.transform.rotation = quat;

		return ret;
	}

	public void Free(Transform root,GameObject obj)
	{
		obj.SetActive(false);
		obj.transform.parent = root;
		m_FreeInstances.Push(obj);
	}
}

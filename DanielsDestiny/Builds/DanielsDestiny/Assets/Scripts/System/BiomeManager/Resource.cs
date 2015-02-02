using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {
	public bool _isActive = true;
	public GameObject resources;
	void Start()
	{
		Load(true);
	}
	public void Load(bool _set)
	{
		Debug.Log(_set);
		_isActive = _set;
		if(!_isActive)
			if(resources != null)
				foreach(Transform t in resources.GetComponentInChildren<Transform>())
					Destroy(t.gameObject);
	}
	public string Save()
	{
		if(resources != null)
		{
			if(resources.GetComponentInChildren<Collider>() != null)
				return "1";
			else
				return "0";
		}
		else
		{
			if(_isActive)
				return "1";
			else
				return "0";
		}
	}
	public void Collect()
	{
		Load(false);
	}
}
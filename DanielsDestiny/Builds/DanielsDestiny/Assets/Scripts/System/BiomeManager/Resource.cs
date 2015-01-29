using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour {
	public bool _isActive = true;
	void Start()
	{
		Load(true);
	}
	public void Load(bool _set)
	{
		_isActive = _set;
		if(!_isActive)
			this.renderer.material.color = Color.red;
		else
			this.renderer.material.color = Color.green;
	}
	public string Save()
	{
		if(_isActive)
			return "1";
		else
			return "0";
	}
	public void Collect()
	{
		Load(false);
	}
}
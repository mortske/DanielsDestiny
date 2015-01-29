using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Biome : MonoBehaviour {
	List<Resource> _resources = new List<Resource>();

	void Start()
	{
		foreach(Resource r in transform.GetComponentsInChildren<Resource>())
		{
			_resources.Add(r);
		}
	}
	public int GetLength()
	{
		return _resources.Count;
	}

	public void LoadResources(string r)
	{
		for(int i = 0; i < _resources.Count; i++)
		{
			int firstDigit = (int)(r[i]) - 48;
			_resources[i].Load(firstDigit == 1);
		}
	}
	public string SaveResources()
	{
		string test = "";
		for(int i = 0; i < _resources.Count; i++)
		{
			test = (test + _resources[i].Save());
		}
		return test;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WorldItemSave{
	public List<string> type = new List<string>();
	public List<Vector3> pos = new List<Vector3>();
	
	public void AddItem(string t, Vector3 p)
	{
		type.Add(t);
		pos.Add(p);
	}
}

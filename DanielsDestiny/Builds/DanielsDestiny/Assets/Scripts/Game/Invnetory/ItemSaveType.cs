using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemSaveType{
	public string type;
	
	public ItemSaveType StringType(string s)
	{
		type = s;
		return this;
	}
}

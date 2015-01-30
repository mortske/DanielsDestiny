using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemSaveType{
	public string type;
	public int _amount;
	
	
	public ItemSaveType StringType(string s)
	{
		type = s;
		return this;
	}
	public int GetSize
	{
		get{return _amount;}
	}
	public int SetSize
	{
		set{_amount = value;}
	}
}

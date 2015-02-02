using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {
	public static ItemManager instance;

	public List<GameObject> itemList = new List<GameObject>();
	List<string> itemNames = new List<string>();
	
	void Awake()
	{
		if(instance == null)
			instance = this as ItemManager;
		else
			Destroy (this.gameObject);
	}
	void Start()
	{
		for(int i = 0; i < itemList.Count; i++)
		{
			itemNames.Add(itemList[i].name);
		}
	}
	public string GetName(int n)
	{
		return itemNames[n];
	}
}

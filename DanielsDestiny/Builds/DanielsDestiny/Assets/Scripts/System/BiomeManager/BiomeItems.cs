using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiomeItems : MonoBehaviour {
	public GameObject[] OriginalItems;
	List<GameObject> NewItems = new List<GameObject>();
	
	string active;
	
	public void SetItems(string s)
	{
		if(s.Length > 0)
		{
			for(int i = 0; i < OriginalItems.Length; i++)
			{
//				Debug.Log("Item Digit: " + (int)(s[i]));
				int firstDigit = (int)(s[i]) - 48;
				if(firstDigit == 0)
				{
					Destroy (OriginalItems[i].gameObject);
				}
					
			}
		}
		Player.instance.pickupEventHandler.ClearItemQueue();
	}
	public string GetItemsInBiome()
	{
		string test = "";
		for(int i = 0; i < OriginalItems.Length; i++)
		{
			if(OriginalItems[i] == null)
				test = (test + "0");
			else
				test = (test + "1");
		}
		return test;
	}
	public void PickItem(GameObject obj)
	{
		for(int i = 0; i < OriginalItems.Length; i++)
		{
			if(OriginalItems[i] == obj)
				OriginalItems[i] = null;
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Player.instance.curBiome = this as BiomeItems;
		}
	}
	
	public WorldItemSave GetNewItems()
	{
		WorldItemSave s = new WorldItemSave();
		for(int i = 0; i < NewItems.Count; i++)
		{
			for(int x = 0; x < ItemManager.instance.itemList.Count; x++)
			{
				if(ItemManager.instance.itemList[x].name.Equals(NewItems[i].name))
					s.AddItem(x.ToString(), NewItems[i].transform.position);
			}
		}
		return s;
	}
	public void SetNewItems(WorldItemSave saveItem)
	{
		for(int i = 0; i < saveItem.type.Count; i++)
		{
			GameObject go = Instantiate(ItemManager.instance.itemList[int.Parse(saveItem.type[i])], saveItem.pos[i], Quaternion.identity) as GameObject;
			go.name = ItemManager.instance.itemList[int.Parse(saveItem.type[i])].name;
			NewItems.Add(go);
		}
	}
	public void AddWorldDrop(GameObject g)
	{
		NewItems.Add(g);
	}
}

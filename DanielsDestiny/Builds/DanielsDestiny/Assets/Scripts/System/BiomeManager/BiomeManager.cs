using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class BiomeManager : MonoBehaviour {
	public static BiomeManager instance;
	public List<Biome> pieces = new List<Biome>();
	public BiomeSave save;
	public Inventory inventorY;

	string path = "Assets/Files/Save.xml";
	// Use this for initialization

	void Awake()
	{
		if(instance == null)
			instance = this as BiomeManager;
		else
			Destroy (this.gameObject);
	}
	void Start()
	{
		inventorY = Player.instance.inventory;
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Period))
		{
			LoadBiomes();
		}
		if(Input.GetKeyUp(KeyCode.Comma))
		{
			string tmpString = "";
			List<ItemSaveType> tmpList = inventorY.GetInventory();
			SaveInventory(tmpList);
		}
	}
	void LoadBiomes()
	{
		if (File.Exists(path))
		{
			StreamReader sr = new StreamReader(path);
			XmlSerializer x = new System.Xml.Serialization.XmlSerializer(save.GetType());
			save = x.Deserialize(sr) as BiomeSave;
			sr.Close();
		}
		else
		{
			Directory.CreateDirectory("Assets/Files/");
			File.Create(path);
		}
		inventorY.SetInventory(save.saveType);
		for(int i = 0; i < pieces.Count; i++)
		{
			pieces[i].GetComponent<BiomeItems>().SetItems(save.biomeItemSave[i]);
			pieces[i].GetComponent<BiomeItems>().SetNewItems(save.worldItemSave[i]);
			pieces[i].LoadResources(save.saveString[i]);
		}
	}
	void SaveBiomes()
	{
		StreamWriter sw = new StreamWriter(path);
		XmlSerializer x = new System.Xml.Serialization.XmlSerializer(save.GetType());
		x.Serialize(sw, save);
		sw.Close();
	}
	public void SaveInventory(List<ItemSaveType> s)
	{
		for(int i = 0; i < s.Count; i++)
		{
			if(save.saveType.Count < s.Count)
			{
				save.saveType.Add(s[i]);
			}
			else
			{
				save.saveType[i] = (s[i]);
			}
		}
		Debug.Log(save.saveType);
		SaveBiomeItems();
	}
	void SaveBiomeItems()
	{
		for(int i = 0; i < pieces.Count; i++)
		{
			if(save.biomeItemSave.Count < pieces.Count)
			{
				save.biomeItemSave.Add(pieces[i].GetComponent<BiomeItems>().GetItemsInBiome());
			}
			else
			{
				save.biomeItemSave[i] = (pieces[i].GetComponent<BiomeItems>().GetItemsInBiome());
			}
		}
		SaveBiomeWorldItems();
	}
	void SaveBiomeWorldItems()
	{
		for(int i = 0; i < pieces.Count; i++)
		{
			if(save.worldItemSave.Count < pieces.Count)
			{
				save.worldItemSave.Add(pieces[i].GetComponent<BiomeItems>().GetNewItems());
			}
			else
			{
				save.worldItemSave[i] = (pieces[i].GetComponent<BiomeItems>().GetNewItems());
			}
		}
		SaveBiomeResource();
	}
	void SaveBiomeResource()
	{
		for(int i = 0; i < pieces.Count; i++)
		{
			if(save.saveString.Count < pieces.Count)
			{
				save.saveString.Add(pieces[i].SaveResources());
			}
			else
			{
				save.saveString[i] = (pieces[i].SaveResources());
			}
		}
		SaveBiomes();
	}
}
[System.Serializable]
public class BiomeSave
{
	public List<string> saveString = new List<string>();
	public List<ItemSaveType> saveType = new List<ItemSaveType>();
	public List<string> biomeItemSave = new List<string>();
	public List<WorldItemSave> worldItemSave = new List<WorldItemSave>();
}

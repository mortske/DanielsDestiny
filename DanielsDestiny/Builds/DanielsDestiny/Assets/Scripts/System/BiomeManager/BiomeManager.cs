using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class BiomeManager : MonoBehaviour {
	public static BiomeManager instance;
	public List<Biome> pieces = new List<Biome>();
	public SaveFile save;
	public Inventory inventorY;
	public GameTime gameTime;

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
	public void SaveGame()
	{
		List<ItemSaveType> tmpList = inventorY.GetInventory();
		SaveInventory(tmpList);
	}
	public void LoadBiomes()
	{
		if (File.Exists(path))
		{
			StreamReader sr = new StreamReader(path);
			XmlSerializer x = new System.Xml.Serialization.XmlSerializer(save.GetType());
			save = x.Deserialize(sr) as SaveFile;
			sr.Close();
			
			if(save.biomeItemSave.Count > 0)
			{
				inventorY.SetInventory(save.saveType);
				for(int i = 0; i < pieces.Count; i++)
				{
					pieces[i].GetComponent<BiomeItems>().SetItems(save.biomeItemSave[i]);
					pieces[i].GetComponent<BiomeItems>().SetNewItems(save.worldItemSave[i]);
					pieces[i].LoadResources(save.saveString[i]);
				}
				LoadPlayer();
			}
		}
	}
	void SaveBiomes()
	{
		StreamWriter sw = new StreamWriter(path);
		XmlSerializer x = new System.Xml.Serialization.XmlSerializer(save.GetType());
		x.Serialize(sw, save);
		sw.Close();
	}
	void SaveInventory(List<ItemSaveType> s)
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
		SavePlayer();
		
	}
	void SavePlayer()
	{

		save.playerPos = Player.instance.transform.position;
		save.playerValues.health = Player.instance.status.health.cur;
		save.playerValues.hunger = Player.instance.status.hunger.cur;
		save.playerValues.thirst = Player.instance.status.thirst.cur;
		save.playerValues.fatigue = Player.instance.status.fatigue.cur;
		save.TempAdjust = Player.instance.status.temperatureAdjustment;
		save.playerValues.temperature = Player.instance.status.temperature.cur;
		save.TimeOfDay = gameTime.TheTime;
		save.Temperature = gameTime.TheTemp;
		SaveBiomes();
	}
	void LoadPlayer()
	{
		Player.instance.status.health.cur = save.playerValues.health;
		Player.instance.status.hunger.cur = save.playerValues.hunger;
		Player.instance.status.thirst.cur = save.playerValues.thirst;
		Player.instance.status.fatigue.cur = save.playerValues.fatigue;
		Player.instance.status.temperature.cur = save.playerValues.temperature;
		Player.instance.transform.position = save.playerPos;
		Player.instance.status.temperatureAdjustment = save.TempAdjust;
		gameTime.TheTime = save.TimeOfDay;
		gameTime.TheTemp = save.Temperature;
	}
}
[System.Serializable]
public class SaveFile
{
	public List<string> saveString = new List<string>();
	public List<ItemSaveType> saveType = new List<ItemSaveType>();
	public List<string> biomeItemSave = new List<string>();
	public List<WorldItemSave> worldItemSave = new List<WorldItemSave>();
	public Vector3 playerPos;
	public PlayerStatusSave playerValues;
	public float TimeOfDay;
	public float Temperature;
	public float TempAdjust;
}

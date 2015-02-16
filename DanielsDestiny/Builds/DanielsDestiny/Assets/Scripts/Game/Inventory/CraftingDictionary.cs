using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingDictionary : MonoBehaviour 
{
	public List<Recepie> recepies;

	private static List<Slot> selectedItems;
	
	public static List<Slot> SelectedItems 
	{
		get { return selectedItems; }
		set { selectedItems = value; }
	}

	private static bool insideArea;

	public static bool InsideArea 
	{
		get {return insideArea;}
		set {insideArea = value;}
	}

	private int allTrue;
	private bool foundRecepie;
	private Inventory inv;
	private Player player;

	private bool placeItem;
	private GameObject tmpPlacingObject;
	private Vector3 tmpPosition;

	void Start ()
	{
		selectedItems = new List<Slot>();
        inv = Player.instance.inventory;
        player = Player.instance;
	}

	void Update()
	{

		if(placeItem)
		{
			if(tmpPlacingObject != null)
			{
				RaycastHit hit;
				int layerMask = 1 << 10;
				Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
				if (Physics.Raycast(ray, out hit, 12, layerMask))
				{
					tmpPlacingObject.transform.position = new Vector3(hit.point.x, hit.point.y  + (tmpPlacingObject.transform.localScale.y / 2), hit.point.z);
				}
				if(Input.GetMouseButtonDown(0))
				{
					//Player.instance.curBiome.AddWorldDrop(tmpPlacingObject);
					placeItem = false;
					tmpPlacingObject.GetComponentInChildren<Item>().selected = false;
					selectedItems[0].CurrentItem.selected = false;
					selectedItems[0].RemoveItem();
					ClearSelectedItem();

				}
				else if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Inventory"))
				{
					Destroy(tmpPlacingObject);
					ClearSelectedItem();
				}
			}
		}

	}

	public void EquipItem()
	{
		if(selectedItems.Count == 1)
		{
			inv.MoveItemToEquipSlot(selectedItems[0].CurrentItem);
		}
		ClearSelectedItem();
	}

	public void UseItem()
	{
		if(selectedItems.Count == 1 && selectedItems[0].CurrentItem.constructable)
		{
			tmpPlacingObject = (GameObject)Instantiate(selectedItems[0].CurrentItem.transform.parent.gameObject);
			tmpPlacingObject.name = selectedItems[0].CurrentItem.transform.parent.name;
			tmpPlacingObject.SetActive(true);
			player.ToggleInventory();

			placeItem = true;
		}
		else if(selectedItems.Count == 1 && insideArea && selectedItems[0].CurrentItem.usable)
		{
			CheckRecepies();
		}
		else
		{
			Debug.Log ("You cant use that item, try to Eat/Drink it maybe.");
			ClearSelectedItem();
		}

	}

	public void EatItem()
	{
		if(selectedItems.Count == 1 && selectedItems[0].CurrentItem.usable)
		{
			selectedItems[0].UseItem();
			
		}
	}

	public void CraftItems()
	{
		if(selectedItems.Count >= 2)
		{
			CheckRecepies();
		}
		else
		{
			Debug.Log ("You cant craft anything from 1 item, try Use or Eat/Drink it");
			ClearSelectedItem();
		}
	}

	public static void ClearSelectedItem()
	{
		if(selectedItems.Count > 0)
		{
			foreach (Slot slot in selectedItems)
			{
				foreach (Item item in slot.Items)
				{
					slot.ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
					item.selected = false;
				}
			}
			selectedItems.Clear();
		}
	}

	private void CheckRecepies()
	{
		foreach (Recepie rec in recepies) 
		{
			if(rec.items.Count == selectedItems.Count)
			{
				bool gotIn = false;
				allTrue = rec.items.Count;
				int checkAllTrue = 0;
				float checkWeight = 0;
				for (int i = 0; i < selectedItems.Count; i++) 
				{
					for (int n = 0; n < rec.items.Count; n++)
					{
						if(selectedItems[i].CurrentItem.transform.parent.name == rec.items[n].name)
						{
							gotIn = true;
							if(selectedItems[i].Items.Count >= rec.amount[n])
							{
								checkAllTrue++;
								checkWeight = checkWeight + selectedItems[i].CurrentItem.weight;
							}
						}
						if(gotIn)
						{
							gotIn = false;
							break;
						}
					}
				}

				if(checkAllTrue == allTrue)
				{
					foundRecepie = true;
					bool getOut = false;

                    GameObject result = (GameObject)Instantiate(rec.result);
                    result.name = rec.result.name;
                    result.GetComponentInChildren<Item>().AddItem();
					//inv.CheckWeight(result.GetComponentInChildren<Item>().weight - checkWeight);
					//inv.AddItem(.transform.FindChild("OverlapSphere").GetComponent<Item>());
					//Message
					Debug.Log ("You created a " + rec.result.name);
					for (int i = 0; i < selectedItems.Count; i++) 
					{
						for (int n = 0; n < rec.items.Count; n++)
						{
							if(selectedItems[i].CurrentItem.transform.parent.name == rec.items[n].name)
							{
								getOut = true;
								for (int x = 0; x < rec.amount[n]; x++) 
								{
									selectedItems[i].CurrentItem.selected = false;
									selectedItems[i].RemoveItem();
								}
							}
							if(getOut)
							{
								getOut = false;
								break;
							}
						}
					}
				}
			}
			if(foundRecepie)
				break;
		}
		
		if(foundRecepie)
		{
			ClearSelectedItem();
			foundRecepie = false;
		}
		else
		{
			//Message: couldnt create anything with those items
			Debug.Log ("You cant create anything with those items.");
			ClearSelectedItem();
		}
	}

}

[System.Serializable]
public class Recepie
{
    public string recepieName = "";
    public List<GameObject> items;
    public List<int> amount;
    public GameObject result;
}

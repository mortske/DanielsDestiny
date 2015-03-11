using UnityEngine;
using UnityEngine.UI;
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

	private static bool canBeFilled;
	public static bool CanBeFilled 
	{
		get {return canBeFilled;}
		set {canBeFilled = value;}
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
					BoxCollider tmpCollider = tmpPlacingObject.GetComponent<BoxCollider>();

					tmpPlacingObject.transform.position = new Vector3(hit.point.x, hit.point.y + tmpCollider.bounds.size.y , hit.point.z);
				}
				if(Input.GetMouseButtonDown(0))
				{
					placeItem = false;
					tmpPlacingObject.transform.FindChild("OverlapSphere").gameObject.SetActive(true);
					tmpPlacingObject.GetComponentInChildren<Item>().selected = false;
					selectedItems[0].CurrentItem.selected = false;
					if(Player.instance.curBiome != null)
						Player.instance.curBiome.AddWorldDrop(tmpPlacingObject);
					selectedItems[0].RemoveItem();
					ClearSelectedItem();

				}
				else if(InputManager.GetKeyDown("Cancel") || InputManager.GetKeyDown("Inventory"))
				{
					Destroy(tmpPlacingObject);
					ClearSelectedItem();
				}
			}
		}

	}

	public void EquipItem()
	{
		if(!inv.hoverTrue)
		{
			if(selectedItems.Count == 1)
			{
				inv.MoveItemToEquipSlot(selectedItems[0].CurrentItem);
			}
			ClearSelectedItem();
		}
	}

	public void UseItem()
	{
		if(!inv.hoverTrue)
		{
			if(selectedItems.Count == 1 && selectedItems[0].CurrentItem.constructable)
			{
				tmpPlacingObject = (GameObject)Instantiate(selectedItems[0].CurrentItem.transform.parent.gameObject);
				tmpPlacingObject.name = selectedItems[0].CurrentItem.transform.parent.name;
				tmpPlacingObject.transform.FindChild("OverlapSphere").gameObject.SetActive(false);
				tmpPlacingObject.SetActive(true);
				player.ToggleInventory();

				placeItem = true;
			}
			else if(selectedItems.Count == 1 && insideArea && selectedItems[0].CurrentItem.usable || selectedItems.Count == 1 && canBeFilled)
			{
				CheckRecepies();
			}
			else
			{
				MessageBox.instance.SendMessage("I cant use that item...");
                SoundManager.instance.Spawn3DSound(inv.iCantDoThat[Random.Range(0, inv.iCantDoThat.Length)], player.transform.position, 1, 5);
				ClearSelectedItem();
			}
		}
	}

	public void EatItem()
	{
		if(!inv.hoverTrue)
		{
			if(selectedItems.Count == 1 && selectedItems[0].CurrentItem.usable)
			{
				selectedItems[0].UseItem();
				ClearSelectedItem();
				
			}
		}
	}

	public void CraftItems()
	{
		if(!inv.hoverTrue)
		{
			if(selectedItems.Count >= 1)
			{
				CheckRecepies();
			}
			else
			{
				MessageBox.instance.SendMessage("I need to select something...");
                SoundManager.instance.Spawn3DSound(inv.iCantDoThat[Random.Range(0, inv.iCantDoThat.Length)], player.transform.position, 1, 5);
				ClearSelectedItem();
			}
		}
	}

	public void DropItem()
	{
		if(selectedItems.Count == 1 && !inv.hoverTrue)
		{
			Inventory.from = selectedItems[0];
			inv.db.Display(selectedItems[0].Items.Count, 0, selectedItems[0].Items.Count);
			CoroutineHandler.instance.DropItemDialouge(inv.db, null);
			ClearSelectedItem();
		}
	}
	
	public static void ClearSelectedItem()
	{
		if(selectedItems.Count > 0)
		{
			foreach (Slot slot in selectedItems)
			{
                slot.GetComponent<Image>().color = Color.white;
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
			if(rec.items.Count == 1 && selectedItems[0].CurrentItem.transform.parent.name == "Stick")
			{
				if(rec.items[0].name == selectedItems[0].CurrentItem.transform.parent.name)
				{
					int multipleStacks = 0;
					List<string> sameName = new List<string>();
					int sameNameTrue = 0;
					for (int i = 0; i < selectedItems.Count; i++)
					{
						multipleStacks += selectedItems[i].Items.Count;
						sameName.Add (selectedItems[i].CurrentItem.transform.parent.name);
						if(rec.items[0].name == sameName[i])
							sameNameTrue ++;
					}

					if(multipleStacks >= rec.amount[0] && sameNameTrue == selectedItems.Count)
					{
						foundRecepie = true;
						bool getOut = false;
						multipleStacks = rec.amount[0];

						GameObject result = (GameObject)Instantiate(rec.result);

						float checkWeight = 0;
						checkWeight += selectedItems[0].CurrentItem.weight * rec.amount[0];
						
						if(inv.CanPickUp(result.GetComponentInChildren<Item>().weight - checkWeight))
						{
							
							for (int i = 0; i < selectedItems.Count; i++) 
							{
								int stackCount = selectedItems[i].Items.Count;
								for (int x = 0; x < stackCount; x++) 
								{
									print (multipleStacks.ToString());
									if(multipleStacks > 0)
									{
										selectedItems[i].CurrentItem.selected = false;
										selectedItems[i].RemoveItem();
										multipleStacks --;
									}
								}
							}

							result.name = rec.result.name;
							result.GetComponentInChildren<Item>().AddItem();
							
							//Message
							MessageBox.instance.SendMessage("I created a " + rec.result.name);
							SoundManager.instance.Spawn3DSound(inv.CraftingSound[Random.Range(0, inv.CraftingSound.Length)], player.transform.position, 1, 5);
						}
						else
						{
							MessageBox.instance.SendMessage("I'm carrying too much.");
							Destroy(result);
						}
					}
				}
			}
			else if(rec.items.Count == selectedItems.Count)
			{
				bool gotIn = false;
				allTrue = rec.items.Count;
				int checkAllTrue = 0;
				float checkWeight = 0;
				List<string> completed = new List<string>();
				bool errorInName = false;
				for (int i = 0; i < selectedItems.Count; i++) 
				{
					for (int n = 0; n < rec.items.Count; n++)
					{
						if(selectedItems[i].CurrentItem.transform.parent.name == rec.items[n].name)
						{
							for (int k = 0; k < completed.Count; k++) 
							{
								if(selectedItems[i].CurrentItem.transform.parent.name == completed[k])
									errorInName = true;
							}

							if(selectedItems[i].Items.Count >= rec.amount[n] && !errorInName)
							{
								checkAllTrue++;
								gotIn = true;
								completed.Add(rec.items[n].name);
								checkWeight += selectedItems[i].CurrentItem.weight;
							}
							errorInName = false;
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

					if(inv.CanPickUp(result.GetComponentInChildren<Item>().weight - checkWeight))
					{

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
						

						result.name = rec.result.name;
						result.GetComponentInChildren<Item>().AddItem();
						
						//Message
						MessageBox.instance.SendMessage("I created a " + rec.result.name);
						SoundManager.instance.Spawn3DSound(inv.CraftingSound[Random.Range(0, inv.CraftingSound.Length)], player.transform.position, 1, 5);
					}
					else
					{
						MessageBox.instance.SendMessage("I'm carrying too much.");
						Destroy(result);
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
			MessageBox.instance.SendMessage("I cant create anything with those items.");
            SoundManager.instance.Spawn3DSound(inv.iCantDoThat[Random.Range(0, inv.iCantDoThat.Length)], player.transform.position, 1, 5);
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

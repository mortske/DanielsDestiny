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

	private int allTrue;
	private bool foundRecepie;
	private Inventory inv;

	void Start ()
	{
		selectedItems = new List<Slot>();
		inv = GameObject.Find("Inventory").GetComponent<Inventory>();
	}

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.C))
		{
			CheckRecepies();
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

	//TODO coconut need 2 clicks to get highlighted

	public void CheckRecepies()
	{
		foreach (Recepie rec in recepies) 
		{
			if(rec.items.Count == selectedItems.Count)
			{
				bool gotIn = false;
				allTrue = rec.items.Count;
				int checkAllTrue = 0;
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
					inv.AddItem(rec.result.transform.FindChild("OverlapSphere").GetComponent<Item>());
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

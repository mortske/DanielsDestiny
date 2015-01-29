using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHight;

    public bool enabled {get; set;}
    public GameObject player;

	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
	public GameObject slotPrefab;

	private static Slot from, to;
	private List<GameObject> allSlots;
	public GameObject iconPrefab;
	private static GameObject hoverObject;
	private static int emptySlots;

	public Canvas canvas;
	private float hoverYOffset;
	public EventSystem eventSystem;
	public static int EmptySlots
	{
		get { return emptySlots; }
		set { emptySlots = value; }
	}

	void Start () 
	{
		CreateLayout();
	}

	void Update () 
	{
		if(Input.GetMouseButtonUp(0))
		{
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null)
            {
                DropItem();
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                
                to = null;
                from = null;
                hoverObject = null;
                
            }
		}

		if(hoverObject != null)
		{
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
			position.Set (position.x, position.y - hoverYOffset);
			hoverObject.transform.position = canvas.transform.TransformPoint(position);

		}
	}

	private void CreateLayout()
	{
		allSlots = new List<GameObject>();

		hoverYOffset = slotSize * 0.1f;

		emptySlots = slots;

		inventoryWidth = (slots/rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

		inventoryRect = GetComponent<RectTransform>();

		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHight);

		int columns = slots/rows;

		for (int y = 0; y < rows; y++) 
		{
			for (int x = 0; x < columns; x++) 
			{
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);

				RectTransform slotRect = newSlot.GetComponent<RectTransform>();

				newSlot.name = "Slot";

				newSlot.transform.SetParent(this.transform.parent);

				slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

				allSlots.Add(newSlot);

			}
		}


	}

	public bool AddItem(Item item)
	{
		if(item.maxSize == 1)
		{
			PlaceEmpty(item);
			return true;
		}
		else
		{
			foreach (GameObject slot in allSlots) 
			{
				Slot tmp = slot.GetComponent<Slot>();

				if(!tmp.isEmpty)
				{
					if(tmp.CurrentItem.name == item.name && tmp.IsAvailable)
					{
						tmp.AddItem(item);
						return true;
					}
				}
			}
			if(emptySlots > 0)
			{
				PlaceEmpty(item);
			}
		}

		return false;
	}

	private bool PlaceEmpty(Item item)
	{
		if (emptySlots > 0) 
		{
			foreach (GameObject slot in allSlots)
			{
				Slot tmp = slot.GetComponent<Slot>();

				if(tmp.isEmpty)
				{
					tmp.AddItem(item);
					emptySlots--;
					return true;
				}
			}
		}

		return false;
	}

	public void MoveItem(GameObject clicked)
	{
		if(from == null)
		{
            if (!clicked.GetComponent<Slot>().isEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;

            }
		}
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
                
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverObject = null;
        }
	}

    public void DropItem()
    {
        Transform itemRoot = from.CurrentItem.transform.parent;
        itemRoot.parent = null;
        itemRoot.gameObject.SetActive(true);
        from.CurrentItem.curSize = from.Items.Count;
    }
	public List<ItemSaveType> GetInventory()
	{
		List<ItemSaveType> saveList = new List<ItemSaveType>();
		
		for(int i = 0; i < allSlots.Count; i++)
		{
			if(allSlots[i].GetComponent<Slot>().isEmpty)
				saveList.Add(new ItemSaveType().StringType("0"));
			else
			{
				for(int x = 0; x < ItemManager.instance.itemList.Count; x++)
				{
					if(allSlots[i].GetComponent<Slot>().CurrentItem.transform.parent.name.Equals(ItemManager.instance.GetName(x)))
					{
						saveList.Add(new ItemSaveType().StringType((x+1).ToString()));
						saveList[i].SetSize = allSlots[i].GetComponent<Slot>().Items.Count;
						Debug.Log("Count:" + allSlots[i].GetComponent<Slot>().Items.Count);
					}
				}
				
			}
		}
		return saveList;
	}
	public void SetInventory(List<ItemSaveType> saved)
	{
		for(int i = 0; i < saved.Count; i++)
		{
			if(int.Parse(saved[i].type) == 0)
			{
				Debug.Log("Running!");
			}
			else
			{
				for(int x  = 0; x < saved[i].GetSize; x++)
				{
					GameObject test = Instantiate(ItemManager.instance.itemList[int.Parse(saved[i].type)-1], this.transform.position, Quaternion.identity)as GameObject;
					test.GetComponentInChildren<Item>().AddItem(player.transform.collider);
				}
			}
		}
	}
}

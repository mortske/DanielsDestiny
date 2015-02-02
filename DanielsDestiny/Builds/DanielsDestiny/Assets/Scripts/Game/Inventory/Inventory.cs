﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	private RectTransform inventoryRect;
	private float inventoryWidth, inventoryHight;

    public bool enabled { get; set; }
    public DialougeBoxInv db;

	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
	public GameObject slotPrefab;

	public static Slot from, to;
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
        transform.parent.GetComponent<Canvas>().enabled = enabled;
	}
	

	void Update () 
	{
		if(Input.GetMouseButtonUp(0))
		{
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null)
            {
                DropItem();
            }
		}

        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
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
					if(tmp.CurrentItem.transform.parent.name == item.transform.parent.name && tmp.IsAvailable)
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
			if(!clicked.GetComponent<Slot>().isEmpty)
			{
				from = clicked.GetComponent<Slot>();
				from.GetComponent<Image>().color = Color.gray;

				hoverObject = (GameObject)Instantiate(iconPrefab);
				hoverObject.GetComponent<Image>().sprite = clicked .GetComponent<Image>().sprite;
				hoverObject.name = "Hover";

				RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
				RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

				hoverObject.transform.SetParent(GameObject.Find("Canvas_inventory").transform, true);
				hoverObject.transform.localScale = from.gameObject.transform.localScale;

			}
		}
		else if(to == null)
		{
            
			to = clicked.GetComponent<Slot>();
			Destroy(GameObject.Find ("Hover"));
		}
		if(to != null && from != null)
		{
            
			Stack<Item> tmpTo = new Stack<Item>(to.Items);
            Stack<Item> tmp = new Stack<Item>();

            if (to.Items.Count > 0 && to.CurrentItem.transform.parent.name != from.CurrentItem.transform.parent.name)
            {
                //move if different items
                to.AddItems(from.Items);
                if (tmpTo.Count == 0)
                {
                    from.ClearSlot();

                }
                else
                {
                    from.AddItems(tmpTo);
                }
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                //split stack
                db = GameObject.Find("MessageboxInv").GetComponent<DialougeBoxInv>();
                db.Display(from.Items.Count - 1, 0, from.Items.Count / 2);
                CoroutineHandler.instance.WaitForDialouge(from, to, db, hoverObject);
            }
            else
            {
                //move items to empty slots + add items together of same itemType
                int toCount = from.Items.Count;
                if (to.Items.Count > 0)
                {
                    toCount = to.CurrentItem.maxSize - to.Items.Count;
                    if (toCount > from.Items.Count) toCount = from.Items.Count;
                }
                int fromCount = from.Items.Count;
                
                GameObject toGameObj = null;
                if(to.Items.Count > 0)
                    toGameObj = to.CurrentItem.transform.parent.gameObject;

                for (int i = 0; i < toCount; i++)
                {
                    to.AddItem(from.Items.Pop());
                    if(from != to)
                        fromCount--;
                }
                if (fromCount == 0)
                {
                    from.ClearSlot();
                    Destroy(toGameObj);
                }
                for (int i = 0; i < fromCount - 1; i++)
                {
                    from.AddItems(from.Items);
                }
            }

			from.GetComponent<Image>().color = Color.white;
			to = null;
			from = null;
			hoverObject = null;
		}
	}

    public void DropItem()
    {
        db = GameObject.Find("MessageboxInv").GetComponent<DialougeBoxInv>();
        db.Display(from.Items.Count, 0, from.Items.Count / 2);
        CoroutineHandler.instance.DropItemDialouge(db, hoverObject);
        
        //TODO: drop specific amount
        //int dropCount = from.Items.Count / 2;
        //int leaveCount = from.Items.Count - dropCount;

        //if (dropCount > 0)
        //{
        //    GameObject go = (GameObject)Instantiate(from.CurrentItem.transform.parent.gameObject, Player.instance.transform.position, Quaternion.identity);
        //    go.name = from.CurrentItem.transform.parent.name;
        //    go.SetActive(true);
        //    Item item = go.GetComponentInChildren<Item>();
        //    item.curSize = dropCount;
        //    go.transform.parent = null;
        //}

        //if (leaveCount == 0)
        //{
        //    from.ClearSlot();
        //}
        //else
        //{
        //    Debug.Log(leaveCount);
        //    for (int i = 0; i < leaveCount; i++)
        //    {
        //        from.RemoveItem();
        //    }
        //}

        //from.GetComponent<Image>().color = Color.white;
        
        //Destroy(GameObject.Find("Hover"));
        //to = null;
        //from = null;
        //hoverObject = null;
    }
}

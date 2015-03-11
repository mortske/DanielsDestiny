using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

	private Stack<Item> items;
    public bool onMouseHover = false;

	[HideInInspector]
    public RectTransform itemNameBox;
    Text itemNameBoxText;
    RectTransform itemNameBoxBox;


	public Stack<Item> Items 
	{
		get {return items;}
		set {items = value;}
	}

	public Text stackTxt;

	public Sprite slotEmpty;
	public Sprite slotHighlight;

	public bool isEmpty
	{
		get {return items.Count == 0; }
	}

	public bool IsAvailable
	{
		get { return CurrentItem.maxSize > items.Count; }
	}

	public Item CurrentItem
	{
		get {return items.Peek(); }
	}

	void Start ()
	{
		items = new Stack<Item>();
		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform txtRect = stackTxt.GetComponent<RectTransform>();

        itemNameBox = GameObject.Find("ItemNameBox").GetComponent<RectTransform>();
        itemNameBoxText = itemNameBox.GetComponentInChildren<Text>();
        itemNameBoxBox = itemNameBox.GetComponentsInChildren<RectTransform>()[1];
        itemNameBoxText.enabled = false;
        itemNameBoxBox.GetComponent<Image>().enabled = false;

		int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
		stackTxt.resizeTextMaxSize = txtScaleFactor;
		stackTxt.resizeTextMinSize = txtScaleFactor;
		
		txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
		txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
	}

	public void AddItem(Item item)
	{
		items.Push(item);

		if(items.Count > 1)
		{
			stackTxt.text = items.Count.ToString();
		}

		ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
	}
	
	public void AddItems(Stack<Item> items)
	{
		this.items = new Stack<Item>(items);
		
		stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
		
		ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
	}

	public void ChangeSprite(Sprite neutral, Sprite highlight)
	{
		GetComponent<Image>().sprite = neutral;

		SpriteState st = new SpriteState();

		st.highlightedSprite = highlight;
		st.pressedSprite = neutral;

		GetComponent<Button>().spriteState = st;
	}
	
	public void UseItem()
	{
		if(!isEmpty)
		{
			items.Pop().Use();

			stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

			if(isEmpty)
			{
				ChangeSprite(slotEmpty, slotHighlight);
				Inventory.EmptySlots++;
			}
		}
	}

    public void RemoveItem()
    {
        Item i = items.Pop();
		stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

		if(isEmpty)
		{
			ChangeSprite(slotEmpty, slotHighlight);
			Inventory.EmptySlots++;
		}
		
		if(gameObject.name != "EquipSlot")
		{
			Player.instance.inventory.currWeight -= i.weight;
			Player.instance.inventory.PrintInventoryWeight();
			i.curSize --;
			if(i.Parent && items.Count <= 0 || i.curSize <= 0)
				Destroy(i.Parent);
		}
    }

	public void ClearSlot()
	{
		items.Clear();
		ChangeSprite(slotEmpty, slotHighlight);
		stackTxt.text = string.Empty;
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right)
		{
			if(items.Count == 0)
			{
				if(CraftingDictionary.SelectedItems.Count > 0)
				{
					CraftingDictionary.ClearSelectedItem();
				}
			}
			else
			{
				if(!CurrentItem.selected && gameObject.name != "EquipSlot")
				{
					ChangeSprite(CurrentItem.spriteHighlighted, CurrentItem.spriteHighlighted);
					gameObject.GetComponent<Image>().color = Color.gray;
					CurrentItem.selected = true;
					CraftingDictionary.SelectedItems.Add(this);
				}
				else if(gameObject.name != "EquipSlot")
				{
					ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
					gameObject.GetComponent<Image>().color = Color.white;
					CurrentItem.selected = false;
					CraftingDictionary.SelectedItems.Remove(this);
				}
				else if(gameObject.name == "EquipSlot")
				{
					if(CurrentItem)
					{
						Player.instance.curEquipment = null;
                        Destroy(Player.instance.visualEquipment.gameObject);
                        Player.instance.curEquipment = null;
						RemoveItem();
						ChangeSprite(slotHighlight, slotHighlight);
					}
				}
			}
		}
	}

    void Update()
    {
        Hover();
    }
    public void StartHover()
    {
        if (items.Count > 0)
        {
            itemNameBoxText.enabled = true;
            itemNameBoxBox.GetComponent<Image>().enabled = true;

            itemNameBoxText.text = CurrentItem.Parent.name;
            itemNameBoxBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemNameBoxText.text.Length * 10);
            itemNameBoxText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemNameBoxText.text.Length * 10);
            onMouseHover = true;
        }
    }
    void Hover()
    {
        if (onMouseHover)
        {
            itemNameBox.position = Input.mousePosition;
        }
    }
    public void EndHover()
    {
        itemNameBoxText.enabled = false;
        itemNameBoxBox.GetComponent<Image>().enabled = false;
        onMouseHover = false;
    }
}

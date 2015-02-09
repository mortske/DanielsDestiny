using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PickupEventHandler : MonoBehaviour 
{
    public List<Item> itemQueue;
    public Canvas infobox;
    public Text infoBoxText;

    void Update()
    {
        if (itemQueue.Count > 0)
        {
            if (!PauseSystem.IsPaused)
            {
                if (Input.GetButtonDown("Pickup"))
                {
                    RemoveItemFromList(itemQueue[0], true);
                }
            }
        }
    }

    public void AddItemToList(Item item)
    {
        itemQueue.Add(item);
        ShowInfoBox();
    }

    public void RemoveItemFromList(Item item, bool pickup)
    {
        if (pickup)
        {
            item.AddItem();
        }
        itemQueue.Remove(item);
        if (itemQueue.Count == 0)
            HideInfoBox();
        else
            ShowInfoBox();
    }

    void ShowInfoBox()
    {
        Item i = itemQueue[0];
        infoBoxText.text = "Press \"PickupKey\" to pickup " + i.Name + " x" + i.curSize;
        infobox.gameObject.SetActive(true);
    }

    void HideInfoBox()
    {
        infobox.gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;

public class Reusable : Item 
{
    public ItemEffect[] effects;
    public GameObject otherItem;
    public bool used;

    public override void Use()
    {
        base.Use();
        if (!used)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                Player.instance.status.GetBar(effects[i].effectTo).adjustCur(effects[i].adjustment);
            }
        }
            GameObject g = (GameObject)Instantiate(otherItem);
            Item item = g.GetComponentInChildren<Item>();
            g.name = otherItem.name;
            item.AddItem();
            Player.instance.inventory.AddItem(item);
            CraftingDictionary.SelectedItems[0].RemoveItem();
    }
}

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
			//Player.instance.inventory.craftingDictionary.CraftItems();
			GameObject g = (GameObject)Instantiate(otherItem);
			g.GetComponentInChildren<Item>().AddItem();
			g.name = otherItem.name;
			Destroy(this.transform.parent.gameObject);
        }
    }
}

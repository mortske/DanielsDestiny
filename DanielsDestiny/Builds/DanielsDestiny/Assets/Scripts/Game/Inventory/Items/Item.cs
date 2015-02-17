using UnityEngine;
using System.Collections;


public class Item : MonoBehaviour 
{
    public bool usable;
	public bool constructable;
	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;
	public int maxSize;
    public int curSize = 1;
	public bool selected;
	public float weight;

	[HideInInspector]
	public bool equipable;
	private bool pickedUp;

    public string Name
    {
        get { return transform.parent.name; }
    }

    public GameObject Parent
    {
        get { return transform.parent.gameObject; }
    }

    public virtual void EquipItem()
    {
        
    }

	public virtual void Use()
	{
		Player.instance.inventory.currWeight -= weight;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.instance.pickupEventHandler.AddItemToList(this);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.instance.pickupEventHandler.RemoveItemFromList(this, false);
        }
    }

    public void AddItem()
    {
        EquipItem();
        if (Player.instance.curBiome != null)
            Player.instance.curBiome.PickItem(this.transform.parent.gameObject);
        Player player = Player.instance;

		for (int i = 0; i < curSize; i++)
		{
			if(player.inventory.CheckWeight(weight))
			{
				pickedUp = true;
				player.inventory.AddItem(this);
			}
			else
				MessageBox.instance.SendMessage("I am carrying too much");
		}
		if(pickedUp)
		{
			transform.parent.position = player.transform.position;
			transform.parent.gameObject.SetActive(false);
			transform.parent.parent = player.transform;
		}
		
    }
}

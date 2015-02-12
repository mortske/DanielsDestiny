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

    public string Name
    {
        get { return transform.parent.name; }
    }

    public GameObject Parent
    {
        get { return transform.parent.gameObject; }
    }

	public virtual void Use()
	{
        Debug.Log("used an item");
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
        if (Player.instance.curBiome != null)
            Player.instance.curBiome.PickItem(this.transform.parent.gameObject);
        Player player = Player.instance;
		for (int i = 0; i < curSize; i++)
		{
			player.inventory.AddItem(this);
		}
		
		transform.parent.position = player.transform.position;
		transform.parent.gameObject.SetActive(false);
		transform.parent.parent = player.transform;
    }
}

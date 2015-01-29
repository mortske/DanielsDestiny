using UnityEngine;
using System.Collections;

public enum ItemType {Health, Water};

public class Item : MonoBehaviour {

	public ItemType type;

	public Sprite spriteNeutral;

	public Sprite spriteHighlighted;

	public int maxSize;

	public void Use()
	{
		switch (type) 
		{
		case ItemType.Health:
			Debug.Log("I just used Health");
			break;
		case ItemType.Water:
			Debug.Log("I just used Water");
			break;
		}
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.GetComponent<Player>().inventory.AddItem(this);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}

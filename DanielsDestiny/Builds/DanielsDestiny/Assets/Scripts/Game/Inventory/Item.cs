using UnityEngine;
using System.Collections;


public class Item : MonoBehaviour {


	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;
	public int maxSize;
    public int curSize = 1;

	public virtual void Use()
	{
        Debug.Log("used an item");
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!PauseSystem.IsPaused)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                	Player.instance.curBiome.PickItem(this.transform.parent.gameObject);
                    AddItem(other);
                }
            }
        }
    }
    public void AddItem(Collider other)
    {
		Player player = other.GetComponent<Player>();
		for (int i = 0; i < curSize; i++)
		{
			player.inventory.AddItem(this);
		}
		
		transform.parent.position = player.transform.position;
		transform.parent.gameObject.SetActive(false);
		transform.parent.parent = player.transform;
    }
}

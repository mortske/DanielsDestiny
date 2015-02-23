using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour 
{
    public GameObject[] itemPrefabs;
    public int life;
    public bool parentIsRoot;
    public AudioClip chopSound;

	EqualsTo retro;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnScreenInformationbox.instance.ShowBox("press \"" + InputManager.GetPrimaryKeyName("Pickup") + "\" to chop");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (InputManager.GetKeyDown("Pickup"))
            {
                if (Player.instance.curEquipment != null && Player.instance.curEquipment.Name == "Machete")
                {
                    Chop();
                }
                else
                {
                    MessageBox.instance.SendMessage("I need to equip something sharp");
                }
            }
        }
    }

    void Chop()
    {
        int noOfItems = Random.Range(2, 6);
        for (int i = 0; i < noOfItems; i++)
        {
            GameObject randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            GameObject go = (GameObject)Instantiate(randomItem);
            go.name = randomItem.name;
			if(Player.instance.inventory.CanPickUp(go.GetComponentInChildren<Item>().weight))
			{
            	go.GetComponentInChildren<Item>().AddItem();
			}
			else
			{
				MessageBox.instance.SendMessage("I am carrying too much");
				go.SetActive(true);
				go.transform.parent = null;
				go.transform.position = new Vector3(Player.instance.transform.position.x + Random.Range(0, 5), Player.instance.transform.position.y, Player.instance.transform.position.z + Random.Range(0, 5));
				if(Player.instance.curBiome != null)
					Player.instance.curBiome.AddWorldDrop(go);
			}
        }
        life--;
        SoundManager.instance.Spawn3DSound(chopSound, Player.instance.transform.position, 1, 5);
        if (life == 0)
        {
            OnScreenInformationbox.instance.HideBox();
            GameObject toDestroy = gameObject;
            if (parentIsRoot)
                toDestroy = transform.parent.gameObject;
            Destroy(toDestroy);
        }
    }

	public void CheckHealth(int health)
	{
		life = health;
		if (life == 0)
		{
			OnScreenInformationbox.instance.HideBox();
			GameObject toDestroy = gameObject;
			if (parentIsRoot)
				toDestroy = transform.parent.gameObject;
			Destroy(toDestroy);
		}
	}
	public int SaveTree()
	{
		return life;
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnScreenInformationbox.instance.HideBox();
        }
    }
}

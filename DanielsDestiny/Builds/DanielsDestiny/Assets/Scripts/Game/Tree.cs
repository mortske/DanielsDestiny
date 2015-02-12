using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour 
{
    public GameObject[] itemPrefabs;
    public int life;
    public bool parentIsRoot;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnScreenInformationbox.instance.ShowBox("press \"PickupKey\" to chop");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetButtonDown("Pickup"))
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
            go.GetComponentInChildren<Item>().AddItem();
        }
        life--;
        if (life == 0)
        {
            OnScreenInformationbox.instance.HideBox();
            GameObject toDestroy = gameObject;
            if (parentIsRoot)
                toDestroy = transform.parent.gameObject;
            Destroy(toDestroy);
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnScreenInformationbox.instance.HideBox();
        }
    }
}

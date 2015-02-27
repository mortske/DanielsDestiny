using UnityEngine;
using System.Collections;

public class BerryBush : MonoBehaviour 
{
    public GameObject berryVisual;
    public GameObject berryItemPrefab;
    public float amount;
    public float respawnTimeMax;
    public float respawnTimeMin;
    bool hasBerries;

    void Start()
    {
        Reset();
    }

    void Reset()
    {
        berryVisual.SetActive(true);
        hasBerries = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(hasBerries)
                OnScreenInformationbox.instance.ShowBox("Press \"" + InputManager.GetPrimaryKeyName("Pickup") + "\" to pick berries");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (InputManager.GetKeyDown("Pickup"))
            {
                if (hasBerries)
                {
                    Use();
                }
            }
        }
    }

    void Use()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject g = (GameObject)Instantiate(berryItemPrefab);
            Item item = g.GetComponentInChildren<Item>();
            g.name = berryItemPrefab.name;
            item.AddItem();
        }
        berryVisual.SetActive(false);
        hasBerries = false;
        OnScreenInformationbox.instance.HideBox();
        Invoke("Reset", Random.Range(respawnTimeMin, respawnTimeMax + 1));
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnScreenInformationbox.instance.HideBox();
        }
    }
}

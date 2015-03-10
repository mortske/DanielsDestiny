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
	public float[] GetBush()
	{
		float[] val = new float[4]{0,0,0,0};
		val[0] = amount;
		val[1] = respawnTimeMax;
		val[2] = respawnTimeMin;
		if(hasBerries)
			val[3] = 1;
		else
			val[3] = 0;
		return val;
	}
	public void SetBush(float[] val)
	{
		if(val != null)
		{
			if(val.Length == 4)
			{
				amount = val[0];
				respawnTimeMax = val[1];
				respawnTimeMin = val[2];
				if(val[3] == 1)
					hasBerries = true;
				else
				{
					berryVisual.SetActive(false);
					hasBerries = false;
					OnScreenInformationbox.instance.HideBox();
					Invoke("Reset", Random.Range(respawnTimeMin, respawnTimeMax + 1));
				}
			}
			else
			{
				Reset();
			}
		}
		else
		{
			Reset();
		}
	}
}

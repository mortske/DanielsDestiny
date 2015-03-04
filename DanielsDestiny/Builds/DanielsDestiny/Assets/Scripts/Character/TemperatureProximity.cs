using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TemperatureProximity : MonoBehaviour 
{
    public float heatTick;

    public List<TemperatureSource> sources;

    void Start()
    {
        sources = new List<TemperatureSource>();
        StartCoroutine("Tick");
    }

    IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(heatTick);
            
            if (sources.Count == 0)
            {
                if (Player.instance.status.temperatureAdjustment > 0)
                    Player.instance.status.temperatureAdjustment--;

                if (Player.instance.status.temperatureAdjustment < 0)
                    Player.instance.status.temperatureAdjustment++;
            }
            else
            {
                for (int i = 0; i < sources.Count; i++)
                {
                    Player.instance.status.temperatureAdjustment += sources[i].adjustment;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TempSource")
        {
            sources.Add(other.GetComponent<TemperatureSource>());
			if(other.transform.parent && other.transform.parent.gameObject.name == "Fire")
				CraftingDictionary.InsideArea = true;
			else if(other.gameObject.name == "WaterSource")
				CraftingDictionary.CanBeFilled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "TempSource")
        {
            sources.RemoveAt(0);
			if(other.transform.parent && other.transform.parent.gameObject.name == "Fire")
				CraftingDictionary.InsideArea = false;
			else if(other.gameObject.name == "WaterSource")
				CraftingDictionary.CanBeFilled = false;
        }
    }
}

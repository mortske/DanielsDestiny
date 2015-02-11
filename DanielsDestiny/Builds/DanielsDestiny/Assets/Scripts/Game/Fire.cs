using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour 
{
    public float tickTime;
    public float adjustmentUp;
    public float adjustmentDown;
    public float myCooling;
    public float heatCap;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
			CraftingDictionary.InsideArea = true;
            myCooling = 0;
            StartCoroutine(HeatUp());
        }
    }

    IEnumerator HeatUp()
    {
        while (Player.instance.status.temperature.cur < heatCap)
        {
            yield return new WaitForSeconds(tickTime);
            myCooling += adjustmentUp;
            Player.instance.status.temperatureAdjustment += adjustmentUp;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
		{
			CraftingDictionary.InsideArea = false;
            StopAllCoroutines();
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown()
    {
        while (myCooling > 0)
        {
            yield return new WaitForSeconds(tickTime);
            myCooling -= adjustmentDown;
            Player.instance.status.temperatureAdjustment -= adjustmentDown;
        }
    }
}

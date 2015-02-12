using UnityEngine;
using System.Collections;

public class Watersource : MonoBehaviour 
{
    public float adjustment;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            OnScreenInformationbox.instance.ShowBox("Press \"PickupKey\" to drink");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.instance.status.thirst.adjustCur(adjustment);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            OnScreenInformationbox.instance.HideBox();
    }
}

using UnityEngine;
using System.Collections;

public class Watersource : MonoBehaviour 
{
    public float adjustment;

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
}

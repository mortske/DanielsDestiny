using UnityEngine;
using System.Collections;

public class DestroyOnKeyPress : MonoBehaviour 
{
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(gameObject);
            }
        }
    }
}

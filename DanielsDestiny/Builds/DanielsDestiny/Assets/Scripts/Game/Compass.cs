using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour 
{
    public GameObject group;
    public Transform pointer;

    void Update()
    {
        if(InputManager.GetKeyDown("Compass"))
        {
            if (group.activeInHierarchy)
            {
                group.SetActive(false);
            }
            else
            {
                group.SetActive(true);
            }
        }
        pointer.rotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
        //pointer.localRotation = Quaternion.Euler(0, -Input.compass.trueHeading, 0);
    }
}

﻿using UnityEngine;
using System.Collections;

public class Watersource : MonoBehaviour 
{
    public float adjustment;
    public AudioClip drinkSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            OnScreenInformationbox.instance.ShowBox("Press \"" + InputManager.GetPrimaryKeyName("Pickup") + "\" to drink");
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (InputManager.GetKeyDown("Pickup"))
            {
                Player.instance.status.thirst.adjustCur(adjustment);
                SoundManager.instance.Spawn3DSound(drinkSound, Player.instance.transform.position, 1, 5);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            OnScreenInformationbox.instance.HideBox();
    }
}

using UnityEngine;
using System.Collections;

public class Cactus : MonoBehaviour 
{
    public GameObject unusedModel;
    public GameObject usedModel;
    public float adjustment;
    public AudioClip drinkSound;
    bool used;

    void Start()
    {
        used = false;
        usedModel.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(!used)
                OnScreenInformationbox.instance.ShowBox("Press \"" + InputManager.GetPrimaryKeyName("Pickup") + "\" to drink from cactus");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!used && InputManager.GetKeyDown("Pickup"))
            {
                if (Player.instance.curEquipment != null && Player.instance.curEquipment.Name == "Machete")
                {
                    Use();
                }
                else
                {
                    MessageBox.instance.SendMessage("I should equip something sharp");
                }
            }
        }
    }

    void Use()
    {
        SoundManager.instance.Spawn3DSound(drinkSound, Player.instance.transform.position, 1, 5);
        unusedModel.SetActive(false);
        usedModel.SetActive(true);
        Player.instance.status.thirst.adjustCur(adjustment);
        used = true;
        OnScreenInformationbox.instance.HideBox();
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnScreenInformationbox.instance.HideBox();
        }
    }
}

using UnityEngine;
using System.Collections;

public class Cactus : MonoBehaviour 
{
    public GameObject unusedModel;
    public GameObject usedModel;
    public float adjustment;
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
                OnScreenInformationbox.instance.ShowBox("Press \"PickupKey\" to drink from cactus");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!used && Input.GetButtonDown("Pickup"))
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

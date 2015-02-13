using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour 
{
    public GameObject trapUntriggered;
    public GameObject trapTriggered;
    public GameObject[] item;

    public float minSpawnTime;
    public float maxSpawnTime;
    float spawnTime;

    enum trapStates
    {
        unactivated,
        activated,
        empty
    }
    trapStates trapstate;

    public void Start()
    {
        SetupTrap();
    }

    void SetupTrap()
    {
        trapstate = trapStates.unactivated;
        trapTriggered.SetActive(false);
        trapUntriggered.SetActive(true);
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("ActivateTrap", spawnTime);
    }

    void ActivateTrap()
    {
        trapTriggered.SetActive(true);
        trapUntriggered.SetActive(false);
        trapstate = trapStates.activated;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnScreenInformationbox.instance.HideBox();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (trapstate == trapStates.activated)
                OnScreenInformationbox.instance.ShowBox("press \"PickupKey\" to empty the trap");
            else if (trapstate == trapStates.empty)
                OnScreenInformationbox.instance.ShowBox("press \"PickupKey\" to set the trap up");

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (trapstate == trapStates.activated)
                {
                    for (int i = 0; i < item.Length; i++)
                    {
                        GameObject go = (GameObject)Instantiate(item[i]);
                        Item newItem = go.GetComponentInChildren<Item>();
                        go.name = item[i].name;
                        Player.instance.inventory.AddItem(newItem);
                        go.transform.position = Player.instance.transform.position;
                        go.transform.gameObject.SetActive(false);
                        go.transform.parent = Player.instance.transform;
                        trapstate = trapStates.empty;
                        OnScreenInformationbox.instance.ShowBox("press \"PickupKey\" to set the trap up");
                    }
                }
                else if (trapstate == trapStates.empty)
                {
                    foreach (GameObject slotgo in Player.instance.inventory.AllSlots)
                    {
                        Slot slot = slotgo.GetComponent<Slot>();
                        if (slot.Items.Count > 0)
                        {
                            if (slot.CurrentItem.Name == "Bug")
                            {
                                OnScreenInformationbox.instance.HideBox();
                                slot.RemoveItem();
                                SetupTrap();
                                return;
                            }
                        }
                    }
                    MessageBox.instance.SendMessage("I need something to bait the trap");
                }
            }
        }
    }
}

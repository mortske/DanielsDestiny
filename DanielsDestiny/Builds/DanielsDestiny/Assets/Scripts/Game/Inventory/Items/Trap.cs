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

    bool activated;

    public void Start()
    {
        SetupTrap();
    }

    void SetupTrap()
    {
        activated = false;
        trapTriggered.SetActive(false);
        trapUntriggered.SetActive(true);
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        Invoke("ActivateTrap", spawnTime);
    }

    void ActivateTrap()
    {
        trapTriggered.SetActive(true);
        trapUntriggered.SetActive(false);
        activated = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && activated)
        {
            if (Input.GetKeyDown(KeyCode.E))
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
                    SetupTrap();
                }
            }
        }
    }
}

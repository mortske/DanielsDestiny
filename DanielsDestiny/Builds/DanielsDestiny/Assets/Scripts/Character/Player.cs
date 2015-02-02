using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player instance;

    public MouseLook mouseLook { get; set; }
    public CharacterMotor motor { get; set; }
    public Status status { get; set; }
    public Inventory inventory;
    
    public BiomeItems curBiome;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mouseLook = GetComponent<MouseLook>();
        motor = GetComponent<CharacterMotor>();
        status = GetComponent<Status>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Inventory"))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        if (!PauseSystem.IsPaused && !inventory.enabled)
        {
            PauseSystem.Pause(true);
            inventory.enabled = true;
            inventory.transform.parent.GetComponent<Canvas>().enabled = true;
            return;
        }
        if (PauseSystem.IsPaused && inventory.enabled)
        {
            PauseSystem.Pause(false);
            inventory.enabled = false;
            inventory.transform.parent.GetComponent<Canvas>().enabled = false;
            return;
        }
    }
}

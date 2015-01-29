using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public MouseLook mouseLook { get; set; }
    public CharacterMotor motor { get; set; }
    public Status status { get; set; }
    public Inventory inventory;

    void Start()
    {
        mouseLook = GetComponent<MouseLook>();
        motor = GetComponent<CharacterMotor>();
        status = GetComponent<Status>();
    }

    void Update()
    {
        if (Input.GetButton("Inventory"))
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
        }
    }
}

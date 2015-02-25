using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player instance;

	InventorySplashScreenGUI invSplash;


    public MouseLook mouseLook { get; set; }
    public CharacterMotor motor { get; set; }
    public Status status { get; set; }
    public Inventory inventory;
    public PickupEventHandler pickupEventHandler { get; set; }
    [HideInInspector] public Equip curEquipment;
    [HideInInspector] public GameObject visualEquipment;
    public Transform handPoint;
    public BiomeItems curBiome;

	private bool hasShownSplashScreen;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
		invSplash = GameObject.Find("SystemManagement").GetComponent<InventorySplashScreenGUI>();
        Screen.showCursor = false;

        mouseLook = GetComponent<MouseLook>();
        motor = GetComponent<CharacterMotor>();
        status = GetComponent<Status>();
        pickupEventHandler = GetComponent<PickupEventHandler>();
    }

    void Update()
    {
		if(!StartGameSplashScreenGUI.startSplashScreenIsActive) {
			if(!InventorySplashScreenGUI.inventorySplashScreenIsActive) {
				if(InputManager.GetKeyDown("Inventory"))
		        {
					print ("hej");
		            ToggleInventory();
					invSplash.StartInventorySplashScreen();
		        }
			}
		}
    }

    public void ToggleInventory()
    {
		if(!StartGameSplashScreenGUI.startSplashScreenIsActive) {
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
}

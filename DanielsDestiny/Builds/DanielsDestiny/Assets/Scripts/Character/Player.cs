using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    public static Player instance;

	SplashScreenGUI splashScreen;

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
		splashScreen = GameObject.Find("SystemManagement").GetComponent<SplashScreenGUI>();
        Screen.showCursor = false;

        mouseLook = GetComponent<MouseLook>();
        motor = GetComponent<CharacterMotor>();
        status = GetComponent<Status>();
        pickupEventHandler = GetComponent<PickupEventHandler>();
    }

    void Update()
    {
		print (SplashScreenGUI.splashScreenIsActive);
		if(!SplashScreenGUI.splashScreenIsActive) {
			if(Input.GetButtonDown("Inventory"))
	        {
	            ToggleInventory();
				if(!hasShownSplashScreen && !splashScreen.disableSplashScreen) {
					hasShownSplashScreen = true;
					splashScreen.InventorySplashScreen(true);
					SplashScreenGUI.splashScreenIsActive = true;
				}
	        }
		}
    }

    public void ToggleInventory()
    {
		if(!SplashScreenGUI.splashScreenIsActive) {
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

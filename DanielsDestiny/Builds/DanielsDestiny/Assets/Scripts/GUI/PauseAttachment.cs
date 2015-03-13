using UnityEngine;
using System.Collections;

public class PauseAttachment : MonoBehaviour {
	
	public static bool isPaused;
	public static bool returnToMenu;
	public static bool isInPauseMenu;
	public static bool canBePaused;
	public static bool yesOrNoMenuActive;
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(!yesOrNoMenuActive) {
				PauseUnPause();
			}
		}
	}
	
	public static void PauseUnPause () {
		if (!StartGameSplashScreenGUI.startSplashScreenIsActive) {
			if (!InventorySplashScreenGUI.inventorySplashScreenIsActive) {
				if (!Player.instance.inventory.enabled) {
					if(!DiaryScript.instance.IsActive())
					{
						if (canBePaused && !isInPauseMenu) {
							isPaused = true;
							PauseSystem.Pause(true);
						}
						if (canBePaused && isInPauseMenu) {
							isPaused = false;
							PauseSystem.Pause(false);
						}
						if (!canBePaused && isInPauseMenu) {
							isPaused = false;
							PauseSystem.Pause(false);
						}
					}
					else
					{
						DiaryScript.instance.Disable();
					}

				} else if (Player.instance.inventory.enabled) {
					Player.instance.ToggleInventory();
				}
			}
		}
	}
}

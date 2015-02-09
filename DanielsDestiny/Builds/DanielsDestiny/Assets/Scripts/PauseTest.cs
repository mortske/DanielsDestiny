using UnityEngine;
using System.Collections;

public class PauseTest : MonoBehaviour {

	public static bool isPaused;
	public static bool returnToMenu;
	public static bool isInPauseMenu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(!isPaused) {
				//pause
				PauseSystem.Pause(true);
				isPaused = true;
			} else if (MenuScript.canBePaused == false && isInPauseMenu == true) {
				MenuScript.canBePaused = true;
				isPaused = false;
				PauseSystem.Pause(false);
			} else {
				//unpause
				PauseSystem.Pause(false);
				isPaused = false;
			}
		}
	}
}

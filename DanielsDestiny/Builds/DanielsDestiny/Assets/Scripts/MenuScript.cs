using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {


	#region Canvases
	private Canvas menuCanvas;
	private Canvas howToPlayCanvas;
	private Canvas settingsCanvas;
	private Canvas creditsCanvas;
	private Canvas highscoreCanvas;
	private Canvas pauseCanvas;
	private Canvas controlsCanvas;
	private Canvas craftingCanvas;
	private Canvas interactionCanvas;
	private Canvas deathCanvas;
	private Canvas backStoryCanvas;
	#endregion

	private List<Canvas> CanvasList = new List<Canvas>();
	private List<Canvas> InfoCanvasList = new List<Canvas>();

	public static bool canBePaused;
	private static bool isPaused;
	private static bool returnToMenu;
	private static bool isInPauseMenu;
	private static bool currentScene;

	void Update () {
		isInPauseMenu = PauseTest.isInPauseMenu;
		isPaused = PauseTest.isPaused;
		returnToMenu = PauseTest.returnToMenu;
		if(canBePaused) {
			if(isPaused) {
				ChangeCanvas(pauseCanvas);
				canBePaused = false;
			} else {
				ResetCanvas(CanvasList);
			}
		}
	}
	// Use this for initialization
	void Start () {
		if(Application.loadedLevel == 0) { // set 0 to death scene
			#region main Canvases
			CanvasList.Add(menuCanvas = GameObject.Find("Canvas_StartMenu").GetComponent<Canvas>());
			CanvasList.Add(howToPlayCanvas = GameObject.Find("Canvas_HowToPlay").GetComponent<Canvas>());
			CanvasList.Add(settingsCanvas = GameObject.Find("Canvas_Settings").GetComponent<Canvas>());
			CanvasList.Add(creditsCanvas = GameObject.Find("Canvas_Credits").GetComponent<Canvas>());
			CanvasList.Add(highscoreCanvas = GameObject.Find("Canvas_Highscore").GetComponent<Canvas>());
			CanvasList.Add(pauseCanvas = GameObject.Find("Canvas_Pause").GetComponent<Canvas>());
			CanvasList.Add(backStoryCanvas = GameObject.Find("Canvas_BackStory").GetComponent<Canvas>());
			#endregion

			InfoCanvasList.Add(controlsCanvas = GameObject.Find("Canvas_HowToPlay_Controls").GetComponent<Canvas>());
			InfoCanvasList.Add(craftingCanvas = GameObject.Find("Canvas_HowToPlay_Crafting").GetComponent<Canvas>());
			InfoCanvasList.Add(interactionCanvas = GameObject.Find("Canvas_HowToPlay_Interaction").GetComponent<Canvas>());
			ChangeCanvas(menuCanvas);
			ResetCanvas(InfoCanvasList);
		} else {
			CanvasList.Add(pauseCanvas = GameObject.Find("Canvas_Pause").GetComponent<Canvas>());
			CanvasList.Add(settingsCanvas = GameObject.Find("Canvas_Settings").GetComponent<Canvas>());
			CanvasList.Add(howToPlayCanvas = GameObject.Find("Canvas_HowToPlay").GetComponent<Canvas>());
			canBePaused = true;

			InfoCanvasList.Add(controlsCanvas = GameObject.Find("Canvas_HowToPlay_Controls").GetComponent<Canvas>());
			InfoCanvasList.Add(craftingCanvas = GameObject.Find("Canvas_HowToPlay_Crafting").GetComponent<Canvas>());
			InfoCanvasList.Add(interactionCanvas = GameObject.Find("Canvas_HowToPlay_Interaction").GetComponent<Canvas>());
		}
	}

	private void ChangeCanvas (Canvas changeTo) {
		print (changeTo);
		if(changeTo == pauseCanvas) {
			PauseTest.isInPauseMenu = true;
			canBePaused = true;
			PauseTest.returnToMenu = false;
		} else {
			PauseTest.isInPauseMenu = false;
		}
		if(changeTo == menuCanvas) {
			PauseTest.returnToMenu = true;
		}
		for (int i = 0; i < CanvasList.Count; i++) {
			if(changeTo != CanvasList[i]) {
				CanvasList[i].enabled = false;
			} else {
				CanvasList[i].enabled = true;
			}
		}
	}

	private void ActivateInfoCanvas (Canvas changeTo) {
		for (int i = 0; i < InfoCanvasList.Count; i++) {
			if(changeTo != InfoCanvasList[i]) {
				InfoCanvasList[i].enabled = false;
			} else {
				InfoCanvasList[i].enabled = true;
			}
		}
	}

	private void ResetCanvas(List<Canvas> list) {
		for (int i = 0; i < list.Count; i++) {
			list[i].enabled = false;
		}
	}

	private void Back() {
		if(!returnToMenu) {
			ChangeCanvas(pauseCanvas);
		} else {
			ChangeCanvas(menuCanvas);
		}
	}

	#region Start Menu Buttons
	public void StartPlayGame () {
		ResetCanvas(InfoCanvasList);
		ResetCanvas(CanvasList);
		canBePaused = true;
		PauseTest.isPaused = false;
		Application.LoadLevel(1);
		//returnToMenu = false;
		//Start Game
	}

	public void StartLoadGame () {
		//Load Game and then Start - Continue
	}

	public void StartHowToPlay () {
		ResetCanvas(InfoCanvasList);
		ChangeCanvas(howToPlayCanvas);
	}

	public void StartSettings () {
		ChangeCanvas(settingsCanvas);
	}

	public void StartBackStory () {
		ChangeCanvas(backStoryCanvas);
	}

	public void StartHighscore () {
		ChangeCanvas(highscoreCanvas);
	}

	public void StartCredits () {
		ChangeCanvas(creditsCanvas);
	}

	public void StartQuit () {
		//Quit Game
	}
	#endregion

	#region How To Play Buttons
	public void HowToPlayControlsInfo () {
		ActivateInfoCanvas(controlsCanvas);
	}

	public void HowToPlayCraftingInfo () {
		ActivateInfoCanvas(craftingCanvas);
	}

	public void HowToPlayWorldInteractionInfo () {
		ActivateInfoCanvas(interactionCanvas);
	}

	public void HowToPlayBack () {
		ResetCanvas(InfoCanvasList);
		Back();
	}
	#endregion

	#region Settings Buttons
	public void SettingsBack () {
		Back();
	}
	#endregion

	#region Credits Buttons
	public void CreditsBack () {
		Back();
	}
	#endregion

	#region Back Story Buttons
	public void BackStoryBack () {
		Back();
	}
	#endregion

	#region Highschore Buttons
	public void HighscoreBack () {
		Back();
	}
	#endregion

	#region Pause Buttons
	public void PauseResume () {
		canBePaused = true;
		PauseTest.isPaused = false;
	}

	public void PauseHowToPlay () {
		ChangeCanvas(howToPlayCanvas);
	}

	public void PauseSettings () {
		ChangeCanvas(settingsCanvas);
	}

	public void PauseMenu () {
		ChangeCanvas(menuCanvas);
	}

	public void PauseQuit () {
		// Quit Game
	}
	#endregion

	#region Death Buttons
	public void DeathMenu () {
		//Load the first level
	}

	public void DeathLoadGame () {
		//Load and restart?
	}

	public void DeathEndGame () {
		//End the Game
	}
	#endregion

}

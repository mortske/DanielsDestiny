using UnityEngine;
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
	private Canvas areYouSureCanvas;
	#endregion
	
	private List<Canvas> CanvasList = new List<Canvas>();
	private List<Canvas> InfoCanvasList = new List<Canvas>();

	private static bool quitGame;
	private static bool menuGame;
	
	void Update () {
		if(PauseAttachment.canBePaused) {
			if(PauseAttachment.isPaused) {
				ChangeCanvas(pauseCanvas);
				PauseAttachment.isInPauseMenu = true;
				PauseAttachment.canBePaused = false;
			}
		}
		if(!PauseAttachment.isPaused && PauseAttachment.isInPauseMenu && !PauseAttachment.canBePaused) {
			ResetCanvas(CanvasList);
			PauseAttachment.isInPauseMenu = false;
			PauseAttachment.canBePaused = true;
		}
	}
	// Use this for initialization
	void Start () {
		if(Application.loadedLevel == 0) {
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
			Screen.showCursor = true;
		} else if(Application.loadedLevel == 2) {
			CanvasList.Add(deathCanvas = GameObject.Find("Canvas_Death").GetComponent<Canvas>());
		} else {
			areYouSureCanvas = GameObject.Find("Canvas_AreYouSure").GetComponent<Canvas>();
            
			PauseAttachment.canBePaused = true;
			areYouSureCanvas.enabled = false;

			CanvasList.Add(pauseCanvas = GameObject.Find("Canvas_Pause").GetComponent<Canvas>());
			CanvasList.Add(settingsCanvas = GameObject.Find("Canvas_Settings").GetComponent<Canvas>());
			CanvasList.Add(howToPlayCanvas = GameObject.Find("Canvas_HowToPlay").GetComponent<Canvas>());
			
			InfoCanvasList.Add(controlsCanvas = GameObject.Find("Canvas_HowToPlay_Controls").GetComponent<Canvas>());
			InfoCanvasList.Add(craftingCanvas = GameObject.Find("Canvas_HowToPlay_Crafting").GetComponent<Canvas>());
			InfoCanvasList.Add(interactionCanvas = GameObject.Find("Canvas_HowToPlay_Interaction").GetComponent<Canvas>());
            ResetCanvas(InfoCanvasList);
		}
	}
	
	private void ChangeCanvas (Canvas changeTo) {
		if(changeTo == menuCanvas) {
			PauseAttachment.returnToMenu = true;
			PauseAttachment.canBePaused = false;
			PauseAttachment.isInPauseMenu = false;
		}
		if(changeTo == pauseCanvas) {
			PauseAttachment.isInPauseMenu = true;
			PauseAttachment.returnToMenu = false;
		} else {
			PauseAttachment.isInPauseMenu = false;
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
		if(!PauseAttachment.returnToMenu) {
			ChangeCanvas(pauseCanvas);
		} else {
			ChangeCanvas(menuCanvas);
		}
	}
	
	#region Start Menu Buttons
	public void StartPlayGame () {
		ResetCanvas(CanvasList); // start game
		PauseAttachment.canBePaused = true;
		Application.LoadLevel(1);
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
        SettingsManager.instance.Save();
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
		PauseAttachment.PauseUnPause();
	}
	
	public void PauseHowToPlay () {
		ChangeCanvas(howToPlayCanvas);
	}
	
	public void PauseSettings () {
		ChangeCanvas(settingsCanvas);
	}
	
	public void PauseMenu () {
		menuGame = true;
		PauseAttachment.yesOrNoMenuActive = true;
		areYouSureCanvas.enabled = true;
		//Application.LoadLevel(0);
	}
	
	public void PauseQuit () {
		quitGame = true;
		PauseAttachment.yesOrNoMenuActive = true;
		areYouSureCanvas.enabled = true;
	}
	#endregion
	
	#region Death Buttons
	public void DeathMenu () {
		Application.LoadLevel(0);
	}
	
	public void DeathLoadGame () {
		//Load and restart?
	}
	
	public void DeathEndGame () {
		Application.Quit();
	}
	#endregion

	#region Are You Sure Buttons
	public void AreYouSureYes () {
		if(menuGame) {
			PauseAttachment.yesOrNoMenuActive = false;
			PauseAttachment.PauseUnPause();
			areYouSureCanvas.enabled = false;
			PauseAttachment.isInPauseMenu = false;
			menuGame = false;
			Application.LoadLevel(0);
			PauseAttachment.canBePaused = false;
		}
		if(quitGame) {
			Application.Quit();
		}
	}

	public void AreYouSureNo () {
		PauseAttachment.yesOrNoMenuActive = false;
		menuGame = false;
		quitGame = false;
		areYouSureCanvas.enabled = false;
	}
	#endregion
	
}


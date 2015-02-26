using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class StartSplash {
	public string name;
	public GameObject gameObject;
	public Image backgroundImage;
	public Text backgroundText;
	public Image arrowImage;
}
public class StartGameSplashScreenGUI : MonoBehaviour {

	public StartSplash[] splashImages;

	[HideInInspector]
	public static bool startSplashScreenIsActive;
	public bool disableSplashScreen;

	[HideInInspector]
	public bool exitOnce;

	private int currentSplash;

	#region Tutorial Title
	private Image tutorialTitleImage;
	private Text tutorialTitleText;
	private Text tutorialTitlePressKey;
	#endregion

	private Button startSkipButton;
	
	void Start () {
		if(!disableSplashScreen) {
			exitOnce = false;
			#region Tutorial Title Find
			tutorialTitleImage = GameObject.Find("TutorialBackground").GetComponent<Image>();
			tutorialTitleText = GameObject.Find("TutorialText").GetComponent<Text>();
			tutorialTitlePressKey = GameObject.Find("TutorialPressKey").GetComponent<Text>();
			#endregion
			startSkipButton = GameObject.Find("StartSplashSkipButton").GetComponent<Button>();
			if(disableSplashScreen) {
				DisableSplashScreenGameObjects();
			} else {
				StartSplashScreen();
			}
		}
	}

	void Update () {
		if(!disableSplashScreen) {
			if(!InventorySplashScreenGUI.inventorySplashScreenIsActive) {
				if(Input.GetKeyDown(KeyCode.Space)) {
					NextSplashInList();
				}
			}
		}
	}

	void Enable (StartSplash _image) {
		_image.backgroundImage.enabled = true;
		_image.backgroundText.enabled = true;
		_image.arrowImage.enabled = true;
	}
	#region Clear Methods
	void Clear (StartSplash _image) {
		_image.backgroundImage.enabled = false;
		_image.backgroundText.enabled = false;
		_image.arrowImage.enabled = false;
	}

	void ClearAll () {
		for (int i = 0; i < splashImages.Length; i++) {
			Clear(splashImages[i]);
		}
	}

	void ClearAllExceptSelected (StartSplash _current) {
		for (int i = 0; i < splashImages.Length; i++) {
			if(_current == splashImages[i]) {
				Enable(_current);
			} else {
				Clear(splashImages[i]);
			}
		}
	}
	#endregion
	
	void DisableTutorialTitle () {
		tutorialTitleImage.enabled = false;
		tutorialTitleText.enabled = false;
		tutorialTitlePressKey.enabled = false;
	}

	void EnableTutorialTitle () {
		tutorialTitleImage.enabled = true;
		tutorialTitleText.enabled = true;
		tutorialTitlePressKey.enabled = true;
	}

	void DisableSplashScreenGameObjects () {
		for (int i = 0; i < splashImages.Length; i++) {
			splashImages[i].gameObject.SetActive(false);
		}
	}

	public void StartSplashScreen() {
		startSkipButton.gameObject.SetActive(true);
		PauseSystem.Pause(true);
		EnableTutorialTitle();
		startSplashScreenIsActive = true;
		NextSplashInList();
		//pauseGame
	}
	void EndSplashScreen() {
		startSkipButton.gameObject.SetActive(false);
		PauseSystem.Pause(false);
		DisableTutorialTitle();
		startSplashScreenIsActive = false;
		currentSplash = splashImages.Length;
		ClearAll();
		//unpauseGame
	}
	
	public void SkipSplashScreen() {
		EndSplashScreen();
	}

	void NextSplashInList () {
		if(currentSplash < splashImages.Length) {
			ClearAllExceptSelected(splashImages[currentSplash]);
			currentSplash++;
		} else if (currentSplash == splashImages.Length) {
			if(!exitOnce) {
				exitOnce = true;
				EndSplashScreen();
			}
		}
	}

	public void SkipButton () {
		if(startSplashScreenIsActive) {
			exitOnce = true;
			EndSplashScreen();
		}
	}

}

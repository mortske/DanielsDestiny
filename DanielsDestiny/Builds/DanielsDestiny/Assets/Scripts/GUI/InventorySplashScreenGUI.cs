using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class InventorySplash {
	public string name;
	public GameObject gameObject;
	public Image image;
	public Text text;
	public Image arrowImage;
}
public class InventorySplashScreenGUI : MonoBehaviour {

	public InventorySplash[] splashImages;

	[HideInInspector]
	public static bool inventorySplashScreenIsActive;
	public bool disableSplashScreen;

	private int currentSplash;

	#region Tutorial Title
	private Image tutorialTitleImage;
	private Text tutorialTitleText;
	private Text tutorialTitlePressKey;
	#endregion

	private Button inventorySkipButton;
	
	void Start () {
		if(!disableSplashScreen) {
			#region Tutorial Title Find
			tutorialTitleImage = GameObject.Find("TutorialBackground").GetComponent<Image>();
			tutorialTitleText = GameObject.Find("TutorialText").GetComponent<Text>();
			tutorialTitlePressKey = GameObject.Find("TutorialPressKey").GetComponent<Text>();
			#endregion
			inventorySkipButton = GameObject.Find("InventorySplashSkipButton").GetComponent<Button>();
			inventorySkipButton.gameObject.SetActive(false);
			if(disableSplashScreen) {
				DisableInventorySplashScreenGameObjects();
			} else {
				ClearAll();
			}
		}
	}

	void Update () {
		if(!disableSplashScreen) {
			if(Input.GetKeyDown(KeyCode.Space)) {
				if(inventorySplashScreenIsActive) {
					NextSplashInList();
				}
			}
		}
	}

	void Enable (InventorySplash _image) {
		_image.image.enabled = true;
		_image.text.enabled = true;
		_image.arrowImage.enabled = true;
	}

	#region Clear Methods
	void Clear (InventorySplash _image) {
		_image.image.enabled = false;
		_image.text.enabled = false;
		_image.arrowImage.enabled = false;
	}

	void ClearAll () {
		for (int i = 0; i < splashImages.Length; i++) {
			Clear(splashImages[i]);
		}
	}
	
	void ClearAllExceptSelected (InventorySplash _current) {
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

	void DisableInventorySplashScreenGameObjects () {
		for (int i = 0; i < splashImages.Length; i++) {
			splashImages[i].gameObject.SetActive(false);
		}
	}

	public void StartInventorySplashScreen () {
		inventorySplashScreenIsActive = true;
		inventorySkipButton.gameObject.SetActive(true);
		EnableTutorialTitle();
		NextSplashInList();
	}

	void EndInventorySplashScreen () {
		inventorySplashScreenIsActive = false;
		inventorySkipButton.gameObject.SetActive(false);
		currentSplash = splashImages.Length;
		DisableTutorialTitle();
		ClearAll();
	}

	void SkipInventorySplashScreen () {
		EndInventorySplashScreen();
	}

	void NextSplashInList () {
		if(currentSplash < splashImages.Length) {
			ClearAllExceptSelected(splashImages[currentSplash]);
			currentSplash++;
		} else if (currentSplash == splashImages.Length) {
			EndInventorySplashScreen();
		}
	}
	public void SkipButton () {
		if(inventorySplashScreenIsActive) {
			EndInventorySplashScreen();
		}
	}
}

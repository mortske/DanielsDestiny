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
	
	void Start () {
		#region Tutorial Title Find
		//tutorialTitleImage = GameObject.Find("Insert Name Here").GetComponent<Image>();
		//tutorialTitleText = GameObject.Find("Insert Name Here").GetComponent<Text>();
		//tutorialTitlePressKey = GameObject.Find("Insert Name Here").GetComponent<Text>();
		#endregion
		if(disableSplashScreen) {
			DisableInventorySplashScreenGameObjects();
		} else {
			ClearAll();
		}
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			if(inventorySplashScreenIsActive) {
				NextSplashInList();
			}
		}
	
	}

	void Enable (InventorySplash _image) {
		_image.image.enabled = true;
		_image.text.enabled = true;
	}

	#region Clear Methods
	void Clear (InventorySplash _image) {
		_image.image.enabled = false;
		_image.text.enabled = false;
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

	void DisableInventorySplashScreenGameObjects () {
		for (int i = 0; i < splashImages.Length; i++) {
			splashImages[i].gameObject.SetActive(false);
		}
	}

	public void StartInventorySplashScreen () {
		inventorySplashScreenIsActive = true;
		NextSplashInList();
	}

	void EndInventorySplashScreen () {
		inventorySplashScreenIsActive = false;
		currentSplash = splashImages.Length;
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
}

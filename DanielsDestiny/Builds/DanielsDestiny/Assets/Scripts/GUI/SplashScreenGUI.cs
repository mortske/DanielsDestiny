using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SplashScreenGUI : MonoBehaviour {

	private Image Arrow;

	private Image pageOne;
	private Image pageTwo;
	private Image pageThree;
	private Image pageFour;

	private Text pageOneText;
	private Text pageTwoText;
	private Text pageThreeText;
	private Text pageFourText;

	private int currentPage;

	private Canvas splashScreenCanvasTitle;
	private Canvas splashScreenCanvas;

	public bool disableSplashScreen;
	public static bool splashScreenIsActive;

	private List<Image> Pages = new List<Image>();
	private List<Text> Texts = new List<Text>();

	// Use this for initialization
	void Start () {
		if(!disableSplashScreen) {
			currentPage = -1;

			splashScreenCanvas = GameObject.Find("Canvas_SplashScreen").GetComponent<Canvas>();
			splashScreenCanvasTitle = GameObject.Find("Canvas_SplashScreenTitle").GetComponent<Canvas>();
			Arrow = GameObject.Find("SplashScreenArrow").GetComponent<Image>();
			#region Pages Addition
			Pages.Add(pageOne = GameObject.Find("InventorySplashScreenOne").GetComponent<Image>());
			Pages.Add(pageTwo = GameObject.Find("InventorySplashScreenTwo").GetComponent<Image>());
			Pages.Add(pageThree = GameObject.Find("InventorySplashScreenThree").GetComponent<Image>());
			Pages.Add(pageFour = GameObject.Find("InventorySplashScreenFour").GetComponent<Image>());
			#endregion
			#region Texts Addition
			Texts.Add(pageOneText = GameObject.Find("PageOneText").GetComponent<Text>());
			Texts.Add(pageTwoText = GameObject.Find("PageTwoText").GetComponent<Text>());
			Texts.Add(pageThreeText = GameObject.Find("PageThreeText").GetComponent<Text>());
			Texts.Add(pageFourText = GameObject.Find("PageFourText").GetComponent<Text>());
			#endregion
		}
	}

	void Update () {
		if(splashScreenIsActive) {
			if(!disableSplashScreen && currentPage != Pages.Count) {
				if(Input.GetKeyDown(KeyCode.Space)) {
					NextPage();
				}
			}
		}
	}

	public void InventorySplashScreen (bool _bool) {
		if(currentPage < Pages.Count) {
			Arrow.enabled = true;
			splashScreenCanvas.enabled = true;
			splashScreenCanvasTitle.enabled = true;
		}
		if(_bool = true) {
			NextPage();
		} else {
			currentPage = Pages.Count + 1;
			DisableList(Pages);
			DisableList(Texts);
		}
	}

	void NextPage () {
		if(currentPage <= Pages.Count) {
			currentPage++;
			print(currentPage + " : " + Pages.Count);
			AdjustCurrentPage(currentPage);
		}
	}

	void DisableList(List<Text> _list) {
		for (int i = 0; i < _list.Count; i++) {
			_list[i].enabled = false;
		}
	}
    void DisableList(List<Image> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            _list[i].enabled = false;
        }
    }

	void AdjustCurrentPage (int _curr) {
		for (int i = 0; i < Pages.Count; i++) {
			if (i == _curr) {
				Pages[i].enabled = true;
				Texts[i].enabled = true;
			} else {
				Pages[i].enabled = false;
				Texts[i].enabled = false;
			}
		}
		if(_curr == Pages.Count) {
			Arrow.enabled = false;
			splashScreenIsActive = false;
			splashScreenCanvas.enabled = false;
		}
	}

	public void SkipSplashScreen () {
		currentPage = Pages.Count;
		Arrow.enabled = false;
		splashScreenCanvas.enabled = false;
		splashScreenCanvasTitle.enabled = false;
		splashScreenIsActive = false;

		DisableList(Pages);
		DisableList(Texts);

	}
}

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

	public bool disableSplashScreen;
	public static bool splashScreenIsActive;

	private List<Image> Pages = new List<Image>();
	private List<Text> Texts = new List<Text>();

	// Use this for initialization
	void Start () {
		if(!disableSplashScreen) {
			//splashScreenIsActive = true;
			//print (splashScreenIsActive);
			currentPage = -1;
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
				if(Input.GetKeyDown(KeyCode.Return)) {
					print (currentPage);
					NextPage();
				}
			}
		}
	}

	public void InventorySplashScreen (bool _bool) {
		splashScreenIsActive = _bool;
		if(_bool = true) {
			NextPage();
		} else {
			currentPage = Pages.Count + 1;
			DisableList(Pages);
			DisableList(Texts);
		}
	}

	void NextPage () {
		if(currentPage < Pages.Count) {
			currentPage++;
			AdjustCurrentPage(currentPage);
		} else {
			splashScreenIsActive = false;
		}
	}

	void DisableList (List<Text> _list) {
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
	}
}

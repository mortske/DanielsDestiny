using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HighscoreGUI : MonoBehaviour {

	private SaveHighscore saveHighscore;

	private Text playThrough0;
	private Text playThrough1;
	private Text playThrough2;
	private Text playThrough3;
	private Text playThrough4;
	private Text playThrough5;
	private Text playThrough6;
	private Text playThrough7;
	private Text playThrough8;
	private Text playThrough9;

	private List<Text> playthroughList = new List<Text>();
	
	private string[] numbers;
	// Use this for initialization
	void Start () {
		#region Add Playthroughs to List
		playthroughList.Add(playThrough0 = GameObject.Find("Text01").GetComponent<Text>());
		playthroughList.Add(playThrough1 = GameObject.Find("Text02").GetComponent<Text>());
		playthroughList.Add(playThrough2 = GameObject.Find("Text03").GetComponent<Text>());
		playthroughList.Add(playThrough3 = GameObject.Find("Text04").GetComponent<Text>());
		playthroughList.Add(playThrough4 = GameObject.Find("Text05").GetComponent<Text>());
		playthroughList.Add(playThrough5 = GameObject.Find("Text06").GetComponent<Text>());
		playthroughList.Add(playThrough6 = GameObject.Find("Text07").GetComponent<Text>());
		playthroughList.Add(playThrough7 = GameObject.Find("Text08").GetComponent<Text>());
		playthroughList.Add(playThrough8 = GameObject.Find("Text09").GetComponent<Text>());
		playthroughList.Add(playThrough9 = GameObject.Find("Text10").GetComponent<Text>());
		#endregion
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GetHighscore () {
		if(HighScore.instance.GetScore() != null) {
			if(HighScore.instance.GetScore().TimeSurvived != null){
				saveHighscore = HighScore.instance.GetScore();
				numbers = new string[saveHighscore.TimeSurvived.Length];
			
				for (int x = 0; x < saveHighscore.TimeSurvived.Length; x++) {
					numbers[x] = x + ". Day: " + saveHighscore.TimeSurvived[x][0].ToString() + " Hours: " + saveHighscore.TimeSurvived[x][1].ToString() + " Minutes: " + saveHighscore.TimeSurvived[x][2].ToString() + " Seconds: " + saveHighscore.TimeSurvived[x][3].ToString();
					playthroughList[x].text = numbers[x];
				}
			}
		}
	}
}

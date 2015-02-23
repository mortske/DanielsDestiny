using UnityEngine;
using System.Collections;

public class Saver : MonoBehaviour {
	bool _active = false;
	bool _showSave = false;
	float s_val = 0.0f;
	// Update is called once per frame
	void Update () {
		if(_active)
		{
			if(Input.GetKeyUp(KeyCode.E))
			{
				_showSave = true;
                PauseSystem.Pause(true);
			}
		}
	}
	void OnGUI()
	{
		if(_showSave)
		{
			GUI.Label(new Rect(Screen.width/2-(Screen.width/4), Screen.height/2, Screen.width/2, 20), "How many hours do you want to sleep?");
			s_val = GUI.HorizontalSlider(new Rect(Screen.width/2-Screen.width/4, Screen.height/2 + 40, Screen.width/2, 20), s_val, 0, 24);
			s_val = Mathf.Round(s_val);
			if(GUI.Button(new Rect(Screen.width/2-25, Screen.height/2+Screen.height/4, 50, 20), "Sleep"))
			{
				Save();
                PauseSystem.Pause(false);
			}
			if(GUI.Button(new Rect(Screen.width/2-25, Screen.height/2+Screen.height/3.5f, 50, 20), "Close"))
			{
				_showSave = false;
                PauseSystem.Pause(false);
			}
		}
	}
	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player")
		{
			if(!_showSave)
			{
				OnScreenInformationbox.instance.ShowBox("Press \"PickupKey\" to sleep");
				_active = true;
			}
		}	
	}
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			OnScreenInformationbox.instance.HideBox();
			_active = false;
		}
			
	}
	void Save()
	{
		MessageBox.instance.SendMessage("Saving... Woke up " + s_val + " hours later!");
		float CycleTime = GameTime.instance.GetDayCycleInSeconds();
//		Debug.Log("Daycycle in seconds: " + CycleTime);
		float curtime = GameTime.instance.TheTime;
		float totaltime = ((CycleTime/24)*s_val) + curtime; //Cycle/24 (How many seconds per hour). Times s_val
		float newTime = totaltime;
		while(newTime > CycleTime)
		{
			newTime = newTime - CycleTime;
		}
//		Debug.Log("The time was: " + GameTime.instance.TheTime + " The time should become " + newTime);
		GameTime.instance.TheTime = newTime;
		GameTime.instance.SetRotationOfSun(newTime);
		GameTime.instance.SaveTime();
		BiomeManager.instance.SaveGame();
		_showSave = false;
	}
}

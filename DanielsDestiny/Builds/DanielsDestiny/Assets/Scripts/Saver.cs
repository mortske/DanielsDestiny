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
			}
			if(GUI.Button(new Rect(Screen.width/2-25, Screen.height/2+Screen.height/3.5f, 50, 20), "Close"))
			{
				_showSave = false;
			}
		}
	}
	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player")
		{
			if(!_showSave)
			{
				_active = true;
				MessageBox.instance.SendMessage("Press 'E' To Sleep!");
			}
		}	
	}
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
			_active = false;
	}
	void Save()
	{
		MessageBox.instance.SendMessage("Saving... Woke up " + s_val + " hours later!");
		GameTime.instance.SaveTime();
		BiomeManager.instance.SaveGame();
	}
}

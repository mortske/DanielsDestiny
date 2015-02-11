using UnityEngine;
using System.Collections;

public class Saver : MonoBehaviour {
	bool _active = false;

	// Update is called once per frame
	void Update () {
		if(_active)
		{
			if(Input.GetKeyUp(KeyCode.E))
			{
				Save();
			}
		}
	}
	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Player")
			_active = true;
	}
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
			_active = false;
	}
	void Save()
	{
		GameTime.instance.SaveTime();
		BiomeManager.instance.SaveGame();
	}
}

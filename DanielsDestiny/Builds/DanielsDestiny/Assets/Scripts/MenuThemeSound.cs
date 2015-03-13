using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuThemeSound : MonoBehaviour {


	public Slider volume;
	private AudioSource audioSource;

	void Start () {
		audioSource = GameObject.Find("MenuThemeMusic").GetComponent<AudioSource>();
	
	}

	public void SetMainMenuAudio () {
		audioSource.volume = volume.value;
	}
}

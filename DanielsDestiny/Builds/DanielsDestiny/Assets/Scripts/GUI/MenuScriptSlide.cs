using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuScriptSlide : MonoBehaviour {

	private RectTransform leftMenuBackground;
	private RectTransform rightMenuBackground;

	private float timer;
	private float timerSpeed = 0.5f;
	private float slideTimer;

	private Vector3 leftOrigin;
	private Vector3 rightOrigin;

	private Vector3 leftDestination;
	private Vector3 rightDestination;

	void Awake () {

	}
	// Use this for initialization
	void Start () {
		PauseSystem.Pause(false);
        Screen.showCursor = true;
		leftMenuBackground = GameObject.Find("menuDecorationLeft").GetComponent<RectTransform>();
		rightMenuBackground = GameObject.Find("menuDecorationRight").GetComponent<RectTransform>();
		
		leftOrigin = new Vector3(leftMenuBackground.localPosition.x - 133, leftMenuBackground.localPosition.y);
		rightOrigin = new Vector3(rightMenuBackground.localPosition.x + 133, rightMenuBackground.localPosition.y);
		
		leftDestination = new Vector3(leftMenuBackground.localPosition.x, leftMenuBackground.localPosition.y);
		rightDestination = new Vector3(rightMenuBackground.localPosition.x, rightMenuBackground.localPosition.y);

		slideSides();
	}

	private void slideSides () {
		StartCoroutine(slideObject(leftMenuBackground, leftOrigin, leftDestination, timer, timerSpeed));
		StartCoroutine(slideObject(rightMenuBackground, rightOrigin, rightDestination, timer, timerSpeed));
	}

	IEnumerator slideObject (RectTransform _selectedObject, Vector3 _origin, Vector3 _destination, float _timer, float _timerSpeed) {
		_timer = 0;
		while(_timer < 1) {
			_selectedObject.transform.localPosition = Vector3.Lerp(_origin, _destination, _timer);
			_timer += Time.deltaTime * _timerSpeed;
			yield return null;
		}
	}
	// Update is called once per frame
	void Update () {
	}
}

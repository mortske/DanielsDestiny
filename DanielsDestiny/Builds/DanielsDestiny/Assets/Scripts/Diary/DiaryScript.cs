using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiaryScript : MonoBehaviour {
	public static DiaryScript instance; 
	PaintScript paint;
	bool _active = false;
	// Use this for initialization
	void Awake() {
		if(instance == null)
			instance = this as DiaryScript;
		else
			Destroy(this.gameObject);
	}
	void Start()
	{
		paint = this.GetComponent<PaintScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.GetKeyDown("Diary") && ((_active == false && !PauseSystem.IsPaused) || (_active == true && PauseSystem.IsPaused)))
		{
			_active = !_active;
			paint.TogglePaint(_active);
		}
	}
	public bool IsActive()
	{
		return _active;
	}
	public void Disable()
	{
		_active = false;
		paint.TogglePaint (_active);
	}

	public List<Color> GetCol()
	{
		return paint.GetColorsPainted();
	}
	public void SetCol(List<Color> col)
	{
		paint.LoadColor(col);
	}
}

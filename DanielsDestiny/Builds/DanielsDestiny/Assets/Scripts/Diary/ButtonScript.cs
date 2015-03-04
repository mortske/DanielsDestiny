using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	public PaintScript paint;
	
	// Use this for initialization
	void OnMouseUp()
	{
		if(transform.name == "Next")
			paint.ChangePage(1);
		if(transform.name == "Last")
			paint.ChangePage(-1);
		if(transform.name == "Erase")
			paint.Erase();
	}
}

using UnityEngine;
using System.Collections;

public class ShadeHandler : MonoBehaviour {
	public GameObject _sunPos;
	GameObject _pl;
	Light _sun;
	public LayerMask mask;
	RaycastHit hit;
	// Use this for initialization
	void Start () {
		_pl = GameObject.FindWithTag("Player");
		_sun = this.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_sun.enabled)
		{
			
			if(Physics.Linecast(_sunPos.transform.position, _pl.transform.position, out hit, mask))
			{
				
				Debug.Log(hit.collider.name);
				InShade();
			}
			else
			{
				InSun();
			}
		}
		
	}
	void InShade()
	{
//		Debug.Log("IN THE SHADE");
	}
	void InSun()
	{
//		Debug.Log("IN THE SUN");
	}
}

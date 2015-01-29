using UnityEngine;
using System.Collections;

public class PickTest : MonoBehaviour {
	Ray ray = new Ray();
	RaycastHit hit;
	Transform camPos;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		{
			camPos = Camera.main.transform;
			ray = new Ray(camPos.position, camPos.forward);
			if(Physics.Raycast(ray, out hit, 200.0f)){
				if(hit.transform.tag == "Resource")
					hit.transform.GetComponent<Resource>().Collect();
			}
		}
	}
}

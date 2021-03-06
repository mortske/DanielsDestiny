﻿using UnityEngine;
using System.Collections;

public class CampSymbol : MonoBehaviour {

	Quaternion rot;
	Transform target;
	Vector3 startScale;
	float startDistance;
	float currDistance;
	Camera campSymbolCam;

	void Start()
	{
		campSymbolCam = Camera.allCameras[1];
		target = Player.instance.transform;
		startScale = transform.localScale;
		startDistance = Vector3.Distance(transform.position, target.position);
	}

	void Update () 
	{
		rot = new Quaternion(0, Quaternion.LookRotation(target.position - transform.position).y, 0, Quaternion.LookRotation(target.position - transform.position).w);
		transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10 * Time.deltaTime);

		currDistance = Vector3.Distance(transform.position, target.position) / 30;
		transform.localScale = new Vector3(startScale.x * currDistance, startScale.y * currDistance, 0);

		if(transform.localScale.x <= 2)
		{
			transform.localScale = startScale;
			renderer.enabled = false;
		}
		else if(transform.localScale.x >= 100)
		{
			transform.localScale = new Vector3(100, 100, 0);
		}
		else
		{
			renderer.enabled = true;
		}
	}
}

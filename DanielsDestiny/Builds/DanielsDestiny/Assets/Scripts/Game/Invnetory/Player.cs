using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;

	public Inventory inventory;

	void Start () {
	
	}
	

	void Update () 
	{
		HandleMovement();
	}

	private void HandleMovement()
	{
		float translation = speed * Time.deltaTime;

		transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation, 0, Input.GetAxis("Vertical") * translation));
	}
}

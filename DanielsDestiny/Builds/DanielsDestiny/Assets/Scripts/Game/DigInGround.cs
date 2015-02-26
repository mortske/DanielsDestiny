using UnityEngine;
using System.Collections;

public class DigInGround : MonoBehaviour {

	public GameObject bugPrefab;
	public float secondsToDig;
	public float procentChanceOfDrop;
	public float fatigueWork;

	private bool isDigging;

	void Update ()
	{
		if(InputManager.GetKeyDown("Pickup") && !isDigging && !PauseSystem.IsPaused && !OnScreenInformationbox.instance.group.activeInHierarchy)
		{
			StartCoroutine(Dig());
		}
	}

	IEnumerator Dig ()
	{
		if(Player.instance.status.fatigue.cur > Player.instance.status.fatigue.min + fatigueWork)
		{
			isDigging = true;
			PauseSystem.IsPaused = true;
			MessageBox.instance.SendMessage("I started digging in the ground..");
			yield return new WaitForSeconds(secondsToDig);
			int rnd = Random.Range(0, 101);
			if(rnd <= procentChanceOfDrop)
			{
				GameObject bug = (GameObject)Instantiate(bugPrefab);
				bug.name = bugPrefab.name;
				bug.GetComponentInChildren<Item>().AddItem();
				MessageBox.instance.SendMessage("..and found a bug.");
			}
			else
			{
				MessageBox.instance.SendMessage("..and didnt find anything.");
			}

			Player.instance.status.fatigue.adjustCur(-fatigueWork);
			PauseSystem.IsPaused = false;
			isDigging = false;
		}
		else
		{
			MessageBox.instance.SendMessage("I am too tired to dig");
		}
	}
}

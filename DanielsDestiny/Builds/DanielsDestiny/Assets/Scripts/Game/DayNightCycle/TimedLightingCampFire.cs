using UnityEngine;
using System.Collections;

[AddComponentMenu("Day Night Cycle/Timed Lighting For CampFire" +
	"")]
public class TimedLightingCampFire : MonoBehaviour {
	public void OnEnable(){
		Messenger<bool>.AddListener("Morning Light Time", OnToggleLight);
	}
	public void OnDisable(){
		Messenger<bool>.RemoveListener("Morning Light Time", OnToggleLight);
	}
	private void OnToggleLight(bool b){
		if(b){
			GetComponent<ParticleEmitter>().emit = false;
		}
		else{
			GetComponent<ParticleEmitter>().emit = true;
		}
	}
}
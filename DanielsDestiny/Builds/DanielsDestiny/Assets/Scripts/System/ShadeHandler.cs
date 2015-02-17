using UnityEngine;
using System.Collections;

public class ShadeHandler : MonoBehaviour {
	public GameObject _sunPos;
	public LayerMask mask;
	Player _pl;
	Light _sun;
	
	RaycastHit hit;
	
	int _heatTick = 0;
	int _wantedHeat = 0;
	int ticks;
	bool _inShade = false;
	// Use this for initialization
	void Start () {
  		_pl = Player.instance;
		_sun = this.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_sun.enabled)
		{
			if(Physics.Linecast(_sunPos.transform.position, _pl.transform.position, out hit, mask))
			{
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
		if(!_inShade)
		{
			_wantedHeat = -10;
			ticks = _heatTick+_wantedHeat;
			StartCoroutine("Shade");
			_inShade = true;
		}
	}
	void InSun()
	{
		if(_inShade)
		{
			_wantedHeat = 0;
			ticks = _heatTick*-1;
			StartCoroutine("Shade");
			_inShade = false;
		}
	}
	IEnumerator Shade()
	{
		for(int x = 0; x < ticks; x++)
		{
			if(_wantedHeat == -10)
			{
				_heatTick--;
				_pl.status.temperatureAdjustment -= 1;
			}
			else
			{
				if(_heatTick < 0)
				{
					_heatTick++;
					_pl.status.temperatureAdjustment += 1;
				}
			}
			yield return new WaitForSeconds(1.0f);
		}
	}
}

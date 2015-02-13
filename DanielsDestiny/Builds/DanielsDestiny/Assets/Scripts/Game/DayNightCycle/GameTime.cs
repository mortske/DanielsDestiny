using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {

    public static GameTime instance;

	public enum TimeOfDay{
		Idle,
		SunRise,
		SunSet
	}
	public Transform[] sun;						//an array to hold all of our suns
	public float dayCycleInMinutes = 1;			//how many real time minutes an in game day will last
	public float StartTimeMilitary;				//this time is in military time
	
	public float sunRise;						//the time of day that we start the sunrise
	public float sunSet;						//the time of day that we start the sunset
	public float skyboxBlendModifier;			//the speed at which the textures in the skyboxes blend
	
	public Color ambLightMax;					//Day amblight
	public Color ambLightMin;					//Night amblight
	
	public float morningLight;
	public float nightLight;
	private bool _isMorning = false, _isNight = false;
	
	private Sun[] _sunScript;
	private float _dayCycleInSeconds;
	
	private const float SECOND = 1;
	private const float MINUTE = 60 * SECOND;
	private const float HOUR = 60 * MINUTE;
	private const float DAY = 24 * HOUR;
	private const float DEGREES_PER_SECOND = 360 / DAY;
	
	private TimeOfDay _tod;
	private float _noonTime;					//this is the time of day when it's noon
	
	private float _degreeRotation;
	
	private float _timeOfDay;
	
	private float _morningLength;
	private float _eveningLength;

    private float _temperature = 0;
    public float _scaledTemperature;

	int TotalTimePassed = 0;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
		_tod = TimeOfDay.Idle;
		
		_dayCycleInSeconds = dayCycleInMinutes * MINUTE;
		
		RenderSettings.skybox.SetFloat("_Blend", 0);
		
		_sunScript = new Sun[sun.Length];
		
		for(int cnt = 0; cnt < sun.Length; cnt++){
			Sun temp = sun[cnt].GetComponent<Sun>();
			
			if(temp == null){
				Debug.LogWarning("Sun script not found. Adding it");
				sun[cnt].gameObject.AddComponent<Sun>();
				temp = sun[cnt].GetComponent<Sun>();
			}
			_sunScript[cnt] = temp;
		}
		
		_timeOfDay = 0;
		_degreeRotation = DEGREES_PER_SECOND * DAY / (_dayCycleInSeconds);
		
		sunRise *= _dayCycleInSeconds;
		sunSet *= _dayCycleInSeconds;
		_noonTime = _dayCycleInSeconds / 2;
		_morningLength = _noonTime - sunRise;		//the length of the morning in seconds
		_eveningLength = sunSet - _noonTime;		//the length of the evening in seconds
		morningLight *= _dayCycleInSeconds;
		nightLight *= _dayCycleInSeconds;
		
		
		//setup lighting to minlight values to start
		SetupLighting();			
	}
	
	// Update is called once per frame
	void Update () {
		for(int cnt = 0; cnt < sun.Length; cnt++)
			sun[cnt].Rotate(new Vector3(_degreeRotation, 0, 0)*Time.deltaTime);
		
		_timeOfDay += Time.deltaTime;
		TotalTimePassed ++;
		if(_timeOfDay > _dayCycleInSeconds)
			_timeOfDay -= _dayCycleInSeconds;
		
	//	Debug.Log(_timeOfDay);
		
		//control the outside lighting effects according to the time of day
		if(!_isMorning && _timeOfDay > morningLight && _timeOfDay < nightLight){
			_isMorning = true;
			_isNight = false;
			Messenger<bool>.Broadcast("Morning Light Time", true);
//			Debug.Log("Morning");
		}
		else if(!_isNight && _timeOfDay > nightLight || _timeOfDay < morningLight){
			_isNight = true;
			_isMorning = false;
			Messenger<bool>.Broadcast("Morning Light Time", false);
//			Debug.Log("Night");
		}
		
		if(_timeOfDay > sunRise && _timeOfDay < _noonTime){
			AdjustLighting(true);
		}
		else if(_timeOfDay > _noonTime && _timeOfDay < sunSet){
			AdjustLighting(false);
		}
		if(_timeOfDay > sunRise && _timeOfDay < sunSet && RenderSettings.skybox.GetFloat("_Blend") < 1){
			_tod = GameTime.TimeOfDay.SunRise;
			BlendSkybox();
		}
		else if(_timeOfDay > sunSet && RenderSettings.skybox.GetFloat("_Blend") > 0){
			_tod = GameTime.TimeOfDay.SunSet;
			BlendSkybox();
		}
		else{
			_tod = GameTime.TimeOfDay.Idle;
		}
		
		if(_timeOfDay < sunSet && _timeOfDay > _noonTime)
		{
			RenderSettings.skybox.SetFloat("_Blend", 1);
		}
		else if(_timeOfDay < sunRise || _timeOfDay > sunSet)
		{
			RenderSettings.skybox.SetFloat("_Blend", 0);
		}

        SetTemperature();
	}

	
	private void BlendSkybox(){
		float temp = 0;
		
		switch(_tod){
		case TimeOfDay.SunRise:
				temp = (_timeOfDay - sunRise) / _dayCycleInSeconds * skyboxBlendModifier;
				break;
		case TimeOfDay.SunSet:
				temp = (_timeOfDay - sunSet) / _dayCycleInSeconds * (skyboxBlendModifier*4);
				temp = 1 - temp;
				break;
		}
		
		RenderSettings.skybox.SetFloat("_Blend", temp);
		
	//Debug.Log(temp);
	}
	private void SetupLighting(){
		RenderSettings.ambientLight = ambLightMin;
		for(int cnt = 0; cnt < _sunScript.Length; cnt++){
			if(_sunScript[cnt].giveLight){
				sun[cnt].GetComponent<Light>().intensity = _sunScript[cnt].minLightBrightness;
			}
		}
	}
	
	private void AdjustLighting(bool brighten){
		float pos = 0;
		
		if(brighten){
			pos = (_timeOfDay - sunRise) / _morningLength;			//get the position of the sun in the morning sky
		}
		else{
			pos = (sunSet - _timeOfDay) / _eveningLength;			//get the position of the sun in the evening sky	
		}
		RenderSettings.ambientLight = new Color(ambLightMin.r + ambLightMax.r * pos,
		                                        ambLightMin.g + ambLightMax.g * pos,
		                                        ambLightMin.b + ambLightMax.b * pos);
		
		for(int cnt = 0; cnt < _sunScript.Length; cnt++){
			if(_sunScript[cnt].giveLight){
				_sunScript[cnt].GetComponent<Light>().intensity = _sunScript[cnt].maxLightBrightness * pos;
			}
		}
	}

    private void SetTemperature()
    {
        if (_timeOfDay > (dayCycleInMinutes * 60) / 2)
            _temperature -= Time.deltaTime;
        else
            _temperature += Time.deltaTime;
        _scaledTemperature = _temperature / _dayCycleInSeconds * 2;
    }
	public float TheTemp
	{
		get{return _temperature;}
		set{_temperature = value;}
	}
    public float TheTime
    {
    	get{return _timeOfDay;}
    	set{_timeOfDay = value;}
    }
	public void SaveTime()
	{
		HighScore.instance.SaveScore(TotalTimePassed);
	}
	public float GetDayCycleInSeconds()
	{
		return _dayCycleInSeconds;
	}
	public void SetRotationOfSun(float seconds)
	{
		if(seconds < morningLight || seconds > nightLight)
		{
			_sunScript[0].GetComponent<Light>().intensity = _sunScript[0].minLightBrightness;
			RenderSettings.ambientLight = ambLightMin;
		}
		else{
			RenderSettings.ambientLight = ambLightMax;
		}
		
		
		_isMorning = false;
		_isNight = false;
		float rot = 270 + (DEGREES_PER_SECOND * DAY / (_dayCycleInSeconds) * seconds);
		sun[0].Rotate(new Vector3(rot, 0, 0));
	}
}

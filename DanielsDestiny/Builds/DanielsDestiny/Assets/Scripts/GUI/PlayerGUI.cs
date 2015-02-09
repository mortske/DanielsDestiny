using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {

	private float speed = 5; // temp
	private int maxHealth = 100;

	#region Health Bar
	public RectTransform healthTransform;
	private float minHealthXValue;
	private float maxHealthXValue;
	private int currentHealth;
	private int CurrentHealth {
		// When the HP is changed, runs Handlehealth
		get { return currentHealth; }
		set { 
			currentHealth = value;
			HandleBars(healthTransform, healthText, currentHealth, maxHealth, minHealthXValue, maxHealthXValue, true);
		}
	}
	public Text healthText;
	public Image visualHealth;
	#endregion
	#region Fatigue Bar
	private int maxFatigue = 100;
	public RectTransform fatigueTransform;
	private float minFatigueXValue;
	private float maxFatigueXValue;
	private int currentFatigue;
	private int CurrentFatigue {
		get { return currentFatigue; }
		set {
			currentFatigue = value;
			HandleBars(fatigueTransform, fatigueText, currentFatigue, maxFatigue, minFatigueXValue, maxFatigueXValue, false);
		}
	}
	public Text fatigueText;
	public Image visualFatigue;
	#endregion
	#region Thirst Bar
	private int maxThirst = 100;
	public RectTransform thirstTransform;
	private float minThirstXValue;
	private float maxThirstXValue;
	private int currentThirst;
	private int CurrentThirst {
		get { return currentThirst; }
		set {
			currentThirst = value;
			HandleBars(thirstTransform, thirstText, currentThirst, maxThirst, minThirstXValue, maxThirstXValue, false);
		}
	}
	public Text thirstText;
	public Image VisualThirst;
	#endregion
	#region Hunger Bar
	private int maxHunger = 100;
	public RectTransform hungerTransform;
	private float minHungerXValue;
	private float maxHungerXValue;
	private int currentHunger;
	private int CurrentHunger {
		get { return currentHunger; }
		set {
			currentHunger = value;
			HandleBars(hungerTransform, hungerText, currentHunger, maxHunger, minHungerXValue, maxHungerXValue, false);
		}
	}

	public Text hungerText;
	public Image visualHunger;
	#endregion
	#region Hot/Cold Bar
	private int maxTemperature = 100;
	public RectTransform temperatureTransform;
	public RectTransform backgroundTemperatureTransform;
	private float minTemperatureXValue;
	private float maxTemperatureXValue;
	private int currentTemperature;
	private int CurrentTemperature {
		get { return currentTemperature; }
		set {
			currentTemperature = value;
			HandleBars(temperatureTransform, temperatureText, currentTemperature, maxTemperature, minTemperatureXValue, maxTemperatureXValue, false);
		}
	}

	public Text temperatureText;
	public Image visualTemperature;
	#endregion

	Player player;
	public RectTransform interfaceBarBackground;
	private float retractedInterfaceY;
	private float expandedInterfaceY;

	private float coolDown;
	private bool onCD;
	private bool InterfaceIsShowing;
	public float bigDamageCooldown;
	
	void Start () {
		#region Assignments
		maxHealthXValue = healthTransform.localPosition.x;
		minHealthXValue = healthTransform.localPosition.x - healthTransform.rect.width;
		currentHealth = maxHealth;

		maxFatigueXValue = fatigueTransform.localPosition.x;
		minFatigueXValue = fatigueTransform.localPosition.x - fatigueTransform.rect.width;
		currentFatigue = maxFatigue;

		maxThirstXValue = thirstTransform.localPosition.x;
		minThirstXValue = thirstTransform.localPosition.x - thirstTransform.rect.width;
		currentThirst = maxThirst;

		maxHungerXValue = hungerTransform.localPosition.x;
		minHungerXValue = hungerTransform.localPosition.x - hungerTransform.rect.width;
		currentHunger = maxHunger;

		maxTemperatureXValue = backgroundTemperatureTransform.localPosition.x + backgroundTemperatureTransform.rect.width / 2;
		minTemperatureXValue = backgroundTemperatureTransform.localPosition.x - backgroundTemperatureTransform.rect.width / 2;
		currentTemperature = maxTemperature / 2;

		expandedInterfaceY = interfaceBarBackground.localPosition.y;
		retractedInterfaceY = interfaceBarBackground.localPosition.y - 70;
		InterfaceIsShowing = true;
		#endregion

		player = GameObject.Find("Player").GetComponent<Player>();

		onCD = false;
	}

	void Update () {
		CurrentHealth = (int)player.status.health.cur;
		CurrentFatigue = (int)player.status.fatigue.cur;
		CurrentHunger = (int)player.status.hunger.cur;
		CurrentThirst = (int)player.status.thirst.cur;
        CurrentTemperature = (int)player.status.temperature.cur;

		if(Input.GetKeyDown(KeyCode.L)) {
			ShowInterface();
		}

	}

	private void ShowInterface () {
		if(InterfaceIsShowing) {
			interfaceBarBackground.localPosition = new Vector3(interfaceBarBackground.localPosition.x, retractedInterfaceY);
			InterfaceIsShowing = false;
		} else {
			interfaceBarBackground.localPosition = new Vector3(interfaceBarBackground.localPosition.x, expandedInterfaceY);
			InterfaceIsShowing = true;
		}
	}

	private void HandleBars (RectTransform currentBar, Text textBar, int currentValue, float maxValue, float minimumXValue, float maximumXValue, bool healthDrop) {
		textBar.text = "" + currentValue;
		float XValue = MapValues(currentValue, 0, maxValue, minimumXValue, maximumXValue);
		currentBar.localPosition = new Vector3(XValue, currentBar.localPosition.y);

		if(healthDrop) {
			//HandleColorChange();
		}

	}

	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax) {
		// x = health
		// inMin = minimum health (0)
		// inMax = maximum health (100)
		// outMin = lowest value on the x axis 
		// outMax = start position (0)
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}

	#region Ajudst Health Methods
//	void HealDamage (int ammountOfDamage) {
//		if (!onCD && currentHealth < maxHealth) {
//			StartCoroutine(CoolDownDmg());
//			CurrentHealth += ammountOfDamage;
//		}
//	}
//
//	void TakeDamage (int ammountOfDamage) {
//		if(ammountOfDamage < 2) {
//			if (!onCD && currentHealth > 0) {
//				StartCoroutine(CoolDownDmg());
//				CurrentHealth -= ammountOfDamage;
//			}
//		} else {
//			if (!onCD && currentHealth > 0) {
//				StartCoroutine(RecieveBigDamage(0.05f, ammountOfDamage));
//			}
//		}
//	}
//
//	IEnumerator RecieveBigDamage(float delay, int damage) {
//		for (int i = 0; i < damage; i++) {
//			//StartCoroutine(CoolDownDmg());
//			CurrentHealth -= 1;
//			yield return new WaitForSeconds(delay);
//		}
//	}
	#endregion
	
	#region Options
	IEnumerator CoolDownDmg() {
		// Adds a cooldown for the damage intake
		onCD = true;
		yield return new WaitForSeconds(coolDown);
		onCD = false;
	}

	private void HandleColorChange() {
		// Changes the color from Green to Red as the player recieves damage
		if (currentHealth > maxHealth / 2) {
			visualHealth.color = new Color32((byte)MapValues(currentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);
		} else {
			visualHealth.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth / 2, 0, 255), 0, 255);
		}
	}
	#endregion
}

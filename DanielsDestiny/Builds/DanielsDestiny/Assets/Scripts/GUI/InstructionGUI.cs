using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InstructionGUI : MonoBehaviour {

	public bool disableInstructionScreen;
	public static bool instructionsAreActive;

	private int currentCanvas;

	#region Canvases
	private Canvas instructionCanvas;

	private Canvas instructionsTitle;
	private Canvas healthBar;
	private Canvas hungerBar;
	private Canvas fatigueBar;
	private Canvas thirstBar;
	private Canvas temperatureBar;
	#endregion

	private List<Canvas> InstructionCanvases = new List<Canvas>();
	
	void Start () {
		currentCanvas = -1;
		 
		#region Add Canvases to List
		InstructionCanvases.Add(healthBar = GameObject.Find("Canvas_HealthBar").GetComponent<Canvas>());
		InstructionCanvases.Add(hungerBar = GameObject.Find("Canvas_HungerBar").GetComponent<Canvas>());
		InstructionCanvases.Add(fatigueBar = GameObject.Find("Canvas_FatigueBar").GetComponent<Canvas>());
		InstructionCanvases.Add(thirstBar = GameObject.Find("Canvas_ThirstBar").GetComponent<Canvas>());
		InstructionCanvases.Add(temperatureBar = GameObject.Find("Canvas_TemperatureBar").GetComponent<Canvas>());
		#endregion

		instructionCanvas = GameObject.Find("Canvas_InstructionScreen").GetComponent<Canvas>();
		instructionsTitle = GameObject.Find("Canvas_InstructionsTitle").GetComponent<Canvas>();

		ClearAllCanvases();
		instructionCanvas.enabled = false;

		if(!disableInstructionScreen) {
			StartInstructions();
			instructionCanvas.enabled = true;
			instructionsTitle.enabled = true;
		}
	}

	void SelectCanvas (int _currCanvas) {
		switch (_currCanvas) {

		case 0:
			EnableDisableCanvases(0);
			break;

		case 1:
			EnableDisableCanvases(1);
			break;

		case 2:
			EnableDisableCanvases(2);
			break;

		case 3:
			EnableDisableCanvases(3);
			break;

		case 4:
			EnableDisableCanvases(4);
			break;
		}
	}

	void ClearAllCanvases () {
		for (int i = 0; i < InstructionCanvases.Count; i++) {
			InstructionCanvases[i].enabled = false;
		}
		instructionsTitle.enabled = false;
	}

	void EnableDisableCanvases (int _enabled) {
		for (int i = 0; i < InstructionCanvases.Count; i++) {
			if(i == _enabled) {
				InstructionCanvases[i].enabled = true;
			} else {
				InstructionCanvases[i].enabled = false;
			}
		}
	}

	void StartInstructions () {
		instructionsAreActive = true;
		PauseSystem.Pause(true);
	}

	void EndInstructions () {
		ClearAllCanvases();
		instructionsAreActive = false;
		PauseSystem.Pause(false);
		instructionCanvas.enabled = false;
	}

	void Update () {
		if(currentCanvas != InstructionCanvases.Count + 1) {
			if(!disableInstructionScreen) {
				if(Input.GetKeyDown(KeyCode.Space)) {
					if(currentCanvas <= InstructionCanvases.Count - 1) {
						currentCanvas++;
						SelectCanvas(currentCanvas);
					} 
				}
				if(currentCanvas == InstructionCanvases.Count) {
					currentCanvas++;
					EndInstructions();
				}
			}
		}
	
	}

	public void Skip () {
		currentCanvas = InstructionCanvases.Count;
		EndInstructions();
	}
}

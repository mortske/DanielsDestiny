// C#
// Creates a simple wizard that lets you create a Light GameObject
// or if the user clicks in "Apply", it will set the color of the currently
// object selected to red

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class WizardCreateItems : ScriptableWizard {
	string path = "Items";
	public string itemName = "New Item";
	public int createNumber = 0;
	public List<Object> Items = new List<Object>();
	Transform[] items;
	
	
	[MenuItem ("GameObject/Create Item Wizard")]
	static void CreateWizard () {
		ScriptableWizard.DisplayWizard<WizardCreateItems>("Create Item", "Create", "Apply");
		//If you don't want to use the secondary button simply leave it out:
		//ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
	}
	void OnWizardCreate () {
		if(Items[createNumber] != null)
		{
			GameObject go = new GameObject();
			go.name = Items[createNumber].name;
		}
			
	}  
	void OnWizardUpdate () {
		// Find all assets labelled with 'concrete' : 
		var guids = AssetDatabase.FindAssets("_Item");
		foreach(var guid in guids)
		{
			Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
			Items.Add(Resources.Load(AssetDatabase.GUIDToAssetPath(guid)));
		}
		

		helpString = "Please set the values of the item!";
	}   
	// When the user pressed the "Apply" button OnWizardOtherButton is called.
	void OnWizardOtherButton () {
		
		if (Selection.activeTransform == null) return;
		
	}
}
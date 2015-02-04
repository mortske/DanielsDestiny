using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingDictionary : MonoBehaviour 
{
	public List<Recepie> recepies;
}

[System.Serializable]
public class Recepie
{
    public string recepieName = "";
    public List<GameObject> items;
    public List<int> amount;
    public GameObject result;
}

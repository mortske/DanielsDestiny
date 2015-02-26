using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour 
{
	public bool isChild;
    public float killTime;
    void Start()
    {
        Invoke("Kill", killTime);
    }

    void Kill()
    {
		if(!isChild)
        	Destroy(gameObject);
		else
			Destroy(gameObject.transform.parent.gameObject);

		CraftingDictionary.InsideArea = false;
    }
}

using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour 
{
    Canvas canvas; 
    public static LoadingScreen instance;
    GUITexture guitexture;
    
    bool _loadGame = false;

    void Awake()
    {
        instance = this;
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        DontDestroyOnLoad(this);
    }

    public void Load(int index)
    {
        canvas.enabled = true;
        Application.LoadLevel(index);
    }
	public bool ResumeGame
	{
		get{return _loadGame;}
		set{_loadGame = value;}
	}
    void Update()
    {
        if (Application.isLoadingLevel)
        {
            canvas.enabled = true;
        }
        else
        {
            canvas.enabled = false;
            if (Application.loadedLevel == 1)
            {
	            if(_loadGame)
	            	BiomeManager.instance.LoadBiomes();
                Destroy(gameObject);
            }
        }
    }
}

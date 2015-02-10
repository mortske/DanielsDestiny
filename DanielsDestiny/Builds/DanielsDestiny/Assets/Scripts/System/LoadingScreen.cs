using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour 
{
    Canvas canvas; 
    public static LoadingScreen instance;
    GUITexture guitexture;

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
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
    public static InputManager instance;
    public InputKey[] keys;

    void Start()
    {
        instance = this;
    }

    public static bool GetKeyDown(string key)
    {
        for (int i = 0; i < instance.keys.Length; i++)
        {
            if (instance.keys[i].name == key)
            {
                if (Input.inputString == instance.keys[i].key && instance.keys[i].key != "")
                {
                    if (!instance.keys[i].used)
                    {
                        instance.keys[i].used = true;
                        return true;
                    }
                }
                if (Input.inputString == instance.keys[i].altKey && instance.keys[i].altKey != "")
                {
                    if (!instance.keys[i].used)
                    {
                        instance.keys[i].used = true;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void LateUpdate()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].used = false;
        }
    }
}

[System.Serializable]
public class InputKey
{
    public string name;
    public string key;
    public string altKey;
    public bool used;

}

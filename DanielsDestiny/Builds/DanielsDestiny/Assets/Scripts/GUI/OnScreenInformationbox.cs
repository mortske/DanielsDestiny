using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnScreenInformationbox : MonoBehaviour 
{
    public static OnScreenInformationbox instance;
    public GameObject group;
    public Text textbox;

    void Awake()
    {
        instance = this;
        group.SetActive(false);
    }

    public void ShowBox(string _msg)
    {
        group.SetActive(true);
        textbox.text = _msg;
    }

    public void HideBox()
    {
        group.SetActive(false);
    }
}

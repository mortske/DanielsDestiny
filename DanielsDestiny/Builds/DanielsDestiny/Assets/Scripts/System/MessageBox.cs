using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageBox : MonoBehaviour 
{
    public static MessageBox instance;
    Text messageBox;

    public float stayTime;
    public float fadeTime;
    float curTime;

    void Awake()
    {
        instance = this;
        messageBox = GetComponent<Text>();
    }

    public void SendMessage(string message)
    {
        StopAllCoroutines();
        
        messageBox.text = message;
        
        StartCoroutine(FadeText());
    }

    IEnumerator FadeText()
    {
        Color color = messageBox.color;
        float alpha = 1;
        float aValue = 0;
        messageBox.color = new Color(color.r, color.g, color.b, 1);

        yield return new WaitForSeconds(stayTime);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            messageBox.color = new Color(color.r, color.g, color.b, Mathf.Lerp(alpha,aValue,t));
            yield return null;
        }
    }
}

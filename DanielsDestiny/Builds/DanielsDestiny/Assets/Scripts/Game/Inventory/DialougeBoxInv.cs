using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class DialougeBoxInv : MonoBehaviour 
{
    public int min, max;
    public int cur;
    public bool isDone;
    public GameObject group;

    public InputField dialougeText;

    public void Display(int max, int min, int _default)
    {
        isDone = false;
        this.max = max;
        this.min = min;
        dialougeText.text = _default.ToString();
        cur = _default;
        group.SetActive(true);
    }

    void Update()
    {
        dialougeText.text = Regex.Replace(dialougeText.text, "[^0-9]", "");
        if (dialougeText.text != "")
        {
            cur = int.Parse(dialougeText.text);
            if (cur > max)
                cur = max;
            if (cur < min)
                cur = min;
            dialougeText.text = cur.ToString();
        }
    }

    public void Hide()
    {
        isDone = true;
        group.SetActive(false);
    }
}

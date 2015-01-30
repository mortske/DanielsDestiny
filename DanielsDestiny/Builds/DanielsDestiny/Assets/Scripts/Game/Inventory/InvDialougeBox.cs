using UnityEngine;
using System.Collections;

public class InvDialougeBox : MonoBehaviour 
{
    public GameObject group;
    int max, min, cur;

    public void Display(int max, int min, int cur)
    {
        this.max = max;
        this.min = min;
        this.cur = cur;

        group.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(true);
    }
}

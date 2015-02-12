using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour 
{
    public float killTime;
    void Start()
    {
        Invoke("Kill", killTime);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}

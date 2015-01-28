using UnityEngine;
using System.Collections;

public class EdgeCollider : MonoBehaviour 
{
    EdgeManager manager;
    public void Start()
    {
        manager = transform.parent.GetComponent<EdgeManager>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            manager.PlayerEntered(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            manager.PlayerLeft();
        }
    }
}

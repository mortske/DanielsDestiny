using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour 
{
    public float baseDamage;
    public float attackTime = 2;
    float curTime;
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            curTime -= Time.deltaTime;


            if (Input.GetMouseButtonDown(0) && curTime <= 0)
            {
                curTime = attackTime;
                if (Player.instance.curEquipment != null)
                {
                    other.GetComponent<AnimalAI>().TakeDamage(Player.instance.curEquipment.damage);
                }
                else
                {
                    other.GetComponent<AnimalAI>().TakeDamage(baseDamage);
                }
            }
        }
    }
}

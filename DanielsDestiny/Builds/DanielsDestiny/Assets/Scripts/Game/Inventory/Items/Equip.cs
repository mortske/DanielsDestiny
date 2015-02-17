using UnityEngine;
using System.Collections;

public class Equip : Item 
{
    public float damage;
	void Start()
	{
		equipable = true;
	}

    public override void EquipItem()
    {
        base.EquipItem();
        equipable = true;
    }
}

﻿using UnityEngine;
using System.Collections;

public class Potion : Item 
{
    public StatusBars effectTo;
    public int adjustment;
    
    public override void Use()
    {
        base.Use();
        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.status.GetBar(effectTo).adjustCur(adjustment);
    }
}

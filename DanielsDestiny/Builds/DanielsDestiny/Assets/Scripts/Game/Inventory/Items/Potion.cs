using UnityEngine;
using System.Collections;

public class Potion : Item 
{
    public ItemEffect[] effects;
    
    public override void Use()
    {
        base.Use();
        for (int i = 0; i < effects.Length; i++)
        {
            Player.instance.status.GetBar(effects[i].effectTo).adjustCur(effects[i].adjustment);
        }
        
    }
}
[System.Serializable]
public class ItemEffect
{
    public StatusBars effectTo;
    public float adjustment;
}

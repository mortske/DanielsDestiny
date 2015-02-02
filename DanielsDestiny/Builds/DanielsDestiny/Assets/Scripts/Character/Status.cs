using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour 
{
    public StatusBarWithTick health;
    public StatusBarWithTick fatigue;
    public StatusBarWithTick hunger;
    public StatusBarWithTick thirst;
    public StatusBarWithTick temperature;

    public StatusBarWithTick GetBar(StatusBars bar)
    {
        switch (bar)
        {
            case StatusBars.health:
                return health;
            case StatusBars.hunger:
                return hunger;
            case StatusBars.thirst:
                return thirst;
            case StatusBars.fatigue:
                return fatigue;
            default:
                return health;
        }
    }

    void Start()
    {
        health.Initialize(this);
        hunger.Initialize(this);
        fatigue.Initialize(this);
        thirst.Initialize(this);
    }

    void Update()
    {
        if (health.UpdateTick())
            health.adjustCur(-health.AdjustValue);

        if (hunger.UpdateTick())
            hunger.adjustCur(-hunger.AdjustValue);
    }
}

public enum StatusBars
{
    health,
    hunger,
    thirst,
    fatigue
}

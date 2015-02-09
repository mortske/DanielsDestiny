using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour 
{
    public StatusBarWithTick health;
    public StatusBarWithTick fatigue;
    public StatusBarWithTick hunger;
    public StatusBarWithTick thirst;
    public StatusBar temperature;
    public float temperaturePadding;
    public float temperatureAdjustment { get; set; }

    public StatusBar GetBar(StatusBars bar)
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
            case StatusBars.temperature:
                return temperature;
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

        if (thirst.UpdateTick())
            thirst.adjustCur(-thirst.AdjustValue);

        if (fatigue.UpdateTick())
            fatigue.adjustCur(-fatigue.AdjustValue);

        float other = 100 - temperaturePadding;
        temperature.cur = GameTime.instance._scaledTemperature * (temperaturePadding - other) + other + temperatureAdjustment;
        temperature.adjustCur(0);
    }
}

public enum StatusBars
{
    health,
    hunger,
    thirst,
    fatigue,
    temperature
}

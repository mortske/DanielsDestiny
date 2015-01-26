using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatusBar 
{
    public float max;
    public float min;
    public float cur;

    public void Initialize()
    {
        adjustCur(0);
    }

    public StatusEvent adjustCur(float _adj)
    {
        cur += _adj;
        if (cur >= max)
        {
            cur = max;
            return StatusEvent.hitMax;
        }

        if(cur <= min)
        {
            cur = min;
            return StatusEvent.hitMin;
        }

        return StatusEvent.nothing;
    }
}

public enum StatusEvent
{
    hitMax,
    hitMin,
    nothing
}

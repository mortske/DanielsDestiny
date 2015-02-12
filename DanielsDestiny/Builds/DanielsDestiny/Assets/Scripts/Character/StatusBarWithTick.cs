using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatusBarWithTick : StatusBar 
{
    Status playerStatus;

    public float tickTimer;
    float curTick;
    public float baseAdjustValue;
    public AffectedByStatus[] affectedBy;
    //public StatusWarning[] statusWarnings;

    public float AdjustValue
    {
        get 
        {
            float tmp = baseAdjustValue;
            foreach (AffectedByStatus affected in affectedBy)
            {
                StatusBar statusbar = playerStatus.GetBar(affected.bar);
                if (affected.whenEquals == EqualsTo.GreaterThan)
                {
                    if (statusbar.cur > affected.whenValueIs)
                        tmp += affected.byHowMuch;
                }
                else if (affected.whenEquals == EqualsTo.LessThan)
                {
                    if (statusbar.cur < affected.whenValueIs)
                    {
                        tmp += affected.byHowMuch;
                    }
                }
                else if (affected.whenEquals == EqualsTo.EqualTo)
                {
                    if (statusbar.cur == affected.whenValueIs)
                    {
                        tmp += affected.byHowMuch;
                    }
                }
            }
            return tmp; 
        }
    }

    public void Initialize(Status status)
    {
        playerStatus = status;
        base.Initialize();
        curTick = tickTimer;
    }

    public bool UpdateTick()
    {
        curTick -= Time.deltaTime;
        if (curTick <= 0)
        {
            curTick = tickTimer;
            return true;
        }
        return false;
    }
}
[System.Serializable]
public struct AffectedByStatus
{
    public StatusBars bar;
    public EqualsTo whenEquals;
    public float whenValueIs;
    public float byHowMuch;
}

public enum EqualsTo
{
    GreaterThan,
    LessThan,
    EqualTo
}



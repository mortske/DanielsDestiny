using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatusBar 
{
    public float max;
    public float min;
    public float cur;

    public StatusWarning[] statusWarnings;

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

        CheckWarning(_adj);

        return StatusEvent.nothing;
    }

    void CheckWarning(float _adj)
    {
        float pre = cur - _adj;

        foreach (StatusWarning warning in statusWarnings)
        {
            if (warning.whenEquals == EqualsTo.GreaterThan)
            {
                if (pre <= warning.whenValueIs)
                    if (cur > warning.whenValueIs)
                        SendWarning(warning);
            }
            else if (warning.whenEquals == EqualsTo.LessThan)
            {
                if(pre >= warning.whenValueIs)
                    if (cur < warning.whenValueIs)
                        SendWarning(warning);
            }
            else if (warning.whenEquals == EqualsTo.EqualTo)
            {
                if (cur == warning.whenValueIs)
                {
                    SendWarning(warning);
                }
            }
        }
    }
    void SendWarning(StatusWarning warning)
    {
        MessageBox.instance.SendMessage(warning.text);
        if (warning.sound != null)
        {
            SoundObject3D obj = SoundManager.instance.Spawn3DSound(warning.sound, Player.instance.transform.position, 1, 5);
            obj.transform.parent = Player.instance.transform;
        }
    }
}

public enum StatusEvent
{
    hitMax,
    hitMin,
    nothing
}

[System.Serializable]
public struct StatusWarning
{
    public string text;
    public AudioClip sound;
    public EqualsTo whenEquals;
    public float whenValueIs;
}
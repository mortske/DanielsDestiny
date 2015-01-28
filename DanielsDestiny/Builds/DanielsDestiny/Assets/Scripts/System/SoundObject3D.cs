using UnityEngine;
using System.Collections;

public class SoundObject3D : MonoBehaviour 
{
    public void Play(AudioClip clip, float volume, float distance)
    {
        audio.clip = clip;
        audio.volume = volume * SettingsManager.instance.settings.sfx.cur;
        audio.minDistance = distance;
        audio.Play();
        Invoke("DestroySelf", clip.length);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}

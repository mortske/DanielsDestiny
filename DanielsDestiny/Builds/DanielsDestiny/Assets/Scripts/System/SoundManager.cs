using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
    public static SoundManager instance;
    public GameObject soundObjectPrefab;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Stop2DSound(5);
        }
    }

    public void Play2Dsound(AudioClip clip, float volume)
    {
        audio.clip = clip;
        audio.volume = volume * SettingsManager.instance.settings.sound.cur;
        audio.Play();
    }
    public void Play2Dsound(AudioClip clip)
    {
        Play2Dsound(clip, 1);
    }

    public void Stop2DSound(float fadeTime)
    {
        if (fadeTime > 0)
        {
            StartCoroutine(FadeOut(fadeTime));
        }
        else
        {
            audio.Stop();
        }
    }

    IEnumerator FadeOut(float fadeTime)
    {
        float from = audio.volume;
        float to = 0;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            audio.volume = Mathf.Lerp(from, to, t);
            yield return null;
        }
        audio.Stop();
        audio.volume = from;
    }

    public SoundObject3D Spawn3DSound(AudioClip clip, Vector3 pos, float volume, float distance)
    {
        GameObject go = (GameObject)Instantiate(soundObjectPrefab, pos, Quaternion.identity);
        SoundObject3D newSound = go.GetComponent<SoundObject3D>();
        newSound.Play(clip, volume, distance);
        return newSound;
    }
}

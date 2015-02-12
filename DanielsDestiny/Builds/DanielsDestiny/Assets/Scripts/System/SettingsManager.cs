using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    string path = "Assets/Files/Settings.xml";

    public SettingStat settings;

    public Slider volumeSlider;
    public Slider sfxSlider;
    public Slider sensitivitySlider;

    void Awake()
    {
        instance = this;
        Load();

        if (volumeSlider != null)
        {
            volumeSlider.maxValue = settings.sound.max;
            volumeSlider.minValue = settings.sound.min;
            volumeSlider.value = settings.sound.cur;
        }

        if (sfxSlider != null)
        {
            sfxSlider.maxValue = settings.sfx.max;
            sfxSlider.minValue = settings.sfx.min;
            sfxSlider.value = settings.sfx.cur;
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.maxValue = settings.mouseSensitivity.max;
            sensitivitySlider.minValue = settings.mouseSensitivity.min;
            sensitivitySlider.value = settings.mouseSensitivity.cur;
        }
        Save();
    }

    public void Save()
    {
        StreamWriter sw = new StreamWriter(path);
        XmlSerializer x = new System.Xml.Serialization.XmlSerializer(settings.GetType());
        x.Serialize(sw, settings);
        sw.Close();
    }

    public void Load()
    {
        if (File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            XmlSerializer x = new System.Xml.Serialization.XmlSerializer(settings.GetType());
            settings = x.Deserialize(sr) as SettingStat;
            sr.Close();
        }
        else
        {
            Directory.CreateDirectory("Assets/Files/");
            settings.sound.max = 1;
            settings.sound.min = 0;
            settings.sound.cur = 1;

            settings.sfx.max = 1;
            settings.sfx.min = 0;
            settings.sfx.cur = 1;

            settings.mouseSensitivity.max = 50;
            settings.mouseSensitivity.min = 1;
            settings.mouseSensitivity.cur = 15;
        }
    }

    public void SaveMusicVolume(Slider _volume)
    {
        settings.sound.cur = _volume.value;
    }

    public void SaveSFXVolume(Slider _volume)
    {
        settings.sfx.cur = _volume.value;
    }

    public void SaveMouseSensitivity(Slider _sensitivity)
    {
        settings.mouseSensitivity.cur = _sensitivity.value;
    }
}

[System.Serializable]
public class SettingStat
{
    public StatusBar sound;
    public StatusBar sfx;
    public StatusBar mouseSensitivity;
}

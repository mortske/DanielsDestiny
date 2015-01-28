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

    void Awake()
    {
        instance = this;
        Load();
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
            File.Create(path);
        }
    }

    public void SaveMusicVolume(float _volume)
    {
        settings.sound.cur = _volume;
    }

    public void SaveSFXVolume(float _volume)
    {
        settings.sfx.cur = _volume;
    }

    public void SaveMouseSensitivity(float _sensitivity)
    {
        settings.mouseSensitivity.cur = _sensitivity;
    }
}

[System.Serializable]
public class SettingStat
{
    public StatusBar sound;
    public StatusBar sfx;
    public StatusBar mouseSensitivity;
}

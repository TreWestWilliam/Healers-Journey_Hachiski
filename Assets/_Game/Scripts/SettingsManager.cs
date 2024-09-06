using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static MySettings _MySettings;
    [SerializeField] private static string path = $"{Application.dataPath}/settings.xml";
    public AudioManager audioManager;
    [Header("Audio Settings")]
    public Slider MasterSlider;
    public Slider SFXSlider;
    public Slider AmbientSlider;
    public Slider MusicSlider;
    public Toggle MasterToggle;
    public Toggle SFXToggle;
    public Toggle MusicToggle;
    public Toggle AmbientToggle;


    // Start is called before the first frame update
    void Start()
    {
        path = $"{Application.persistentDataPath}/settings.xml";
        _MySettings ??= InitializeSettings();
        SaveSettings();
        ChangeEscapeMenuValues();

        audioManager = (audioManager == null) ? FindObjectOfType<AudioManager>() : audioManager;
        if (audioManager == null)
        {
            Debug.Log("Settings manager couldn't find the Audio Manager.");
        }
        else 
        {
            //Audiomanager loads stuff
            audioManager.LoadSettings(_MySettings);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeEscapeMenuValues() 
    {
        MasterSlider.value = _MySettings.Master.Volume;
        MasterToggle.isOn = _MySettings.Master.IsMuted;
        SFXSlider.value = _MySettings.SFX.Volume;
        SFXToggle.isOn = _MySettings.SFX.IsMuted;
        AmbientSlider.value = _MySettings.Ambient.Volume;
        AmbientToggle.isOn = _MySettings.Ambient.IsMuted;
        MusicSlider.value = _MySettings.Music.Volume;
        MusicToggle.isOn = _MySettings.Music.IsMuted;
    }
    private MySettings InitializeSettings() 
    {
        MySettings OurSettings = new();


        if (File.Exists(path))
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(MySettings));
                using StreamReader sr = new StreamReader(path);
                {
                    OurSettings = (MySettings)ser.Deserialize(sr);
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("Issue with loading settings: " + e.Message);

                // probably remake the settings file
            }
        }


        return OurSettings;
    }

    public static void SaveSettings() 
    {
        if (_MySettings == null) 
        {
            Debug.LogError("Trying to save settings, but settings are null.");
            return;
        }

        XmlSerializer Serializer = new XmlSerializer(typeof(MySettings));
        if (!Directory.Exists(Application.dataPath))
            Directory.CreateDirectory(Application.dataPath);
        using var writer = new StreamWriter(path);
        Serializer.Serialize(writer, _MySettings);
        writer.Close();
        Debug.Log("Settings have been saved: " + path );

    }
}

[System.Serializable]
public class MySettings 
{
    public VolumeSetting Master;
    public VolumeSetting SFX;
    public VolumeSetting Music;
    public VolumeSetting Ambient;

    public MySettings() 
    {
        Master = new();
        Music = new();
        Ambient = new();
        Master.Volume = 0.5f;
        Master.IsMuted = true;
        Music.IsMuted = true;
        Music.Volume = 1f;
        Ambient.IsMuted = true;
        Ambient.Volume = 1f;
        SFX = new();
        SFX.IsMuted = true;
        SFX.Volume = 1f;
    }
}

public struct VolumeSetting 
{
    public bool IsMuted;
    public float Volume ;

}
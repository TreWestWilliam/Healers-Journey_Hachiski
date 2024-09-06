using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer audioMixer;

    public AudioSource sfxPrefab;

    #region Controls
    private void setVolume(string type, float volume)
    {
        audioMixer.SetFloat(type, Mathf.Log10(volume) * 20f);

        if (SettingsManager._MySettings == null) 
        {
            Debug.LogWarning("Settings aren't intiialized!");
            return;
        }

        switch (type) 
        {
            case "masterVolume": SettingsManager._MySettings.Master.Volume = volume; break;
            case "ambientVolume": SettingsManager._MySettings.Ambient.Volume = volume; break;
            case "musicVolume": SettingsManager._MySettings.Music.Volume = volume;break;
            case "sfxVolume": SettingsManager._MySettings.SFX.Volume = volume;break;
        }
    }

    private void setMute(string type, bool mute)
    {
        float volume = !mute ? -80f : 0f;
        audioMixer.SetFloat(type, volume);

        if (SettingsManager._MySettings == null)
        {
            Debug.LogWarning("Settings aren't intiialized!");
            return;
        }

        switch (type)
        {
            case "masterMute": SettingsManager._MySettings.Master.IsMuted = mute; break;
            case "ambientMute": SettingsManager._MySettings.Ambient.IsMuted = mute; break;
            case "musicMute": SettingsManager._MySettings.Music.IsMuted = mute; break;
            case "sfxMute": SettingsManager._MySettings.Music.IsMuted = mute; break;
        }

    }
    public void setMasterVolume(float volume)
    {
        setVolume("masterVolume", volume);
    }

    public void setAmbientVolume(float volume)
    {
        setVolume("ambientVolume", volume);
    }

    public void setMusicVolume(float volume)
    {
        setVolume("musicVolume", volume);
    }

    public void setSFXVolume(float volume)
    {
        setVolume("sfxVolume", volume);
    }

    public void setMasterMute(bool mute)
    {
        setMute("masterMute", mute);
    }

    public void setAmbientMute(bool mute)
    {
        setMute("ambientMute", mute);
    }

    public void setMusicMute(bool mute)
    {
        setMute("musicMute", mute);
    }

    public void setSFXMute(bool mute)
    {
        setMute("sfxMute", mute);
    }
    #endregion

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSettings(MySettings setting)
    {

        audioMixer.SetFloat("masterVolume", Mathf.Log10(setting.Master.Volume) * 20f);
        audioMixer.SetFloat("ambientVolume", Mathf.Log10(setting.Ambient.Volume) * 20f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(setting.Music.Volume) * 20f);
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(setting.SFX.Volume) * 20f);
        setMasterMute(setting.Master.IsMuted);
        setMusicMute(setting.Music.IsMuted);
        setSFXMute(setting.SFX.IsMuted);
        setAmbientMute(setting.Ambient.IsMuted);
    }


    public void playAudioClip(AudioClip audioClip, Transform location, float volume)
    {
        AudioSource audioSource = Instantiate<AudioSource>(sfxPrefab, location.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}

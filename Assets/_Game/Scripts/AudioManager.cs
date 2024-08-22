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
    }

    private void setMute(string type, bool mute)
    {
        float volume = !mute ? -80f : 0f;
        audioMixer.SetFloat(type, volume);

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

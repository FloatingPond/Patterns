using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class SoundManager : MonoBehaviour
{
    public AudioSource Music, SFX, Voice;
    public AudioClip audioClip_SFX, audioClip_Music, audioClip_Voice;

    public AudioMixer audioMixer;
    public float masterFloat, musicFloat, SFX_Float, voiceFloat;

    public Slider masterSlider, musicSlider, SFX_slider, voiceSlider;

    [Button(ButtonSizes.Small)]
    private void SetMusicVolumeButton()
    {
        SetMusicVolume(musicFloat);
    }
    [Button(ButtonSizes.Small)]
    private void GetMusicVolume()
    {
        float volume = GetMusicVolumeFunction();
        Debug.Log("Button: Volume after:" + volume);
    }

    public float GetMusicVolumeFunction()
    {
        float value;
        bool result = audioMixer.GetFloat("Music_Volume", out value);
        if (result)
        {
            return value;
        }
        else
        {
            return 69f;
        }
    }

    private void Awake()
    {
        LoadAudioClip_Music("Funky Funky loop");
        PlayMusic();
        IEnumerator c = FuckAudioMixer();
        StartCoroutine(c);
    }

    IEnumerator FuckAudioMixer()
    {
        yield return new WaitForSeconds(0.1f);
        SetMusicVolume(musicFloat);
        SetMasterVolume(masterFloat);
        SetSFXVolume(SFX_Float);
        SetVoiceVolume(voiceFloat);
    }

    public void LoadSliders()
    {
        masterSlider.value = masterFloat;
        musicSlider.value = musicFloat;
        SFX_slider.value = SFX_Float;
        voiceSlider.value = voiceFloat;

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(SFX_slider.value);
        SetVoiceVolume(voiceSlider.value);
    }

    public void SetMusicVolume(float volume)
    {
        musicFloat = musicSlider.value;
        audioMixer.SetFloat("Music_Volume", volume);

        float volumeee = GetMusicVolumeFunction();
        Debug.Log("Volume after:" + volumeee);

        //ES3.Save<float>("Music_Volume", volume, "settings.data");
    }
    public void SetSFXVolume(float volume)
    {
        SFX_Float = SFX_slider.value;
        audioMixer.SetFloat("SFX_Volume", volume);
        //ES3.Save<float>("SFX_Volume", volume, "settings.data");
    }
    public void SetVoiceVolume(float volume)
    {
        voiceFloat = voiceSlider.value;
        audioMixer.SetFloat("Voice_Volume", volume);
        //ES3.Save<float>("SFX_Volume", volume, "settings.data");
    }
    public void SetMasterVolume(float volume)
    {
        masterFloat = masterSlider.value;
        audioMixer.SetFloat("Master_Volume", volume);
        //ES3.Save<float>("SFX_Volume", volume, "settings.data");
    }
    public void LoadAudioClip_SFX(string clipPath)
    {
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        audioClip_SFX = Resources.Load<AudioClip>("Sounds/SFX/" + clipPath);
    }
    public void LoadAudioClip_Music(string clipPath)
    {
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        audioClip_Music = Resources.Load<AudioClip>("Sounds/Music/" + clipPath);
    }
    public void LoadAudioClip_Voice(string clipPath)
    {
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        audioClip_Voice = Resources.Load<AudioClip>("Sounds/Voice/" + clipPath);
    }
    public void PlaySFX()
    {
        SFX.clip = audioClip_SFX;
        SFX.Play();
    }
    public void PlayMusic()
    {
        Music.clip = audioClip_Music;
        Music.Play();
    }
    public void PlayVoice()
    {
        Voice.clip = audioClip_Voice;
        Voice.Play();
    }
}

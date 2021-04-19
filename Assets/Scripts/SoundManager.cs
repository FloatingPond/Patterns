using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioSource Music, SFX, Voice;
    public AudioClip audioClip_SFX, audioClip_Music, audioClip_Voice;

    public AudioMixer audioMixer;
    public float masterFloat, musicFloat, SFX_Float, voiceFloat;

    public Slider masterSlider, musicSlider, SFX_slider, voiceSlider;
    private void Start()
    {
        LoadAudioClip_Music("Chill Funky loop");
        PlayMusic();
        masterSlider.value = masterFloat;
        SetMasterVolume(masterFloat);
        musicSlider.value = musicFloat;
        SetMusicVolume(musicFloat);
        SFX_slider.value = SFX_Float;
        SetSFXVolume(SFX_Float);
        voiceSlider.value = voiceFloat;
        SetVoiceVolume(voiceFloat);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music_Volume", volume);
        //ES3.Save<float>("Music_Volume", volume, "settings.data");
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX_Volume", volume);
        //ES3.Save<float>("SFX_Volume", volume, "settings.data");
    }
    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("Voice_Volume", volume);
        //ES3.Save<float>("SFX_Volume", volume, "settings.data");
    }
    public void SetMasterVolume(float volume)
    {
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

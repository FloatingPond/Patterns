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
    public float masterFloat = 1, musicFloat = 1, SFX_Float = 1, voiceFloat = 1;

    public Slider masterSlider, musicSlider, SFX_slider, voiceSlider;
    private void Awake()
    {
        LoadAudioClip_Music("Funky Funky loop");
        PlayMusic();
    }

    public void LoadSliders()
    {
        //SetMasterVolume(masterFloat);
        //SetMusicVolume(musicFloat);
        //SetSFXVolume(SFX_Float);
        //SetVoiceVolume(voiceFloat);

        //masterSlider.value = masterFloat;
        //musicSlider.value = musicFloat;
        //SFX_slider.value = SFX_Float;
        //voiceSlider.value = voiceFloat;
    }

    public void LoadSliders2()
    {
        SetMasterVolume(masterFloat);
        SetMusicVolume(musicFloat);
        SetSFXVolume(SFX_Float);
        SetVoiceVolume(voiceFloat);

        masterSlider.value = masterFloat;
        musicSlider.value = musicFloat;
        SFX_slider.value = SFX_Float;
        voiceSlider.value = voiceFloat;
    }

    private void Start()
    {

        LoadSliders2();
        
        //masterSlider.value = PlayerPrefs.GetFloat("Master_Volume", 1);
        //musicSlider.value = PlayerPrefs.GetFloat("Music_Volume", 1);
        //SFX_slider.value = PlayerPrefs.GetFloat("SFX_Volume", 1);
        //voiceSlider.value = PlayerPrefs.GetFloat("Voice_Volume", 1);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music_Volume", Mathf.Log(volume) * 20);
        //PlayerPrefs.SetFloat("Music_Volume", volume);
        musicFloat = volume;
        //ES3.Save<float>("Music_Volume", volume, "settings.data");
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX_Volume", Mathf.Log(volume) * 20);
        //PlayerPrefs.SetFloat("SFX_Volume", volume);
        SFX_Float = volume;
        //ES3.Save<float>("SFX_Volume", volume, "settings.data");
    }
    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("Voice_Volume", Mathf.Log(volume) * 20);
        //PlayerPrefs.SetFloat("Voice_Volume", volume);
        voiceFloat = volume;
        //ES3.Save<float>("SFX_Volume", volume, "settings.data");
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master_Volume", Mathf.Log(volume) * 20);
        //PlayerPrefs.SetFloat("Master_Volume", volume);
        masterFloat = volume;
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

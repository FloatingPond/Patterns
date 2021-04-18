using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Music, SFX, Voice;
    public AudioClip audioClip_SFX, audioClip_Music, audioClip_Voice;

    public void LoadAudioClip_SFX(string clipPath)
    {
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        audioClip_SFX = Resources.Load<AudioClip>("Sounds/" + clipPath);
    }
    public void LoadAudioClip_Music(string clipPath)
    {
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        audioClip_Music = Resources.Load<AudioClip>("Sounds/" + clipPath);
    }
    public void LoadAudioClip_Voice(string clipPath)
    {
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        audioClip_Voice = Resources.Load<AudioClip>("Sounds/" + clipPath);
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

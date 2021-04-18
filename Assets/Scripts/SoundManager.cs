using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Music, SFX;
    public AudioClip audioClip_SFX, audioClip_Music;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAudioClip_SFX(string clipPath)
    {
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        audioClip_SFX = Resources.Load<AudioClip>("Sounds/SFX/announcer_go_01");
    }
    public void LoadAudioClip_Music(string clipPath)
    {
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)
        audioClip_Music = Resources.Load<AudioClip>("Sounds/SFX/announcer_go_01");
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
}

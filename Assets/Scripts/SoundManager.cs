using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource Music, SFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySFX(AudioClip clip)
    {
        SFX.clip = clip;
        SFX.Play();
    }
    public void PlayMusic(AudioClip track)
    {
        Music.clip = track;
        Music.Play();
    }
}

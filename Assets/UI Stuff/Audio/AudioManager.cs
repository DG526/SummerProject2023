using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Can manipulate the volume
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("SFX")]
    public AudioClip gemPickUp;
    public AudioClip powerUp;
    public AudioClip buy;
    public AudioClip enemyDeath;
    public AudioClip select;
    public AudioClip start;

    [Header ("Shooting")]
    public AudioClip fire;
    public AudioClip water;
    public AudioClip earth;
    public AudioClip wind;
    public AudioClip poison;
    public AudioClip lightning;
    public AudioClip light;

    // Start is called before the first frame update
    void Start()
    {
        //musicSource.clip = background;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySelectSound()
    {
        sfxSource.PlayOneShot(select);
    }

    public void PlayStartGameSound()
    {
        sfxSource.PlayOneShot(start);
    }

    /* Calls the the gameObject 
     * audio = GameObject.Find("Audio").GetComponent<AudioManager>();
     * 
     * Plays the sound
     * audio.PlaySFX(audio.buy);
     */
}

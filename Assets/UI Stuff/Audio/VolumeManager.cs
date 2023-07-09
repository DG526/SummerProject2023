using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider, musicSlider, soundSlider;
    static float masterVolume = 0.8f, bgmVolume = 0.7f, sfxVolume = 0.9f;
    // Start is called before the first frame update
    void Start()
    {
        masterSlider.value = masterVolume;
        musicSlider.value = bgmVolume;
        soundSlider.value = sfxVolume;
        mixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat("MusicVolume", Mathf.Log10(bgmVolume) * 20);
        mixer.SetFloat("SoundVolume", Mathf.Log10(sfxVolume) * 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void Initialize()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        else
            masterVolume = 0.8f;
        if (PlayerPrefs.HasKey("MusicVolume"))
            bgmVolume = PlayerPrefs.GetFloat("MusicVolume");
        else
            bgmVolume = 0.7f;
        if (PlayerPrefs.HasKey("SoundVolume"))
            sfxVolume = PlayerPrefs.GetFloat("SoundVolume");
        else
            sfxVolume = 0.9f;
    }
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        bgmVolume = volume;
        mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    public void SetSoundVolume(float volume)
    {
        sfxVolume = volume;
        mixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }
    public static float GetMusicVolume()
    {
        return bgmVolume * masterVolume;
    }
    public static float GetSoundVolume()
    {
        return sfxVolume * masterVolume;
    }
    public static float GetMusicVolumeModifier()
    {
        return bgmVolume;
    }
    public static float GetSoundVolumeModifier()
    {
        return sfxVolume;
    }
}

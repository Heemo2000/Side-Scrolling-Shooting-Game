using System;
using System.Collections.Generic;

using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : GenericSingleton<SoundManager>
{
    
    [SerializeField]private AudioSource musicAudioSource;
    [SerializeField]private AudioSource sfxAudioSource;
    [SerializeField]private Sound[] sounds;

    [SerializeField]private SoundType startingMusic;
    [SerializeField]private AudioMixer musicAudioMixer;
    [SerializeField]private AudioMixer sfxAudioMixer;
    private Dictionary<SoundType,Sound> soundDictionary;
    protected override void Awake() {
        
        base.Awake();
        DontDestroyOnLoad(gameObject);
        soundDictionary = new Dictionary<SoundType, Sound>();        
        foreach(Sound sound in sounds)
        {
            soundDictionary.Add(sound.soundType,sound);
        }
    }

    private void Start() 
    {
        PlayMusic(startingMusic);
    }

    public void PlayMusic(SoundType soundType)
    {
        Play(soundType,musicAudioSource);
    }

    public void PlaySFX(SoundType soundType)
    {
        Play(soundType,sfxAudioSource);
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    public void ResumeMusic()
    {
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }


    public void ResumeSFX()
    {
        sfxAudioSource.Play();
    }

    public void StopSFX()
    {
        sfxAudioSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("musicVol",volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioMixer.SetFloat("sfxVol",volume);
    }
    private void Play(SoundType soundType,AudioSource source)
    {
        if(soundDictionary.TryGetValue(soundType,out Sound sound))
        {
            source.clip = sound.clip;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.volume = sound.volume;
            source.Play();
        }
        else
        {
            Debug.Log("Sound to play not found!!");
        }        
    }
}

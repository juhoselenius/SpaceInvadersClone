using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/*
 * @Author: Juho Selenius
 * With the help of "Introduction to AUDIO in Unity"
 * by Brackeys (https://www.youtube.com/watch?v=6OT43pvUyfY)
 * and "Separate Volumes for Music & Sound Effects! - Unity Tutorial"
 * by Ricky Dev (https://www.youtube.com/watch?v=LfU5xotjbPw).
 */

public class AudioManager : MonoBehaviour
{
    public static AudioManager aManager;

    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundEffectsMixerGroup;

    public Sound[] sounds;

    private void Awake()
    {
        if (aManager == null)
        {
            DontDestroyOnLoad(gameObject);
            aManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            switch (s.audioType)
            {
                case Sound.AudioTypes.soundEffects:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;

                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }
        }
    }

    private void Start()
    {
        Play("MainTheme");
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(0.51f) * 20);
    }

    private void Update()
    {
        //Debug.Log("Music value: " + GetMusicValue());
        //Debug.Log("Sound Effects value: " + GetSoundEffectsValue());
    }

    public void Play(string clipName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        if(s == null)
        {
            Debug.Log("Couldn't find the \"" + clipName + "\" sound from the sound list. Check spelling!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string clipName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == clipName);
        if (s == null)
        {
            Debug.Log("Couldn't find the \"" + clipName + "\" sound from the sound list. Check spelling!");
            return;
        }
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.source.isPlaying)
            {
                Debug.Log(SceneManager.GetActiveScene().name + " scene stops (StopPlayingAllSounds) sound: " + sound.source.clip.name);
                sound.source.Stop();
            }
        }
    }

    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(SetVolume.musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(SetVolume.soundEffectsVolume) * 20);
    }

    public float GetMusicValue()
    {
        float value;
        bool result = musicMixerGroup.audioMixer.GetFloat("MusicVolume", out value);
        if (result)
        {
            return Mathf.Pow(10, (value / 20));
        }
        else
        {
            Debug.Log("No value");
            return 0f;
        }
    }

    public float GetSoundEffectsValue()
    {
        float value;
        bool result = soundEffectsMixerGroup.audioMixer.GetFloat("SoundEffectsVolume", out value);
        if (result)
        {
            return Mathf.Pow(10, (value / 20));
        }
        else
        {
            Debug.Log("No value");
            return 0f;
        }
    }
}

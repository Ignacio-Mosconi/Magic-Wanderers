using System;
using UnityEngine;
using UnityEngine.Audio;

public enum MixerType
{
    Sfx,
    Music
}

public class AudioManager : MonoBehaviour
{
    #region Singleton
    static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<AudioManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("Audio Manager");
                    instance = gameObj.AddComponent<AudioManager>();
                }
            }

            return instance;
        }
    }
    #endregion

    [SerializeField] AudioMixer[] audioMixers;
    [SerializeField] Sound[] soundsUI;

    const float MixerMultiplier = 12f;
    const float MuteValue = -80f;

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogError("Warning: more than one Audio Manager in the secene.", gameObject);
            return;
        }

        foreach (Sound sound in soundsUI)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void PlaySound(string soundName)
    {
        Sound sound = Array.Find(soundsUI, s => s.name == soundName);
        if (sound != null)
            sound.audioSource.Play();
        else
            Debug.Log("Warning: the '" + soundName + "' could not be found.");
    }

    public void SetMixerVolume(MixerType mixerType, float volume)
    {
        audioMixers[(int)mixerType].SetFloat("Volume", Mathf.Log(volume) * MixerMultiplier);
    }

    public void MuteMixer(MixerType mixerType)
    {
        audioMixers[(int)mixerType].SetFloat("Volume", MuteValue);
    }

    public void UnmuteMixer(MixerType mixerType)
    {
        float originalVolume = 0f;

        switch (mixerType)
        {
            case MixerType.Sfx:
                originalVolume = AppManager.Instance.SfxVolume;
                break;
            case MixerType.Music:
                originalVolume = AppManager.Instance.MusicVolume;
                break;
            default:
                originalVolume = MuteValue;
                break;
        }

        audioMixers[(int)mixerType].SetFloat("Volume", originalVolume);
    }

    public bool IsMixerMuted(MixerType mixerType)
    {
        float mixerVolume;
        
        audioMixers[(int)mixerType].GetFloat("Volume", out mixerVolume);

        return (mixerVolume == MuteValue);
    }
}
using System;
using UnityEngine;
using UnityEngine.Audio;

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

    [SerializeField] Sound[] soundsUI;

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
}
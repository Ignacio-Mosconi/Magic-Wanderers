using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct Sound
{
    public AudioClip audioClip;
    public string name;
    public AudioMixerGroup audioMixerGroup;
    [Range(0f, 1f)] public float volume;
    [Range(-3f, 3f)] public float pitch;
    public bool loop;
    [HideInInspector] public AudioSource audioSource;
}
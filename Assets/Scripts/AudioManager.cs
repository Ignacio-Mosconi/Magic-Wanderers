using System;
using UnityEngine;
using UnityEngine.Audio;

public enum MixerType
{
    Sfx,
    Music,
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
    [SerializeField] AudioClip[] soundsUI;
    [SerializeField] AudioClip[] themes;

    AudioSource sfxSource;
    AudioSource musicSource;

    const float MixerMultiplier = 12f;
    const float MuteValue = -80f;

    void Awake()
    {
        if (Instance != this)
            Debug.LogError("Warning: more than one Audio Manager in the secene.", gameObject);
    }

    void Start()
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = audioMixers[(int)MixerType.Sfx].outputAudioMixerGroup;
        sfxSource.playOnAwake = false;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = audioMixers[(int)MixerType.Music].outputAudioMixerGroup;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.7f;
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = Array.Find(soundsUI, c => c.name == soundName);

        if (clip)
            sfxSource.PlayOneShot(clip);
        else
            Debug.Log("Warning: the '" + soundName + "' sound could not be found.", gameObject);
    }

    public void PlayTheme(string themeName)
    {
        AudioClip clip = Array.Find(themes, c => c.name == themeName);

        if (clip)
            musicSource.PlayOneShot(clip);
        else
            Debug.Log("Warning: the '" + themeName + "' theme could not be found.", gameObject);
    }

    public void PlayRandomTheme()
    {
        int randomIndex = UnityEngine.Random.Range(0, themes.GetLength(0));
        
        musicSource.PlayOneShot(themes[randomIndex]);
    }

    public void StopMusicPlayback()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
        else
            Debug.Log("Warning: no themes are currently being played.", gameObject);
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
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Button[] muteButtons;
    [SerializeField] Sprite[] soundIcons;

    void OnEnable()
    {
        sfxVolumeSlider.value = AppManager.Instance.SfxVolume;
        musicVolumeSlider.value = AppManager.Instance.MusicVolume;

        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void SetSfxVolume(float volume)
    {
        AppManager.Instance.SfxVolume = volume;
        AudioManager.Instance.SetMixerVolume(MixerType.Sfx, volume);

        if (volume == 0f)
            muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[1];
        else
            if (muteButtons[(int)MixerType.Sfx].image.sprite != soundIcons[0])
                muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[0];
    }

    public void SetMusicVolume(float volume)
    {
        AppManager.Instance.MusicVolume = volume;
        AudioManager.Instance.SetMixerVolume(MixerType.Music, volume);

        if (volume == 0f)
            muteButtons[(int)MixerType.Music].image.sprite = soundIcons[1];
        else
            if (muteButtons[(int)MixerType.Music].image.sprite != soundIcons[0])
                muteButtons[(int)MixerType.Music].image.sprite = soundIcons[0];
    }

    public void ChangeSfxAvailability()
    {
        if (!AudioManager.Instance.IsMixerMuted(MixerType.Sfx))
        {
            AudioManager.Instance.MuteMixer(MixerType.Sfx);
            muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[1];
        }
        else
        {
            AudioManager.Instance.UnmuteMixer(MixerType.Sfx);
            muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[0];
        }
    }

    public void ChangeMusicAvailability()
    {
        if (!AudioManager.Instance.IsMixerMuted(MixerType.Music))
        {
            AudioManager.Instance.MuteMixer(MixerType.Music);
            muteButtons[(int)MixerType.Music].image.sprite = soundIcons[1];
        }
        else
        {
            AudioManager.Instance.UnmuteMixer(MixerType.Music);
            muteButtons[(int)MixerType.Music].image.sprite = soundIcons[0];
        }
    }
}
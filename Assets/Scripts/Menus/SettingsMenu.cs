using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public const float DefaultVolumeSliderValue = 0.75f;

    [Header("Audio References")]
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Button[] muteButtons;
    [SerializeField] Sprite[] soundIcons;

    [Header("Profiles References")]
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TMP_InputField[] nameInputFields;
    [SerializeField] TMP_Dropdown[] symbolDropdowns;

    [Header("Preferences References")]
    [SerializeField] ToggleValue[] defaultStartingLifeTVs;
    [SerializeField] ToggleValue[] defaultNumberOfPlayersTVs;

    bool isSettingUpValues = true;

    void Start()
    {
        bool wasSfxMuted = AppManager.Instance.IsMixerMuted(MixerType.Sfx);
        bool wasMusicMuted = AppManager.Instance.IsMixerMuted(MixerType.Music);

        sfxVolumeSlider.value = AppManager.Instance.SfxVolume;
        musicVolumeSlider.value = AppManager.Instance.MusicVolume;

        if (wasSfxMuted)
        {
            muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[1];
            StartCoroutine(MuteAudio(MixerType.Sfx, 0f));
        }
        if (wasMusicMuted)
        {
            muteButtons[(int)MixerType.Music].image.sprite = soundIcons[1];
            StartCoroutine(MuteAudio(MixerType.Music, 0f));
        }

        foreach (ToggleValue tv in defaultStartingLifeTVs)
        {
            tv.toggle.isOn = (tv.value == AppManager.Instance.DefaultStartingLife);
            tv.toggle.onValueChanged.AddListener(ChangeDefaultStartingLife);
            tv.toggle.GetComponentInChildren<TextMeshProUGUI>().text = tv.value.ToString();
        }

        foreach (ToggleValue tv in defaultNumberOfPlayersTVs)
        {
            tv.toggle.isOn = (tv.value == AppManager.Instance.DefaultNumberOfPlayers);
            tv.toggle.onValueChanged.AddListener(ChangeDefaultNumberOfPlayers);
            tv.toggle.GetComponentInChildren<TextMeshProUGUI>().text = tv.value.ToString();
        }

        int i = 0;

        foreach (TMP_InputField nameInputField in nameInputFields)
        {
            nameInputField.text = AppManager.Instance.GetPlayerName(i);
            i++;
        }

        i = 0;

        foreach (TMP_Dropdown symbolDropdown in symbolDropdowns)
        {
            symbolDropdown.value = (int)AppManager.Instance.GetPlayerSymbol(i);
            i++;
        }

        scrollRect.verticalNormalizedPosition = 1f;
        isSettingUpValues = false;
    }

    IEnumerator MuteAudio(MixerType mixerType, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        AudioManager.Instance.MuteMixer(mixerType);

        if (mixerType == MixerType.Music)
            AudioManager.Instance.StopMusicPlayback();
    }

    IEnumerator DisableInputFieldInteractability(int playerIndex)
    {
        yield return new WaitForEndOfFrame();

        nameInputFields[playerIndex].interactable = false;
    }

    public void SetSfxVolume(float volume)
    {
        AppManager.Instance.SfxVolume = volume;
        AudioManager.Instance.SetMixerVolume(MixerType.Sfx, volume);

        if (AudioManager.Instance.IsVolumeLevelNull(volume))
        {
            AppManager.Instance.SetMixerMuted(MixerType.Sfx, true);
            muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[1];
        }
        else
            if (muteButtons[(int)MixerType.Sfx].image.sprite != soundIcons[0])
            {
                AppManager.Instance.SetMixerMuted(MixerType.Sfx, false);
                muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[0];
            }
    }

    public void SetMusicVolume(float volume)
    {
        AppManager.Instance.MusicVolume = volume;
        AudioManager.Instance.SetMixerVolume(MixerType.Music, volume);

        if (AudioManager.Instance.IsVolumeLevelNull(volume))
        {
            AppManager.Instance.SetMixerMuted(MixerType.Music, true);
            muteButtons[(int)MixerType.Music].image.sprite = soundIcons[1];
            AudioManager.Instance.StopMusicPlayback();
        }
        else
            if (muteButtons[(int)MixerType.Music].image.sprite != soundIcons[0])
            {
                AppManager.Instance.SetMixerMuted(MixerType.Music, false);
                muteButtons[(int)MixerType.Music].image.sprite = soundIcons[0];
                AudioManager.Instance.PlayTheme("Menu Theme");
            }
    }

    public void ChangeSfxAvailability()
    {
        if (!AudioManager.Instance.IsMixerMuted(MixerType.Sfx))
        {
            float waitTime = AudioManager.Instance.GetSoundLength("Menu Return");
            
            AudioManager.Instance.PlaySound("Menu Return");
            
            muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[1];
            StartCoroutine(MuteAudio(MixerType.Sfx, waitTime));
        }
        else
        {
            AudioManager.Instance.UnmuteMixer(MixerType.Sfx);
            if (AudioManager.Instance.IsVolumeLevelNull(AppManager.Instance.SfxVolume))
                sfxVolumeSlider.value = DefaultVolumeSliderValue;
            muteButtons[(int)MixerType.Sfx].image.sprite = soundIcons[0];
        }
    }

    public void ChangeMusicAvailability()
    {
        if (!AudioManager.Instance.IsMixerMuted(MixerType.Music))
        {   
            float waitTime = AudioManager.Instance.GetSoundLength("Menu Return");
            
            AudioManager.Instance.PlaySound("Menu Return");

            muteButtons[(int)MixerType.Music].image.sprite = soundIcons[1];
            StartCoroutine(MuteAudio(MixerType.Music, waitTime));
        }
        else
        {
            AudioManager.Instance.UnmuteMixer(MixerType.Music);
            if (AudioManager.Instance.IsVolumeLevelNull(AppManager.Instance.MusicVolume))
                musicVolumeSlider.value = DefaultVolumeSliderValue;
            muteButtons[(int)MixerType.Music].image.sprite = soundIcons[0];
            AudioManager.Instance.PlayTheme("Menu Theme");
        }
    }

    public void ChangePlayerName(int playerIndex)
    {
        if (!isSettingUpValues)
        {
            string name = nameInputFields[playerIndex].text;
            
            AppManager.Instance.SetPlayerName(name, playerIndex);
            StartCoroutine(DisableInputFieldInteractability(playerIndex));

            AudioManager.Instance.PlaySound("Menu Close");
        }
    }

    public void ChangePlayerSymbol(int playerIndex)
    {
        if (!isSettingUpValues)
        {
            Symbol symbol = (Symbol)symbolDropdowns[playerIndex].value;
            
            AppManager.Instance.SetPlayerSymbol(symbol, playerIndex);
            
            AudioManager.Instance.PlaySound("Menu Close");
        }
    }

    public void ChangeDefaultStartingLife(bool wasToggledOn)
    {
        if (!isSettingUpValues && wasToggledOn)
        {
            ToggleValue toggleValue = Array.Find(defaultStartingLifeTVs, tv => tv.toggle.isOn);
            AppManager.Instance.DefaultStartingLife = toggleValue.value;

            AudioManager.Instance.PlaySound("Menu Select");
        }
    }

    public void ChangeDefaultNumberOfPlayers(bool wasToggledOn)
    {
        if (!isSettingUpValues && wasToggledOn)
        {
            ToggleValue toggleValue = Array.Find(defaultNumberOfPlayersTVs, tv => tv.toggle.isOn);
            AppManager.Instance.DefaultNumberOfPlayers = toggleValue.value;

            AudioManager.Instance.PlaySound("Menu Select");
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] ToggleValue[] defaultStartingLifeTVs;
    [SerializeField] ToggleValue[] defaultNumberOfPlayersTVs;
    [SerializeField] Button[] muteButtons;
    [SerializeField] Sprite[] soundIcons;
    [SerializeField] TMP_InputField[] nameInputFields;
    [SerializeField] TMP_Dropdown[] symbolDropdowns;

    bool isSettingUpValues = true;

    void OnEnable()
    {
        sfxVolumeSlider.value = AppManager.Instance.SfxVolume;
        musicVolumeSlider.value = AppManager.Instance.MusicVolume;

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

    public void ChangePlayerName(int playerIndex)
    {
        if (!isSettingUpValues)
        {
            string name = nameInputFields[playerIndex].text;
            
            AppManager.Instance.SetPlayerName(name, playerIndex);

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
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider defaultStartingLifeSlider;
    [SerializeField] Slider defaultNumberOfPlayersSlider;
    [SerializeField] Button[] muteButtons;
    [SerializeField] Sprite[] soundIcons;
    [SerializeField] TMP_InputField[] nameInputFields;
    [SerializeField] TMP_Dropdown[] symbolDropdowns;

    void OnEnable()
    {
        sfxVolumeSlider.value = AppManager.Instance.SfxVolume;
        musicVolumeSlider.value = AppManager.Instance.MusicVolume;
        defaultStartingLifeSlider.value = AppManager.Instance.DefaultStartingLife / AppManager.StartingLifeMultiplier;
        defaultNumberOfPlayersSlider.value = AppManager.Instance.DefaultNumberOfPlayers;

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
        string name = nameInputFields[playerIndex].text;
        
        AppManager.Instance.SetPlayerName(name, playerIndex);
    }

    public void ChangePlayerSymbol(int playerIndex)
    {
        Symbol symbol = (Symbol)symbolDropdowns[playerIndex].value;
        
        AppManager.Instance.SetPlayerSymbol(symbol, playerIndex);
    }

    public void ChangeDefaultStartingLife(float value)
    {
        AppManager.Instance.DefaultStartingLife = (int)value * AppManager.StartingLifeMultiplier;
    }

    public void ChangeDefaultNumberOfPlayers(float value)
    {
        AppManager.Instance.DefaultNumberOfPlayers = (int)value;
    }
}
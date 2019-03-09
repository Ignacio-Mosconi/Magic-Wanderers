using System;
using UnityEngine;

public struct PlayerProfile
{
    public string name;
    public Symbol symbol;
}

public class AppManager : MonoBehaviour
{
    #region Singleton
    static AppManager instance;

    public static AppManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<AppManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("App Manager");
                    instance = gameObj.AddComponent<AppManager>();
                }
            }

            return instance;
        }
    }
    #endregion

    public const int MaxPlayers = 10;
    public const int MinPlayers = 2;
    public const int MaxSimultaneousPlayers = 4;
    public const int TargetFrameRate = 15;

    PlayerProfile[] playerProfiles;
    int defaultStartingLife;
    int defaultNumberOfPlayers;
    float sfxVolume;
    float musicVolume;
    bool[] isMixerMuted = { false, false };

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogError("Warning: more than one App Manager in the secene.", gameObject);
            return;
        }

        Application.targetFrameRate = TargetFrameRate;

        LoadPreferences();
    }

    void LoadPreferences()
    {
        playerProfiles = new PlayerProfile[MaxPlayers];

        defaultStartingLife = PlayerPrefs.GetInt("DefaultStartingLife", 20);
        defaultNumberOfPlayers = PlayerPrefs.GetInt("DefaultNumberOfPlayers", MinPlayers);
        for (int i = 0; i < MaxPlayers; i++)
        {
            playerProfiles[i].name = PlayerPrefs.GetString("Player" + (i + 1) + "Name", "Planeswalker #" + (i + 1));
            playerProfiles[i].symbol = (Symbol)PlayerPrefs.GetInt("Player" + (i + 1) + "Symbol", UnityEngine.Random.Range(0, (int)Symbol.Count));
        }
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        isMixerMuted[(int)MixerType.Sfx] = Convert.ToBoolean(PlayerPrefs.GetInt("SfxMuted", 0));
        isMixerMuted[(int)MixerType.Music] = Convert.ToBoolean(PlayerPrefs.GetInt("MusicMuted", 0));
    }

    #region Getters & Setters

    public string GetPlayerName(int playerIndex)
    {
        return playerProfiles[playerIndex].name;
    }

    public Symbol GetPlayerSymbol(int playerIndex)
    {
        return playerProfiles[playerIndex].symbol;
    }

    public Symbol GetPlayerSymbol(string playerName)
    {
        PlayerProfile playerProfile = Array.Find(playerProfiles, profile => profile.name == playerName);

        if (playerProfile.name == "")
            Debug.LogError("Warning: there are no players that have the name given.", gameObject);

        return playerProfile.symbol;
    }

    public bool IsMixerMuted(MixerType mixerType)
    {
        return isMixerMuted[(int)mixerType];
    }

    public void SetPlayerName(string name, int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerProfiles.GetLength(0))
        {
            playerProfiles[playerIndex].name = name;
            PlayerPrefs.SetString("Player" + (playerIndex + 1) + "Name", playerProfiles[playerIndex].name);
        }
        else
            Debug.LogError("Warning: attempted to access to a player index out of range.");
    }

    public void SetPlayerSymbol(Symbol symbol, int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < playerProfiles.GetLength(0))
        {
            playerProfiles[playerIndex].symbol = symbol;
            PlayerPrefs.SetInt("Player" + (playerIndex + 1) + "Symbol", (int)playerProfiles[playerIndex].symbol);
        }
        else
            Debug.LogError("Warning: attempted to access to a player index out of range.");
    }

    public void SetMixerMuted(MixerType mixerType, bool value)
    {
        isMixerMuted[(int)mixerType] = value;
        
        switch (mixerType)
        {
            case MixerType.Sfx:
                PlayerPrefs.SetInt("SfxMuted", Convert.ToInt16(value));
                break;
            case MixerType.Music:
                PlayerPrefs.SetInt("MusicMuted", Convert.ToInt16(value));
                break;
        }
    }

    public int DefaultStartingLife
    {
        get { return defaultStartingLife; }
        set
        {
            defaultStartingLife = value;
            PlayerPrefs.SetInt("DefaultStartingLife", defaultStartingLife);
        }
    }

    public int DefaultNumberOfPlayers
    {
        get { return defaultNumberOfPlayers; }
        set
        {
            defaultNumberOfPlayers = value;
            PlayerPrefs.SetInt("DefaultNumberOfPlayers", defaultNumberOfPlayers);
        }
    }

    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = value;
            PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
        }
    }

    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }
    }
    #endregion
}
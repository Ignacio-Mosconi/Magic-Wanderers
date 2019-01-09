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

    const int MaxPlayers = 10;
    const int MinPlayers = 2;

    PlayerProfile[] playerProfiles;
    int defaultNumberOfPlayers;
    int defaultStartingLife;
    float sfxVolume;
    float musicVolume;

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogError("Warning: more than one App Manager in the secene.", gameObject);
            return;
        }

        LoadPreferences();
    }

    void LoadPreferences()
    {
        playerProfiles = new PlayerProfile[MaxPlayers];

        defaultNumberOfPlayers = PlayerPrefs.GetInt("DefaultNumberOfPlayers", MinPlayers);
        defaultStartingLife = PlayerPrefs.GetInt("DefaultStartingLife", 20);
        for (int i = 0; i < MaxPlayers; i++)
        {
            playerProfiles[i].name = PlayerPrefs.GetString("Player" + (i + 1) + "Name", "Planeswalker #" + (i + 1));
            playerProfiles[i].symbol = (Symbol)PlayerPrefs.GetInt("Player" + (i + 1) + "Symbol", UnityEngine.Random.Range(0, (int)Symbol.Count));
        }
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
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

    public int DefaultNumberOfPlayers
    {
        get { return defaultNumberOfPlayers; }
        set
        {
            defaultNumberOfPlayers = value;
            PlayerPrefs.SetInt("DefaultNumberOfPlayers", defaultNumberOfPlayers);
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
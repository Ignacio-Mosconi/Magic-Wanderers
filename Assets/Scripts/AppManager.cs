using UnityEngine;

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

    const int MaxPlayers = 4;
    const int MinPlayers = 2;

    int numberOfPlayers;
    Symbol[] defaultSymbols;
    int startingLife;
    float sfxVolume;
    float musicVolume;

    void Awake()
    {
        defaultSymbols = new Symbol[MaxPlayers];

        numberOfPlayers = PlayerPrefs.GetInt("NumberOfPlayers", MinPlayers);
        for (int i = 0; i < MaxPlayers; i++)
            defaultSymbols[i] = (Symbol)PlayerPrefs.GetInt("Player" + (i + 1) + "Symbol", Random.Range(0, (int)Symbol.Count));
        startingLife = PlayerPrefs.GetInt("StartingLife", 20);
        sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

        DuelManager.Instance.SetUpBoard();
    }

    #region Getters & Setters
    public void SetDefaultSymbol(Symbol symbol, int playerIndex)
    {
        if (playerIndex >= 0 && playerIndex < defaultSymbols.GetLength(0))
        {
            defaultSymbols[playerIndex] = symbol;
            PlayerPrefs.SetInt("Player" + (playerIndex + 1) + "Symbol", (int)defaultSymbols[playerIndex]);
        }
        else
            Debug.LogError("Warning: attempted to access to a player index out of range.");
    }

    public Symbol[] DefaultSymbols
    {
        get { return defaultSymbols; }
    }

    public int NumberOfPlayers
    {
        get { return numberOfPlayers; }
        set
        {
            numberOfPlayers = value;
            PlayerPrefs.SetInt("NumberOfPlayers", numberOfPlayers);
        }
    }

    public int StartingLife
    {
        get { return startingLife; }
        set
        {
            startingLife = value;
            PlayerPrefs.SetInt("StartingLife", startingLife);
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
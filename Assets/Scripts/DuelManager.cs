using UnityEngine;

public class DuelManager : MonoBehaviour
{
    #region Singleton
    static DuelManager instance;

    public static DuelManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<DuelManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("Duel Manager");
                    instance = gameObj.AddComponent<DuelManager>();
                }
            }

            return instance;
        }
    }
    #endregion

    DuelBoard duelBoard;
    int currentNumberOfPlayers;
    int currentStartingLife;

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogError("Warning: more than one Duel Manager in the scene.", gameObject);
            return;
        }

        duelBoard = GetComponentInChildren<DuelBoard>(true);
    }

    public void EnableDuelBoard(int numberOfPlayers, int startingLife)
    {
        currentNumberOfPlayers = numberOfPlayers;
        currentStartingLife = startingLife;
        
        duelBoard.gameObject.SetActive(true);
        duelBoard.SetUpBoard(currentNumberOfPlayers, currentStartingLife);
    }

    public void DisableDuelBoard()
    {
        currentNumberOfPlayers = 0;
        currentStartingLife = 0;

        duelBoard.gameObject.SetActive(false);
    }

    public int CurrentNumberOfPlayers
    {
        get { return currentNumberOfPlayers; }
    }

    public int CurrentStartingLife
    {
        get { return currentStartingLife; }
    }
}
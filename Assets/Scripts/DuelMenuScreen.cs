using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DuelMenuScreen : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject[] playerOptions;
    [SerializeField] Slider playersSlider;
    [SerializeField] Slider startingLifeSlider;
    [SerializeField] TMP_Dropdown[] playerDropdowns;

    string[] playerNames;
    int numberOfPlayers;
    int startingLife;

    void Awake()
    {
        playerNames = new string[AppManager.MaxSimultaneousPlayers];
    }

    void OnEnable()
    {
        startingLife = AppManager.Instance.DefaultStartingLife;
        numberOfPlayers = AppManager.Instance.DefaultNumberOfPlayers;

        startingLifeSlider.value = startingLife / AppManager.StartingLifeMultiplier;
        playersSlider.value = numberOfPlayers;

        scrollRect.verticalNormalizedPosition = 1f;

        int i = 0;
        
        for (i = numberOfPlayers; i < AppManager.MaxSimultaneousPlayers; i++)
            playerOptions[i].SetActive(false);
        
        for (i = numberOfPlayers - 1; i >= 0; i--)
            playerOptions[i].SetActive(true);

        i = 0;

        foreach (TMP_Dropdown dropdown in playerDropdowns)
        {
            int j = 0;
            string name = AppManager.Instance.GetPlayerName(i);

            dropdown.value = i;
            dropdown.captionText.text = name;
            playerNames[i] = name;

            foreach (TMP_Dropdown.OptionData option in dropdown.options)
            {
                dropdown.options[j].text = AppManager.Instance.GetPlayerName(j);
                j++;
            }
            i++;
        }
    }


    public void SetStartingLife(float life)
    {
        startingLife = (int)life * AppManager.StartingLifeMultiplier;
    }

    public void SetNumberOfPlayers(float players)
    {
        if (players > numberOfPlayers)
        {
            for (int i = numberOfPlayers; i < players; i++)
                playerOptions[i].SetActive(true);
        }
        else
        {
            for (int i = numberOfPlayers - 1; i >= players; i--)
                playerOptions[i].SetActive(false);
        }

        numberOfPlayers = (int)players;
    }

    public void SetPlayerName(int playerIndex)
    {
        playerNames[playerIndex] = playerDropdowns[playerIndex].captionText.text;
    }

    public void StartDuel()
    {
        MainMenu.Instance.DisableMainMenu();
        DuelManager.Instance.EnableDuelBoard(numberOfPlayers, startingLife, playerNames);
    }
}
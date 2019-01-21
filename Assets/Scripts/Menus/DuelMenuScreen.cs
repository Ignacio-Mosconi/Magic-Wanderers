using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct ToggleValue
{
    public Toggle toggle;
    public int value;
}

public class DuelMenuScreen : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject[] playerOptions;
    [SerializeField] ToggleValue[] startingLifeToggleValues;
    [SerializeField] ToggleValue[] numberOfPlayersToggleValues;
    [SerializeField] TMP_Dropdown[] playerDropdowns;

    string[] playerNames;
    int numberOfPlayers;
    int startingLife;
    bool isSettingUpValues = true;

    void Awake()
    {
        playerNames = new string[AppManager.MaxSimultaneousPlayers];
    }

    void OnEnable()
    {
        startingLife = AppManager.Instance.DefaultStartingLife;
        numberOfPlayers = AppManager.Instance.DefaultNumberOfPlayers;

        foreach (ToggleValue tv in startingLifeToggleValues)
        {
            tv.toggle.isOn = (tv.value == startingLife);
            tv.toggle.onValueChanged.AddListener(SetStartingLife);
            tv.toggle.GetComponentInChildren<TextMeshProUGUI>().text = tv.value.ToString();
        }
        
        foreach (ToggleValue tv in numberOfPlayersToggleValues)
        {
            tv.toggle.isOn = (tv.value == numberOfPlayers);
            tv.toggle.onValueChanged.AddListener(SetNumberOfPlayers);
            tv.toggle.GetComponentInChildren<TextMeshProUGUI>().text = tv.value.ToString();
        }

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

        scrollRect.verticalNormalizedPosition = 1f;

        isSettingUpValues = false;
    }

    void SetStartingLife(bool wasToggledOn)
    {
        if (!isSettingUpValues && wasToggledOn)
        {
            ToggleValue toggleValue = Array.Find(startingLifeToggleValues, tv => tv.toggle.isOn);
            startingLife = toggleValue.value;

            AudioManager.Instance.PlaySound("Menu Select");
        }
    }

    public void SetNumberOfPlayers(bool wasToggledOn)
    {
        if (!isSettingUpValues && wasToggledOn)
        {
            ToggleValue toggleValue = Array.Find(numberOfPlayersToggleValues, tv => tv.toggle.isOn);
            int playersSet = toggleValue.value;


            if (playersSet > numberOfPlayers)
            {
                for (int i = numberOfPlayers; i < playersSet; i++)
                    playerOptions[i].SetActive(true);
            }
            else
            {
                for (int i = numberOfPlayers - 1; i >= playersSet; i--)
                    playerOptions[i].SetActive(false);
            }
            
            numberOfPlayers = playersSet;

            AudioManager.Instance.PlaySound("Menu Select");
        }
    }

    public void SetPlayerName(int playerIndex)
    {
        if (!isSettingUpValues)
        {
            playerNames[playerIndex] = playerDropdowns[playerIndex].captionText.text;
            
            AudioManager.Instance.PlaySound("Menu Close");
        }
    }

    public void StartDuel()
    {
        MainMenu.Instance.DisableMainMenu();
        DuelManager.Instance.EnableDuelBoard(numberOfPlayers, startingLife, playerNames);
    }
}
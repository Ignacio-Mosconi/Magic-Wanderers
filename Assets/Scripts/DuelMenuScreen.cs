﻿using UnityEngine;
using UnityEngine.UI;

public class DuelMenuScreen : MonoBehaviour
{
    [SerializeField] Slider playersSlider;
    [SerializeField] Slider startingLifeSlider;

    const int StartingLifeMultiplier = 10;

    int numberOfPlayers;
    int startingLife;

    void Awake()
    {
        numberOfPlayers = AppManager.Instance.NumberOfPlayers;
        startingLife = AppManager.Instance.StartingLife;

        playersSlider.value = numberOfPlayers;
        startingLifeSlider.value = startingLife / StartingLifeMultiplier;
    }

    public void SetNumberOfPlayers(float players)
    {
        numberOfPlayers = (int)players;
    }

    public void SetStartingLife(float life)
    {
        startingLife = (int)life * StartingLifeMultiplier;
    }

    public void StartDuel()
    {
        MainMenu.Instance.DisableMainMenu();
        DuelManager.Instance.EnableDuelBoard(numberOfPlayers, startingLife);
    }
}
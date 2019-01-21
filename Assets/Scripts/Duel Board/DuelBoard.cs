using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DuelBoard : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] GameObject[] layouts;
    [SerializeField] GameObject duelMenu;
    [SerializeField] Button duelMenuMainButton;

	[Header("Duel Menu Anchors")]
	[SerializeField] Vector2 duelMenuMiddleMinAnchors = new Vector2(0f, 0.4f);
	[SerializeField] Vector2 duelMenuMiddleMaxAnchors = new Vector2(1f, 0.6f);
	[SerializeField] Vector2 duelMenuThreePlayersMinAnchors = new Vector2(0f, 0.3f);
	[SerializeField] Vector2 duelMenuThreePlayersMaxAnchors = new Vector2(1f, 0.5f);
	[SerializeField] Vector2 duelMenuButtonMiddleMinAnchors = new Vector2(0.4f, 0.45f);
	[SerializeField] Vector2 duelMenuButtonMiddleMaxAnchors = new Vector2(0.6f, 0.55f);
	[SerializeField] Vector2 duelMenuButtonThreePlayersMinAnchors = new Vector2(0.4f, 0.35f);
	[SerializeField] Vector2 duelMenuButtonThreePlayersMaxAnchors = new Vector2(0.6f, 0.45f);


	const float PostDiceThrowDelay = 0.1f;
	
	LifeCounter[] playersLifecounters;
	ExtrasCounter[] playersExtrasCounters;
	BackgroundChanger[] playersBackgroundChangers;
	DiceThrower[] playersDiceThrowers;
	UIHandler[] playersUIHandlers;
	Button[] duelMenuButtons;
	Animator duelMenuButtonsAnimator;
	Animator duelMenuMainButtonAnimator;

	void Awake()
	{
		duelMenuButtonsAnimator = duelMenu.GetComponent<Animator>();
		duelMenuMainButtonAnimator = duelMenuMainButton.GetComponent<Animator>();

		int i = 0;
		duelMenuButtons = new Button[duelMenu.transform.childCount];
		
		foreach (Transform element in duelMenu.transform)
		{
			duelMenuButtons[i] = element.GetComponent<Button>();
			i++;
		}
	}

    void SetUpDice()
    {
		foreach (UIHandler uiHandler in playersUIHandlers)
		{
			uiHandler.HideEverything();
            uiHandler.ShowDiePanel();
		}
		duelMenu.SetActive(false);
		duelMenuMainButtonAnimator.SetTrigger("Hide Menu");
		duelMenuMainButton.interactable = false;
    }

	void DeactivateMenu()
	{
		duelMenu.SetActive(false);
        foreach (UIHandler handler in playersUIHandlers)
            handler.ShowMainButtons();
		duelMenuMainButton.interactable = true;
	}

	void PositionDuelMenuElement(RectTransform rectTransform, Vector2 minAnchors, Vector2 maxAnchors)
	{
       rectTransform.anchorMin = minAnchors;
       rectTransform.anchorMax = maxAnchors;
       rectTransform.anchoredPosition = Vector2.zero;
	}

	IEnumerator PerformDiceRoll(float rollDuration)
	{
        int highestRoll = 0;
        int highestRollIndex = 0;
        int[] results = new int[DuelManager.Instance.CurrentNumberOfPlayers];
        int i = 0;

        AudioManager.Instance.PlaySound("Dice Roll");

        foreach (DiceThrower diceThrower in playersDiceThrowers)
            diceThrower.RollDie(rollDuration);
        
		yield return new WaitForSeconds(rollDuration + PostDiceThrowDelay);
		
		foreach (DiceThrower diceThrower in playersDiceThrowers)
        {
			results[i] = diceThrower.FetchLastRollResult();
            
			while (results[i] == highestRoll)
                results[i] = diceThrower.QuickRoll();
            
			if (results[i] > highestRoll)
            {
                highestRoll = results[i];
                highestRollIndex = i;
            }
            i++;
        }

        i = 0;

        AudioManager.Instance.PlaySound("Bell Ring");

        foreach (DiceThrower diceThrower in playersDiceThrowers)
        {
            diceThrower.ChangeResultText(i == highestRollIndex);
			playersUIHandlers[i].EnableDieResultText();
            i++;
        }

		float waitTime = playersDiceThrowers[0].ResultScreenDuration + playersUIHandlers[0].GetDiceTextAnimationDuration() - PostDiceThrowDelay;

        yield return new WaitForSeconds(waitTime);
		
		duelMenuMainButton.interactable = true;

		foreach (UIHandler handler in playersUIHandlers)
		{
			handler.ShowMainButtons();
			handler.ShowLife();
		}
	}

    public void SetUpBoard(int players, int startingLife, string[] playerNames)
    {
        RectTransform menuTransform = duelMenu.GetComponent<RectTransform>();
        RectTransform menuButtonTransform = duelMenuMainButton.GetComponent<RectTransform>();

        foreach (GameObject layout in layouts)
            layout.SetActive(false);

        duelMenuMainButton.gameObject.SetActive(true);

        switch (players)
        {
            case 2:
				PositionDuelMenuElement(menuTransform, duelMenuMiddleMinAnchors, duelMenuMiddleMaxAnchors);
				PositionDuelMenuElement(menuButtonTransform, duelMenuButtonMiddleMinAnchors, duelMenuButtonMiddleMaxAnchors);
                layouts[0].SetActive(true);
                break;
            case 3:
                PositionDuelMenuElement(menuTransform, duelMenuThreePlayersMinAnchors, duelMenuThreePlayersMaxAnchors);
                PositionDuelMenuElement(menuButtonTransform, duelMenuButtonThreePlayersMinAnchors, duelMenuButtonThreePlayersMaxAnchors);
				layouts[1].SetActive(true);
                break;
            case 4:
                PositionDuelMenuElement(menuTransform, duelMenuMiddleMinAnchors, duelMenuMiddleMaxAnchors);
                PositionDuelMenuElement(menuButtonTransform, duelMenuButtonMiddleMinAnchors, duelMenuButtonMiddleMaxAnchors);
                layouts[2].SetActive(true);
                break;
            default:
                Debug.LogError("Warning: the number of players might be wrong.");
                break;
        }

        playersLifecounters = new LifeCounter[players];
        playersExtrasCounters = new ExtrasCounter[players];
        playersBackgroundChangers = new BackgroundChanger[players];
        playersDiceThrowers = new DiceThrower[players];
        playersUIHandlers = new UIHandler[players];

        playersLifecounters = GetComponentsInChildren<LifeCounter>();
        playersExtrasCounters = GetComponentsInChildren<ExtrasCounter>();
        playersBackgroundChangers = GetComponentsInChildren<BackgroundChanger>();
        playersDiceThrowers = GetComponentsInChildren<DiceThrower>();
        playersUIHandlers = GetComponentsInChildren<UIHandler>();

		foreach (LifeCounter lifeCounter in playersLifecounters)
			lifeCounter.Life = startingLife;
		foreach (ExtrasCounter extrasCounter in playersExtrasCounters)
			extrasCounter.SetMaxPoison(players, startingLife);
        
		int i = 0;
        
		foreach (UIHandler handler in playersUIHandlers)
        {
            handler.ChangePlayerName(playerNames[i]);
            i++;
        }

        i = 0;

        foreach (BackgroundChanger bgChanger in playersBackgroundChangers)
        {
            bgChanger.ChangeBackgroundColor((int)AppManager.Instance.GetPlayerSymbol(playerNames[i]));
            i++;
        }
    }

	public void ToggleMenu()
	{
		if (!duelMenu.activeInHierarchy)
		{
			duelMenu.SetActive(true);
			foreach (Button button in duelMenuButtons)
				button.interactable = true;
			duelMenuMainButtonAnimator.SetTrigger("Show Menu");
            AudioManager.Instance.PlaySound("Menu Pop Up");
            foreach (UIHandler handler in playersUIHandlers)
                handler.HideButtons();
		}
		else
		{
			duelMenuMainButton.interactable = false;
            foreach (Button button in duelMenuButtons)
                button.interactable = false;
			duelMenuButtonsAnimator.SetTrigger("Hide");
			duelMenuMainButtonAnimator.SetTrigger("Hide Menu");
            AudioManager.Instance.PlaySound("Menu Close");
            Invoke("DeactivateMenu", duelMenuButtonsAnimator.GetCurrentAnimatorStateInfo(0).length);
		}
	}

	public void ResetDuel()
	{
		int i = 0;
        
		AudioManager.Instance.PlaySound("Menu Close");
		Array.Find(duelMenuButtons, button => button.gameObject.name == "Reset Button").interactable = false;
		foreach (LifeCounter lifecounter in playersLifecounters)
		{
			playersUIHandlers[i].PlayResetLifeCounterAnimation();
			playersLifecounters[i].Invoke("ResetLife", playersUIHandlers[i].GetLifeTextAnimatorResetTime());
			playersExtrasCounters[i].ResetAllCounters();
			i++;
		}
    }

	public void RollDice()
	{
		SetUpDice();
		StartCoroutine(PerformDiceRoll(playersUIHandlers[0].GetDiceRollAnimationDuration()));
	}

	public void LeaveDuel()
	{
		ToggleMenu();
		duelMenuMainButton.gameObject.SetActive(false);
		
		MainMenu.Instance.EnableMainMenu();
		DuelManager.Instance.DisableDuelBoard();
	}
}
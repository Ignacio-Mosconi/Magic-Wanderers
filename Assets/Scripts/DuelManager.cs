using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
	[SerializeField] GameObject[] layouts;
	[SerializeField] GameObject duelMenu;
	[SerializeField] Button duelMenuMainButton;

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
		if (Instance != this)
		{
			Debug.LogError("Warning: more than one Duel Manager in the scene", gameObject);
			return;
		}

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

	void OnEnable()
    {
        foreach (GameObject layout in layouts)
            layout.SetActive(false);

        switch (AppManager.Instance.NumberOfPlayers)
        {
            case 2:
                layouts[0].SetActive(true);
                break;
            case 3:
                layouts[1].SetActive(true);
                break;
            case 4:
                layouts[2].SetActive(true);
                break;
            default:
                Debug.LogError("Warning: the number of players might be wrong.");
                break;
        }

        playersLifecounters = new LifeCounter[AppManager.Instance.NumberOfPlayers];
        playersExtrasCounters = new ExtrasCounter[AppManager.Instance.NumberOfPlayers];
        playersBackgroundChangers = new BackgroundChanger[AppManager.Instance.NumberOfPlayers];
        playersDiceThrowers = new DiceThrower[AppManager.Instance.NumberOfPlayers];
        playersUIHandlers = new UIHandler[AppManager.Instance.NumberOfPlayers];

        playersLifecounters = GetComponentsInChildren<LifeCounter>();
        playersExtrasCounters = GetComponentsInChildren<ExtrasCounter>();
        playersBackgroundChangers = GetComponentsInChildren<BackgroundChanger>();
        playersDiceThrowers = GetComponentsInChildren<DiceThrower>();
        playersUIHandlers = GetComponentsInChildren<UIHandler>();
	}

	void Start()
	{
		int i = 0;
		foreach (BackgroundChanger bgChanger in playersBackgroundChangers)
		{
        	bgChanger.ChangeBackgroundColor((int)AppManager.Instance.DefaultSymbols[i]);
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

	IEnumerator PerformDiceRoll(float rollDuration)
	{
        int highestRoll = 0;
        int highestRollIndex = 0;
        int[] results = new int[AppManager.Instance.NumberOfPlayers];
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

	public void SaveBackgroundPreference(BackgroundChanger backgroundChanger, Symbol symbol)
	{
		int playerIndex = Array.FindIndex(playersBackgroundChangers, bgChanger => bgChanger == backgroundChanger);
		
		AppManager.Instance.SetDefaultSymbol(symbol, playerIndex);
	}
}
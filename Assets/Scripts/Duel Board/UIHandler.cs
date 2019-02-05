using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
	[SerializeField] GameObject life;
	[SerializeField] GameObject symbolsPanel;
	[SerializeField] GameObject countersPanel;
	[SerializeField] GameObject diePanel;
	[SerializeField] GameObject closeButton;
	[SerializeField] GameObject mainButtons;
	[SerializeField] TextMeshProUGUI playerNameText;
	[SerializeField] Button[] lifeChangeButtons;
	[SerializeField] Animator lifeTextAnimator;
	[SerializeField] Animator symbolSelectorAnimator;
	[SerializeField] Animator countersPanelAnimator;
	[SerializeField] Animator dieImageAnimator;
	[SerializeField] Animator dieTextAnimator;
	[SerializeField] AnimationClip dieTextSlideOutAnimation;

    void DeactivateSymbolSelector()
    {
        symbolsPanel.SetActive(false);
    }

    void DeactivateCountersPanel()
    {
        countersPanel.SetActive(false);
    }

    void DeactivateDiePanel()
    {
		dieTextAnimator.gameObject.SetActive(false);
        diePanel.SetActive(false);
    }

	void HideDiePanel()
	{
		dieTextAnimator.SetTrigger("Hide");
		Invoke("DeactivateDiePanel", dieTextAnimator.GetCurrentAnimatorStateInfo(0).length);
	}

	public void HideButtons()
	{
		symbolsPanel.SetActive(false);
		countersPanel.SetActive(false);
		closeButton.SetActive(false);
		mainButtons.SetActive(false);
		foreach (Button button in lifeChangeButtons)
			button.gameObject.SetActive(false);
	}

	public void HideEverything()
	{
        symbolsPanel.SetActive(false);
        countersPanel.SetActive(false);
        closeButton.SetActive(false);
        mainButtons.SetActive(false);
		life.SetActive(false);
	}

	public void ShowMainButtons()
	{
		mainButtons.SetActive(true);
        foreach (Button button in lifeChangeButtons)
            button.gameObject.SetActive(true);
	}

	public void ShowLife()
	{
		life.SetActive(true);
	}

	public void ChangePlayerName(string name)
	{
		playerNameText.text = name;
	}

	public void PlayResetLifeCounterAnimation()
	{
        lifeTextAnimator.SetTrigger("Reset");
	}

    public void PopOutSymbolSelector()
    {
		if (symbolSelectorAnimator.isActiveAndEnabled)
		{
			symbolSelectorAnimator.SetTrigger("Hide");
			Invoke("DeactivateSymbolSelector", symbolSelectorAnimator.GetCurrentAnimatorStateInfo(0).length);
		}
    }

    public void PopOutCountersPanel()
    {
		if (countersPanelAnimator.isActiveAndEnabled)
		{
			countersPanelAnimator.SetTrigger("Hide");
			Invoke("DeactivateCountersPanel", countersPanelAnimator.GetCurrentAnimatorStateInfo(0).length);
		}
    }

    public void ShowDiePanel()
    {
		DiceThrower diceThrower = GetComponent<DiceThrower>();    
		diePanel.SetActive(true);
		
		float showTime = dieImageAnimator.GetCurrentAnimatorStateInfo(0).length + dieTextSlideOutAnimation.length + 
						diceThrower.ResultScreenDuration;
		Invoke("HideDiePanel", showTime);
    }

	public void EnableDieResultText()
	{
		dieTextAnimator.gameObject.SetActive(true);
	}

	public void PlayUISound(string soundName)
	{
		AudioManager.Instance.PlaySound(soundName);
	}

	public float GetLifeTextAnimatorResetTime()
	{
		return (lifeTextAnimator.GetCurrentAnimatorStateInfo(0).length / 2f);
	}

	public float GetDiceRollAnimationDuration()
	{
		return (dieImageAnimator.GetCurrentAnimatorStateInfo(0).length);
	}

	public float GetDiceTextAnimationDuration()
	{
		return (dieImageAnimator.GetCurrentAnimatorStateInfo(0).length);
	}
}
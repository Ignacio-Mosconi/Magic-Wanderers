using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum CounterType
{
    Poison,
    Loyalty,
    Energy,
    Count
}

public class ExtrasCounter : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI[] countersTexts;
	[SerializeField] Button[] increaseButtons;
	[SerializeField] Button[] decreaseButtons;

	const int MaxPoison = 10;
	const int MaxPoison2v2 = 15;
	const int MinPoison = 0;
	const int CriticalPoison = 7;
	const int CriticalPoison2v2 = 10;
	const int MaxLoyalty = 99;
	const int MinLoyalty = 0;
	const int CriticalLoyalty = 3;
	const int MaxEnergy = 99;
	const int MinEnergy = 0;
    const float HoldValueChangeInterval = 0.15f;

	Dictionary<CounterType, int> countersDictionary = new Dictionary<CounterType, int>();
    Coroutine[] increaseRoutines;
    Coroutine[] decreaseRoutines;
    int maxPoison;
    int criticalPoison;

	void Awake()
	{
        increaseRoutines = new Coroutine[(int)CounterType.Count];
        decreaseRoutines = new Coroutine[(int)CounterType.Count];

		for (int i = 0; i < (int)CounterType.Count; i++)
			countersDictionary.Add((CounterType)i, 0);
	}

    IEnumerator IncreaseCounterGradually(CounterType counterType)
    {
        switch (counterType)
        {
            case CounterType.Poison:
                while (increaseButtons[(int)CounterType.Poison].gameObject.activeInHierarchy)
                {
                    if (decreaseRoutines[(int)CounterType.Poison] == null)
                        IncreasePoisonCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                }
                break;
            case CounterType.Loyalty:
                while (increaseButtons[(int)CounterType.Loyalty].gameObject.activeInHierarchy)
                {
                    if (decreaseRoutines[(int)CounterType.Loyalty] == null)
                        IncreaseLoyaltyCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                }
                break;
            case CounterType.Energy:
                while (increaseButtons[(int)CounterType.Energy].gameObject.activeInHierarchy)
                {
                    if (decreaseRoutines[(int)CounterType.Energy] == null)
                        IncreaseEnergyCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                }
                break;
        }
    }

    IEnumerator DecreaseCounterGradually(CounterType counterType)
    {
        switch (counterType)
        {
            case CounterType.Poison:
                while (decreaseButtons[(int)CounterType.Poison].gameObject.activeInHierarchy)
                {
                    if (increaseRoutines[(int)CounterType.Poison] == null)
                        DecreasePoisonCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                }
                break;
            case CounterType.Loyalty:
                while (decreaseButtons[(int)CounterType.Loyalty].gameObject.activeInHierarchy)
                {
                    if (increaseRoutines[(int)CounterType.Loyalty] == null)
                        DecreaseLoyaltyCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                }
                break;
            case CounterType.Energy:
                while (decreaseButtons[(int)CounterType.Energy].gameObject.activeInHierarchy)
                {
                    if (increaseRoutines[(int)CounterType.Energy] == null)
                        DecreaseEnergyCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                }
                break;
        }
    }

	void IncreasePoisonCounter()
	{
		countersDictionary[CounterType.Poison]++;
		countersTexts[(int)CounterType.Poison].text = countersDictionary[CounterType.Poison].ToString();
        if (countersTexts[(int)CounterType.Poison].color != Color.red && countersDictionary[CounterType.Poison] >= criticalPoison)
            countersTexts[(int)CounterType.Poison].color = Color.red;
		if (countersDictionary[CounterType.Poison] == maxPoison)
        {
			increaseButtons[(int)CounterType.Poison].gameObject.SetActive(false);
            increaseRoutines[(int)CounterType.Poison] = null;
        }
		if (!decreaseButtons[(int)CounterType.Poison].isActiveAndEnabled)
			decreaseButtons[(int)CounterType.Poison].gameObject.SetActive(true);
	}

    void DecreasePoisonCounter()
    {
        countersDictionary[CounterType.Poison]--;
        countersTexts[(int)CounterType.Poison].text = countersDictionary[CounterType.Poison].ToString();
        if (countersTexts[(int)CounterType.Poison].color != Color.white && countersDictionary[CounterType.Poison] < criticalPoison)
            countersTexts[(int)CounterType.Poison].color = Color.white;
        if (countersDictionary[CounterType.Poison] == MinPoison)
        {
            decreaseButtons[(int)CounterType.Poison].gameObject.SetActive(false);
            decreaseRoutines[(int)CounterType.Poison] = null;
        }
        if (!increaseButtons[(int)CounterType.Poison].isActiveAndEnabled)
            increaseButtons[(int)CounterType.Poison].gameObject.SetActive(true);
    }

    void IncreaseLoyaltyCounter()
    {
        countersDictionary[CounterType.Loyalty]++;
        countersTexts[(int)CounterType.Loyalty].text = countersDictionary[CounterType.Loyalty].ToString();
        if (countersTexts[(int)CounterType.Loyalty].color != Color.white && countersDictionary[CounterType.Loyalty] > CriticalLoyalty)
            countersTexts[(int)CounterType.Loyalty].color = Color.white;
        if (countersDictionary[CounterType.Loyalty] == MaxLoyalty)
        {
            increaseButtons[(int)CounterType.Loyalty].gameObject.SetActive(false);
            increaseRoutines[(int)CounterType.Loyalty] = null;
        }
        if (!decreaseButtons[(int)CounterType.Loyalty].isActiveAndEnabled)
            decreaseButtons[(int)CounterType.Loyalty].gameObject.SetActive(true);
    }

    void DecreaseLoyaltyCounter()
    {
        countersDictionary[CounterType.Loyalty]--;
        countersTexts[(int)CounterType.Loyalty].text = countersDictionary[CounterType.Loyalty].ToString();
        if (countersTexts[(int)CounterType.Loyalty].color != Color.red && countersDictionary[CounterType.Loyalty] <= CriticalLoyalty)
            countersTexts[(int)CounterType.Loyalty].color = Color.red;
        if (countersDictionary[CounterType.Loyalty] == MinLoyalty)
        {
            decreaseButtons[(int)CounterType.Loyalty].gameObject.SetActive(false);
            decreaseRoutines[(int)CounterType.Loyalty] = null;
        }
        if (!increaseButtons[(int)CounterType.Loyalty].isActiveAndEnabled)
            increaseButtons[(int)CounterType.Loyalty].gameObject.SetActive(true);
    }

    void IncreaseEnergyCounter()
    {
        countersDictionary[CounterType.Energy]++;
        countersTexts[(int)CounterType.Energy].text = countersDictionary[CounterType.Energy].ToString();
        if (countersDictionary[CounterType.Energy] == MaxEnergy)
        {
            increaseButtons[(int)CounterType.Energy].gameObject.SetActive(false);
            increaseRoutines[(int)CounterType.Energy] = null;
        }
        if (!decreaseButtons[(int)CounterType.Energy].isActiveAndEnabled)
            decreaseButtons[(int)CounterType.Energy].gameObject.SetActive(true);
    }

    void DecreaseEnergyCounter()
    {
        countersDictionary[CounterType.Energy]--;
        countersTexts[(int)CounterType.Energy].text = countersDictionary[CounterType.Energy].ToString();
        if (countersDictionary[CounterType.Energy] == MinEnergy)
        {
            decreaseButtons[(int)CounterType.Energy].gameObject.SetActive(false);
            decreaseRoutines[(int)CounterType.Energy] = null;
        }
        if (!increaseButtons[(int)CounterType.Energy].isActiveAndEnabled)
            increaseButtons[(int)CounterType.Energy].gameObject.SetActive(true);
    }

	public void ResetAllCounters()
	{
		for (int i = 0; i < countersDictionary.Keys.Count; i++)
		{
			countersDictionary[(CounterType)i] = 0;
			countersTexts[i].text = countersDictionary[(CounterType)i].ToString();
			countersTexts[i].color = Color.white;
			if (!increaseButtons[i].gameObject.activeSelf)
				increaseButtons[i].gameObject.SetActive(true);		
			if (decreaseButtons[i].gameObject.activeSelf)
				decreaseButtons[i].gameObject.SetActive(false);		
		}
	}

    public void IncreaseCounterOnHold(int counterTypeIndex)
    {
        switch ((CounterType)counterTypeIndex)
        {
            case CounterType.Poison:
            case CounterType.Loyalty:
            case CounterType.Energy:
                increaseRoutines[counterTypeIndex] = StartCoroutine(IncreaseCounterGradually((CounterType)counterTypeIndex));
                break;
            default:
                Debug.LogError("Warning: the index doesn't correspond to any counter type.");
                break;
        }
    }

    public void IncreaseCounterOnRelease(int counterTypeIndex)
    {
        if (increaseRoutines[counterTypeIndex] != null)
        {
            StopCoroutine(increaseRoutines[counterTypeIndex]);
            increaseRoutines[counterTypeIndex] = null;
        }
    }

    public void DecreaseCounterOnHold(int counterTypeIndex)
    {
        switch ((CounterType)counterTypeIndex)
        {
            case CounterType.Poison:
            case CounterType.Loyalty:
            case CounterType.Energy:
                decreaseRoutines[counterTypeIndex] = StartCoroutine(DecreaseCounterGradually((CounterType)counterTypeIndex));
                break;
            default:
                Debug.LogError("Warning: the index doesn't correspond to any counter type.");
                break;
        }
    }

    public void DecreaseCounterOnRelease(int counterTypeIndex)
    {
        if (decreaseRoutines[counterTypeIndex] != null)
        {
            StopCoroutine(decreaseRoutines[counterTypeIndex]);
            decreaseRoutines[counterTypeIndex] = null;
        }
    }

    public void SetMaxPoison(int players, int startingLife)
    {
        if (players == 2 && startingLife == 30)
        {
            maxPoison = MaxPoison2v2;
            criticalPoison = CriticalPoison2v2;
        }
        else
        {
            maxPoison = MaxPoison;
            criticalPoison = CriticalPoison;
        }
    }
}
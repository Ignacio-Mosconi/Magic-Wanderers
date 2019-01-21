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

    void OnEnable()
    {
        ResetAllCounters();
    }

    IEnumerator IncreaseCounterGradually(CounterType counterType)
    {
        switch (counterType)
        {
            case CounterType.Poison:
                do
                {
                    if (decreaseRoutines[(int)CounterType.Poison] == null)
                        IncreasePoisonCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                } while (increaseRoutines[(int)CounterType.Poison] != null && countersDictionary[CounterType.Poison] < maxPoison);
                break;
            
            case CounterType.Loyalty:
                do
                {
                    if (decreaseRoutines[(int)CounterType.Loyalty] == null)
                        IncreaseLoyaltyCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                } while (increaseRoutines[(int)CounterType.Loyalty] != null && countersDictionary[CounterType.Loyalty] < MaxLoyalty);
                break;
            
            case CounterType.Energy:
                do
                {
                    if (decreaseRoutines[(int)CounterType.Energy] == null)
                        IncreaseEnergyCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                } while (increaseRoutines[(int)CounterType.Energy] != null && countersDictionary[CounterType.Energy] < MaxEnergy);
                break;
        }
    }

    IEnumerator DecreaseCounterGradually(CounterType counterType)
    {
        switch (counterType)
        {
            case CounterType.Poison:
                do
                {
                    if (increaseRoutines[(int)CounterType.Poison] == null)
                        DecreasePoisonCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                } while (decreaseRoutines[(int)CounterType.Poison] != null && countersDictionary[CounterType.Poison] > MinPoison);
                break;
            
            case CounterType.Loyalty:
                do
                {
                    if (increaseRoutines[(int)CounterType.Loyalty] == null)
                        DecreaseLoyaltyCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                } while (decreaseRoutines[(int)CounterType.Loyalty] != null && countersDictionary[CounterType.Loyalty] > MinLoyalty);
                break;
            
            case CounterType.Energy:
                do
                {
                    if (increaseRoutines[(int)CounterType.Energy] == null)
                        DecreaseEnergyCounter();
                    yield return new WaitForSeconds(HoldValueChangeInterval);
                } while (decreaseRoutines[(int)CounterType.Energy] != null && countersDictionary[CounterType.Energy] > MinEnergy);
                break;
        }
    }

	void IncreasePoisonCounter()
	{
		countersDictionary[CounterType.Poison]++;
		countersTexts[(int)CounterType.Poison].text = countersDictionary[CounterType.Poison].ToString();
        if (countersTexts[(int)CounterType.Poison].color != Color.red && countersDictionary[CounterType.Poison] >= criticalPoison)
            countersTexts[(int)CounterType.Poison].color = Color.red;
	}

    void DecreasePoisonCounter()
    {
        countersDictionary[CounterType.Poison]--;
        countersTexts[(int)CounterType.Poison].text = countersDictionary[CounterType.Poison].ToString();
        if (countersTexts[(int)CounterType.Poison].color != Color.white && countersDictionary[CounterType.Poison] < criticalPoison)
            countersTexts[(int)CounterType.Poison].color = Color.white;
    }

    void IncreaseLoyaltyCounter()
    {
        countersDictionary[CounterType.Loyalty]++;
        countersTexts[(int)CounterType.Loyalty].text = countersDictionary[CounterType.Loyalty].ToString();
        if (countersTexts[(int)CounterType.Loyalty].color != Color.white && countersDictionary[CounterType.Loyalty] > CriticalLoyalty)
            countersTexts[(int)CounterType.Loyalty].color = Color.white;
    }

    void DecreaseLoyaltyCounter()
    {
        countersDictionary[CounterType.Loyalty]--;
        countersTexts[(int)CounterType.Loyalty].text = countersDictionary[CounterType.Loyalty].ToString();
        if (countersTexts[(int)CounterType.Loyalty].color != Color.red && countersDictionary[CounterType.Loyalty] <= CriticalLoyalty)
            countersTexts[(int)CounterType.Loyalty].color = Color.red;
    }

    void IncreaseEnergyCounter()
    {
        countersDictionary[CounterType.Energy]++;
        countersTexts[(int)CounterType.Energy].text = countersDictionary[CounterType.Energy].ToString();
    }

    void DecreaseEnergyCounter()
    {
        countersDictionary[CounterType.Energy]--;
        countersTexts[(int)CounterType.Energy].text = countersDictionary[CounterType.Energy].ToString();
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

        int maxCounter = 0;

        switch ((CounterType)counterTypeIndex)
        {
            case CounterType.Poison:
                maxCounter = maxPoison;
                break;
            case CounterType.Loyalty:
                maxCounter = MaxLoyalty;
                break;
            case CounterType.Energy:
                maxCounter = MaxEnergy;
                break;
            default:
                maxCounter = 999;
                break;
        }

        if (countersDictionary[(CounterType)counterTypeIndex] == maxCounter)
            increaseButtons[counterTypeIndex].gameObject.SetActive(false);
        if (!decreaseButtons[counterTypeIndex].isActiveAndEnabled)
            decreaseButtons[counterTypeIndex].gameObject.SetActive(true);
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

        int minCounter = 0;

        switch ((CounterType)counterTypeIndex)
        {
            case CounterType.Poison:
                minCounter = MinPoison;
                break;
            case CounterType.Loyalty:
                minCounter = MinLoyalty;
                break;
            case CounterType.Energy:
                minCounter = MinEnergy;
                break;
            default:
                minCounter = 0;
                break;
        }

        if (countersDictionary[(CounterType)counterTypeIndex] == minCounter)
            decreaseButtons[counterTypeIndex].gameObject.SetActive(false);
        if (!increaseButtons[counterTypeIndex].isActiveAndEnabled)
            increaseButtons[counterTypeIndex].gameObject.SetActive(true);
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
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] Button increaseButton;
    [SerializeField] Button decreaseButton;

    const int MaxLife = 999;
    const int MinLife = 0;
    const int WarningLife = 10;
    const int CriticalLife = 5;
    const int DefaultLife = 20;
    const float HoldValueChangeInterval = 0.15f;

    Coroutine increaseLifeRoutine;
    Coroutine decreaseLifeRoutine;
	int life = DefaultLife;

    IEnumerator IncreaseLifeGradually()
    {
        while (increaseButton.gameObject.activeInHierarchy)
        {
            if (decreaseLifeRoutine == null)
                IncreaseLife();
            yield return new WaitForSeconds(HoldValueChangeInterval);
        }
    }

    IEnumerator DecreaseLifeGradually()
    {
        while (decreaseButton.gameObject.activeInHierarchy)
        {
            if (increaseLifeRoutine == null)
                DecreaseLife();
            yield return new WaitForSeconds(HoldValueChangeInterval);
        }
    }

    void IncreaseLife()
    {
        life++;
        lifeText.text = life.ToString();
        if (life > CriticalLife)
        {
            if (lifeText.color != Color.white && life > WarningLife)
                lifeText.color = Color.white;
            else
                if (lifeText.color != Color.yellow && life <= WarningLife)
                    lifeText.color = Color.yellow;
        }
        if (life == MaxLife)
        {
            increaseButton.gameObject.SetActive(false);
            increaseLifeRoutine = null;
        }
        if (!decreaseButton.isActiveAndEnabled)
            decreaseButton.gameObject.SetActive(true);
    }

    void DecreaseLife()
    {
        life--;
        lifeText.text = life.ToString();
        if (life <= WarningLife)
        {
            if (lifeText.color != Color.red && life <= CriticalLife)
                lifeText.color = Color.red;
            else
                if (lifeText.color != Color.yellow && life > CriticalLife)
                    lifeText.color = Color.yellow;
        }
        if (life == MinLife)
        {
            decreaseButton.gameObject.SetActive(false);
            decreaseLifeRoutine = null;
        }
        if (!increaseButton.isActiveAndEnabled)
            increaseButton.gameObject.SetActive(true);
    }

    public void ResetLife()
    {
        life = DefaultLife;
        lifeText.text = life.ToString();
        lifeText.color = Color.white;
    }

    public void IncreaseLifeOnHold()
    {
        increaseLifeRoutine = StartCoroutine(IncreaseLifeGradually());
    }

    public void DecreaseLifeOnHold()
    {
        decreaseLifeRoutine = StartCoroutine(DecreaseLifeGradually());
    }

    public void IncreaseLifeOnRelease()
    {
        if (increaseLifeRoutine != null)
        {
            StopCoroutine(increaseLifeRoutine);
            increaseLifeRoutine = null;
        }
    }

    public void DecreaseLifeOnRelease()
    {
        if (decreaseLifeRoutine != null)
        {
            StopCoroutine(decreaseLifeRoutine);
            decreaseLifeRoutine = null;
        }
    }
}
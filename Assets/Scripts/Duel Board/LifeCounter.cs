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
    const float HoldValueChangeInterval = 0.15f;

    Coroutine increaseLifeRoutine;
    Coroutine decreaseLifeRoutine;
	int life;

    void OnEnable()
    {
        ResetLife();
    }

    IEnumerator IncreaseLifeGradually()
    {
        do
        {
            if (decreaseLifeRoutine == null)
                IncreaseLife();
            yield return new WaitForSeconds(HoldValueChangeInterval);
        } while (increaseLifeRoutine != null && life < MaxLife);
    }

    IEnumerator DecreaseLifeGradually()
    {
        do
        {
            if (increaseLifeRoutine == null)
                DecreaseLife();
            yield return new WaitForSeconds(HoldValueChangeInterval);
        } while (decreaseLifeRoutine != null && life > MinLife);
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
    }

    public void ResetLife()
    {
        life = DuelManager.Instance.CurrentStartingLife;
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

        if (life == MaxLife)
            increaseButton.gameObject.SetActive(false);
        if (!decreaseButton.isActiveAndEnabled)
            decreaseButton.gameObject.SetActive(true);
    }

    public void DecreaseLifeOnRelease()
    {
        if (decreaseLifeRoutine != null)
        {
            StopCoroutine(decreaseLifeRoutine);
            decreaseLifeRoutine = null;
        }

        if (life == MinLife)
            decreaseButton.gameObject.SetActive(false);
        if (!increaseButton.isActiveAndEnabled)
            increaseButton.gameObject.SetActive(true);
    }

    public int Life
    {
        set 
        {
            if (value >= MinLife && value <= MaxLife)
            {
                life = value;
                lifeText.text = life.ToString();
            }
            else
                Debug.LogError("Attempted to set an invalid life value", gameObject);
        }
    }
}
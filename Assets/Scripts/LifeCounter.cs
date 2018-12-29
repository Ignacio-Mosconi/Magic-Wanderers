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

	int life = DefaultLife;

    public void IncreaseLife()
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
            increaseButton.gameObject.SetActive(false);
        if (!decreaseButton.isActiveAndEnabled)
            decreaseButton.gameObject.SetActive(true);
    }

    public void DecreaseLife()
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
            decreaseButton.gameObject.SetActive(false);
        if (!increaseButton.isActiveAndEnabled)
            increaseButton.gameObject.SetActive(true);
    }

    public void ResetLife()
    {
        life = DefaultLife;
        lifeText.text = life.ToString();
        lifeText.color = Color.white;
    }
}
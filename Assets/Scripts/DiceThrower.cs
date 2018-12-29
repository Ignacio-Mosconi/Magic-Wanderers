using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceThrower : MonoBehaviour
{
    [SerializeField] Image dieImage;
    [SerializeField] Sprite[] dieSprites;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] [Range(0.1f, 1f)] float switchInterval;
    [SerializeField] [Range(1f, 5f)] float resultScreenDuration;
    [SerializeField] string winText;
    [SerializeField] string loseText;
    
    int lastNumberRolled = 0;

    IEnumerator ThrowDie(float duration)
    {
        int totalSpins = (int)(duration / switchInterval);

        for (int i = 0; i < totalSpins; i++)
        {
            yield return new WaitForSeconds(switchInterval);
            lastNumberRolled = Random.Range(1, dieSprites.GetLength(0) + 1);
            dieImage.sprite = dieSprites[lastNumberRolled - 1];
        }
    }

    public int QuickRoll()
    {
        int result = Random.Range(1, dieSprites.GetLength(0) + 1);
        dieImage.sprite = dieSprites[result - 1];

        return result;
    }

    public void RollDie(float rollDuration)
    {
        StartCoroutine(ThrowDie(rollDuration));
    }

    public int FetchLastRollResult()
    {
        int result = lastNumberRolled;
        lastNumberRolled = 0;

        return result;
    }

    public void ChangeResultText(bool hasWon)
    {
        if (hasWon)
        {
            resultText.text = winText;
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = loseText;
            resultText.color = Color.red;
        }
    }

    public float ResultScreenDuration
    {
        get { return resultScreenDuration;}
    }
}
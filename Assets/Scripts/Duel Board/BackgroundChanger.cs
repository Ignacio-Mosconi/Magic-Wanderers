using UnityEngine;
using UnityEngine.UI;

public enum Symbol
{
    White,
    Blue,
    Black,
    Red,
    Green,
	Count
}

public class BackgroundChanger : MonoBehaviour
{	
	[SerializeField] Button symbolButton;
	[SerializeField] Sprite[] symbolSprites;
	[SerializeField] Sprite[] backgroundSprites;

	Image backgroundImage;

	void Awake()
	{
		backgroundImage = GetComponent<Image>();
	}

	public void ChangeBackgroundColor(int symbolIndex)
	{
		switch ((Symbol)symbolIndex)
		{
			case Symbol.White:
			case Symbol.Blue:
			case Symbol.Black:
			case Symbol.Red:
			case Symbol.Green:
				symbolButton.image.sprite = symbolSprites[symbolIndex];
				backgroundImage.sprite = backgroundSprites[symbolIndex];
				break;
			default:
				Debug.LogError("The selected symbol doesn't exist.", gameObject);
				break;
		}
	}
}
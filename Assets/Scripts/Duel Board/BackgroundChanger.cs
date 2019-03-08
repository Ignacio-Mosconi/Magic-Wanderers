using UnityEngine;
using UnityEngine.UI;

public enum Symbol
{
    White,
    Blue,
    Black,
    Red,
    Green,
	Boros,
	Dimir,
	Golgari,
	Izzet,
	Selesnya,
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
			case Symbol.Boros:
			case Symbol.Dimir:
			case Symbol.Golgari:
			case Symbol.Izzet:
			case Symbol.Selesnya:
				symbolButton.image.sprite = symbolSprites[symbolIndex];
				backgroundImage.sprite = backgroundSprites[symbolIndex];
				break;
			default:
				Debug.LogError("The selected symbol doesn't exist.", gameObject);
				break;
		}
	}
}
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CustomDropdown : TMP_Dropdown
{
    public Color backgroundBlockerColor;

    protected override GameObject CreateBlocker(Canvas rootCanvas)
    {
        GameObject blocker = base.CreateBlocker(rootCanvas);

        blocker.GetComponent<Image>().color = backgroundBlockerColor;

        return blocker;
    }

    protected override void DestroyBlocker(GameObject blocker)
    {
        interactable = false;

        AudioManager.Instance.PlaySound("Menu Close");
        
        base.DestroyBlocker(blocker);
    }

    public void ShowDropdown()
    {
        interactable = true;

        Show();
        AudioManager.Instance.PlaySound("Menu Pop Up");
    }
}
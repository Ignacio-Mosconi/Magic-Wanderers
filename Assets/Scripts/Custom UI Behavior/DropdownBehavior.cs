using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class DropdownBehavior : MonoBehaviour
{
    [SerializeField] Color backgroundColor;

    void OnEnable()
    {
        StartCoroutine(ChangeBackgroundColor()); 
        AudioManager.Instance.PlaySound("Menu Pop Up");
    }

    IEnumerator ChangeBackgroundColor()
    {
        yield return new WaitForEndOfFrame();

        Image background = GameObject.Find("Blocker").GetComponent<Image>();
        background.color = backgroundColor;
    }
}
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class DropdownSound : MonoBehaviour
{
    void OnEnable()
    {
        AudioManager.Instance.PlaySound("Menu Pop Up");
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MenuScreen
{
    public GameObject screen;
    public int index;
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] MenuScreen[] menuScreens;
    [SerializeField] GameObject mainScreen;
    [SerializeField] Button returnButton;

    MenuScreen currentScreen;
    MenuScreen previousScreen;
    Animator[] currentScreenAnimators;

    void Awake()
    {
        currentScreen = Array.Find(menuScreens, menuScreen => menuScreen.screen == mainScreen);

        if (currentScreen.screen != mainScreen)
        {
            Debug.LogError("There is no 'menu screen' with an index of 'zero'.", gameObject);
            return;
        }

        currentScreenAnimators = currentScreen.screen.GetComponentsInChildren<Animator>();
    }

    void EnablePreviousScreen()
    {
        currentScreen.screen.SetActive(false);
        previousScreen.screen.SetActive(true);

        currentScreen = previousScreen;
        currentScreenAnimators = currentScreen.screen.GetComponentsInChildren<Animator>();

        if (currentScreen.index > 0)
            previousScreen = Array.Find(menuScreens, menuScreen => menuScreen.index == currentScreen.index - 1);
        else
            returnButton.gameObject.SetActive(false);
    }

    void DisablePreviousScreen()
    {
        previousScreen.screen.SetActive(false);

        returnButton.gameObject.SetActive(true);
        returnButton.interactable = true;
    }

    public void ReturnToPreviousScreen()
    {
        float transitionTime = 0f;

        returnButton.interactable = false;

        foreach (Animator animator in currentScreenAnimators)
        {
            animator.SetTrigger("Hide");

            float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

            if (animationDuration > transitionTime)
                transitionTime = animationDuration;
        }

        Invoke("EnablePreviousScreen", transitionTime);
    }

    public void MoveToScreen(GameObject nextScreen)
    {
        float transitionTime = 0f;

        foreach (Animator animator in currentScreenAnimators)
        {
            animator.SetTrigger("Hide");

            float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

            if (animationDuration > transitionTime)
                transitionTime = animationDuration;
        }

        previousScreen = currentScreen;
        currentScreen = Array.Find(menuScreens, menuScreen => menuScreen.screen == nextScreen);

        nextScreen.SetActive(true);

        Debug.Log(currentScreen.screen.name);
        
        currentScreenAnimators = currentScreen.screen.GetComponentsInChildren<Animator>();

        Invoke("DisablePreviousScreen", transitionTime);
    }
}
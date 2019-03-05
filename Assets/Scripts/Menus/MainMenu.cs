using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MenuScreen
{
    public GameObject screen;
    public GameObject previousScreen;
}

public class MainMenu : MonoBehaviour
{
    #region Singleton
    static MainMenu instance;

    public static MainMenu Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<MainMenu>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("Main Menu");
                    instance = gameObj.AddComponent<MainMenu>();
                }
            }

            return instance;
        }
    }
    #endregion
    
    [Header("App Screens")]
    [SerializeField] MenuScreen[] menuScreens;
    [Header("Other Menu References")]
    [SerializeField] GameObject mainScreen;
    [SerializeField] Button returnButton;

    MenuScreen currentScreen;
    MenuScreen previousScreen;
    Animator[] currentScreenAnimators;
    Animator returnButtonAnimator;

    void Awake()
    {
        if (Instance != this)
        {
            Debug.LogError("Warning: more than one Main Menu in the scene.", gameObject);
            return;
        }

        currentScreen = Array.Find(menuScreens, menuScreen => menuScreen.screen == mainScreen);

        if (currentScreen.screen != mainScreen)
        {
            Debug.LogError("There is no 'menu screen' with an index of 'zero'.", gameObject);
            return;
        }

        currentScreenAnimators = currentScreen.screen.GetComponentsInChildren<Animator>();
        returnButtonAnimator = returnButton.GetComponent<Animator>();

    }

    void Start()
    {
        AudioManager.Instance.PlayTheme("Menu Theme");
    }

    void EnablePreviousScreen()
    {
        currentScreen.screen.SetActive(false);
        previousScreen.screen.SetActive(true);

        currentScreen = previousScreen;
        currentScreenAnimators = currentScreen.screen.GetComponentsInChildren<Animator>();

        if (currentScreen.previousScreen)
            previousScreen = Array.Find(menuScreens, menuScreen => menuScreen.screen == currentScreen.previousScreen);
        else
            returnButton.gameObject.SetActive(false);
    }

    void DisablePreviousScreen()
    {
        previousScreen.screen.SetActive(false);
        returnButton.gameObject.SetActive(true);
        returnButton.interactable = true;
    }

    void DisableCurrentScreen()
    {
        currentScreen.screen.SetActive(false);
        returnButton.gameObject.SetActive(false);
        returnButton.interactable = false;
    }

    public void ReturnToPreviousScreen()
    {
        float transitionTime = 0f;

        if (!currentScreen.previousScreen)
        {
            returnButtonAnimator.SetTrigger("Hide");
            returnButton.interactable = false;
            transitionTime = returnButtonAnimator.GetNextAnimatorStateInfo(0).length;
        }

        foreach (Animator animator in currentScreenAnimators)
        {
            animator.SetTrigger("Hide");

            float animationDuration = animator.GetNextAnimatorStateInfo(0).length;

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
        
        currentScreenAnimators = currentScreen.screen.GetComponentsInChildren<Animator>();

        Invoke("DisablePreviousScreen", transitionTime);
    }

    public void DisableMainMenu()
    {
        float transitionTime = 0f;

        returnButtonAnimator.SetTrigger("Hide");
        transitionTime = returnButtonAnimator.GetCurrentAnimatorStateInfo(0).length;

        foreach (Animator animator in currentScreenAnimators)
        {
            animator.SetTrigger("Hide");

            float animationDuration = animator.GetNextAnimatorStateInfo(0).length;

            if (animationDuration > transitionTime)
                transitionTime = animationDuration;
        }
        
        AudioManager.Instance.StopMusicPlayback();
        Invoke("DisableCurrentScreen", transitionTime);
    }

    public void EnableMainMenu()
    {
        currentScreen.screen.SetActive(true);
        if (currentScreen.previousScreen)
        {
            returnButton.gameObject.SetActive(true);
            returnButton.interactable = true;
        }
        AudioManager.Instance.PlayTheme("Menu Theme");
    }

    public void PlayMenuSound(string soundName)
    {
        AudioManager.Instance.PlaySound(soundName);
    }
}
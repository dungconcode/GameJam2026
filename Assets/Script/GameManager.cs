using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private DollManager dollManager;

    [Header("Intro UI")]
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject letterButton;
    [SerializeField] private GameObject buttonsRoot;

    private bool gameStarted;
    private bool openedLetter;
    private void Awake()
    {
        openedLetter = false;
        //gameStarted = false;
        if (blackScreen != null) blackScreen.SetActive(false);
        if (letterButton != null) letterButton.SetActive(true);

        if (buttonsRoot != null) buttonsRoot.SetActive(false);
        if (dollManager != null)
            dollManager.SetUIState(true, true);
    }
    public void OnLetterPressed()
    {
        if (openedLetter) return;
        openedLetter = true;
        //Debug.Log("Letter Pressed");
        if (blackScreen != null) blackScreen.SetActive(true);
        letterButton.SetActive(false);
        //StartGameplay();
    }

    public void StartGameplay()
    {
        //if (gameStarted) return;
        //gameStarted = true;

        if (blackScreen != null) blackScreen.SetActive(false);
        if (letterButton != null) letterButton.SetActive(false);
        if (buttonsRoot != null) buttonsRoot.SetActive(true);

        if (dollManager != null)
            dollManager.BeginGame();
    }
}

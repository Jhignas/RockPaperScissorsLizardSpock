using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [SerializeField]
    private GameObject HomeScreen;
    [SerializeField]
    private GameObject GameScreen;
    [SerializeField]
    private GameObject ResultsScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    internal void GoToGameScreenFromHomeScreen()
    {
        //create functions to activate and deactivate screens and setup the screen, also add in any required animatons
        HomeScreen.SetActive(false);
        GameScreen.SetActive(true);
    }

    internal void GoToResultsScreenFromGameScreen()
    {
        GameScreen.SetActive(false);
        ResultsScreen.SetActive(true);
    }

    internal void GoToHomeScreenFromResultsScreen()
    {
        ResultsScreen.SetActive(false);
        HomeScreen.SetActive(true);
    }
    internal void GoToGameScreenFromResultsScreen()
    {
        ResultsScreen.SetActive(false);
        GameScreen.SetActive(true);
    }
}

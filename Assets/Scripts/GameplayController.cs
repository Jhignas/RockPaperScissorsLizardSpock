using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class GameplayController : MonoBehaviour
{
    public enum Choice
    {
        ROCK,
        PAPER,
        SCISSORS,
        LIZARD,
        SPOCK,
        NONE
    }
    public static GameplayController Instance { get; private set; }

    [SerializeField]
    private GameObject playerChoiceObject;
    [SerializeField]
    private GameObject opponentChoiceObject;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Image timerCircle;
    [SerializeField]
    private Text roundResult;
    [SerializeField]
    private Text gameScore;
    [SerializeField]
    private Text currentScore;
    [SerializeField]
    private Text highScore;

    private readonly int[,] resultsArray =
        new int[,] { 
            { 0, -1, 1, 1, -1 },
            { 1, 0, -1, -1, 1 },
            { -1, 1, 0, 1, -1 },
            { -1, 1, -1, 0, 1 },
            { 1, -1, 1, -1, 0 }
        };

    private bool timerActive = false;
    private double currentTime = 0;
    private double roundTime = 0;
    private int highScoreValue;
    private TimeSpan timeSpan;

    private AnimationController animationController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;

        highScoreValue = PlayerPrefs.GetInt("Highscore");
        highScore.text = highScoreValue.ToString();

        animationController = GetComponent<AnimationController>();
    }

    private void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            timeSpan = TimeSpan.FromSeconds(currentTime);
            timerText.text = timeSpan.Seconds.ToString();
            timerCircle.fillAmount = Mathf.InverseLerp(1, (float)roundTime, (float)currentTime);
        }
    }
    public void SetPlayerChoice(string playerChoice)
    {
        Text playerChoiceText = this.playerChoiceObject.GetComponentInChildren<Text>();
        if (playerChoiceText == null)
            return;
        playerChoiceText.text = playerChoice;
    }
    private void SetOpponentChoice(string opponentChoice)
    {
        Text oppponentChoiceText = this.opponentChoiceObject.GetComponentInChildren<Text>();
        if (oppponentChoiceText != null)
            oppponentChoiceText.text = opponentChoice;
    }

    internal void StartCampaign(int timer = 5, String opponent = "")
    {
        StartCoroutine(RunRounds(timer,opponent));
    }

    private string getResultText(int playerWin)
    {
        switch (playerWin)
        {
            case -1:
                return "YOU LOSE!";

            case 0:
                return "IT'S A DRAW!";

            case 1:
                return "YOU WIN!";

            default:
                return "";
        }
    }

    //return
    // 1 if win
    // 0 if draw
    //-1 if lost
    private int DetermineWinner(Choice playerChoice, Choice oppponentChoice)
    {
        if (playerChoice == Choice.NONE)
            return -1;
        return resultsArray[(int)playerChoice, (int)oppponentChoice];
    }

    private Choice GetRandomOpponentChoice()
    {
        Random random = new Random();
        switch (random.Next(0, 5))
        {
            case 0:
                return Choice.ROCK;

            case 1:
                return Choice.PAPER;

            case 2:
                return Choice.SCISSORS;

            case 3:
                return Choice.LIZARD;

            case 4:
                return Choice.SPOCK;

            default:
                return Choice.NONE;
        }
    }

    private Choice GetChoiceFromText(string choiceText)
    {
        switch (choiceText.ToUpper())
        {
            case "ROCK":
                return Choice.ROCK;

            case "PAPER":
                return Choice.PAPER;

            case "SCISSORS":
                return Choice.SCISSORS;

            case "LIZARD":
                return Choice.LIZARD;

            case "SPOCK":
                return Choice.SPOCK;

            default:
                return Choice.NONE;
        }
    }

    IEnumerator RunRounds(double timer = 5, String opponent = "")
    {
        int playerWin = 0;
        int scoreValue = 0;
        currentScore.text = scoreValue.ToString();
        Choice playerChoice;
        Choice opponentChoice;

        while (playerWin >= 0)
        {
            roundResult.gameObject.SetActive(false);
            timerText.gameObject.SetActive(true);
            SetPlayerChoice("?");

            //set timer
            currentTime = timer;
            roundTime = currentTime;
            timeSpan = TimeSpan.FromSeconds(currentTime);
            timerText.text = timeSpan.Seconds.ToString();
            timerCircle.fillAmount = Mathf.InverseLerp(1, (float)roundTime, (float)currentTime);

            //run animation to shuffle opponent choice
            opponentChoiceObject.SetActive(false);
            animationController.ShuffleOpponentChoicesAnimation();
            yield return new WaitForSeconds(1.5f);
            opponentChoiceObject.SetActive(true);

            //set Opponent choice
            if (opponent.Equals("Sheldon"))
            {
                opponentChoice = Choice.SPOCK;
            }
            else
            {
                opponentChoice = GetRandomOpponentChoice();
            }
            SetOpponentChoice(opponentChoice.ToString());

            //start timer
            timerActive = true;
            //wait for timer
            yield return new WaitForSeconds((int)timer);

            //stop timer
            timerActive = false;

            //determine the winner
            string playerChoiceText = this.playerChoiceObject.GetComponentInChildren<Text>().text;
            playerChoice = GetChoiceFromText(playerChoiceText);
            playerWin = DetermineWinner(playerChoice, opponentChoice);
            if (playerWin == 1)
            {
                scoreValue++;
                currentScore.text = scoreValue.ToString();
            }

            //Show if the player won this roound
            timerText.gameObject.SetActive(false);
            roundResult.text = getResultText(playerWin);
            roundResult.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            //loop again if win/draw or go to results screen
            if(0.9*timer>1)timer *= 0.9;
        }

        gameScore.text = scoreValue.ToString();
        if (scoreValue>highScoreValue) {
            highScoreValue = scoreValue;
            PlayerPrefs.SetInt("Highscore", highScoreValue);
            highScore.text = highScoreValue.ToString();
        }
        ScreenManager.Instance.GoToResultsScreenFromGameScreen();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndTimer : MonoBehaviour
{
    public int TimeLimit;
    public Text timerText;
    public Text Team1Score;
    public Text Team2Score;
    public Text VictoryText;
    public GameObject VictoryButton;
    public GameObject HelpLeft;
    public GameObject HelpRight;

    private float timeLeft;
    public int team1Score;
    public int team2Score;
    public bool gameOver;
    private Startup startup;

    public int amountGoldBombs;
    public int maxGoldBombsAllowed;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = TimeLimit;
        team1Score = 0;
        team2Score = 0;
        gameOver = false;
        startup = FindObjectOfType<Startup>();

        amountGoldBombs = 0;
        maxGoldBombsAllowed = 0;

        /*float yPosition = Screen.height/2/100*90;
        float xOffset = Screen.width/2/100*80;
        Team1Score.transform.localPosition = new Vector2((xOffset*-1), yPosition);
        Team2Score.transform.localPosition = new Vector2(xOffset, yPosition);
        timerText.transform.localPosition = new Vector2(0, yPosition);*/
    }

    void Update()
    {
        //Keep updating the score and timer until the game is over
        if (!gameOver)
        {
            SetText();
        }
    }

    //Makes the current time and score visible on screen 
    //and calculates how many gold bombs are allowed to be in the game at a time as long as the time isn't over yet
    void SetText()
    {
        if (timeLeft > 0)
        {
            float roundedTimeLeft = Mathf.Round(timeLeft);
            if(roundedTimeLeft == TimeLimit/2)
            {
                maxGoldBombsAllowed = 1;
                timerText.GetComponent<Animator>().SetBool("HalfWayPoint", true);
            }
            else if(roundedTimeLeft == TimeLimit/4)
            {
                maxGoldBombsAllowed = 2;
            }
            else if(roundedTimeLeft == 10)
            {
                timerText.GetComponent<Animator>().SetBool("RunningOutOftime", true);
            }

            timeLeft -= Time.deltaTime;
            timerText.text = Mathf.Round(timeLeft).ToString();
            
            Team1Score.text = "Team 1\n" + team1Score;
            Team2Score.text = "Team 2\n" + team2Score;
        }
        else
        {
            timerText.GetComponent<Animator>().SetBool("RunningOutOftime", false);
            FinishGame();
        }
    }

    //Disables the game from going on further and determines the winner of the game
    void FinishGame()
    {
        startup.DisableButtons();
        startup.DisableBombs();
        startup.DisableConveyors();
        if (team1Score > team2Score)
        {
            VictoryText.text = "Team 1 heeft gewonnen!";
        }
        else if (team1Score < team2Score)
        {
            VictoryText.text = "Team 2 heeft gewonnen!";
        }
        else
        {
            VictoryText.text = "Gelijkspel!";
        }
        HelpLeft.SetActive(false);
        HelpRight.SetActive(false);
        //VictoryText.text += "\n\nGooi tegen het scherm aan\nvoor de volgende ronde";
        VictoryText.enabled = true;
        VictoryButton.SetActive(true);
        gameOver = true;
    }
}

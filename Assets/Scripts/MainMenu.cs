using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System;

public class MainMenu : MonoBehaviour
{
    [Title("Canvases")]
    public int test = 1;

    public Canvas Gameplay, MainMenuCanvas, Endgame,StatsSound;
    
    [Title("Managers")]
    public GameManager gm;

    public AdManager am; 

    public GameObject tClassicHighscore, tRandomHighscore, tGame3Highscore, tGame4Highscore;
    //Streak main menu
    public GameObject tStreak, tStreakDesc;

    //Stats
    [Title("Statistics Objects")]
    public GameObject tTimePlayed;
    public GameObject tButtonsPressed;
    public GameObject tadRewards;

    public void Start()
    {
        DisplayHighScores();
        //gm.LoadGame();
        DisplayStreak();
    }

    

    public void Start_Gamemode(string gamemode)
    {
        SwitchToCanvas("game");
        //AddToButton not needed as that is in RestartGame method below
        Gameplay.enabled = true;
        //No longer used
        gm.RestartGame(gamemode);
    }
    public void Update()
    {
        if (Gameplay.enabled == true)
        {
            gm.AddToGameTime(Time.deltaTime);
        }
        if (MainMenuCanvas.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                QuitGame();
                
        }
        else if (StatsSound.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ReturnToMainMenuFromStats();
        }
    }

    void QuitGame()
    {
        //am.CloseBannerAd();
        Application.Quit();
    }

    private void SwitchToGameCanvas(bool state)
    {
        Gameplay.enabled = state;
        Endgame.enabled = state;
        MainMenuCanvas.enabled = !state;
    }
    public void ReturnToMainMenu()
    {
        gm.currentGamemode = "";
        gm.fTimedRoundTimer = 60;
        gm.tTimedRoundsTimer.text = "";
        gm.EndGame();
        gm.MakeButtonsInteractable();
        //To display those sweet new high scores
        DisplayHighScores();
        SwitchToCanvas("mainmenu");
        gm.AddToButtonPressed();
    }

    public void ReturnToMainMenuFromStats()
    {
        //To display those sweet new high scores
        DisplayHighScores();
        DisplayStreak();
        SwitchToCanvas("mainmenu");
    }
    public void ShowStats()
    {
        DisplayStats();
        SwitchToCanvas("stats");
    }
    private void SwitchToCanvas(string canvas)
    {
        if (canvas == "mainmenu")
        {
            Gameplay.enabled = false;
            Endgame.enabled = false;
            MainMenuCanvas.enabled = true;
            StatsSound.enabled = false;
        }
        else if (canvas == "stats")
        {
            Gameplay.enabled = false;
            Endgame.enabled = false;
            MainMenuCanvas.enabled = false;
            StatsSound.enabled = true;
        }
        else if (canvas == "game")
        {
            Gameplay.enabled = true;
            Endgame.enabled = true;
            MainMenuCanvas.enabled = false;
            StatsSound.enabled = false;
        }
        else
        {
            //wut
        }
    }

    void DisplayHighScores()
    {
        PlayerData data = SaveSystem.LoadGame();

        if (data  != null)
            { 
            //High scores
            tClassicHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[0].ToString();
            
            tRandomHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[1].ToString();
            tGame3Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[2].ToString();
            tGame4Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[3].ToString();
            //Streak

            int hours = (int)(DateTime.Now - gm.dateLastAcquiredStreak).TotalHours;
            int days = (int)(DateTime.Now - gm.dateLastAcquiredStreak).TotalDays;
            int minutes = (DateTime.Now - gm.dateLastAcquiredStreak).Minutes;
            string textstuff = "";
            if (gm.dateLastAcquiredStreak != null)
            {
                textstuff = "M:" + minutes.ToString() + ",H:" + hours.ToString() + ",D:" + days.ToString();
            }
            if (data.gameStreak > 1)
            {
                tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + data.gameStreak.ToString() + " days!";
                //tStreakDesc.GetComponent<TextMeshProUGUI>().text = "Woo!";
                tStreakDesc.GetComponent<TextMeshProUGUI>().text = textstuff;
            }
            else
            {
                tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + data.gameStreak.ToString() + " day";
                //tStreakDesc.GetComponent<TextMeshProUGUI>().text = "EastEnders";
                tStreakDesc.GetComponent<TextMeshProUGUI>().text = textstuff;
            }
        }
        else
        {
            tClassicHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: 0";
            tRandomHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: 0";
            tGame3Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: 0";
            tGame4Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: 0";

            tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of 0 days";
            tStreakDesc.GetComponent<TextMeshProUGUI>().text = "";
        }

    }
    void DisplayStreak() //Called when game starts and when player returns to main menu
    {
        int streak = SaveSystem.GetStreak();

        if(streak != 0)
        {
            //For hours
            DateTime dLastAcquired = gm.dateLastAcquiredStreak;
            //For days
            DateTime dLastAcquiredDateOnly = dLastAcquired.Date;
            int minutes = (DateTime.Now - dLastAcquired).Minutes;
            int hoursSinceLastStreak = (int)(DateTime.Now - dLastAcquired).TotalHours;
            int days = (int)(DateTime.Now.Date - dLastAcquiredDateOnly).TotalDays;
            string textstuff = "";

            //Until can get another streak

            //Old
            if (gm.dateLastAcquiredStreak != null)
            {
                //Old - still works but old
                //textstuff = "M:" + minutes.ToString() + ",H:" + hoursSinceLastStreak.ToString() + ",D:" + days.ToString();
            }
            if (gm.dateLastAcquiredStreak != null)
            {
                if (days < 1)
                {
                    textstuff = "Come back tomorrow!";
                }
                else if (hoursSinceLastStreak < 6)
                {
                    textstuff = "Come back in " + (6 - hoursSinceLastStreak).ToString() + " hours.";
                }
                else if (days > 0 && days < 2)
                {
                    textstuff = "Play now to keep the streak!";
                }
            }

            if (streak > 1)
            {
                tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + streak + " days!";
                tStreakDesc.GetComponent<TextMeshProUGUI>().text = textstuff;
            }
            else
            {
                tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + streak + " day";
                tStreakDesc.GetComponent<TextMeshProUGUI>().text = textstuff;
            }
        }
        else
        {
            //Streak is zero, so either:
            //  New file, so no other data
            //  User has lost streak
            tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + streak + " days";
            tStreakDesc.GetComponent<TextMeshProUGUI>().text = "Get a new streak today!";
        }

    }
    public void DisplayStats()
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        { 
            float hours = Mathf.FloorToInt((data.secondsPlayed / 60) / 60);
            float minutes = Mathf.FloorToInt(data.secondsPlayed / 60);
            float seconds = Mathf.FloorToInt(data.secondsPlayed % 60);

            tTimePlayed.GetComponent<TextMeshProUGUI>().text = "Time played: " + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            tButtonsPressed.GetComponent<TextMeshProUGUI>().text = "Buttons Pressed: " + data.buttonsPressed.ToString();
            tadRewards.GetComponent<TextMeshProUGUI>().text = "Reward Ads Watched: " + data.adsRewardsWatched;
        }
        else
        {
            //No data, likely because no file exists.
            tTimePlayed.GetComponent<TextMeshProUGUI>().text = "Time played: 00:00:00";
            tButtonsPressed.GetComponent<TextMeshProUGUI>().text = "Buttons Pressed: 0";
            tadRewards.GetComponent<TextMeshProUGUI>().text = "Reward Ads Watched: 0";
        }
    }
}

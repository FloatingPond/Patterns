using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System;

public class MainMenu : MonoBehaviour
{
    [Title("Canvas")]
    public Canvas canvas;

    [Title("Panels")]
    public int test = 1;

    public GameObject  MainMenuPanel, Gameplay, Endgame, Stats, SoundSettings, Premium;

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
    public GameObject tHighestStreak;
    public GameObject tStreakAchieved;


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
        Gameplay.SetActive(true);
        //No longer used
        gm.RestartGame(gamemode);
    }
    public void Update()
    {
        //Counts when player is playing the game
        if (Gameplay.activeInHierarchy)
        {
            gm.AddToGameTime(Time.deltaTime);
        }
        //Allows player to instantly quit game when escape button is pressed
        if (MainMenuPanel.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                QuitGame();
                
        }
        //Returns player to main menu on escape button press
        else if (Stats.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ReturnToMainMenuFromStats();
        }
    }
    
    void QuitGame() //Called from update
    {
        //am.CloseBannerAd();
        Application.Quit();
    }
    
    private void SwitchToGameCanvas(bool state) //Main void for switching state of the game
    {
        Gameplay.SetActive(state);
        Endgame.SetActive(state);
        MainMenuPanel.SetActive(!state);
    }

    public void ReturnToMainMenu() //Used to return to the main menu from gaming
    {
        gm.currentGamemode = "";
        gm.fTimedRoundTimer = 60;
        gm.tTimedRoundsTimer.text = "";
        gm.EndGame();
        gm.MakeButtonsInteractable();
        //To display those sweet new high scores
        DisplayHighScores();
        DisplayStreak();
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

    public void ShowPremium()
    {
        SwitchToCanvas("premium");
    }

    public void SwitchToCanvas(string canvas)
    {
        if (canvas == "mainmenu")
        {
            Gameplay.SetActive(false);
            Endgame.SetActive(false);
            MainMenuPanel.SetActive(true);
            Stats.SetActive(false);
            SoundSettings.SetActive(true);
            Premium.SetActive(false);
        }
        else if (canvas == "stats")
        {
            Gameplay.SetActive(false);
            Endgame.SetActive(false);
            MainMenuPanel.SetActive(false);
            Stats.SetActive(true);
            SoundSettings.SetActive(false);
            Premium.SetActive(false);
        }
        else if (canvas == "game")
        {
            Gameplay.SetActive(true);
            Endgame.SetActive(true);
            MainMenuPanel.SetActive(false);
            Stats.SetActive(false);
            SoundSettings.SetActive(false);
            Premium.SetActive(false);
        }
        else if (canvas == "premium")
        {
            Gameplay.SetActive(false);
            Endgame.SetActive(false);
            MainMenuPanel.SetActive(false);
            Stats.SetActive(false);
            SoundSettings.SetActive(false);
            Premium.SetActive(true);

        }
        // This is so that you can open/close the sound settings from any canvas
        if (canvas == "soundsettings")
        {
            SoundSettings.SetActive(true);
        }
        else
        {
            SoundSettings.SetActive(false);
        }
    }

    void DisplayHighScores()
    {
        PlayerData data = SaveSystem.LoadGame();

        if (data  != null) //Data exists
            { 
            //High scores
            tClassicHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[0].ToString();
            
            tRandomHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[1].ToString();
            tGame3Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[2].ToString();
            tGame4Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[3].ToString();
            
            //No longer handled in here. Fuck knows why it was
            //Streak

            //int hours = (int)(DateTime.Now - gm.dateLastAcquiredStreak).TotalHours;
            //int days = (int)(DateTime.Now - gm.dateLastAcquiredStreak).TotalDays;
            //int minutes = (DateTime.Now - gm.dateLastAcquiredStreak).Minutes;
            //string textstuff = "";
            //if (gm.dateLastAcquiredStreak != null)
            //{
            //    textstuff = "M:" + minutes.ToString() + ",H:" + hours.ToString() + ",D:" + days.ToString();
            //}
            //if (data.gameStreak > 1)
            //{
            //    tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + data.gameStreak.ToString() + " days!";
            //    //tStreakDesc.GetComponent<TextMeshProUGUI>().text = "Woo!";
            //    tStreakDesc.GetComponent<TextMeshProUGUI>().text = textstuff;
            //}
            //else
            //{
            //    tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + data.gameStreak.ToString() + " day";
            //    //tStreakDesc.GetComponent<TextMeshProUGUI>().text = "EastEnders";
             //   tStreakDesc.GetComponent<TextMeshProUGUI>().text = textstuff;
            //}
        }
        else //NO DATA
        {
            tClassicHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: 0";
            tRandomHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: 0";
            tGame3Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: 0";
            tGame4Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: 0";
        //
        //    tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of 0 days";
        //    tStreakDesc.GetComponent<TextMeshProUGUI>().text = "";
        }

    }

    void DisplayStreak() //Called when game starts and when player returns to main menu from game or stats or anything else
    {
        //No streak changing logic is present
        //Merely displaying messages

        int streak = SaveSystem.GetStreak();

        if(streak != 0) //Actually has a streak
        {
            //For hours
            DateTime dLastAcquired = gm.dateLastAcquiredStreak;
            //For days
            DateTime dLastAcquiredDateOnly = dLastAcquired.Date;
            int minutes = (DateTime.Now - dLastAcquired).Minutes;
            int hoursSinceLastStreak = (int)(DateTime.Now - dLastAcquired).TotalHours;
            int daysViaDate = (int)(DateTime.Now.Date - dLastAcquiredDateOnly).TotalDays;
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
                if (daysViaDate == 0) //Less than one day
                {
                    textstuff = "Come back tomorrow!";
                }
                else if (daysViaDate == 1) //If equals 1
                {
                    if (hoursSinceLastStreak < 6) //Less than 6 hours
                    {
                        textstuff = "Come back in " + (6 - hoursSinceLastStreak).ToString() + " hours.";
                    }
                    else
                    { 
                    textstuff = "Play now to keep the streak!";
                    }
                }
                else if (daysViaDate > 1)
                {
                    //Bad, shouldn't be displayed
                    textstuff = "Why is this displaying?";
                }
                else if (daysViaDate < 0) //Date last required is in the future
                {
                    textstuff = "Calm down, time-traveller";
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
    public void DisplayStats() //Called from DisplayStats - Loads data and displays statistics on statistics page
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
            tHighestStreak.GetComponent<TextMeshProUGUI>().text = "Highest Streak: " + data.gameStreakHighscore;
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

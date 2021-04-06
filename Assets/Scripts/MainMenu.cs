using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class MainMenu : MonoBehaviour
{
    [Title("Canvases")]
    public int test = 1;

    public Canvas Gameplay, MainMenuCanvas, Endgame,StatsSound;
    
    [Title("Managers")]
    public GameManager gm;

    //public AdManager am; 

    public GameObject tClassicHighscore, tRandomHighscore, tGame3Highscore, tGame4Highscore;
    //Streak main menu
    public GameObject tStreak, tStreakDesc;

    //Stats
    public GameObject tTimePlayed, tButtonsPressed;
    public void Start()
    {
        DisplayHighScores();
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

        
            if (data.gameStreak > 1)
            {
                tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + data.gameStreak.ToString() + " days!";
                tStreakDesc.GetComponent<TextMeshProUGUI>().text = "Woo!";
            }
            else
            {
                tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + data.gameStreak.ToString() + " day";
                tStreakDesc.GetComponent<TextMeshProUGUI>().text = "EastEnders";
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
    void DisplayStreak()
    {
        int streak = SaveSystem.GetStreak();

        if (streak > 1)
        {
            tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + streak + " days!";
            tStreakDesc.GetComponent<TextMeshProUGUI>().text = "Woo!";
        }
        else
        {
            tStreak.GetComponent<TextMeshProUGUI>().text = "Streak of " + streak + " day";
            tStreakDesc.GetComponent<TextMeshProUGUI>().text = "EastEnders";
        }


    }
    void DisplayStats()
    {
        PlayerData data = SaveSystem.LoadGame();

        float hours = Mathf.FloorToInt((data.secondsPlayed / 60) / 60);
        float minutes = Mathf.FloorToInt(data.secondsPlayed / 60);
        float seconds = Mathf.FloorToInt(data.secondsPlayed % 60);

        tTimePlayed.GetComponent<TextMeshProUGUI>().text = "Time played: " + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        tButtonsPressed.GetComponent<TextMeshProUGUI>().text = "Buttons Pressed: " + data.buttonsPressed.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Canvas Gameplay, MainMenuCanvas, Endgame,StatsSound;
    public GameManager gm;

    public GameObject tClassicHighscore, tRandomHighscore, tGame3Highscore, tGame4Highscore;

    //Stats

    public GameObject tTimePlayed, tButtonsPressed;

    public void Start()
    {
        DisplayHighScores();
    }

    public void Start_Gamemode()
    {
        SwitchToCanvas("game");
        //AddToButton not needed as that is in RestartGame method below
        Gameplay.enabled = true;
        //No longer used
        gm.RestartGame();
    }
    public void Update()
    {
        if (Gameplay.enabled == true)
        {
            gm.AddToGameTime(Time.deltaTime);
        }
    }

    private void SwitchToGameCanvas(bool state)
    {
        Gameplay.enabled = state;
        Endgame.enabled = state;
        MainMenuCanvas.enabled = !state;
    }
    public void ReturnToMainMenu()
    {
        //To display those sweet new high scores
        DisplayHighScores();
        SwitchToCanvas("mainmenu");
        gm.AddToButtonPressed();
    }

    public void ReturnToMainMenuFromStats()
    {
        //To display those sweet new high scores
        DisplayHighScores();
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

        tClassicHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[0].ToString();
        tRandomHighscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[1].ToString();
        tGame3Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[2].ToString();
        tGame4Highscore.GetComponent<TextMeshProUGUI>().text = "High Score: " + data.highscores[3].ToString();

    }
    void DisplayStats()
    {
        PlayerData data = SaveSystem.LoadGame();

        tTimePlayed.GetComponent<TextMeshProUGUI>().text = "Time played: " + data.secondsPlayed.ToString();
        tButtonsPressed.GetComponent<TextMeshProUGUI>().text = "Buttons Pressed: " + data.buttonsPressed.ToString();
    }

}

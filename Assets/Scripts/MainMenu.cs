using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas Gameplay, MainMenuCanvas, Endgame;
    public GameManager gm;
    public void Start_Gamemode()
    {
        SwitchToCanvas("game");
        Gameplay.enabled = true;
        //No longer used
        gm.RestartGame();
    }

    private void SwitchToGameCanvas(bool state)
    {
        Gameplay.enabled = state;
        Endgame.enabled = state;
        MainMenuCanvas.enabled = !state;
    }
    public void ReturnToMainMenu()
    {
        SwitchToCanvas("mainmenu");
    }

    private void SwitchToCanvas(string canvas)
    {
        if (canvas == "mainmenu")
        {
            Gameplay.enabled = false;
            Endgame.enabled = false;
            MainMenuCanvas.enabled = true;
        }
        else if (canvas == "stats")
        {
            Gameplay.enabled = false;
            Endgame.enabled = false;
            MainMenuCanvas.enabled = false;
        }
        else if (canvas == "game")
        {
            Gameplay.enabled = true;
            Endgame.enabled = true;
            MainMenuCanvas.enabled = false;
        }
        else
        {
            //wut
        }
    }

}

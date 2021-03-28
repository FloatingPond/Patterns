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
        SwitchToGameCanvas(true);
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
        SwitchToGameCanvas(false);
    }

}

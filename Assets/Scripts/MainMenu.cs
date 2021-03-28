using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas Gameplay, MainMenuCanvas, Endgame, Text;
    public GameManager gm;
    public void Start_Classic()
    {
        HideCanvases(false);
        Gameplay.enabled = true;
        //No longer used
        gm.RestartGame();
    }

    private void HideCanvases(bool state)
    {
        Gameplay.enabled = state;
        Endgame.enabled = state;
        MainMenuCanvas.enabled = state;
    }
    public void ReturnToMainMenu()
    {
        HideCanvases(true);
    }

}

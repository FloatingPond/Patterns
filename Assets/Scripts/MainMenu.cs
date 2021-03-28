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
        HideCanvases();
        Gameplay.enabled = true;
        Text.enabled = true;
        gm.RestartGame();
    }

    private void HideCanvases()
    {
        Gameplay.enabled = false;
        Endgame.enabled = false;
        Text.enabled = false;
        MainMenuCanvas.enabled = false;

    }
}

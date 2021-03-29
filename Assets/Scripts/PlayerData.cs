using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string fileVersion;
    public int[] highscores;
    public float secondsPlayed; //NEEDS TO BE CHANGED TO MINUTES OR HOURS PRIOR TO RELEASE

    public PlayerData(GameManager gm)
    {
        fileVersion = "dev01";
        highscores = new int[4];
        highscores[0] = gm.Highscore; //Standard
        highscores[1] = 0; //Random
        highscores[2] = 1; //Game 3
        highscores[3] = 2; //Game 4
        secondsPlayed = gm.fGameTime; //FOR NOW, SECONDS
    }


}

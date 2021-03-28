using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string fileVersion;
    public int[] highscores;
    public float hoursPlayed;

    public PlayerData(GameManager gm)
    {
        fileVersion = "dev01";
        highscores = new int[4];
        highscores[0] = gm.Highscore; //Standard
        highscores[1] = 0; //Random
        highscores[2] = 1; //Game 3
        highscores[3] = 2; //Game 4
        hoursPlayed = 1f;
    }


}

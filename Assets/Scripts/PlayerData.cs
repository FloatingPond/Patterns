using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string fileVersion;
    public int[] highscores;
    
    public float secondsPlayed; //NEEDS TO BE CHANGED TO MINUTES OR HOURS PRIOR TO RELEASE
    //Games played per gametype []
    public int[] gamesplayed;
    //Buttons pressed
    public int buttonsPressed;
    //Total high scores - could either save it or calculate it on loaded data

    public PlayerData(GameManager gm)
    {
        fileVersion = "dev02";
        highscores = new int[4];
        highscores[0] = gm.Highscore; //Standard
        highscores[1] = 0; //Random
        highscores[2] = 1; //Game 3
        highscores[3] = 2; //Game 4
        secondsPlayed = gm.fGameTime; //FOR NOW, SECONDS

        gamesplayed = new int[4];

        buttonsPressed = gm.buttonsPressed;

        //Streak stuff
    }


}

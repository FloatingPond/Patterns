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

    //Streak
    public int gameStreak;
    public int[] dateLastPlayed; //HH,MM,DD,MM,YYYY

    public int gameStreakHighscore;
    public int[] dateGameStreakHighscoreAcquired;



    public PlayerData(GameManager gm)
    {
        fileVersion = "dev03";
        highscores = new int[4];
        highscores[0] = gm.Highscore[0]; //Classic
        highscores[1] = gm.Highscore[1]; ; //Random
        highscores[2] = gm.Highscore[2]; //Game 3
        highscores[3] = gm.Highscore[3]; //Game 4
        secondsPlayed = gm.fGameTime; //FOR NOW, SECONDS

        gamesplayed = new int[4];

        buttonsPressed = gm.buttonsPressed;

        //Streak stuff
        dateLastPlayed = new int[5]; //HH,MM,DD,MM,YYYY

        gameStreak = gm.GetGameStreak();
        gameStreakHighscore = gm.GetGameStreakHighscore();
    }


}

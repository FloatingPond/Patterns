using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    //Streak stuff

    //Main integer
    public int gameStreak;
    //Used and specified at the DateTime when the user acquires a new number
    //Also used to be compared against as to whether:
    //  If not long enough since last time before can give a new streak
    //  If has been enough time
    //  If has been too long since player played
    public string dateLastAcquiredStreak; //yyyy-MM-dd HH:mm tt

    //Last Streak
    public int gameStreakLast;
    public string dateLastAcquiredStreakLast; //yyyy-MM-dd HH:mm tt

    //Best ever streak highscore
    public int gameStreakHighscore; 
    //Date acquired gsHighscore - set to DateTime of gameStreak if gs is same as gsh
    //Else is the date of the last highest streak
    public int[] dateGameStreakHighscoreAcquired; //yyyy-MM-dd HH:mm tt



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
        gameStreak = gm.GetGameStreak();
        gameStreakHighscore = gm.GetGameStreakHighscore();
    }


}

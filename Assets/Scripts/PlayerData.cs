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
    
    public int[] gamesplayed; //Games played per gametype []
    
    public int buttonsPressed; //Buttons pressed
    
    ////Streak stuff
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
    public string dateGameStreakHighscoreAcquired; //yyyy-MM-dd HH:mm tt

    ////Ads
    public int adsRewardsWatched; //Reward Ads Watched

    public string dateLastRewardAdWatched; //The datetime that a reward ad was last watched
    //This is to be converted to a datettime on load
    //When the first load fails for new player, a date far into the past will be saved so it can be rewritten when player watched a reward ad
    //The saved datetime will be checked and if is less than X days (believe 7) then the reward button will not show, and banner ads will not be shown either

    //Sound settings
    public float master, music, sfx, voice;

    public PlayerData(GameManager gm)
    {
        fileVersion = "dev16";
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
        dateLastAcquiredStreak = gm.dateLastAcquiredStreak.ToString("yyyy-MM-dd HH:mm tt");

        gameStreakHighscore = gm.GetGameStreakHighscore();
        dateGameStreakHighscoreAcquired = gm.dateStreakHighscore.ToString("yyyy-MM-dd HH:mm tt");

        gameStreakLast = gm.GetGameStreakLast();
        dateLastAcquiredStreakLast = gm.dateLastAcquiredStreakLast.ToString("yyyy-MM-dd HH:mm tt");

        //Ads
        adsRewardsWatched = gm.am.GetadsRewardsWatched();
        dateLastRewardAdWatched = gm.am.dtLastTimeRewardAdWatched.ToString("yyyy-MM-dd HH:mm tt");

        //Sound
        master = gm.sm.masterFloat;
        music = gm.sm.musicFloat;
        sfx = gm.sm.SFX_Float;
        voice = gm.sm.voiceFloat;

    }
}

using UnityEngine;
using UnityEngine.UI;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;


public class AchievementManager : MonoBehaviour
{
    public int playerScore;
    public TextMeshProUGUI textStatus;

    string leaderboardID = "CgkIq77noacSEAIQAQ"; //Classic High Score

    string achievementID = "CgkIq77noacSEAIQAA"; //Play 1 of any Game mode
    //
    public static PlayGamesPlatform platform;
    

    void Start()
    {
        Debug.Log("START OF GOOGLE PLAY GAMES");
        if (platform == null) //Makes a platform if does not exist
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }

        AuthenticateGoogleV3();

        //UnlockAchievement();
    }

    private void AuthenticateGoogleV1()
    {
        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
            {
                textStatus.text = "Success";
                Debug.Log("Logged in successfully");
            }
            else
            {
                textStatus.text = "Failed";
                Debug.Log("Login Failed");
            }
        });
    }

    public void AuthenticateGoogleV2()
    {
        String text1 = textStatus.text;
        platform.Authenticate(SignInInteractivity.CanPromptAlways, (result) =>
        {
            switch (result)
            {
                case SignInStatus.Success:
                    textStatus.text = text1 + "Success";
                    Debug.Log("Logged in successfully");
                    break;
                default:
                    textStatus.text = text1 + "Failed";
                    Debug.Log("Login Failed - " + result);
                    break;
            }
        });
    }

    public void AuthenticateGoogleV3()
    {
        String text1 = textStatus.text;
        platform.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            switch (result)
            {
                case SignInStatus.Success:
                    textStatus.text = text1 + "Success";
                    Debug.Log("Logged in successfully");
                    break;
                default:
                    textStatus.text = text1 + "Failed";
                    Debug.Log("Login Failed " + result);
                    break;
            }
        });
    }

    public void AddScoreToLeaderboard()
    {
        if (Social.Active.localUser.authenticated)
        {

            Social.ReportScore(playerScore, leaderboardID, success => { });
        }
    }

    public void UnlockAchievement() //Basic from tutorial
    {
        if (Social.Active.localUser.authenticated)
        {
            Social.ReportProgress(achievementID, 100f, success => { });
        }
    }

    public void UnlockGameplayAchievement(string gamemode, int score, float stopwatch, GameManager gm)
    {
        if (Social.Active.localUser.authenticated) //Ensure GPG is enabled an player logged in
        { 
            //Unlock "Play one game of any game mode"
            //-

            //Achieve high score of 12 in Classic
            if (gamemode == "classic" && score >= 12)
            {
                //-
            }
            //Achieve high score of 7 in random game
            if (gamemode == "random" && score >= 7)
            {
                //-
            }
            //Achieve score of 90 in Timed Round
            if (gamemode == "timedround" && score >= 90)
            {
                //-
            }
            //Achieve high score of 5 in Match
            if (gamemode == "match" && score >= 5)
            {
                //-
            }
            //In timed rounds, get a minimum score of 100 without pressing a single wrong button
            if (gamemode == "timedround" && score >= 100 && gm.iTimedRoundWrongButtonsPressed == 0)
            {
                //-
            }
        }
    }

    public void UnlockAchievementPlayAllGamemodes(GameManager gm) //Method especially for '//Play all game modes'
    {
        if (Social.Active.localUser.authenticated) //Ensure GPG is enabled an player logged in
        {
            //If all 4 games have a high score higher than 1, the player has most likely played all the games
            //They'll definitely be players who just press play and exit, but they'll need a score of at least 1Get
            if (gm.Highscore[0] > 0 && gm.Highscore[1] > 0 && gm.Highscore[2] > 0 && gm.Highscore[3] > 0)
            {
                //-
            }
        }
    }

    public void UnlockAchievementStreak(GameManager gm) //Method used for any and all streak achievements
    {
        if (Social.Active.localUser.authenticated) //Ensure GPG is enabled an player logged in
        {
            //Get a Steak for 2 days "You returned!"
            if (gm.GetGameStreak() == 2)
            {
                //-
            }
            //Get a streak for 7 days
            if (gm.GetGameStreak() == 7)
            {
                //-
            }
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.Active.localUser.authenticated)
        {
            platform.ShowLeaderboardUI();
        }
    }

    public void ShowAchievements()
    {
        if (Social.Active.localUser.authenticated)
        {
            platform.ShowAchievementsUI();
        }
    }
    public void UpScore()
    {
        playerScore++;
    }

    
}
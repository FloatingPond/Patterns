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
    //Achievements
    string aIdFirstTimeOpen = "CgkIq77noacSEAIQAA"; //Start Game

    string aIdOneGamePlayed = "CgkIq77noacSEAIQAA"; //Play 1 of any Game mode

    string aIdAllGameModesPlayed = "CgkIq77noacSEAIQCA"; //Play all game modes

    string aIdOneGameZeroScore = "CgkIq77noacSEAIQCg"; //Play 1 game and get a score of zero

    string aIdClassicMode12Score = "CgkIq77noacSEAIQAw"; //Achieve high score of 12 in Classic Mode

    string aIdRandom8Score = "CgkIq77noacSEAIQBQ"; //Achieve high score of 8 in Random Mode

    string aIdMatch5Score = "CgkIq77noacSEAIQBw"; //Achieve high score of 5 in Match Mode //"I have no matches."

    string aIdTimedRound101Score = "CgkIq77noacSEAIQBg"; //Achieve high score of 101 in Timed Round Mode

    string aIdClassMode13Score120Seconds = "CgkIq77noacSEAIQCQ"; //Achieve high score of 13 in Classic in under 120 seconds

    string aIdButtons1000 = "CgkIq77noacSEAIQCw"; //Press button 1000 times

    string aIdButtons9000 = "CgkIq77noacSEAIQDA"; //Press button 9000 times

    string aIdStreak2Days = "CgkIq77noacSEAIQDQ"; //It's coming home

    string aIdStreak7Days = "CgkIq77noacSEAIQDg"; //A week of Patterns

    public static PlayGamesPlatform platform;

    public GameObject bLogIn; //Disabled if game actually turns on

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
                    bLogIn.SetActive(false);
                    break;
                default:
                    textStatus.text = text1 + "Failed";
                    Debug.Log("Login Failed - " + result);
                    bLogIn.SetActive(true);
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
                    bLogIn.SetActive(false);
                    UnlockAchievement(); 
                    break;
                default:
                    textStatus.text = text1 + "Failed";
                    Debug.Log("Login Failed " + result);
                    bLogIn.SetActive(true);
                    break;
            }
        });
    }

    public void AddScoreToLeaderboard() //Currently not used or implemented in main build
    {
        if (Social.Active.localUser.authenticated)
        {

            Social.ReportScore(playerScore, leaderboardID, success => { });
        }
    }

    public void UnlockAchievement() //Basic from tutorial but used for first time open achievement 'Start Game'
    {
        //
        if (Social.Active.localUser.authenticated)
        {
            Social.ReportProgress(aIdFirstTimeOpen, 100f, success => { });
        }
    }

    public void UnlockGameplayAchievement(string gamemode, int score, float stopwatch, GameManager gm) //Called at end of a game
    {
        if (Social.Active.localUser.authenticated) //Ensure GPG is enabled an player logged in
        {
            //Unlock "Play one game of any game mode"
            Social.ReportProgress(aIdOneGamePlayed, 100f, success => { });

            //Check if all games have been played by seeing if player has high score of 1 in all
            if (CheckIfAllGamemodesHaveBeenPlayed(gm))
            {
                Social.ReportProgress(aIdAllGameModesPlayed, 100f, success => { });
            }

            if (score == 0)
            {
                Social.ReportProgress(aIdOneGameZeroScore, 100f, success => { });
            }

            //Achieve high score of 12 in Classic
            if (gamemode == "classic" && score >= 12)
            {
                //-
                Social.ReportProgress(aIdClassicMode12Score, 100f, success => { });
            }
            //Achieve high score of 8 in random game
            if (gamemode == "random" && score >= 8)
            {
                Social.ReportProgress(aIdRandom8Score, 100f, success => { });
            }
            //Achieve score of 101 in Timed Round
            if (gamemode == "timedround" && score >= 101)
            {
                Social.ReportProgress(aIdTimedRound101Score, 100f, success => { });
            }
            //Achieve high score of 5 in Match
            if (gamemode == "match" && score >= 5)
            {
                Social.ReportProgress(aIdMatch5Score, 100f, success => { });
            }
            
            //MORE ADVANCED ACHIEVEMENTS 
            //In timed rounds, get a minimum score of 100 without pressing a single wrong button
            if (gamemode == "timedround" && score >= 100 && gm.iTimedRoundWrongButtonsPressed == 0)
            {
                //-
            }
            if (gm.Highscore[0] > 0 && gm.Highscore[1] > 0 && gm.Highscore[2] > 0 && gm.Highscore[3] > 0)
            {
                Social.ReportProgress(aIdAllGameModesPlayed, 100f, success => { });
            }
        }
    }

    public void UnlockGameplayAchievementDuringActiveGame(string gamemode, int score, float stopwatch, GameManager gm) //Called after a round win during gameplay
    {
        if (Social.Active.localUser.authenticated) //Ensure GPG is enabled an player logged in
        {
            //In Classic, get a score of 13 in under x seconds
            if (gamemode == "classic" && score >= 13 && stopwatch <= 120)
            {
                Social.ReportProgress(aIdClassMode13Score120Seconds, 100f, success => { });
            }
        }
    }

    public void UnlockAchievementStreak(GameManager gm) //Method used for any and all streak achievements
    {
        if (Social.Active.localUser.authenticated) //Ensure GPG is enabled an player logged in
        {
            //Get a Steak for 2 days "You returned!"
            if (gm.GetGameStreak() >= 2)
            {
                Social.ReportProgress(aIdStreak2Days, 100f, success => { });
            }
            //Get a streak for 7 days
            if (gm.GetGameStreak() == 7)
            {
                Social.ReportProgress(aIdStreak7Days, 100f, success => { });
            }
        }
    }

    private bool CheckIfAllGamemodesHaveBeenPlayed(GameManager gm) //Called within the UnlockGameplayAchievement method
    {
        if (gm.Highscore[0] > 0 && gm.Highscore[1] > 0 && gm.Highscore[2] > 0 && gm.Highscore[3] > 0)
        {
            return true;
        }
        return false;
    }

    public void UnlockButtonAchivement(int buttons)
    {
        if (Social.Active.localUser.authenticated) //Ensure GPG is enabled an player logged in
        {
            if (buttons > 1000)
            {
                Social.ReportProgress(aIdButtons1000, 100f, success => { });
            }
            if (buttons > 9000)
            {
                Social.ReportProgress(aIdButtons9000, 100f, success => { });
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
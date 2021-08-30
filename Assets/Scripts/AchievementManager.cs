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

    string leaderboardID = "CgkI3M-O2b8NEAIQAg";

    string achievementID = "CgkI3M-O2b8NEAIQAA"; //First one
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

        AuthenticateGoogleV2();

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

    private void AuthenticateGoogleV2()
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
                    Debug.Log("Login Failed");
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

    public void UnlockAchievement()
    {
        if (Social.Active.localUser.authenticated)
        {
            Social.ReportProgress(achievementID, 100f, success => { });
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
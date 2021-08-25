using UnityEngine;
using UnityEngine.UI;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using Sirenix.OdinInspector;


public class PlayGames2 : MonoBehaviour
{
    public int playerScore;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textStatus;

    string leaderboardID = "CgkI3M-O2b8NEAIQAg";
    string achievementID = "CgkI3M-O2b8NEAIQAA";
    public static PlayGamesPlatform platform;
    [Button(ButtonSizes.Small)]
    [PropertyOrder(1)]
    private void SetScoreFromText()
    {
        SetScore();
    }

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
        //UnlockAchievement();
    }

    public void SetScore()
    {
        //playerScore = int.Parse(textScore.text);
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

    
}
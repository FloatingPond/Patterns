using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using TMPro;


public class AdManager : MonoBehaviour
{
    //Reward Ad Constant
    private const float RewardAdNoAdsHours = 48;

    //Banner
    private BannerView adBannerBottom;
    private string adUnitIdBanner = "ca-app-pub-5688744298036134/5023660262";
    private string adUnitIdRewardPremium = "ca-app-pub-5688744298036134/5669677461";
    private string adUnitIdRewardStreak = "ca-app-pub-5688744298036134/5669677461";

    private AdRequest requestForBanner;
    
    //Reward Ad stuff
    RewardedAd rewardedAd;
    private RewardBasedVideoAd adRbva;
    private RewardedAd adReward;
    private AdRequest requestForReward;
    public DateTime dtLastTimeRewardAdWatched;

    public GameObject buttonRewardAd;
    public GameObject tRewardAdDate;

    private int adsRewardsWatched;

    public MainMenu mm;

    public GameManager gm;
    
    
    public void SetupOnStart(bool firstTime) //Called in GameManager's Awake
    {
        //Always needs to be called
        requestForBanner = new AdRequest.Builder().Build();
        MobileAds.Initialize(initStatus => { });

        if (CheckIfPlayerHasRewardAdPremiumBoolEdition()) //Returns true if the player has premium via reward ad
        {
            EnableRewardAdElements(false);
        }
        else if (firstTime) //Delay ads by 60 seconds
        {
            EnableRewardAdElements(true);
            StartCoroutine(WaitBeforeRequestingBanner(60));
        }
        else //Returning non-reward ad play - Initialise ads 12s on launch
        {
            EnableRewardAdElements(true);
            //Banner ad stuff
            StartCoroutine(WaitBeforeRequestingBanner(12));
        }
    }

    IEnumerator WaitBeforeRequestingBanner(float time) //Holds showinf the banner for an amount of time
    {
        yield return new WaitForSeconds(time);
        RequestBanner();
        adBannerBottom.LoadAd(requestForBanner);
    }
    
    private void EnableRewardAdElements(bool state) //Enables or disables showing the reward ad button
    {
        buttonRewardAd.SetActive(state);
        StartCoroutine(DelayEnableRewardTextEndDate(!state)); //Done due to error if done instantly
    }
    
    public void RewardAdNoBanners() //WORKS - And is used for no ads
    {
        rewardedAd = new RewardedAd(adUnitIdRewardPremium);
        
        rewardedAd.OnAdLoaded += HandleOnRewardAdLoaded; // Called when an ad request has successfully loaded.
        
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // Called when an ad request failed to load.
        
        rewardedAd.OnUserEarnedReward += HandleUserEarnedRewardNoAds; // Called when the user should be rewarded for interacting with the ad.

        AdRequest request = new AdRequest.Builder().Build(); // Create an empty ad request.

        rewardedAd.LoadAd(request); // Load the rewarded ad with the request.

    }

    public void RewardAdReclaimStreak() //WORKS - And is used for claiming streak back
    {
        rewardedAd = new RewardedAd(adUnitIdRewardStreak);
        
        rewardedAd.OnAdLoaded += HandleOnRewardAdLoaded; // Called when an ad request has successfully loaded.
        
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // Called when an ad request failed to load.
        
        rewardedAd.OnUserEarnedReward += HandleUserEarnedRewardReclaimStreak; // Called when the user should be rewarded for interacting with the ad.
        
        AdRequest request = new AdRequest.Builder().Build(); // Create an empty ad request.
        
        rewardedAd.LoadAd(request); // Load the rewarded ad with the request.
    }

    private void RequestBanner()
    {
        adBannerBottom = new BannerView(adUnitIdBanner, AdSize.Banner, AdPosition.Bottom); // Create a 320x50 banner at the top of the screen.
    }

    public void CloseBannerAd()
    {
        if (adBannerBottom != null)
        { 
            adBannerBottom.Destroy();
        }
        else
        {
            Debug.LogError("Banner ad is trying to be destroyed but does not exist!");
        }
    }

    public void HandleUserEarnedRewardNoAds(object sender, Reward args) //Player has completed a Rewards Ad For No Ads
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

        adsRewardsWatched++;
        
        dtLastTimeRewardAdWatched = DateTime.Now; //Sets last time user watched reward ad to now
        
        //Method to disable ads
        CheckIfPlayerHasRewardAdPremium(); //Used to disable ads - DISABLED
        
        //Closes ad if open
        CloseBannerAd();

        gm.Save(); //Saves as rewards watched has gone up
        mm.DisplayStats();
    }

    public void HandleUserEarnedRewardReclaimStreak(object sender, Reward args) //Player has completed a Rewards Ad For Streak Return
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
        adsRewardsWatched++;

        dtLastTimeRewardAdWatched = DateTime.Now; //Sets last time user watched reward ad to now

        CloseBannerAd();

        gm.ReclaimStreak(); //Give back streak
        mm.DisplayStreak(); //Show it on main menu

        gm.Save();
        mm.DisplayStats(); //Saves as rewards watched has gone up
    }

    public void CheckIfPlayerHasRewardAdPremium() //Called on Reward Win
    {
        int hours = (int)(DateTime.Now - dtLastTimeRewardAdWatched).TotalHours;

        if (hours <= RewardAdNoAdsHours)
        {
            EnableRewardAdElements(false); //Disable showing reward ad button
            
        }
        else
        { 
            tRewardAdDate.SetActive(false);
            buttonRewardAd.SetActive(true);
        }
    }
    public bool CheckIfPlayerHasRewardAdPremiumBoolEdition() //Called on start for checking stuff
    {
        int hours = (int)(DateTime.Now - dtLastTimeRewardAdWatched).TotalHours;

        if (hours <= RewardAdNoAdsHours)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HandleOnRewardAdLoaded(object sender, EventArgs args)
    {
        if (rewardedAd.IsLoaded())
            rewardedAd.Show();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        rewardedAd.Show();
        MonoBehaviour.print("HandleRewardedAdLoaded event received");

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args) //Reward ad didn't load
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public int GetadsRewardsWatched()
    {
        return adsRewardsWatched;
    }

    public void SetadsRewardsWatched(int number) //Called in GM Load
    {
        adsRewardsWatched = number;
    }

    IEnumerator DelayEnableRewardTextEndDate(bool showState)
    {
        yield return new WaitForSeconds(2f);
        tRewardAdDate.SetActive(showState);
        
        if (showState == true)
        {
            int hours2 = (int)(DateTime.Now - dtLastTimeRewardAdWatched).TotalHours;
            //TextMeshProUGUI tText = tRewardAdDate.GetComponent<TextMeshProUGUI>(); //Gets the text component from the text
            //tText.text = "AD-free ends in " + (72 - hours2) + " hours."; //Sets the text from the component above
            tRewardAdDate.GetComponent<TextMeshProUGUI>().text = "AD-free ends in " + (RewardAdNoAdsHours - hours2) + " hours."; //Sets the text from the component above
        }
    }
    
}

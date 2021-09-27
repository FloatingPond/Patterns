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
    private string adUnitId = "ca-app-pub-3940256099942544/6300978111";
    private AdRequest requestForBanner;
    //Reward
    private string adUnitId2 = "ca-app-pub-3940256099942544/5224354917";
    
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
    
    void Start() //Not used
    {

    }

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

    IEnumerator WaitBeforeRequestingBanner(float time)
    {
        yield return new WaitForSeconds(time);
        RequestBanner();
        adBannerBottom.LoadAd(requestForBanner);
    }
    
    private void EnableRewardAdElements(bool state)
    {
        buttonRewardAd.SetActive(state);
        StartCoroutine(DelayEnableRewardTextEndDate(!state)); //Done due to error if done instantly
    }
    
    public void RewardAdNoBanners() //WORKS - And is used for no ads
    {
        //TUTORIAL REWARDS
        string adUnitId;
        #if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IPHONE
                            adUnitId = "ca-app-pub-3940256099942544/1712485313";
        #else
                            adUnitId = "unexpected_platform";
        #endif

        rewardedAd = new RewardedAd(adUnitId);
        
        List<string> testids = new List<string>();
        testids.Add("21B75031C51D44C92C2561822796725B");
        RequestConfiguration config = new RequestConfiguration.Builder().SetTestDeviceIds(testids).build();
        MobileAds.SetRequestConfiguration(config);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleOnRewardAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedRewardNoAds;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder()
                         .AddTestDevice("34343")
                         .Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        
    }

    public void RewardAdReclaimStreak() //WORKS - And is used for claiming streak back
    {
        //TUTORIAL REWARDS
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
                            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
                            adUnitId = "unexpected_platform";
#endif

        rewardedAd = new RewardedAd(adUnitId);

        List<string> testids = new List<string>();
        testids.Add("21B75031C51D44C92C2561822796725B");
        RequestConfiguration config = new RequestConfiguration.Builder().SetTestDeviceIds(testids).build();
        MobileAds.SetRequestConfiguration(config);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleOnRewardAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedRewardReclaimStreak;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder()
                         .AddTestDevice("34343")
                         .Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);

    }

    private void RequestBanner()
    {
        // Create a 320x50 banner at the top of the screen.
        adBannerBottom = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
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
        
        //If we make it - set the "Watched 1 reward ad" achievement to acquired
        //-

        //Method to disable ads
        CheckIfPlayerHasRewardAdPremium(); //Used to disable ads - DISABLED
        
        //Closes ad if open
        CloseBannerAd();

        gm.Save();
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
        mm.DisplayStats();
    }

    public void CheckIfPlayerHasRewardAdPremium() //Called on Reward Win
    {
        //if has bought premium
        //return true

        //if has been less than 3 days since last watched reward ad
        int hours = (int)(DateTime.Now - dtLastTimeRewardAdWatched).TotalHours;

        if (hours <= RewardAdNoAdsHours)
        {
            EnableRewardAdElements(false); //Disable showing reward ad button

            //tRewardAdDate.SetActive(true); //Shows the text
            //TextMeshProUGUI tText = tRewardAdDate.GetComponent<TextMeshProUGUI>(); //Gets the text component from the text
            //tText.text = "AD-free ends in " + (72 - hours) + " hours."; //Sets the text from the component above
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

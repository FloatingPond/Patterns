using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class AdManager : MonoBehaviour
{
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

    [Title("Counter")]
    [SerializeField]
    private int adsRewardsWatched;

    [Title("Managers")]
    public MainMenu mm;

    public GameManager gm;

    

    [Button(ButtonSizes.Large)]

    private void BigButton()
    {
        TutorialRewardAdWORKS();
    }

    void Start()
    {
        ////Always needs to be called
        //MobileAds.Initialize(initStatus => { });

        ////TutorialStuffThatDidntWork();


        ////Banner

        //RequestBanner();

        //requestForBanner = new AdRequest.Builder().Build();

        //adBannerBottom.LoadAd(requestForBanner);

        ////Reward

        //// adRbva = RewardBasedVideoAd.Instance;
    }

    public void SetupOnStart(bool firstTime)
    {
        //Always needs to be called
        MobileAds.Initialize(initStatus => { });

        RequestBanner();

        requestForBanner = new AdRequest.Builder().Build();
        //Loads a bottom banner ad if this is not the first time the player has played
        //To later be more advanced should the player use a reward ad or buy premium
        if (!firstTime) 
        { 
            adBannerBottom.LoadAd(requestForBanner);
        }

        //Reward

        // adRbva = RewardBasedVideoAd.Instance;
    }

    public void TutorialRewardAdWORKS() //WORKS
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
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

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

    private void RequestReward() //DOES NOT WORK
    {
        Debug.Log("Reward Ad Made");

        requestForReward = new AdRequest.Builder().Build();

        adReward.LoadAd(requestForReward);
    }

    public void RequestReward2() //DOES NOT WORK
    {
        //Reward - IS NOT WORKING
        Debug.Log("BUTTON PRESSED");

        List<string> testids = new List<string>();
        testids.Add("21B75031C51D44C92C2561822796725B");
        RequestConfiguration config = new RequestConfiguration.Builder().SetTestDeviceIds(testids).build();
        MobileAds.SetRequestConfiguration(config);

        adReward = new RewardedAd(adUnitId2);

        requestForReward = new AdRequest.Builder().Build();

        adReward.LoadAd(requestForReward);

        adReward.Show();
    }

    public void RequestReward3() //DOES NOT WORK
    {
        //RequestConfiguration.Builder().setTestDeviceIds(Arrays.asList("21B75031C51D44C92C2561822796725B"));
        List<string> testids = new List<string>();
        testids.Add("21B75031C51D44C92C2561822796725B");
        RequestConfiguration config = new RequestConfiguration.Builder().SetTestDeviceIds(testids).build();
        MobileAds.SetRequestConfiguration(config);


        requestForReward = new AdRequest.Builder().Build();

        string test = "";
        adRbva.LoadAd(requestForReward, test);

        adRbva.OnAdLoaded += this.HandleOnRewardAdLoaded;

        // Called when an ad request failed to load.
        //adRbva.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
    }

    public void CloseBannerAd()
    {
        StopAllCoroutines();
        adBannerBottom.Destroy();
    }

    public void HandleUserEarnedReward(object sender, Reward args) //Player has completed a Rewards Ad
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
        mm.test += 1;
        adsRewardsWatched++;

        dtLastTimeRewardAdWatched = DateTime.Now; //Sets last time user watched reward ad to now
        //If we make it - set the "Watched 1 reward ad" achievement to acquired
        gm.Save();
        mm.DisplayStats();
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

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public int GetadsRewardsWatched()
    {
        return adsRewardsWatched;
    }

    public void SetadsRewardsWatched(int number)
    {
        adsRewardsWatched = number;
    }




}

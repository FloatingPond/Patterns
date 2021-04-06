using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using Sirenix.OdinInspector;


public class AdManager : MonoBehaviour
{
    //Banner
    private BannerView adBannerBottom;
    private string adUnitId =  "ca-app-pub-3940256099942544/6300978111";
    private AdRequest requestForBanner;
    //Reward
    private string adUnitId2 = "ca-app-pub-3940256099942544/5224354917";
    private RewardedAd adReward;
    private AdRequest requestForReward;

    [Button(ButtonSizes.Large)]

    private void BigButton()
    {
        RequestReward();
    }


    void Start()
    {
        //Always needs to be called
        MobileAds.Initialize(initStatus => { });
        Debug.Log("AdManager initialised.");

        //Banner

        RequestBanner();

        requestForBanner = new AdRequest.Builder().Build();

        adBannerBottom.LoadAd(requestForBanner);

        //Reward - IS NOT WORKING

        //adReward = new RewardedAd(adUnitId2);

        //requestForReward = new AdRequest.Builder().Build();

        //adReward.LoadAd(requestForReward);

        

        

    }

    public void TutorialStuffThatDidntWork()
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

        RewardedAd rewardedAd = new RewardedAd(adUnitId);

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

    private void RequestReward()
    {
        Debug.Log("Reward Ad Made");

        requestForReward = new AdRequest.Builder().Build();

        adReward.LoadAd(requestForReward);
    }

    public void CloseBannerAd()
    {
        StopAllCoroutines();
        adBannerBottom.Destroy();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
        Debug.Log("ERROR NO AD");
    }


}

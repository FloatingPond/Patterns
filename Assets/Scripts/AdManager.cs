using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using Sirenix.OdinInspector;


public class AdManager : MonoBehaviour
{
    private BannerView adBannerBottom;

    private RewardedAd adReward;

    private string adUnitId = "ca-app-pub-3940256099942544/6300978111";
    private string adUnitId2 = "ca-app-pub-3940256099942544/5224354917";



    private AdRequest requestForBanner;
    private AdRequest requestForReward;

    [Button(ButtonSizes.Large)]

    private void BigButton()
    {
        RequestReward();
    }


    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        Debug.Log("AdManager initialised.");

        //Banner
        RequestBanner();

        requestForBanner = new AdRequest.Builder().Build();

        adBannerBottom.LoadAd(requestForBanner);

        //Reward
        //adReward = new RewardedAd(adUnitId2);

        //requestForReward = new AdRequest.Builder().Build();

        //adReward.LoadAd(requestForReward);
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

    
}

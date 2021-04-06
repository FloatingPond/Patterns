using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    [SerializeField]
    private BannerView adBannerBottom;

    [SerializeField]
    private int test;

    private AdRequest request;

    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        Debug.Log("AdManager initialised.");

        RequestBanner();

        request = new AdRequest.Builder().Build();

        adBannerBottom.LoadAd(request);
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        
        // Create a 320x50 banner at the top of the screen.
        adBannerBottom = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
    }

    public void CloseBannerAd()
    {
        adBannerBottom.Destroy();
    }
}

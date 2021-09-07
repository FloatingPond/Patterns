using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;


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

    public GameObject buttonRewardAd;
    public GameObject tRewardAdDate;

    [Title("Counter")]
    [SerializeField]
    private int adsRewardsWatched;

    [Title("Managers")]
    public MainMenu mm;

    public GameManager gm;

    [Title("Buttons")]

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

    public void SetupOnStart(bool firstTime) //Called in GameManager's Awake
    {
        //Always needs to be called
        requestForBanner = new AdRequest.Builder().Build();
        MobileAds.Initialize(initStatus => { });

        if (CheckIfPlayerHasRewardAdPremiumBoolEdition()) //Returns true if the player has premium via reward ad
        {
            EnableRewardAdElements(false);
        }
        else if (CheckIfPlayerHasPremium()) //Paid Premium
        {

        }
        else if (firstTime) //Delay ads by 3 minutes
        {
            EnableRewardAdElements(true);
            StartCoroutine(WaitBeforeRequestingBanner());
        }
        
        else //Initialise ads immediately on launch
        {
            EnableRewardAdElements(true);
            //Banner ad stuff
            RequestBanner();
            adBannerBottom.LoadAd(requestForBanner);
        }
        
        // adRbva = RewardBasedVideoAd.Instance; //Reward
    }

    IEnumerator WaitBeforeRequestingBanner()
    {
        yield return new WaitForSeconds(60f);
        RequestBanner();
        adBannerBottom.LoadAd(requestForBanner);

    }
    
    public bool CheckIfPlayerHasPremium()
    {
        return false;
    }

    private void EnableRewardAdElements(bool state)
    {
        buttonRewardAd.SetActive(state);
        StartCoroutine(DelayEnableRewardTextEndDate(!state));
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
        //StopAllCoroutines(); //Not sure why this was here
        if (adBannerBottom != null)
        { 
            adBannerBottom.Destroy();
        }
        else
        {
            Debug.LogError("Banner ad is trying to be destroyed but does not exist!");
        }
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
        //-

        //Method to disable ads
        CheckIfPlayerHasRewardAdPremium(); //Used to disable ads - DISABLED
        
        //Closes ad if open
        CloseBannerAd();

        gm.Save();
        mm.DisplayStats();
    }
    public void CheckIfPlayerHasRewardAdPremium() //Called on Reward Win
    {
        //if has bought premium
        //return true

        //if has been less than 3 days since last watched reward ad
        int hours = (int)(DateTime.Now - dtLastTimeRewardAdWatched).TotalHours;

        if (hours <= 72)
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
        //if has bought premium
        //return true

        //if has been less than 3 days since last watched reward ad
        int hours = (int)(DateTime.Now - dtLastTimeRewardAdWatched).TotalHours;

        if (hours <= 72)
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

    IEnumerator DelayEnableRewardTextEndDate(bool showState)
    {
        yield return new WaitForSeconds(2f);
        tRewardAdDate.SetActive(showState);
    }

    




}

using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class Admanager : MonoBehaviour {

    public static Admanager instance;
    public PickupsAndStats stats;
    public GameControl control;

    //dummy ads still loading even without IDs
    private string appID = "ca-app-pub-1906605411705517~2269664916";

    private BannerView bannerView;
    private string bannerID = "ca-app-pub-3940256099942544/6300978111";

    private RewardBasedVideoAd rewardBasedVideo;

    private void Awake()
    {
        DontDestroyOnLoad(this);
       

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        
    }

    private void Start()
    {
        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        this.RequestRewardBasedVideo();

        //Event Declarations//

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

    }

    public void RequestRewardBasedVideo()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }

    public void UserOptToWatchAd()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
            RequestRewardBasedVideo();
        }
    }

    ///Handler Events///

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        //get 20 crystals
        for (int i = 0; i < 20; i++)
        {
            stats.CrystalIncrease();
                }
        //save game
        control.SaveGame();
        print("User rewarded with: " + amount.ToString() + " " + type);
    }


    public void HandleRewardBasedVideoClosed(object sender, System.EventArgs args)
    {
        Debug.Log("Video Closed event?");
        this.RequestRewardBasedVideo();
    }


    public void HandleRewardBasedVideoLoaded(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }


    public void HandleRewardBasedVideoLeftApplication(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

}

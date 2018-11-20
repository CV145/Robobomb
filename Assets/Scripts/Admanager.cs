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

        //rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
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


    public void UserOptToWatchAd()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
    }

    //public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    //{
    //    this.RequestRewardBasedVideo();
    //}
}

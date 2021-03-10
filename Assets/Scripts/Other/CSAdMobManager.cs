using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class CSAdMobManager : MonoBehaviour {

    [Header("Admob App Id")]
    public string adMobAppIdiOS;
    public string adMobAppIdAndroid;

    [Header("Admob Interstitial")]
    public string adMobAppInterstitialiOS;
    public string adMobAppInterstitialAndroid;

    [Header("Admob Reward")]
    public string adMobAppRewardiOS;
    public string adMobAppRewardAndroid;

    public static CSAdMobManager instance = null;
    private InterstitialAd _interstitial;
    private bool _live = false;
    private RewardBasedVideoAd _rewardBasedVideo;
    private Action _RewardVideoCallback;
    private Action _InterstitialCallback;

    void Awake()
    {
        if (instance == null)
        {
            _live = true;
            DontDestroyOnLoad(gameObject);
            instance = this;
            Loaded();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (!_live)
            return;

        HideInterstitialAd();
    }

    private void Loaded()
    {
        InitAdmob();

        _interstitial = CreateInterstitialAd();

        _rewardBasedVideo = RewardBasedVideoAd.Instance;
        _rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        _rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        _rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        _rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        _rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;


        RequestInterstitialAd(_interstitial);

        RequestRewardBasedVideo();
    }

    private void InitAdmob()
    {
        string appId = "unexpected_platform";
#if UNITY_ANDROID
        appId = adMobAppIdAndroid;
#elif UNITY_IPHONE
        appId = adMobAppIdiOS;
#endif

        //MobileAds.Initialize(appId);
        MobileAds.Initialize(status => {
            Debug.Log("AdMob Initialized");
        });
    }

    private void RequestRewardBasedVideo()
    {
        string adUnitId = "unexpected_platform";
#if UNITY_ANDROID
        adUnitId = adMobAppRewardAndroid;
#elif UNITY_IPHONE
        adUnitId = adMobAppRewardiOS;
#endif
        AdRequest request = CreateAdRequest();
        _rewardBasedVideo.LoadAd(request, adUnitId);
        Debug.Log("!@#$ RequestRewardBasedVideo: " + adUnitId);
    }

    public bool ShowVideoRewardAd(Action callback = null)
    {
        bool val = false;
#if UNITY_IOS || UNITY_ANDROID
        if (_rewardBasedVideo == null)
        {
            return false;
        }

        if (!_rewardBasedVideo.IsLoaded())
        {
            return false;
        }
            
        _RewardVideoCallback = callback;

        _rewardBasedVideo.Show();
        val = true;
#endif
        return val;
    }

    public bool ShowInterstitialAd(Action callback = null)
    {
        bool val = false;
#if UNITY_IOS || UNITY_ANDROID
        val = _interstitial.IsLoaded();
        if (val)
        {
            _interstitial.Show();
            _InterstitialCallback = callback;
        }
#endif
        return val;
    }

    public void HideInterstitialAd()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (_interstitial != null)
            _interstitial.Destroy();
#endif
    }

    private void RequestInterstitialAd(InterstitialAd ad)
    {
        ad.LoadAd(CreateAdRequest());
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
                            .AddTestDevice(AdRequest.TestDeviceSimulator)
                            .AddTestDevice("335310d3efdc0a0d6c5c10e20dad1528")
                            .Build();
    }

    private InterstitialAd CreateInterstitialAd()
    {
        string adUnitId = "unexpected_platform";
#if UNITY_ANDROID
        adUnitId = adMobAppInterstitialAndroid;
#elif UNITY_IPHONE
        adUnitId = adMobAppInterstitialiOS;
#endif

        InterstitialAd interstitial = new InterstitialAd(adUnitId);
        interstitial.OnAdFailedToLoad += HandleInterstitialAdFailedToLoad;
        interstitial.OnAdOpening += HandleInterstitialAdOnAdOpened;
        interstitial.OnAdClosed += HandleInterstitialAdOnAdClosed;

        return interstitial;
    }

    public void ShowAds(Action callback = null)
    {
        if (_rewardBasedVideo.IsLoaded())
        {
            ShowVideoRewardAd(callback);
        }
        else
        {
            ShowInterstitialAd(callback);
        }
    }

    public void HandleInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("!@#$ HandleInterstitialAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialAdOnAdOpened(object sender, EventArgs args)
    {

    }

    public void HandleInterstitialAdOnAdClosed(object sender, EventArgs args)
    {
        if (_InterstitialCallback != null)
        {
            _InterstitialCallback();
            _InterstitialCallback = null;
        }

        _interstitial = CreateInterstitialAd();
        RequestInterstitialAd(_interstitial);
    }

    // Reward
    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("!@#$ HandleRewardBasedVideoOpened event received: " + args);
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("!@#$ HandleRewardBasedVideoClosed: " + args);
        RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        //args.Amount true reward
        Debug.Log("!@#$ HandleRewardBasedVideoRewarded: " + args);
        if (_RewardVideoCallback != null)
        {
            _RewardVideoCallback();
            _RewardVideoCallback = null;
        }
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("!@#$ HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("!@#$ HandleRewardBasedVideoLoaded: " + args);
    }
}

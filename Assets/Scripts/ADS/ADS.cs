using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine;

public class ADS : MonoBehaviour, IAppodealInitializationListener, IInterstitialAdListener
{
    public static ADS Instance;

    private int _showIndex;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        int adTypes = Appodeal.INTERSTITIAL;
        string appKey = "e58401a27a71fc206c8c22d2ce65dec9d4282af02533a444";
        Appodeal.initialize(appKey, adTypes, this);
    }

    public void onInitializationFinished(List<string> errors) { }

    #region Interstitial callback handlers
    public void ShowInterstitial()
    {
        _showIndex++;

        //Debug.Log((_showIndex % 4) + "  " + (_showIndex % 4 != 0));

        if (_showIndex % 2 != 0) return;

        Debug.Log("SHOW INTER");

        if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
        {
            FirebaseAnalytics.LogEvent("INTERSTITIAL_SHOW");

            Appodeal.show(Appodeal.INTERSTITIAL);
        }
    }

    public void onInterstitialLoaded(bool isPrecache)
    {
        
    }

    public void onInterstitialFailedToLoad()
    {
        
    }

    public void onInterstitialShowFailed()
    {
        
    }

    public void onInterstitialShown()
    {
        
    }

    public void onInterstitialClosed()
    {
        
    }

    public void onInterstitialClicked()
    {
        
    }

    public void onInterstitialExpired()
    {
        
    }
    #endregion


}

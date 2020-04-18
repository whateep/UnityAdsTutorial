using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    [Header("Unity Ads settings")]
    public string gameId_ios;
    public string gameId_android;
    public bool testMode;

    private string gameId; // Set in Start()
    private const string placementId_interstitial = "video";
    private const string placementId_rewarded = "rewardedVideo";

    // For the purpose of this tutorial
    private int rewardsGiven = 0;
    public Text rewardDisplayText;

    private void Start()
    {
        // Set the correct game id depending on the current platform
        switch (Application.platform) {
            case RuntimePlatform.IPhonePlayer:
                gameId = gameId_ios;
                break;
            case RuntimePlatform.Android:
                gameId = gameId_android;
                break;
        }

        // Initialize Unity Ads
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    // Functions used to show an ad to the player
    public void ShowInterstitialAd() => StartCoroutine(WaitForAd(placementId_interstitial));
    public void ShowRewardedAd() => StartCoroutine(WaitForAd(placementId_rewarded));

    private IEnumerator WaitForAd(string placementId) {

        // Wait for an ad to be ready
        while (!Advertisement.IsReady(placementId)) yield return null;

        Advertisement.Show(placementId);
    }

    void IUnityAdsListener.OnUnityAdsReady(string placementId)
    {
        // Optional code to be executed when an ad is available
        // (e.g. notify the player with a sound or some icon...)
    }

    void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
    {
        // Optional code to be executed when an ad starts 
        // (e.g. pause the game, save progress...)
    }

    void IUnityAdsListener.OnUnityAdsDidError(string message)
    {
        // Optional code to be executed when an ad starts 
        // (e.g. log the error to the console, pop up a ui window to notify the player...)
    }


    void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Only evaluate show result if the ad was a rewarded one
        if (placementId == placementId_rewarded)
        {
            switch (showResult)
            {
                case ShowResult.Finished:
                    // Reward the player
                    rewardsGiven++;
                    rewardDisplayText.text = "Rewards: " + rewardsGiven.ToString();
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The player was not rewarded for skipping the ad.");
                    break;
                case ShowResult.Failed:
                    Debug.LogWarning("The ad did not finish due to an error.");
                    break;
            }
        }
    }
}




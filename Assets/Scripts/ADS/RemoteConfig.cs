using Firebase.Crashlytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Collections.Generic;
using UnityEngine;

public class RemoteConfig : MonoBehaviour
{
    public static RemoteConfig Instance { get; private set; }

    public bool IsLoaded { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    //private void Start()
    //{
    //    InitializeFirebaseAndStartGame();
    //}

    public Firebase.FirebaseApp app = null;
    private void InitializeFirebaseAndStartGame()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(
  previousTask =>
  {
      var dependencyStatus = previousTask.Result;
      if (dependencyStatus == Firebase.DependencyStatus.Available)
      {
          // Create and hold a reference to your FirebaseApp,
          app = Firebase.FirebaseApp.DefaultInstance;
          // Set the recommended Crashlytics uncaught exception behavior.
          Crashlytics.ReportUncaughtExceptionsAsFatal = true;
          SetRemoteConfigDefaults();
      }
      else
      {
          UnityEngine.Debug.LogError(
             $"Could not resolve all Firebase dependencies: {dependencyStatus}\n" +
             "Firebase Unity SDK is not safe to use here");
      }
  });
    }

    // Sets Remote Config default values and fetchs new ones
    // before starting the game.
    private void SetRemoteConfigDefaults()
    {
        var defaults = new Dictionary<string, object>();


        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        remoteConfig.SetDefaultsAsync(defaults).ContinueWithOnMainThread(
           previousTask =>
           {
               FetchRemoteConfig(InitializeCommonDataAndStartGame);
           }
        );
    }

    private void InitializeCommonDataAndStartGame()
    {
        IsLoaded = true;
    }

    // (Re)fetches Remote Config values and pass down the onFetchAndActivateSuccessful callback.
    // Called during the initialization flow but can also be called indepedently.
    public void FetchRemoteConfig(System.Action onFetchAndActivateSuccessful)
    {
        if (app == null)
        {
            Debug.LogError($"Do not use Firebase until it is properly initialized by calling {nameof(InitializeFirebaseAndStartGame)}.");
            return;
        }

        Debug.Log("Fetching data...");
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        remoteConfig.FetchAsync(System.TimeSpan.Zero).ContinueWithOnMainThread(
           previousTask =>
           {
               if (!previousTask.IsCompleted)
               {
                   Debug.LogError($"{nameof(remoteConfig.FetchAsync)} incomplete: Status '{previousTask.Status}'");
                   return;
               }
               ActivateRetrievedRemoteConfigValues(onFetchAndActivateSuccessful);
           });
    }


    // The final method in the initialization flow that will activate fetched values
    // and on Success will call onFetchAndActivateSuccessful.
    private void ActivateRetrievedRemoteConfigValues(System.Action onFetchAndActivateSuccessful)
    {
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus == LastFetchStatus.Success)
        {
            remoteConfig.ActivateAsync().ContinueWithOnMainThread(
               previousTask =>
               {
                   Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                   onFetchAndActivateSuccessful();
               });
        }
    }

}

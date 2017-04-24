using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class GooglePlayLogin : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // Create client configuration
        PlayGamesClientConfiguration config = new
            PlayGamesClientConfiguration.Builder()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;

        // Initialize and activate the platform
        PlayGamesPlatform.Activate();

        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate((bool success) => {
                if (success)
                {
                    /// Signed in! Hooray!
                    PlayGamesPlatform.Instance.ReportProgress(
                           "CgkIi7HoiqAfEAIQAQ",
                           100.0f, (bool successs) => {
                               Debug.Log("(Lollygagger) Welcome Unlock: " +
                             successs);
                           });
                }
                else {
                    /// Not signed in. We'll want to show a sign in button

                }
            }, true);   /// <--- That "true" is very important!
        }
        else {
            Debug.Log("We're already signed in");
        }

    }

    public void SignInCallback(bool success)
    {
        if (success)
        {
            Debug.Log("(Lollygagger) Signed in!");
        }
        else {
            Debug.Log("(Lollygagger) Sign-in failed...");
        }
    }

    public void GooglePlayStart()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // Sign in with Play Game Services, showing the consent dialog
            // by setting the second parameter to isSilent=false.
            Debug.Log("Signing in please wait!");
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        }
    }

    public void ShowLeaderBoards()
    {
        if(PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            Debug.Log("Cannot show leaderboard: not authenticated");
        }
    }

    public void ShowAchievements()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
        else
        {
            Debug.Log("Cannot show achievements: not authenticated");
        }
    }
}

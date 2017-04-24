using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class StatsMenu : MonoBehaviour {
    private string Leaderboards;
    private int currentscore;
    [SerializeField]
    private GameObject score;
    [SerializeField]
    private Text PlayerScore;
    [SerializeField]
    private Text PlayerGems;

    public void ExitLevel()
    {
        //GetComponent<PlayerShooting>().Fire();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Time.timeScale = 1;
    }
    void Start()
    {
        PlayGamesPlatform.Activate();

     switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 2:
                    Leaderboards = "CgkIi7HoiqAfEAIQBw";
                    break;
                case 3:
                    Leaderboards = "CgkIi7HoiqAfEAIQCA";
                    break;
                case 4:
                    Leaderboards = "CgkIi7HoiqAfEAIQCQ";
                    break;
                case 5:
                    Leaderboards = "CgkIi7HoiqAfEAIQCg";
                    break;
            }
    }

    public void ShowLeaderboards()
    {
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(Leaderboards, LeaderboardTimeSpan.Weekly, null);
    }

    public void SendScore(int Gems)
    {
        //currentscore = score.GetComponent<Score>().score;
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // Note: make sure to add 'using GooglePlayGames'
            PlayGamesPlatform.Instance.ReportScore(currentscore, "CgkIi7HoiqAfEAIQCQ", (bool success) =>
                {
                    Debug.Log("Leaderboard update success: " + success);
                });
        }
        //PlayerScore.text = "Current Score: " + currentscore;
        //PlayerGems.text = "Collected Gems: " + Gems;


    }
           /* PlayGamesPlatform.Instance.ReportProgress(
                 "CgkIi7HoiqAfEAIQAw",
                 100.0f, (bool success) => {
                         success);
                 });*/
            /*switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 2:
                    Leaderboards = "CgkIi7HoiqAfEAIQBw";
                    break;
                case 3:
                    Leaderboards = "CgkIi7HoiqAfEAIQCA";
                    break;
            }*/

    public void ResetLevel()
    {
       GetComponent<PlayerShooting>().Fire();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}

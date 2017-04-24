using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using UnityEngine.SocialPlatforms;

public class PlayerCollision : MonoBehaviour
{
	public int CurrentLives = 1;
	public GameObject mWallBreak;
	private bool mPowerUpWallBreak = false;
	// Use this for initialization
	public AudioClip collisionSound;
	public AudioClip deathSound;
	private AudioSource sourceColl;
	private AudioSource sourceDeath;
	[SerializeField]
	private float PhaseTime;
	[SerializeField]
	private GameObject Score;
	[SerializeField]
	private Text text;
	[SerializeField]
	private GameObject StatsMenu;
	[SerializeField]
	private Text PlayerScore;
	[SerializeField]
	private GameObject powerup1;
	[SerializeField]
	private GameObject powerup2;
	[SerializeField]
	private Sprite checkmark;
	[SerializeField]
	private Sprite emptypowerup;
    [SerializeField]
    private Text PlayerGems;
    [SerializeField]
    private Text PlayerLogin;
    [SerializeField]
    private Text debugtext;
    private ScoreManager mScoreManager;
	private float MaxPhaseTime;
	private bool PlayerHit;
	private int MaxLives;
	private Rigidbody mRigidBody;
	private GameObject WallColl;
	private int countGems;
	public Text gemText;
	private int currentscore;
	public int CrystalsShot;
	private string Leaderboards;
	private bool SuperJump = false;
	private bool DubbleJump = false;
    private bool ScoreSend = false;
    public static ILeaderboard HighScoreLeaderboard;
    public static int MyHighScore;

    void Awake ()
	{
		mScoreManager = FindObjectOfType<ScoreManager> ();
		sourceColl = GetComponent<AudioSource> ();
		sourceDeath = GetComponent<AudioSource> ();
		Physics.IgnoreLayerCollision (8, 9, true);
        
        if (!PlayGamesPlatform.Instance.localUser.authenticated) {

			PlayGamesClientConfiguration config = new
           PlayGamesClientConfiguration.Builder ()
           .Build ();
			PlayGamesPlatform.InitializeInstance (config);
			// Enable debugging output (recommended)
			PlayGamesPlatform.DebugLogEnabled = true;

			// Initialize and activate the platform
			PlayGamesPlatform.Activate ();
			// Sign in with Play Game Services, showing the consent dialog
			// by setting the second parameter to isSilent=false.
			Debug.Log ("Signing in please wait!");
			PlayGamesPlatform.Instance.Authenticate (SignInCallback, false);
		}
        PlayGamesPlatform.Activate();
    }

	public void SignInCallback (bool success)
	{
		if (success) {
			Debug.Log ("(Lollygagger) Signed in!");
		} else {
			Debug.Log ("(Lollygagger) Sign-in failed...");
		}
	}

    private void SendScore()
    {
        if (!ScoreSend)
        {
            if (PlayGamesPlatform.Instance.localUser.authenticated)
            {
                // Note: make sure to add 'using GooglePlayGames'
                PlayGamesPlatform.Instance.ReportScore(Score.GetComponent<Score>().score, Leaderboards, (bool success) =>
                {
                    debugtext.text = "Score send!!...." + Score.GetComponent<Score>().score + " " + Leaderboards;
                    StatsMenu.SetActive(true);
                    getHighScoreFromLeaderboard();
                    ShowStats();
                    ScoreSend = true;
                    
                });
            }
        }
    }

	void Start ()
	{
        Leaderboards = "CgkIi7HoiqAfEAIQCA";
        countGems = 0;
		MaxLives = CurrentLives;
		MaxPhaseTime = PhaseTime;
		mRigidBody = GetComponent<Rigidbody> ();

		switch (SceneManager.GetActiveScene ().buildIndex) {
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

	void Update ()
	{

		switch (countGems) {

		case 1:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQBA",
				20.0f, (bool success) => {
			});
			break;
		case 2:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQBA",
				40.0f, (bool success) => {
			});
			break;
		case 3:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQBA",
				60.0f, (bool success) => {
			});
			break;
		case 4:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQBA",
				80.0f, (bool success) => {
			});
			break;
		case 5:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQBA",
				100.0f, (bool success) => {
			});
			break;

		}

		switch (CrystalsShot) {

		case 1:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQAg",
				20.0f, (bool success) => {
				Debug.Log ("(Lollygagger) Welcome Unlock: " +
				success);
			});
			break;
		case 2:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQAg",
				40.0f, (bool success) => {
				Debug.Log ("(Lollygagger) Welcome Unlock: " +
				success);
			});
			break;
		case 3:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQAg",
				60.0f, (bool success) => {
				Debug.Log ("(Lollygagger) Welcome Unlock: " +
				success);
			});
			break;
		case 4:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQAg",
				80.0f, (bool success) => {
				Debug.Log ("(Lollygagger) Welcome Unlock: " +
				success);
			});
			break;
		case 5:
			PlayGamesPlatform.Instance.ReportProgress (
				"CgkIi7HoiqAfEAIQAg",
				100.0f, (bool success) => {
				Debug.Log ("(Lollygagger) Welcome Unlock: " +
				success);
			});
			break;

		}


		mRigidBody.transform.localEulerAngles = new Vector3 (0, 90, 0);
		if (PlayerHit) {
			PhaseTime -= Time.deltaTime;
			if (PhaseTime <= 0) {
				PlayerHit = false;

			}
		}


		if (mPowerUpWallBreak) {
			mWallBreak.transform.position = this.transform.position;

		}

        if (CurrentLives <= 0)
        {
            SendScore();
            getHighScoreFromLeaderboard();
            //ShowStats();
        }
    }

	public void AddScore (int scoreToAdd)
	{
		if (scoreToAdd != 0) {
			Score.GetComponent<Score> ().score 
                += scoreToAdd;
			mScoreManager.UpdateScore (scoreToAdd);
		}
	}

	public void HandleSuperJump ()
	{
		if (SuperJump) {
			GetComponent<Rigidbody> ().AddForce (new Vector3 (900, 0, 0));
			SuperJump = false;
			powerup1.GetComponent<Image> ().sprite = emptypowerup;
		}
		if (DubbleJump) {
			GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 400, 0));
			DubbleJump = false;
			powerup2.GetComponent<Image> ().sprite = emptypowerup;
		}
	}

    private void getHighScoreFromLeaderboard()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            string[] UserFilter = { Social.localUser.id }; // Get the local player username
            HighScoreLeaderboard = PlayGamesPlatform.Instance.CreateLeaderboard(); // initiate 
            HighScoreLeaderboard.id = Leaderboards; // The string ID from Google console
            HighScoreLeaderboard.SetUserFilter(UserFilter); // Filter out other players
            HighScoreLeaderboard.LoadScores(RWResult =>
            {
                if (RWResult)
                {
                    debugtext.text = "found score: " + (int)HighScoreLeaderboard.localUserScore.value;
                    PlayerScore.text = " Current highscore: " + (int)HighScoreLeaderboard.localUserScore.value;
                    ShowStats();
                }
                else
                {
                    debugtext.text = "Could not get score...";
                    ShowStats();
                }
            });
        }
        else
        {
            ShowStats();
        }
    }


    public void ShowStats()
    {

        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {

            /* PlayGamesPlatform.Instance.LoadScores(
                 leaderboards,
                     LeaderboardStart.PlayerCentered,
                     1,
                     LeaderboardCollection.Public,
                     LeaderboardTimeSpan.Weekly,
                  (LeaderboardScoreData data) =>
                  {
                      Debug.Log(data.Valid);
                      Debug.Log(data.Id);
                      Debug.Log(data.PlayerScore);
                      Debug.Log(data.PlayerScore.userID);
                      Debug.Log(data.PlayerScore.formattedValue);
                      StatsMenu.SetActive(true);
                      PlayerScore.text =" Current highscore: " + data.PlayerScore.formattedValue;
                  //((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(data.Id, LeaderboardTimeSpan.Weekly, null);
              });*/
            
            StatsMenu.SetActive(true);
            PlayerGems.text = " Gems collected: " + countGems;
            
            if ((int)HighScoreLeaderboard.localUserScore.value < Score.GetComponent<Score>().score)
            {
                PlayerScore.text = " Current highscore: " + Score.GetComponent<Score>().score;
            }
        }
        else
        {
            StatsMenu.SetActive(true);
            PlayerLogin.text = "Not signed in. Please sign in to save your scores.";
        }
        if (!StatsMenu.activeInHierarchy)
        {
            StatsMenu.SetActive(true);
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(Leaderboards, LeaderboardTimeSpan.Weekly, null);
        }
        Time.timeScale = 1;


    }

    // Update is called once per frame
    void OnTriggerEnter (Collider coll)
	{
		if (coll.gameObject.CompareTag ("HitTrap")) {
			AddScore (-100);
			coll.gameObject.SetActive (false);
		}

		if (coll.gameObject.CompareTag ("Heart")) {
			CurrentLives++;
			coll.gameObject.SetActive (false);
		}

		if (coll.gameObject.CompareTag ("Gem1")) {
			countGems = countGems + 1;
			AddScore (10);
			setGemText ();
			coll.gameObject.SetActive (false);
		}

		if (coll.gameObject.CompareTag ("Gem5")) {
			countGems = countGems + 5;
			AddScore (50);
			setGemText ();
			coll.gameObject.SetActive (false);
		}
		if (coll.gameObject.CompareTag ("SuperJump")) {
			SuperJump = true;
			powerup1.GetComponent<Image> ().sprite = checkmark;
			coll.gameObject.SetActive (false);
		}
		if (coll.gameObject.CompareTag ("DubbleJump")) {
			DubbleJump = true;
			powerup2.GetComponent<Image> ().sprite = checkmark;
			coll.gameObject.SetActive (false);
		}

		if (coll.gameObject == mWallBreak) {
			mWallBreak.transform.localScale = mWallBreak.transform.localScale * 2;
			mPowerUpWallBreak = true;
		}

		if (coll.gameObject.CompareTag ("Coin")) {
            //StatsMenu.GetComponent<StatsMenu> ().SendScore (countGems);
            SendScore();
            getHighScoreFromLeaderboard();

        }

		if (coll.gameObject.CompareTag ("Wall")) {
			if (!mPowerUpWallBreak) {
				CurrentLives--;
				sourceColl.PlayOneShot (collisionSound);
			} else {
				mPowerUpWallBreak = false;
				mWallBreak.gameObject.SetActive (false);
			}
			coll.gameObject.SetActive (false);
			Debug.Log (CurrentLives);
		}
	}

	public int MDefenseLevel {
		get {
			return PlayerPrefs.GetInt ("defense_level", 0);
		}
		set {
			PlayerPrefs.SetInt ("defense_level", value);
		}
	}

	public int MUtilityLevel {
		get {
			return PlayerPrefs.GetInt ("utility_level", 0);
		}
		set {
			PlayerPrefs.SetInt ("utility_level", value);
		}
	}

	public void HitCrystal ()
	{
		if (!mPowerUpWallBreak) {
			AddScore (-300);
			sourceColl.PlayOneShot (collisionSound);
		} else {
			mPowerUpWallBreak = false;
			mWallBreak.gameObject.SetActive (false);
		}
	}

	void setGemText ()
	{
		gemText.text = "Gems: " + countGems.ToString ();
	}

	void  OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.CompareTag ("DeathTrap")) {
            if (CurrentLives > 0)
            {
                CurrentLives -= 1;
            }
            //StatsMenu.GetComponent<StatsMenu>().SendScore(countGems);
            //getHighScoreFromLeaderboard();
        }
		if (coll.gameObject.CompareTag ("FloorOfDeath")) {
            if (CurrentLives > 0)
            {
                CurrentLives -= 1;
            }
            //StatsMenu.GetComponent<StatsMenu>().SendScore(countGems);
            //getHighScoreFromLeaderboard();
        }


		if (coll.gameObject.tag == "Dropject") {
			Physics.IgnoreCollision (coll.collider, GetComponent<Collider> ());
		}
		if (!PlayerHit) {
			if (coll.gameObject.CompareTag ("WallSpike")) {
				AddScore (-100);
				PlayerHit = true;
				PhaseTime = MaxPhaseTime;
				WallColl = coll.gameObject;
				this.transform.position -= new Vector3 (2, 0, 0);
			}
		}

	


		if (coll.gameObject.CompareTag ("Wall")) {
			if (!mPowerUpWallBreak) {
				CurrentLives--;
				sourceColl.PlayOneShot (collisionSound);
			} else {
				mPowerUpWallBreak = false;
				mWallBreak.gameObject.SetActive (false);
			}
			coll.gameObject.SetActive (false);
			Debug.Log (CurrentLives);
		}
			
	}
}

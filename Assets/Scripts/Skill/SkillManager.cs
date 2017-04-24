using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillManager : MonoBehaviour
{
	private static string SCORE = "player_score";
	private static string POINTS = "player_points";

	[SerializeField] private float mMaxSkillPoints = 1.00f;
	[SerializeField] private float mUpdatePoints = 0.1f;
	[SerializeField] private float mMaxUpdateTime = 2f;
	private float mUpdateTime = 0f;
	private bool mStart = false;

	[SerializeField] private GameObject mSkillObject;

	private GameObject mSkillObjectInstance;
	private Image mSkillFlued;
	private Text mSkillPoints;

	// Use this for initialization
	void Awake ()
	{
		GetComponents ();
	}

	void Start ()
	{
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.SetFloat (POINTS, 5);
		mSkillObjectInstance.SetActive (false);
		mSkillFlued = mSkillObjectInstance.transform.FindChild ("Bar").transform.FindChild ("Flued").GetComponent<Image> ();
		mSkillPoints = mSkillObject.transform.FindChild ("Icon").transform.FindChild ("Icon").GetComponent<Text> ();
	}

	void Update ()
	{
		if (mStart) {
			mUpdateTime += Time.deltaTime;
		}

		if (mUpdateTime >= mMaxUpdateTime) {
			mUpdateTime = 0f;
			mSkillObjectInstance.SetActive (false);
			mStart = false;
		}

		ScoreChecker ();
	}

	void GetComponents ()
	{
		mSkillObjectInstance = Instantiate (mSkillObject, mSkillObject.transform.position, Quaternion.identity) as GameObject;
		mSkillObjectInstance.transform.SetParent (transform, false);
	}

	public void UpdateExp ()
	{
		float points = PlayerPrefs.GetFloat (SCORE);
		points += mUpdatePoints;
		PlayerPrefs.SetFloat (SCORE, points);
		ShowSkillObject ();
	}

	void ScoreChecker ()
	{
		if (PlayerPrefs.GetFloat (SCORE) >= mMaxSkillPoints) {
			PlayerPrefs.SetFloat (POINTS, (PlayerPrefs.GetFloat (POINTS) + 1));
			PlayerPrefs.SetFloat (SCORE, 0);
		}
	}

	void ShowSkillObject ()
	{
		mSkillObjectInstance.SetActive (true);
		mSkillFlued.fillAmount = PlayerPrefs.GetFloat (SCORE);
		mSkillPoints.text = PlayerPrefs.GetFloat (POINTS).ToString ();
		mStart = true;
	}
}

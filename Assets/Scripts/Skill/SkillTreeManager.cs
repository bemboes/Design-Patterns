using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class SkillTreeManager : MonoBehaviour, OnUpgradeListener<GameObject>
{
	private enum Skill
	{
		mUtitility,
		mDefense,
	}

	//Sprite
	[SerializeField] private Sprite mCheckSprite;

	//Prefabs
	[SerializeField] private GameObject mInformationWindow;
	[SerializeField] private GameObject mToastWindow;
	[SerializeField] private GameObject mSkillWindow;

	//Instances of the prefabs
	private GameObject mInformationInstance;
	private GameObject mToastInstance;
	private GameObject mSkillInstance;

	// Use this for initialization
	void Start ()
	{
		PlayerPrefs.DeleteAll ();
		MGlobalLevel = 5;
		ShowSkillWindow ();
		UpdateSkillObjectsPerSkill (Skill.mUtitility);
		UpdateSkillObjectsPerSkill (Skill.mDefense);
	}

	void Awake ()
	{
		InstantiatePrefabs ();
	}

	void InstantiatePrefabs ()
	{
		mSkillInstance = Instantiate (mSkillWindow) as GameObject;
		mSkillInstance.transform.SetParent (gameObject.transform);
		mSkillInstance.transform.localPosition = mSkillWindow.transform.position;
		mSkillInstance.transform.localRotation = mSkillWindow.transform.rotation;
		mSkillInstance.SetActive (false);

		mInformationInstance = Instantiate (mInformationWindow) as GameObject;
		mInformationInstance.transform.SetParent (gameObject.transform);
		mInformationInstance.transform.localPosition = mInformationWindow.transform.position;
		mInformationInstance.transform.localRotation = mInformationWindow.transform.rotation;
		mInformationInstance.GetComponent<Window> ().MOnUpgradeListener = this;
		mInformationInstance.SetActive (false);

		mToastInstance = Instantiate (mToastWindow) as GameObject;
		mToastInstance.transform.SetParent (gameObject.transform);
		mToastInstance.transform.localPosition = mToastWindow.transform.position;
		mToastInstance.transform.localRotation = mToastWindow.transform.rotation;
		mToastInstance.SetActive (false);
	}

	private void UpdateSkillObjectsPerSkill (Skill skill)
	{
		int size = 0;
		string name = null;
		switch (skill) {
		case Skill.mDefense:
			size = MDefenseLevel;
			name = "Utility_";
			break;
		case Skill.mUtitility:
			size = MUtilityLevel;
			name = "Defense_";
			break;
		}

		for (int i = 0; i < size; i++) {
			transform
				.FindChild ("Tree")
				.FindChild (name + (i + 1).ToString ()).gameObject.transform
				.FindChild ("Check").gameObject
				.GetComponent<Image> ().sprite = mCheckSprite;
		}
	}

	private void UpdateSkillObject (GameObject objectToProcess)
	{
		objectToProcess.transform
			.FindChild ("Check").gameObject
			.GetComponent<Image> ().sprite = mCheckSprite;
	}

	private void DecreaseGlobalLevel ()
	{
		int points = MGlobalLevel;
		points--;
		MGlobalLevel = points;
		ShowSkillWindow ();
	}

	private void IncreaseSkillLevel (Skill skill)
	{
		int points = GetSkillLevel (skill);
		points++;
		switch (skill) {
		case Skill.mDefense:
			MDefenseLevel = points;
			break;
		case Skill.mUtitility:
			MUtilityLevel = points;
			break;
		}
	}

	private string GetRightTitle (GameObject clickedObject)
	{
		switch (clickedObject.name) {
		case "Utility_1":
			return "Slow down";
		case "Utility_2":
			return "Drop chance";
		case "Utility_3":
			return "Drop chance";
		case "Utility_4":
			return "Crystal worth";
		case "Defense_1":
			return "Damage reduction";
		case "Defense_2":
			return "Damage dodging";
		case "Defense_3":
			return "Damage reduction";
		default:
			return null;
		}
	}

	private string GetRightText (GameObject clickedObject)
	{
		switch (clickedObject.name) {
		case "Utility_1":
			return "Slow player down by 50% for 1 second after being hit.";
		case "Utility_2":
			return "Drop chance of rare items increased by 5%.";
		case "Utility_3":
			return "Drop chance of rare items increased by 5%.";
		case "Utility_4":
			return "Crystals are worth 10% more score.";
		case "Defense_1":
			return "Damage reduction by 10%";
		case "Defense_2":
			return "Gives a 5% chance of dodging the damage when being hit.";
		case "Defense_3":
			return "Damage reduction by 10%";
		default:
			return null;
		}
	}

	//	================================================================================
	//	GUI Showers
	//	================================================================================
	private void ShowInformationWindow (GameObject clickedObject)
	{
		mInformationInstance
			.GetComponent<Window> ().MClickedSkill = clickedObject;
		mInformationInstance
			.GetComponent<Window> ().MWindowTitle = GetRightTitle (clickedObject);
		mInformationInstance
			.GetComponent<Window> ().MWindowText = GetRightText (clickedObject);
		mInformationInstance.SetActive (true);
	}

	private IEnumerator ShowToastWindow (string textToShow)
	{
		mToastInstance.transform
			.FindChild ("Message").gameObject
			.GetComponent<Text> ().text = textToShow;
		mToastInstance.SetActive (true);
		yield return new WaitForSeconds (1);
		mToastInstance.SetActive (false);
	}

	private void ShowSkillWindow ()
	{
		mSkillInstance.transform
			.FindChild ("Bar")
			.FindChild ("Flued").gameObject
			.GetComponent<Image> ().fillAmount = MGlobalScore;
		mSkillInstance.transform
			.FindChild ("Icon")
			.FindChild ("Icon").gameObject
			.GetComponent<Text> ().text = MGlobalLevel.ToString ();
		if (!mSkillInstance.activeSelf) {
			mSkillInstance.SetActive (true);
		}
	}

	//	================================================================================
	//	Checks
	//	================================================================================
	private bool IsGlobalLevelHighEnough (int costInGlobalLevels)
	{
		if (MGlobalLevel >= costInGlobalLevels) {
			return true;
		} else {
			StartCoroutine (ShowToastWindow ("You do not have enough talent points to upgrade this talent."));
			return false;
		}
	}

	private bool IsSkillUnlockable (Skill skillToProcess, GameObject objectToProcess)
	{
		int skillLevel = GetSkillLevel (skillToProcess);
		int objectLevel = GetObjectLevel (objectToProcess);

		Debug.Log ("skillLevel: " + skillLevel + " | objectLevel: " + objectLevel);

		if (skillLevel == objectLevel) {
			Debug.Log ("They are equal so we can upgrade");
			return true;
		} else if (skillLevel > objectLevel) {
			Debug.Log ("My level is higher than the upgrade I'm trying to do. So I already unlocked this one");
		} else {
			Debug.Log ("My level is lower than the upgrade I'm trying to do. So I need to do another upgrade first");
			StartCoroutine (ShowToastWindow ("You need another upgrade first"));
		}
		return false;
	}

	//	================================================================================
	//	Custom Getters
	//	================================================================================
	private int GetSkillLevel (Skill skillToProcess)
	{
		switch (skillToProcess) {
		case Skill.mDefense:
			return MDefenseLevel;
		case Skill.mUtitility:
			return MUtilityLevel;
		default:
			return -1;
		}
	}

	private int GetObjectLevel (GameObject objectToProcess)
	{
		int number = 0;
		string[] numbers = Regex.Split (objectToProcess.name, @"\D+");
		foreach (string value in numbers) {
			if (!string.IsNullOrEmpty (value)) {
				number = int.Parse (value);
			}
		}
		number--;
		return number;
	}
		
	//	================================================================================
	//	On Click Listeners
	//	================================================================================
	public void DefenseClick (GameObject clickedObject)
	{
		if (IsSkillUnlockable (Skill.mDefense, clickedObject)) {
			if (IsGlobalLevelHighEnough (1)) {
				mInformationInstance
					.GetComponent<Window> ().MSkill = Window.Skill.mDefense;
				ShowInformationWindow (clickedObject);
			}
		}
	}

	public void UtilityClick (GameObject clickedObject)
	{
		if (IsSkillUnlockable (Skill.mUtitility, clickedObject)) {
			if (IsGlobalLevelHighEnough (1)) {
				mInformationInstance
					.GetComponent<Window> ().MSkill = Window.Skill.mUtitility;
				ShowInformationWindow (clickedObject);
			}
		}
	}

	//	================================================================================
	//	Callbacks
	//	================================================================================

	public void OnUtilitySkillClicked (GameObject clickedObject)
	{
		//Update the checkmark sprite
		UpdateSkillObject (clickedObject);
		//Remove a global level
		DecreaseGlobalLevel ();
		//Update skill level
		IncreaseSkillLevel (Skill.mUtitility);
		//Tell the user about it
		StartCoroutine (ShowToastWindow ("You upgraded your utitlity talent"));
	}

	public void OnDefenseSkillClicked (GameObject clickedObject)
	{
		//Update the checkmark sprite
		UpdateSkillObject (clickedObject);
		//Remove a global level
		DecreaseGlobalLevel ();
		//Update skill level
		IncreaseSkillLevel (Skill.mDefense);
		//Tell the user about it
		StartCoroutine (ShowToastWindow ("You upgraded your defensive talent"));
	}

	//	================================================================================
	//	Getters & Setters
	//	================================================================================
	public float MGlobalScore {
		get {
			return PlayerPrefs.GetFloat ("player_score", 0f);
		}
		set {
			PlayerPrefs.SetFloat ("player_score", value);
		}
	}

	public int MGlobalLevel {
		get {
			return PlayerPrefs.GetInt ("player_level", 0);
		}
		set {
			PlayerPrefs.SetInt ("player_level", value);
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
}

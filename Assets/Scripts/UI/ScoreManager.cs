using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	//Variables
	public float mScore;

	[SerializeField]private float mAnimationDuration = 2f;
	[SerializeField]private float mMaxTextSize = 100f;
	[SerializeField]private float mMinTextSize = 25f;

	//Components
	private Text mTotalScoreText;

	[SerializeField]private Canvas mCanvas;
	[SerializeField]private GameObject mScorePrefab;

	// Use this for initialization
	private void Awake ()
	{

		GetComponents ();
	}

	private void GetComponents ()
	{
		
		mTotalScoreText = GetComponent<Text> ();
	}

	private void Start ()
	{
		mScore = 0f;
	}

	public float CurrentScore {
		get{ return mScore; }
	}

	public void UpdateScore (float score)
	{
		//Instantiate
		GameObject scoreObject = Instantiate (mScorePrefab, mScorePrefab.transform.position, mScorePrefab.transform.rotation) as GameObject;
		//Set canvas as parent
		scoreObject.transform.SetParent (mCanvas.transform, false);
		//Get text component from prefab
		Text text = scoreObject.GetComponent<Text> ();
		//Start text animation
		StartCoroutine (AnimateText (scoreObject, Random.ColorHSV (), score));
	}

	private IEnumerator AnimateText (GameObject scoreObject, Color bulletColor, float score)
	{
		//Get text component from prefab
		Text text = scoreObject.GetComponent<Text> ();

        //Handle the text
        if (score > 0)
        {
            text.text = "+" + score.ToString() + "!";
        }
        else
        {
            text.text = "" + score.ToString() + "!";
        }

		//Get the positions
		Vector3 startingPos = text.transform.position;
		Vector3 endPos = this.transform.position;

		//Get the timers
		float duration = mAnimationDuration;
		float elapsedTime = 0f;

		//Get the sizes
		bool hasReachedMax = false;

		while (elapsedTime < duration) {
			text.transform.position = Vector3.Lerp (startingPos, endPos, (elapsedTime / duration));
			text.color = Color.Lerp (bulletColor, Color.clear, (elapsedTime / duration));
			while (text.fontSize != mMaxTextSize && !hasReachedMax) {
				text.fontSize = (int)Mathf.Lerp (mMinTextSize, mMaxTextSize, (elapsedTime / (duration / 2)));
				hasReachedMax = true;
			}
			text.fontSize = (int)Mathf.Lerp (mMaxTextSize, mMinTextSize, (elapsedTime / (duration)));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		//Destroy the instance
		Destroy (scoreObject, 0f);

		//Update the score
		mScore += score;

		//Update the total amount
		mTotalScoreText.text = "Score:   " + mScore.ToString ();
	}
}

using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
	//Variables
	public float speed = 90f;
	public float hoverForce = 65f;
	public float hoverHeight = 3.5f;

	//Components
	private Rigidbody mRigidbody;
	private SkillManager mSkillManager;
	private ScoreManager mScoreManager;
	private Canvas mCanvas;

	[SerializeField] private GameObject mExplosion;


	void Awake ()
	{
		GetComponents ();

		mRigidbody.AddRelativeForce (0f, 0f, speed * 10f);
	}

	private void Explode ()
	{
		GameObject explosion = Instantiate (mExplosion, transform.position, Quaternion.identity) as GameObject;
		Destroy (explosion, 1f);
	}

	private void GetComponents ()
	{
		mRigidbody = GetComponent<Rigidbody> ();
		mScoreManager = FindObjectOfType<ScoreManager> ();
		mCanvas = FindObjectOfType (typeof(Canvas)) as Canvas;
		mSkillManager = mCanvas.GetComponent<SkillManager> ();
	}


	public void Kill (float scoreToAdd)
	{
		Explode ();
		if (scoreToAdd != 0) {
			mScoreManager.UpdateScore (scoreToAdd);
		}
		mSkillManager.UpdateExp ();
		Destroy (gameObject, 0f);
	}

	void AddHoverBehaviour ()
	{
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, hoverHeight)) {
			float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
			mRigidbody.AddForce (appliedHoverForce, ForceMode.Acceleration);
		}

		mRigidbody.AddRelativeForce (0f, 0f, speed);
	}

	void FixedUpdate ()
	{
		AddHoverBehaviour ();
	}
}
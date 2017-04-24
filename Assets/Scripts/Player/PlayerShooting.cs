using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
	//Variables
	[SerializeField] private float mCooldownSeconds;

	private float mCurrentCooldown;

	//Components
	[SerializeField] private GameObject mBullet;
	[SerializeField] private GameObject mGunPoint;

	//Sound
	public AudioClip shootSound;
	private AudioSource sourceShot;


	//Components
	private Rigidbody mRigidBody;

	void Awake ()
	{
		mCurrentCooldown = mCooldownSeconds;
		sourceShot = GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start ()
	{
		//Get the components of the object
		GetComponents ();
		TouchInput.OnShoot += Fire;
	}

	private void GetComponents ()
	{
		mRigidBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate ()
	{
		mCurrentCooldown += Time.deltaTime;
	}

	public void Fire ()
	{
		if (mCurrentCooldown >= mCooldownSeconds) {
            //sourceShot.PlayOneShot (shootSound);
            GetComponent<PlayerCollision>().AddScore(-20);
			GameObject bullet = Instantiate (mBullet, mGunPoint.transform.position, mBullet.transform.rotation) as GameObject;
			Destroy (bullet, 2f);
			mCurrentCooldown = 0f;
            
		}
	}
}

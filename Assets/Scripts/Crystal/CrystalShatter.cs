using UnityEngine;
using System.Collections;

public class CrystalShatter : MonoBehaviour
{
	//GameObjects
	[SerializeField] private GameObject mShatterPiece;
	[SerializeField] private GameObject mExplosionPoint;
	[SerializeField] private GameObject mNormalGem;
	[SerializeField] private GameObject mRareGem;
	[SerializeField] private GameObject mPlayer;

	private GameObject[] mShatterPieceArray;

	//Components
	private MeshRenderer mMeshRenderer;

	//Variables
	[SerializeField] private int mShatterPieceCount;
	[SerializeField] private float mExplosionPower;
    [SerializeField] private bool SpecialCrystal;
	private Color mCrystalColor;
    [SerializeField]private int rarity =0;

	void Awake ()
	{
		//Initializing
		GetComponents ();
	}

	void Start ()
	{
		//Handlers
		HandleCrystalColor ();
		HandleCrystalShatter ();

        if(SpecialCrystal)
        {
            HandleSpecialCrystal ();
        }
	}

	private void GetComponents ()
	{
		mMeshRenderer = GetComponent<MeshRenderer> ();
	}

	private void HandleCrystalShatter ()
	{
		mShatterPieceArray = new GameObject[mShatterPieceCount];
	}

	private void HandleCrystalColor ()
	{
		//mCrystalColor = Random.ColorHSV ();
		//mMeshRenderer.materials [0].color = mCrystalColor;
	}

    private void HandleSpecialCrystal()
    {
        float roll = Random.Range(0, 100);
        
        //legendary 1%
        if(roll >= 0 && roll < 1)
        {
            rarity = 4;
            mMeshRenderer.materials[0].color = new Color(1, 0.498f, 0, 1);
        }
        //epic  4%
        else if (roll >= 2 && roll < 5)
        {
            rarity = 3;
            mMeshRenderer.materials[0].color = Color.magenta;
        }
        //rare  15%
        else if(roll >= 5 && roll < 21)
        {
            rarity = 2;
            //30-144-255
            mMeshRenderer.materials[0].color = new Color32(30, 144, 255, 255);
        }
        //uncommon  20%
        else if(roll >= 21 && roll < 41)
        {
            rarity = 1;
            mMeshRenderer.materials[0].color = Color.green;
        }
        //common    60%
        else if(roll >= 41 && roll <= 100)
        {
            rarity = 0;
        }

    }

	private void DestroyCrystal ()
	{
		for (int i = 0; i < mShatterPieceArray.Length; i++) {
			mShatterPieceArray [i] = Instantiate (mShatterPiece, transform.position, Random.rotation) as GameObject;
			Rigidbody rigidBody = mShatterPieceArray [i].GetComponent<Rigidbody> ();
			MeshRenderer meshRenderer = mShatterPieceArray [i].GetComponent<MeshRenderer> ();
			rigidBody.AddExplosionForce (mExplosionPower, mExplosionPoint.transform.position, 5, 1, ForceMode.Impulse);
			meshRenderer.materials [0].color = mCrystalColor;
			Physics.IgnoreCollision (mShatterPieceArray [i].GetComponent<Collider> (), mPlayer.GetComponent<Collider> ());
			Destroy (mShatterPieceArray [i], 2f);
		}
		Destroy (gameObject);
	}

	private void DropGem ()
	{
		int dropChance = Random.Range (0, 9);
		GameObject instance;

		// 1/10 chance to drop rare
		// 3/10 chance to drop normal
		switch (dropChance) {
		case 2:
			instance = Instantiate (mNormalGem, transform.position, Random.rotation) as GameObject;
			break;
		case 5:
			instance = Instantiate (mRareGem, transform.position, Random.rotation) as GameObject;
			break;
		case 7:
			instance = Instantiate (mNormalGem, transform.position, Random.rotation) as GameObject;
			break;
		case 9:
			instance = Instantiate (mNormalGem, transform.position, Random.rotation) as GameObject;
			break;
		default:
			//Drop nothing

			instance = null;
			break;
		}

		if (instance != null) {
			Destroy (instance, 2f);
		}
	}

	void OnTriggerEnter (Collider collision)
	{
		switch (collision.tag) {
		case "Bullet":
            DestroyCrystal ();
			DropGem ();
			

                switch(rarity)
                {
                    case 0:
                        mPlayer.GetComponent<PlayerCollision>().AddScore(100);
                        collision.gameObject.GetComponent<Rocket>().Kill(100);
                        break;
                    case 1:
                        mPlayer.GetComponent<PlayerCollision>().AddScore(200);
                        collision.gameObject.GetComponent<Rocket>().Kill(200);
                        break;
                    case 2:
                        mPlayer.GetComponent<PlayerCollision>().AddScore(300);
                        collision.gameObject.GetComponent<Rocket>().Kill(300);
                        break;
                    case 3:
                        mPlayer.GetComponent<PlayerCollision>().AddScore(500);
                        collision.gameObject.GetComponent<Rocket>().Kill(500);
                        break;
                    case 4:
                        mPlayer.GetComponent<PlayerCollision>().AddScore(1500);
                        collision.gameObject.GetComponent<Rocket>().Kill(1500);
                        break;
                }
                
			break;
		case "Player":
			DestroyCrystal ();
			collision.gameObject.GetComponent<PlayerCollision> ().HitCrystal ();
			break;
		}
	}
}

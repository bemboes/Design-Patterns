using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	//Variables

    public float jumpSpeed = 10f;
    [SerializeField]
    private float mPlayerSpeed = 1f;
    private bool mGroundCheck = false, mRollCheck = false;
    private Vector3 mOriginalPlayerScale, mOriginalPlayerVelocity;
    

    //Components
    private Rigidbody mRigidBody;

	// Use this for initialization
	void Start ()
	{
		//Get the components of the object
		GetComponents ();
    }

	private void GetComponents ()
	{
		mRigidBody = GetComponent<Rigidbody> ();
        
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (!mRollCheck) 
        {
            mRigidBody.velocity = new Vector2(mPlayerSpeed, mRigidBody.velocity.y);
        }
        else
        {
            mRigidBody.velocity = new Vector2(mPlayerSpeed * 2f, mRigidBody.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Space) && mGroundCheck)
        {
            mRigidBody.velocity = new Vector2(mPlayerSpeed, 1 * jumpSpeed) ;
            mGroundCheck = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl) && !mRollCheck)
        {
            mOriginalPlayerScale = this.transform.localScale;
            mOriginalPlayerVelocity = mRigidBody.velocity;

            mRigidBody.velocity = new Vector2(mPlayerSpeed * 2f, mRigidBody.velocity.y);
            this.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            mRollCheck = true;
            StartCoroutine(Rolling());
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ground"){mGroundCheck = true;}
    }

    IEnumerator Rolling()
    {
        yield return new WaitForSeconds(1.5f);
        this.transform.localScale = mOriginalPlayerScale;
        mRigidBody.velocity = mOriginalPlayerVelocity;
        mRollCheck = false;
    }

}

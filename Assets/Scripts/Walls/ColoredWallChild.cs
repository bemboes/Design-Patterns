using UnityEngine;
using System.Collections;

public class ColoredWallChild : MonoBehaviour
{
	public int ColorNr;

	[SerializeField]
	private GameObject Other;
	[SerializeField]
	private GameObject Parent;
	[SerializeField]
	private GameObject Player;
	private bool NoColor = true;


	// Use this for initialization
	void Start ()
	{
		//random number to determine the color
		ColorNr = Random.Range (1, 4);
		//while loop to prevent the object from having no color
		while (NoColor) {
			//switch with ColorNr to detirme the color
			switch (ColorNr) {
			case 1:
                    //check if the other obeject doesn't have the same color
				if (Other.GetComponent<Renderer> ().material.color != Color.red) {
					gameObject.GetComponent<Renderer> ().material.color = Color.red;
					NoColor = false;
				} else {
					//If the color of the other object is the same a new random color will be generated
					ColorNr = Random.Range (1, 4);
				}
				break;
			case 2:
				if (Other.GetComponent<Renderer> ().material.color != Color.blue) {
					gameObject.GetComponent<Renderer> ().material.color = Color.blue;
					NoColor = false;
				} else {
					ColorNr = Random.Range (1, 4);
				}
				break;
			case 3:
				if (Other.GetComponent<Renderer> ().material.color != Color.yellow) {
					gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					NoColor = false;
				} else {
					ColorNr = Random.Range (1, 4);
				}
				break;

			}
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnCollisionEnter (Collision collision)
	{
		//check if there is only one child left in the parent object.
      
		//if it is the only child a collison check will take place for the player
		if (collision.collider.CompareTag ("Player")) {
			//if colliding with the player the wall deactivates itself and subtracts the LivesOnHit from CurrentLives.
			gameObject.SetActive (false);
		}  

	}

	void OnTriggerEnter (Collider coll)
	{
		if (this.gameObject.activeInHierarchy) {




			//collsion check for bullet
			if (coll.gameObject.CompareTag ("Bullet")) {
				Debug.Log ("collides");
				//checks if on of the childs is active to shoot at.

				//Switch to checks if the color of the first child matches the color of the bullet.
				switch (ColorNr) {
				case 1:
					if (coll.gameObject.GetComponent<SpriteRenderer> ().color == Color.red) {
						Debug.Log ("colorcheck");
						//if color matches the bullet and the wall child get deactivated
						this.gameObject.SetActive (false);
						coll.gameObject.GetComponent <Rocket> ().Kill (100);
					}
					break;
				case 2:
					if (coll.gameObject.GetComponent<SpriteRenderer> ().color == Color.blue) {
						this.gameObject.SetActive (false);
						coll.gameObject.GetComponent <Rocket> ().Kill (100);
					}
					break;
				case 3:
					if (coll.gameObject.GetComponent<SpriteRenderer> ().color == Color.yellow) {
						this.gameObject.SetActive (false);
						coll.gameObject.GetComponent <Rocket> ().Kill (100);
					}
					break;

				}


				if (coll.gameObject.activeInHierarchy) {
					//even if color doesn't match the bullet will get geactivated.
					coll.gameObject.GetComponent <Rocket> ().Kill (0);
				}
			}

		}
	}

}

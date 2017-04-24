using UnityEngine;
using System.Collections;

public class ColoredWall : MonoBehaviour
{
	public int ColorNr;

	[SerializeField]
	private GameObject player;
	// Use this for initialization
	void Start ()
	{
		//random number to assign random color
		ColorNr = Random.Range (1, 4);
		//switch to determine the color of the object
		switch (ColorNr) {
		case 1:
			gameObject.GetComponent<Renderer> ().material.color = Color.red;
			break;
		case 2:
			gameObject.GetComponent<Renderer> ().material.color = Color.blue;
			break;
		case 3:
			gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
			break;

		}
	}

	// Update is called once per frame
	void Update ()
	{
	}

	void OnTriggerEnter (Collider collision)
	{

		//collision check for bullet
		if (collision.gameObject.CompareTag ("Bullet")) {
			Debug.Log ("collision");

			//switch to match the color of the bullet with the color of the wall
			switch (ColorNr) {
			case 1:
				if (collision.gameObject.GetComponent<SpriteRenderer> ().color == Color.red) {
					gameObject.SetActive (false);
					collision.gameObject.GetComponent <Rocket> ().Kill(250);
				}
				break;
			case 2:
				if (collision.gameObject.GetComponent<SpriteRenderer> ().color == Color.blue) {
					gameObject.SetActive (false);		
					collision.gameObject.GetComponent <Rocket> ().Kill(250);
				}
				break;
			case 3:
				if (collision.gameObject.GetComponent<SpriteRenderer> ().color == Color.yellow) {
					gameObject.SetActive (false);
					collision.gameObject.GetComponent <Rocket> ().Kill(250);
				}
				break;

			}

			//when collliding with a bullet it deactivates te bullet on collision
			if (collision.gameObject.activeInHierarchy) {
				collision.gameObject.GetComponent <Rocket> ().Kill(0);
			}
		}
	}
}

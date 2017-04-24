using UnityEngine;
using System.Collections;

public class FixedColoredWall : MonoBehaviour
{
	/* ColorNr word de kleur mee bepaald 
     * red 
     * blue
     * 2 = yellow
     */
	public Color color;
	//    int spikeHash = Animator.StringToHash("spike");
	//    Animator anim;
	public bool explode;
	[SerializeField]
	private GameObject shatter;
	[SerializeField]
	private GameObject player;
	[SerializeField]
	private GameObject onePointCrystal;
	[SerializeField]
	private GameObject fivePointCrystal;
	[SerializeField]
	private GameObject SuperRare;
	private GameObject[] dropjects;
	private Color32 CrystalColor;

	// Use this for initialization
	void Start ()
	{
        
        explode = false;
		//random number to assign random color
		//switch to determine the color of the object
       
		//gameObject.GetComponent<Renderer> ().material.color = color;
		dropjects = GameObject.FindGameObjectsWithTag ("Dropject");
		//CrystalColor = this.gameObject.GetComponent<CrystalUnlit> ().color;
		//anim = GetComponent<Animator>();
		//anim.speed = 0;
	}

	// Update is called once per frame
	void Update ()
	{

		/*if (this.GetComponent<Rigidbody>().position.x - player.GetComponent<Rigidbody>().position.x < 5)
        {
            //anim.speed = 1;
        }*/
		if (explode) {
			/*dropjects = GameObject.FindGameObjectsWithTag ("Dropject");
			foreach (GameObject dropject in dropjects) {
				dropject.GetComponent<MeshRenderer> ().sharedMaterial.color = CrystalColor;
				dropject.GetComponent<MeshRenderer> ().sharedMaterial.SetColor ("_EmissionColor", CrystalColor);
			}*/
			gameObject.SetActive (false);
		}

	}

	void OnTriggerEnter (Collider collision)
	{

		//collision check for bullet
		if (collision.gameObject.CompareTag ("Bullet")) {
			//switch to match the color of the bullet with the color of the wall
			Instantiate (shatter, new Vector3 (transform.position.x+3, transform.position.y+2, transform.position.z + 1), Quaternion.identity);
            GameObject player1 = GameObject.Find("Player");
            player1.GetComponent<PlayerCollision>().CrystalsShot += 1;
            int random = Random.Range (0, 100);
			if (random < 100) {
				if (random < 40) {
					if (random < 5) {
						//super rare
						Instantiate (SuperRare, new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
					} else {
						//5 point gems
						for (int i = 0; i < Random.Range (1, 3); i++) {
							Instantiate (fivePointCrystal, new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
						}
					}
				} else {
					//single point gems
					for (int i = 0; i < Random.Range (1, 3); i++) {
						Instantiate (onePointCrystal, new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
					}
				}
			}
            
			collision.gameObject.GetComponent<Rocket> ().Kill (100);
			explode = true;
		}
	}
}

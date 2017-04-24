using UnityEngine;
using System.Collections;

public class DartShooter : MonoBehaviour {
    [SerializeField]
    private GameObject Dart;
    [SerializeField]
    private GameObject DartTrigger;
    public float firerate = 1;
    float timer = 0; 
    GameObject clone;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if(DartTrigger.GetComponent<DartTrigger>().trigger)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                clone = (GameObject)Instantiate(Dart, transform.position, transform.rotation);
                clone.GetComponent<Rigidbody>().velocity = new Vector3(-15, 0, 0);
                timer = firerate;
            }
        }
	}
}

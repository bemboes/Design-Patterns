using UnityEngine;
using System.Collections;

public class BoulderSpawn : MonoBehaviour {
    [SerializeField]
    private GameObject Trigger;
    [SerializeField]
    private GameObject boulder;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Trigger.GetComponent<playerTrigger>().trigger)
        {
			Debug.Log("WTF");
			Instantiate(boulder,transform.position,transform.rotation);
            Trigger.GetComponent<playerTrigger>().trigger = false;
        }
	}
}

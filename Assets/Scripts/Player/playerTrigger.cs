using UnityEngine;
using System.Collections;

public class playerTrigger : MonoBehaviour {

    public bool trigger = false;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            trigger = true;
        }
    }  
}

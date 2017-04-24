using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Coins : MonoBehaviour {

    private GameObject scoreObject;
	// Use this for initialization
	void Start () {
        scoreObject = GameObject.Find("Score");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
			SceneManager.LoadScene("Main Menu");
            this.gameObject.SetActive(false);
        }
    
    }
}

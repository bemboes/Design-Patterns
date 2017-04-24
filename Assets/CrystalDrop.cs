using UnityEngine;
using System.Collections;

public class CrystalDrop : MonoBehaviour {

    [SerializeField]
    private GameObject crystal;
    [SerializeField]
    private GameObject[] dropjects;
    private float DestroyTime = 5;
    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(DestroyTime <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            DestroyTime -= Time.deltaTime;
        }

	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {


    [SerializeField]
    private Text text;
    public int score = 0;

	
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale > 0.5)
        {
            text.GetComponent<Text>().text = "Score:   " + score.ToString();
            //score++;
        }
    }
}

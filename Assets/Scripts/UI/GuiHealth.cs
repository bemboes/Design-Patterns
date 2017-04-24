using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuiHealth : MonoBehaviour {

    [SerializeField]
    private GameObject FirstHeart;
    [SerializeField]
    private GameObject SecondHeart;
    [SerializeField]
    private GameObject ThirdHeart;
    [SerializeField]
    private GameObject player;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    switch(player.GetComponent<PlayerCollision>().CurrentLives)
        {
            case 0:
                FirstHeart.SetActive(false);
                SecondHeart.SetActive(false);
                ThirdHeart.SetActive(false);
                break;
            case 1:
                FirstHeart.SetActive(true);
                SecondHeart.SetActive(false);
                ThirdHeart.SetActive(false);
                break;
            case 2:
                ThirdHeart.SetActive(false);
                SecondHeart.SetActive(true);
                FirstHeart.SetActive(true);
                break;
            case 3:
                FirstHeart.SetActive(true);
                SecondHeart.SetActive(true);
                ThirdHeart.SetActive(true);
                break;
        }


	}
}

using UnityEngine;
using System.Collections;

public class ColoredWallParent : MonoBehaviour {
    public bool singelcoll = false;

    [SerializeField]
    private GameObject FirstColorChild;
    [SerializeField]
    private GameObject SecondColorChild;
    [SerializeField]
    private GameObject player;

    
    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {

        //Checks if both childs are active
        if (FirstColorChild.activeInHierarchy && SecondColorChild.activeInHierarchy)
        {
            //checks the color of first child
            if (FirstColorChild.GetComponent<Renderer>().material.color == Color.red)
            {
                //checks the color of second child
                if (SecondColorChild.GetComponent<Renderer>().material.color == Color.blue)
                {
                    //If both colors are matched the mixed color gets assigend to the material color of both objects.
                    Color32 color = new Color32(128, 0, 128, 255);
                    SecondColorChild.GetComponent<Renderer>().material.color = color;
                    FirstColorChild.GetComponent<Renderer>().material.color = color;
                }
                //checks the color of second child if not blue.
                else if (SecondColorChild.GetComponent<Renderer>().material.color == Color.yellow)
                {
                    Color32 color = new Color32(255, 140, 0, 255);
                    SecondColorChild.GetComponent<Renderer>().material.color = color;
                    FirstColorChild.GetComponent<Renderer>().material.color = color;
                }

            }
            else if (FirstColorChild.GetComponent<Renderer>().material.color == Color.blue)
            {
                if (SecondColorChild.GetComponent<Renderer>().material.color == Color.red)
                {
                    Color32 color = new Color32(255, 140, 0, 255);
                    SecondColorChild.GetComponent<Renderer>().material.color = color;
                    FirstColorChild.GetComponent<Renderer>().material.color = color;
                }
                else if (SecondColorChild.GetComponent<Renderer>().material.color == Color.yellow)
                {
                    SecondColorChild.GetComponent<Renderer>().material.color = Color.green;
                    FirstColorChild.GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else if (FirstColorChild.GetComponent<Renderer>().material.color == Color.yellow)
            {
                if (SecondColorChild.GetComponent<Renderer>().material.color == Color.red)
                {
                    Color32 color = new Color32(255,140,0,255);
                    SecondColorChild.GetComponent<Renderer>().material.color = color;
                    FirstColorChild.GetComponent<Renderer>().material.color = color;
                }
                else if (SecondColorChild.GetComponent<Renderer>().material.color == Color.blue)
                {
                    SecondColorChild.GetComponent<Renderer>().material.color = Color.green;
                    FirstColorChild.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }


    }

    void OnTriggerEnter(Collider coll)
    {
        
        //collsion check for bullet
        if (coll.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("collides");
            //checks if on of the childs is active to shoot at.
            if (FirstColorChild.activeInHierarchy || SecondColorChild.activeInHierarchy)
            {
                //Switch to checks if the color of the first child matches the color of the bullet.
                switch (FirstColorChild.GetComponent<ColoredWallChild>().ColorNr)
                {
                    case 1:
                        if (coll.gameObject.GetComponent<SpriteRenderer>().color == Color.red)
                        {
                            Debug.Log("colorcheck");
                            //if color matches the bullet and the wall child get deactivated
                            FirstColorChild.SetActive(false);
                            coll.gameObject.SetActive(false);
                        }
                        break;
                    case 2:
                        if (coll.gameObject.GetComponent<SpriteRenderer>().color == Color.blue)
                        {
                            FirstColorChild.SetActive(false);
                            coll.gameObject.SetActive(false);
                        }
                        break;
                    case 3:
                        if (coll.gameObject.GetComponent<SpriteRenderer>().color == Color.yellow)
                        {
                            FirstColorChild.SetActive(false);
                            coll.gameObject.SetActive(false);
                        }
                        break;

                }
                //Switch to check is the color of the first child matches the color of the bullet.
                switch (SecondColorChild.GetComponent<ColoredWallChild>().ColorNr)
                {
                    case 1:
                        if (coll.gameObject.GetComponent<SpriteRenderer>().color == Color.red)
                        {
                            //if the color matches second child and bullet will get deactivated.
                            SecondColorChild.SetActive(false);
                            coll.gameObject.SetActive(false);
                        }
                        break;
                    case 2:
                        if (coll.gameObject.GetComponent<SpriteRenderer>().color == Color.blue)
                        {
                            SecondColorChild.SetActive(false);
                            coll.gameObject.SetActive(false);
                        }
                        break;
                    case 3:
                        if (coll.gameObject.GetComponent<SpriteRenderer>().color == Color.yellow)
                        {
                            SecondColorChild.SetActive(false);
                            coll.gameObject.SetActive(false);
                        }
                        break;

                }
            }
            if(coll.gameObject.activeInHierarchy)
            {
                //even if color doesn't match the bullet will get geactivated.
                coll.gameObject.SetActive(false);
            }
        }
    }
    
}

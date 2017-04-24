using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class FloorOfDeath : MonoBehaviour
{
	private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning(coll.gameObject.name + " collided with Floor of Death.");
        }
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    [SerializeField]
    private bool loadNext = false;
    [SerializeField]
    private float loadDelay = 1.8f;

    void Update ()
    {
	    if(loadNext)
        {
            loadNext = false;
            StartCoroutine(LoadNextScene());
        }
	}

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(loadDelay);
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(buildIndex == -1 ? 0 : Mathf.Clamp(buildIndex + 1, 0, SceneManager.sceneCount));
    }
}

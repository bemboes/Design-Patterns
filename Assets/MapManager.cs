using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour {

    public void StartLevel(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }
}

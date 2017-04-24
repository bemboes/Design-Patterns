using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{

	[SerializeField]
	private GameObject mSettingPanel, mPauseMenuPanel;
	private bool openSettings = false;

	// Update is called once per frame
	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.Escape)) { //TODO add a GUI button event if there is no escape key
			if (mPauseMenuPanel.activeSelf == true) {
				Resume ();
			} else {
				Pause ();
				mSettingPanel.SetActive (false);
			}
		}
	}

	public void Pause ()
	{
		mPauseMenuPanel.SetActive (true);
		Time.timeScale = 0;
	}

	public void Resume ()
	{
		mPauseMenuPanel.SetActive (false);
		Time.timeScale = 1;
	}

	public void StartGame ()
	{
		SceneManager.LoadScene ("WorldMap", LoadSceneMode.Single); //TODO add level road map.
		Time.timeScale = 1;
	}

	public void ExitGame ()
	{
		Application.Quit ();
	}

	public void ExitLevel ()
	{
        GetComponent<PlayerShooting>().Fire();
        SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

	public void ResetLevel ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		Time.timeScale = 1;
	}

	public void Settings ()
	{
		openSettings = !openSettings;
		mSettingPanel.SetActive (openSettings);
	}
    
}

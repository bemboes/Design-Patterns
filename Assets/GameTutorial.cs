using UnityEngine;
using System.Collections;

public class GameTutorial : MonoBehaviour
{
    public bool DEBUG_ALWAYS_ON = true;

    [SerializeField]
    private TutElement jumpTutorial;
    [SerializeField]
    private TutElement rollTutorial;
    [SerializeField]
    private TutElement fireTutorial;

    //private TutElement jumpTut, rollTut, fireTut;

    public const string TUT_PREF_NAME = "CompletedTutorial";
    private bool skipTutorial = false;

    public bool HasCompletedTutorial
    {
        get { return PlayerPrefs.GetInt(TUT_PREF_NAME, -1) > 0; }
    }

    public static GameTutorial Instance
    {
        get { return instance; }
    }

    private static GameTutorial instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("An instance of Tutorial already exists in the scene!");
            return;
        }

        if(PlayerPrefs.GetInt(TUT_PREF_NAME, -1) < 0)
        {
            PlayerPrefs.SetInt(TUT_PREF_NAME, 0);
        }

        if (HasCompletedTutorial)
        {
            skipTutorial = true;
        }
    }

    private void Start ()
    {
	    if(jumpTutorial == null || rollTutorial == null || fireTutorial == null)
        {
            Debug.LogError("Some tutorial elements are missing! (Check inspector)");
            return;
        }

        //jumpTut = GameObject.Instantiate<GameObject>
        //    (jumpTutorial).GetComponent<TutElement>();

        //rollTut = GameObject.Instantiate<GameObject>
        //    (jumpTutorial).GetComponent<TutElement>();

        //fireTut = GameObject.Instantiate<GameObject>
        //    (jumpTutorial).GetComponent<TutElement>();

        //jumpTut.transform.parent = canvas.transform;
        //rollTut.transform.parent = canvas.transform;
        //fireTut.transform.parent = canvas.transform;

        instance = this;
    }

    public void FinishTutorial()
    {
        PlayerPrefs.SetInt(TUT_PREF_NAME, 1);
        PlayerPrefs.Save();
    }


    public void ShowExample(TutElement.AnimType animType)
    {
        if (skipTutorial && !DEBUG_ALWAYS_ON) return;

        switch (animType)
        {
            case TutElement.AnimType.SlideUp:
                jumpTutorial.Animate();
                break;
            case TutElement.AnimType.SlideDown:
                rollTutorial.Animate();
                break;
            case TutElement.AnimType.Tap:
                fireTutorial.Animate();
                break;
        }
    }
}

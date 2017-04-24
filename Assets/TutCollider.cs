using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class TutCollider : MonoBehaviour
{
    [SerializeField]
    private TutElement.AnimType animType;
    [SerializeField]
    private bool completesTutorial = false;

    private void Awake()
    {
        BoxCollider coll = gameObject.GetComponent<BoxCollider>();

        if(coll == null)
        {
            Debug.LogError("Couldnt find collider!");
            return;
        }

        coll.isTrigger = true;
        coll.enabled = true;
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (completesTutorial)
        {
            GameTutorial.Instance.FinishTutorial();
            return;
        }

        if(coll.tag == "Player")
            GameTutorial.Instance.ShowExample(animType);
    }
}

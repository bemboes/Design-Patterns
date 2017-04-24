using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]
    private float timerInSeconds = 1.0f;
    
    private void OnEnable()
    {
        StartCoroutine(DelayKill());
    }

    private IEnumerator DelayKill()
    {
        yield return new WaitForSeconds(timerInSeconds);
        Destroy(gameObject);
    }
}

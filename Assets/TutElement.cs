using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutElement : MonoBehaviour
{
    [SerializeField]
    private Image handImage;

    public enum AnimType
    {
        SlideUp,
        SlideDown,
        Tap
    }

    [SerializeField]
    private AnimType animType;

    private const float velocityCM = 0.2f;
    private float velocityPixels =0;
    private bool isAnimating = false;

    private Vector3 initPos, initScale;
    private Quaternion initRot;

    private const float ANIM_LENGTH = 4.0f;

    [SerializeField]
    private Image[] images;
    private Color[] colors;

    private void Awake()
    {
        velocityPixels = VectorMath.ConvertCmToPixel(velocityCM);
        colors = new Color[images.Length];

        for(int i = 0; i < images.Length; i++)
        {
            colors[i] = images[i].color;
        }
    }

    private void Start()
    {
        initPos = transform.localPosition;
        initRot = transform.localRotation;
        initScale = transform.localScale;
        gameObject.SetActive(false);
        isAnimating = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void ResetObject()
    {
        StopAllCoroutines();
        transform.localPosition = initPos;
        transform.localRotation = initRot;
        transform.localScale = initScale;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = colors[i];
        }
    }

    public void Animate()
    {
        if (isAnimating)
            return;
        isAnimating = true;

        ResetObject();
        // ( ; --_-)...
        gameObject.SetActive(true);

        switch (animType)
        {
            case AnimType.SlideUp:
                StartCoroutine(SlideUp());
                break;
            case AnimType.SlideDown:
                StartCoroutine(SlideDown());
                break;
            case AnimType.Tap:
                StartCoroutine(Tap());
                break;
        }
    }

    private IEnumerator FadeOut()
    {
        float timeStamp = Time.realtimeSinceStartup;
        Color alphaValue = new Color(0, 0, 0, 1);
        while (Time.realtimeSinceStartup - ANIM_LENGTH < timeStamp)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color -= alphaValue * (Time.deltaTime / ANIM_LENGTH);
            }
            yield return null;
        }
        ResetObject();
        gameObject.SetActive(false);
    }

    private IEnumerator SlideUp()
    {
        StartCoroutine(FadeOut());
        float timeStamp = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - ANIM_LENGTH < timeStamp)
        {
            transform.localPosition += (Vector3) Vector2.up * velocityPixels * Time.deltaTime;
            handImage.transform.localPosition += (Vector3)Vector2.up * (velocityPixels / 3f) * Time.deltaTime;
            yield return null;
        }
        ResetObject();
        gameObject.SetActive(false);
    }

    private IEnumerator SlideDown()
    {
        StartCoroutine(FadeOut());
        float timeStamp = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - ANIM_LENGTH < timeStamp)
        {
            transform.localPosition -= (Vector3)Vector2.up * velocityPixels * Time.deltaTime;
            handImage.transform.localPosition -= (Vector3)Vector2.up * (velocityPixels / 3f) * Time.deltaTime;
            yield return null;
        }
        ResetObject();
        gameObject.SetActive(false);
    }

    private IEnumerator Tap()
    {
        StartCoroutine(FadeOut());
        float timeStamp = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - (ANIM_LENGTH/3) < timeStamp)
        {
            handImage.transform.localScale -= Vector3.one * (Time.deltaTime / ANIM_LENGTH / 3);
            //handImage.transform.localPosition += (Vector3)Vector2.up * (velocityPixels / 3f) * Time.deltaTime;
            yield return null;
        }

        handImage.transform.localScale = Vector3.one;

        while (Time.realtimeSinceStartup - (ANIM_LENGTH / (3*2)) < timeStamp)
        {
            handImage.transform.localScale -= Vector3.one * (Time.deltaTime / ANIM_LENGTH / 3);
            //handImage.transform.localPosition += (Vector3)Vector2.up * (velocityPixels / 3f) * Time.deltaTime;
            yield return null;
        }

        handImage.transform.localScale = Vector3.one;

        while (Time.realtimeSinceStartup - ANIM_LENGTH < timeStamp)
        {
            handImage.transform.localScale -= Vector3.one * (Time.deltaTime / ANIM_LENGTH / 3);
            //handImage.transform.localPosition += (Vector3)Vector2.up * (velocityPixels / 3f) * Time.deltaTime;
            yield return null;
        }

        ResetObject();
        gameObject.SetActive(false);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public GameObject frontIcon;
    public GameObject backIcon;
    private bool isFlipped = false;
    public float duraTime = 0.25f;
    public string icon;

    private void Awake()
    {
        frontIcon = transform.Find("Front").gameObject;
        backIcon = transform.Find("Back").gameObject;
    }


    public void OnTileClicked()
    {
        if (GameManager.Instance.GetCheckStatus() == true) return;
        Flip();
        GameManager.Instance.TileClicked(this);
    }

    public void Flip()
    {
        if (!isFlipped) StartCoroutine(FlipAnimation());
        else StartCoroutine(FlipBackAnimation());
    }
    private IEnumerator FlipAnimation()
    {
        float fadeDuration = duraTime;
        float fadeTime = 0f;

        while (fadeTime < fadeDuration)
        {
            fadeTime += Time.deltaTime;
            float angle = Mathf.Lerp(0, 90, fadeTime / fadeDuration);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

        isFlipped = !isFlipped;
        frontIcon.SetActive(isFlipped);
        backIcon.SetActive(!isFlipped);

        fadeTime = 0f;
        while (fadeTime < fadeDuration)
        {
            fadeTime += Time.deltaTime;
            float angle = Mathf.Lerp(90, 180, fadeTime / fadeDuration);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

    }
    private IEnumerator FlipBackAnimation()
    {
        float fadeDuration = duraTime;
        float fadeTime = 0f;

        while (fadeTime < fadeDuration)
        {
            fadeTime += Time.deltaTime;
            float angle = Mathf.Lerp(180, 90, fadeTime / fadeDuration);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

        isFlipped = !isFlipped;
        frontIcon.SetActive(isFlipped);
        backIcon.SetActive(!isFlipped);

        fadeTime = 0f;
        while (fadeTime < fadeDuration)
        {
            fadeTime += Time.deltaTime;
            float angle = Mathf.Lerp(90, 0, fadeTime / fadeDuration);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }
    }

}

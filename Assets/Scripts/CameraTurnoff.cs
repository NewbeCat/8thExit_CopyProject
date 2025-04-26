using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraTurnoff : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float fadeDuration = 3f;

    void Start()
    {
        fadeGroup.alpha = 1f;
        FadeIn();
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0f, 1f));
    }

    public void FadeIn()
    {
        //필름소리 1f간 내기
        StartCoroutine(Fade(1f, 0f));
    }

    public void TurnOff()
    {
        fadeGroup.alpha = 1f;
    }

    public void TurnOn()
    {
        fadeGroup.alpha = 0f;
    }

    private IEnumerator Fade(float from, float to)
    {
        yield return new WaitForSeconds(1f);
        float time = 0f;
        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            fadeGroup.alpha = Mathf.Lerp(from, to, t);
            time += Time.deltaTime;
            yield return null;
        }
        fadeGroup.alpha = to;
    }
}

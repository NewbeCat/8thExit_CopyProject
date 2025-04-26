using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraTurnoff : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float fadeDuration = 3f;
    public float filmDuration = 3f;

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
        // TODO 필름소리를 filmDuration시간 동안 내주세요.
        StartCoroutine(Fade(1f, 0f, true));
    }

    public void TurnOff()
    {
        fadeGroup.alpha = 1f;
    }

    public void TurnOn()
    {
        fadeGroup.alpha = 0f;
    }

    private IEnumerator Fade(float from, float to, bool wait = false)
    {
        if (wait) yield return new WaitForSeconds(filmDuration);
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

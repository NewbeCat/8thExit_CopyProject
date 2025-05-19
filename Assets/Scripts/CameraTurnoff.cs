using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraTurnoff : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float fadeDuration = 2f;
    public float filmDuration = 2f;
    private PlayerController playerController;
    private CameraController cameraController;
    private float horizontal;
    private float vertical;

    void Start()
    {
        GameObject playerObj = GameObject.Find("PlayerController");
        if (playerObj == null)
        {
            Debug.LogError("PlayerController object not found");
            return;
        }
        playerController = playerObj.GetComponent<PlayerController>();
        Transform camTransform = playerObj.transform.Find("Main Camera");
        cameraController = camTransform.GetComponent<CameraController>();

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
        playerController.enabled = false;
        cameraNoTurn(false);

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

        playerController.enabled = true;
        cameraNoTurn(true);
    }

    private void cameraNoTurn(bool on)
    {
        if (!on)
        {
            (horizontal, vertical) = cameraController.SensitivityRead();
            cameraController.SensitivitySet(0f, 0f);
        }
        else
        {
            cameraController.SensitivitySet(horizontal, vertical);
        }
    }
}

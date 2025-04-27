using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class EndSequence : MonoBehaviour
{
    [SerializeField] private Vector3 endDestination;
    [SerializeField] private CameraTurnoff cameraTurnoff;
    [SerializeField] private GameObject player;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float zoomDuration = 3f;
    [SerializeField] private float creditDuration = 15f;
    [SerializeField] private float rollup = 15f;

    [SerializeField] private GameObject eraseScreen1;
    [SerializeField] private GameObject eraseScreen2;
    [SerializeField] private GameObject newScreen;
    [SerializeField] private GameObject credits;

    [SerializeField] private AudioSource source;

    private PlayerController playerController;
    private Camera playerCamera;
    private float originalFOV;
    private bool sequenceStarted = false;

    private Vector2 rotationLimitsX;
    private Vector2 rotationLimitsY;
    private float originalRotationX;
    private float originalRotationY;
    private bool fading = false;

    private void Start()
    {
        GameObject playerObj = GameObject.Find("PlayerController");
        if (playerObj == null)
        {
            Debug.LogError("PlayerController object not found");
            return;
        }

        player = playerObj;
        playerController = player.GetComponent<PlayerController>();
        playerCamera = player.GetComponentInChildren<Camera>();

        GameObject mapObj = GameObject.Find("Map");
        if (mapObj == null)
        {
            Debug.LogError("Map object not found");
            return;
        }

        cameraTurnoff = mapObj.GetComponentInChildren<CameraTurnoff>();

        if (cameraTurnoff == null)
        {
            Debug.LogError("CameraTurnoff component not found");
            return;
        }
        originalFOV = playerCamera.fieldOfView;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !sequenceStarted)
        {
            sequenceStarted = true;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(EndSequenceRoutine());
        }
    }

    private IEnumerator EndSequenceRoutine()
    {
        playerController.enabled = false;
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(ZoomCamera(zoomFOV, zoomDuration));
        cameraTurnoff.TurnOff();
        Managers.Instance.Sound.PlaySFX(ESoundClip.LightTurnoff);
        // TODO 불꺼지는 소리 oneshot
        player.transform.position = endDestination;
        player.transform.rotation = Quaternion.Euler(0, -90, 0);
        eraseScreen1.SetActive(false);
        eraseScreen2.SetActive(false);
        newScreen.SetActive(true);
        playerCamera.fieldOfView = originalFOV;
        yield return new WaitForSeconds(1f);
        cameraTurnoff.TurnOn();
        Managers.Instance.Sound.PlaySFX(ESoundClip.LightTurnon);
        // TODO 불켜지는 소리 oneshot
        yield return new WaitForSeconds(1f);
        playerController.enabled = true;


        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(RollCredits());
    }

    private IEnumerator RollCredits()
    {
        Vector3 startPos = credits.transform.position;
        Vector3 endPos = startPos + new Vector3(0, rollup, 0);

        Managers.Instance.Sound.PlaySFX(ESoundClip.AmbienceEnding, source.transform.position);
        source.clip = Managers.Instance.Sound.GetAudioClip(ESoundClip.Flim);
        source.Play();
        // TODO 박수 소리 등 엔딩 소리 시작

        float elapsed = 0f;

        while (elapsed < creditDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / creditDuration);
            credits.transform.position = Vector3.Lerp(startPos, endPos, t);

            if (!fading && creditDuration - elapsed <= 5f)
            {
                // TODO 소리 페이드 아웃 3f  -- 소리 음량 서서히 줄이다가 3f때 음량 = 0 ==> 그러면 소리 아예 끄기, 카메라 fadeout과 동시에 실행되어야함.
                cameraTurnoff.FadeOut();
                StartCoroutine(CoFadeFlimSound());
                fading = true;
            }
            yield return null;
        }

        credits.transform.position = endPos;
        SceneManager.LoadScene("BetterMap"); // TODO 태초마을 스타일로 처음부터 다시!
    }

    private IEnumerator CoFadeFlimSound()
    {
        float fadeDuration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float diff = fadeDuration - elapsedTime;
            source.volume = diff;
            yield return null;
        }

        if (source)
        {
            source.Stop();
        }
    }

    private IEnumerator RotatePlayer(Vector3 targetEulerAngles, float duration)
    {
        Quaternion startRotation = player.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(targetEulerAngles);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            player.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        player.transform.rotation = endRotation;
    }

    private IEnumerator ZoomCamera(float targetFOV, float duration)
    {
        float startFOV = playerCamera.fieldOfView;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            playerCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;
    }
}

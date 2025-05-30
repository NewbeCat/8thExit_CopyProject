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
    //[SerializeField] private GameObject ambiencePlayer;
    [SerializeField] private AmbienceController ambienceController;

    [SerializeField] private AK.Wwise.Event endingScreenEvent;
    [SerializeField] private AK.Wwise.Event lightOffEvent;
    [SerializeField] private AK.Wwise.Event lightOnEvent;
    [SerializeField] private AK.Wwise.Event ambienceEndingEvent;

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
            endingScreenEvent.Post(gameObject);
            StartCoroutine(EndSequenceRoutine());
        }
    }

    private IEnumerator EndSequenceRoutine()
    {
        playerController.enabled = false;
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(ZoomCamera(zoomFOV, zoomDuration));
        cameraTurnoff.TurnOff();
        lightOffEvent.Post(gameObject);
        // TODO 불꺼지는 소리 oneshot
        player.transform.position = endDestination;
        player.transform.rotation = Quaternion.Euler(0, -90, 0);
        eraseScreen1.SetActive(false);
        eraseScreen2.SetActive(false);
        newScreen.SetActive(true);
        playerCamera.fieldOfView = originalFOV;
        yield return new WaitForSeconds(3f);
        cameraTurnoff.TurnOn();
        lightOnEvent.Post(gameObject);
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

        ambienceEndingEvent.Post(gameObject);
        // TODO 박수 소리 등 엔딩 소리 시작

        float elapsed = 0f;

        while (elapsed < creditDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / creditDuration);
            credits.transform.position = Vector3.Lerp(startPos, endPos, t);

            if (!fading && creditDuration - elapsed <= 5f)
            {
                StartCoroutine(CoFadeFlimSound()); // 기존 엔딩 사운드 정지
                StartCoroutine(CoFadeOutGeneralAmbience()); // 일반 BGM 페이드 아웃 추가
                // TODO 소리 페이드 아웃 3f  -- 소리 음량 서서히 줄이다가 3f때 음량 = 0 ==> 그러면 소리 아예 끄기, 카메라 fadeout과 동시에 실행되어야함.
                cameraTurnoff.FadeOut();
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

        AkUnitySoundEngine.ExecuteActionOnEvent(
            ambienceEndingEvent.Name,                          // 이벤트 이름
            AkActionOnEventType.AkActionOnEventType_Stop,      // 동작: 정지
            gameObject,                                        // 대상 게임오브젝트
            (int)(fadeDuration * 1000),                        // 밀리초 단위 시간 (3000ms)
            AkCurveInterpolation.AkCurveInterpolation_Linear   // 볼륨 감소 커브
        );

        yield return new WaitForSeconds(fadeDuration);
    }


    // 기존의 앰비언스를 멈추는 함수
    private IEnumerator CoFadeOutGeneralAmbience()
    {
        if (ambienceController == null)
        {
            Debug.LogWarning("AmbienceController가 할당되지 않았습니다.");
            yield break;
        }

        ambienceController.StopAmbience(3f);
        yield return new WaitForSeconds(3f);
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

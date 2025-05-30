using UnityEngine;

public class AmbienceController : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event ambienceEvent;

    private static AmbienceController instance;
    private bool isPlaying = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환에도 유지
    }

    private void Start()
    {
        if (!isPlaying)
        {
            ambienceEvent.Post(gameObject);
            isPlaying = true;
        }
    }

    public void StopAmbience(float fadeDuration = 3f)
    {
        AkSoundEngine.ExecuteActionOnEvent(
            ambienceEvent.Name,
            AkActionOnEventType.AkActionOnEventType_Stop,
            gameObject,
            (int)(fadeDuration * 1000),
            AkCurveInterpolation.AkCurveInterpolation_Linear
        );

        isPlaying = false;
    }
}

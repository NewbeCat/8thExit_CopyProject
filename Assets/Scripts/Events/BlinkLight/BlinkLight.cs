using System.Collections;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] private GameObject[] lights;
    [SerializeField] private BoxEventTrigger trigger;

    private bool isTriggered;

    private void Awake()
    {
        trigger.OnTrigger += TriggerLights;
    }

    private void TriggerLights()
    {
        if (isTriggered)
        {
            return;
        }

        isTriggered = true;
        foreach (GameObject light in lights)
        {
            Managers.Instance.StartCoroutine(CoBlinkLight(light));
        }
    }

    private IEnumerator CoBlinkLight(GameObject light)
    {
        float delay = Random.Range(1f, 5f);
        yield return Managers.Instance.Coroutine.GetWaitForSeconds(delay);
        Managers.Instance.Sound.PlaySFX(ESoundClip.LightTurnoff);
        light.SetActive(false);
        // 끄기 사운드
        delay = Random.Range(0f, 3f);
        yield return Managers.Instance.Coroutine.GetWaitForSeconds(delay);
        // 키기 사운드
        Managers.Instance.Sound.PlaySFX(ESoundClip.LightTurnon);
        light.SetActive(true);

        Managers.Instance.StartCoroutine(CoBlinkLight(light));
    }
}

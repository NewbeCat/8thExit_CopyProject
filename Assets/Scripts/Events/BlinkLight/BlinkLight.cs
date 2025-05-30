using System.Collections;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] private GameObject[] lights;
    [SerializeField] private BoxEventTrigger trigger;

    [SerializeField] private AK.Wwise.Event lightOffEvent;  // 불 꺼지는 사운드
    [SerializeField] private AK.Wwise.Event lightOnEvent;   // 불 켜지는 사운드

    private bool isTriggered;

    private void Awake()
    {
        trigger.OnTrigger += TriggerLights;
    }

    private void TriggerLights()
    {
        if (isTriggered)
            return;

        isTriggered = true;
        foreach (GameObject light in lights)
        {
            StartCoroutine(CoBlinkLight(light));
        }
    }

    private IEnumerator CoBlinkLight(GameObject light)
    {
        float delay = Random.Range(1f, 5f);
        yield return new WaitForSeconds(delay);

        // 불 꺼지는 사운드
        lightOffEvent?.Post(light);
        light.SetActive(false);

        delay = Random.Range(0f, 3f);
        yield return new WaitForSeconds(delay);

        // 불 켜지는 사운드
        lightOnEvent?.Post(light);
        light.SetActive(true);

        StartCoroutine(CoBlinkLight(light));
    }
}

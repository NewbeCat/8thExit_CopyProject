using System.Collections;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] private GameObject[] lights;
    [SerializeField] private BoxEventTrigger trigger;

    [SerializeField] private AK.Wwise.Event lightOffEvent;  // �� ������ ����
    [SerializeField] private AK.Wwise.Event lightOnEvent;   // �� ������ ����

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

        // �� ������ ����
        lightOffEvent?.Post(light);
        light.SetActive(false);

        delay = Random.Range(0f, 3f);
        yield return new WaitForSeconds(delay);

        // �� ������ ����
        lightOnEvent?.Post(light);
        light.SetActive(true);

        StartCoroutine(CoBlinkLight(light));
    }
}

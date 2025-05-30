using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DarkIsComing : MonoBehaviour
{
    [SerializeField] private BoxEventTrigger trigger;
    [SerializeField] private BoxEventTrigger[] triggers;
    [SerializeField] private AK.Wwise.Event darkComingEvent;
    private Light directionalLight;

    private bool isTriggered = false;

    private Sequence sequence;

    private void Awake()
    {
        directionalLight = GameObject.FindWithTag("DirectionalLight").GetComponent<Light>();
        
        trigger.OnTrigger += TriggerLightOff;
        foreach (var trigger in triggers)
        {
            trigger.OnTrigger += TriggerLightOn;
        }
    }

    private void TriggerLightOff()
    {
        if (isTriggered)
        {
            return;
        }

        isTriggered = true;

        sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => directionalLight.intensity, x => directionalLight.intensity = x, 0f, 0.1f) // 2�� ���� 0����
            .SetEase(Ease.InQuad));
        darkComingEvent.Post(gameObject); // Wwise Event ȣ��
        Managers.Instance.Player.LightObject.SetActive(true);
    }

    private void TriggerLightOn()
    {
        DOTween.Kill(sequence);
        directionalLight.intensity = 2f;
        Managers.Instance.Player.LightObject.SetActive(false);
    }
}

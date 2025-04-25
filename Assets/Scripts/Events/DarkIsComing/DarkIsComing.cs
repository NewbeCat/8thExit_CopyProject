using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DarkIsComing : MonoBehaviour
{
    [SerializeField] private BoxEventTrigger trigger;
    [SerializeField] private BoxEventTrigger[] triggers;
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
        sequence.Append(DOTween.To(() => directionalLight.intensity, x => directionalLight.intensity = x, 0f, 2f) // 2초 동안 0으로
            .SetEase(Ease.InQuad));
        Managers.Instance.Player.LightObject.SetActive(true);
    }

    private void TriggerLightOn()
    {
        DOTween.Kill(sequence);
        directionalLight.intensity = 2f;
        Managers.Instance.Player.LightObject.SetActive(false);
    }
}

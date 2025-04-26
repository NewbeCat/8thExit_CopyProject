using UnityEngine;

public class SpotLightEvent : MonoBehaviour
{
    [SerializeField] private GameObject spotLightParent;
    [SerializeField] private BoxEventTrigger boxTrigger;

    private void Awake()
    {
        boxTrigger.OnTrigger += TriggerLight;
    }

    private void TriggerLight()
    {
        Managers.Instance.Sound.PlaySFX(ESoundClip.Spotlight);
        spotLightParent.SetActive(true);
    }
}
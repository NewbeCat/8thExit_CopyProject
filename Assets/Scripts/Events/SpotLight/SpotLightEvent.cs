using UnityEngine;
using System.Collections;
using AK.Wwise;

public class SpotLightEvent : MonoBehaviour
{
    [SerializeField] private GameObject spotLightParent;
    [SerializeField] private BoxEventTrigger boxTrigger;
    [SerializeField] private GameObject[] spotLights;
    //[SerializeField] private AK.Wwise.Event spotlightEvent;

    private bool hasPlayed = false;

    private void Awake()
    {
        boxTrigger.OnTrigger += TriggerLight;
    }

    private void TriggerLight()
    {
        if (hasPlayed)
            return;

        hasPlayed = true;
        spotLightParent.SetActive(true);
        
    }
}
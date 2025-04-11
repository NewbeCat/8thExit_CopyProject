using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvents/SwitchingObjectEvent")]
public class SwitchingObjectEvent : GameEvent
{
    [SerializeField] private GameObject _originObject;
    [SerializeField] private GameObject _subObject;

    protected override void Execute()
    {
        _originObject.SetActive(false);
        _subObject.SetActive(true);
        Debug.Log("SwitchingObject Event Executed");
    }

    protected override void ResetEvent()
    {
        _originObject.SetActive(true);
        _subObject.SetActive(false);
        Debug.Log("SwitchingObject Event Reset");
    }
}

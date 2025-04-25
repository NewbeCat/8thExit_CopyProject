using System;
using UnityEngine;

public class BoxEventTrigger : MonoBehaviour
{
    public event Action OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke();
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> subtleEvent;
    [SerializeField] private List<GameObject> obviousEvent;

    private GameEvent curEvent = null;
    public static EventManager Instance { get; private set; }

    private void Awake()
    {

    }


    public void CallEvent(int eventType, int eventID, bool isRoom2)
    {

    }

    public void KillEvent()
    {

    }

    public int GetSubtleEventCount() => subtleEvent.Count;
    public int GetObviousEventCount() => obviousEvent.Count;
}
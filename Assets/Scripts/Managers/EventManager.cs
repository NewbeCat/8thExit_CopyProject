using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<AssetReferenceGameEvent> subtleEventReferences;
    [SerializeField] private List<AssetReferenceGameEvent> obviousEventReferences;

    private Dictionary<int, GameEvent> cachedSubtleEvents = new Dictionary<int, GameEvent>();
    private Dictionary<int, GameEvent> cachedObviousEvents = new Dictionary<int, GameEvent>();

    private GameEvent curEvent = null;
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        PreloadEvents();
    }

    private void PreloadEvents()
    {
        for (int i = 0; i < subtleEventReferences.Count; i++)
        {
            int index = i;
            subtleEventReferences[i].LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    cachedSubtleEvents[index] = handle.Result;
                }
            };
        }

        for (int i = 0; i < obviousEventReferences.Count; i++)
        {
            int index = i;
            obviousEventReferences[i].LoadAssetAsync().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    cachedObviousEvents[index] = handle.Result;
                }
            };
        }
    }

    public void CallEvent(int eventType, int eventID, bool isRoom2)
    {
        if (eventType == 1 && cachedSubtleEvents.TryGetValue(eventID, out var subtleEvent))
        {
            curEvent = subtleEvent;
        }
        else if (eventType == 2 && cachedObviousEvents.TryGetValue(eventID, out var obviousEvent))
        {
            curEvent = obviousEvent;
        }
        else
        {
            Debug.LogError($"[EventManager] Event ID {eventID} not preloaded.");
            return;
        }

        curEvent?.Trigger(isRoom2);
    }

    public void KillEvent()
    {
        if (curEvent != null)
        {
            curEvent.Kill();
            curEvent = null;
        }
    }

    public int GetSubtleEventCount() => subtleEventReferences.Count;
    public int GetObviousEventCount() => obviousEventReferences.Count;
}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<AssetReferenceGameEvent> subtleEventReferences1;
    [SerializeField] private List<AssetReferenceGameEvent> obviousEventReferences1;
    [SerializeField] private List<AssetReferenceGameEvent> subtleEventReferences2;
    [SerializeField] private List<AssetReferenceGameEvent> obviousEventReferences2;

    private Dictionary<int, GameEvent> loadedSubtleEvents1 = new Dictionary<int, GameEvent>();
    private Dictionary<int, GameEvent> loadedObviousEvents1 = new Dictionary<int, GameEvent>();
    private Dictionary<int, GameEvent> loadedSubtleEvents2 = new Dictionary<int, GameEvent>();
    private Dictionary<int, GameEvent> loadedObviousEvents2 = new Dictionary<int, GameEvent>();

    private GameEvent curEvent = null;
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CallEvent(int eventType, int eventID, bool isRoom2)
    {
        if (!isRoom2)
        {
            LoadOrTriggerEvent(eventType, eventID, subtleEventReferences1, obviousEventReferences1, loadedSubtleEvents1, loadedObviousEvents1);
        }
        else
        {
            LoadOrTriggerEvent(eventType, eventID, subtleEventReferences2, obviousEventReferences2, loadedSubtleEvents2, loadedObviousEvents2);
        }
    }

    public void KillEvent()
    {
        if (curEvent != null)
        {
            curEvent.Kill();
            curEvent = null;
        }
    }

    public int GetSubtleEventCount() => subtleEventReferences1.Count;
    public int GetObviousEventCount() => obviousEventReferences1.Count;

    private void LoadOrTriggerEvent(int eventType, int eventID, List<AssetReferenceGameEvent> subtleRefs, List<AssetReferenceGameEvent> obviousRefs,
        Dictionary<int, GameEvent> loadedSubtleEvents, Dictionary<int, GameEvent> loadedObviousEvents)
    {
        if (eventType == 1)
        {
            if (loadedSubtleEvents.ContainsKey(eventID))
            {
                curEvent = loadedSubtleEvents[eventID];
                curEvent?.Trigger();
            }
            else
            {
                LoadEvent(subtleRefs[eventID], eventID, loadedSubtleEvents, true);
            }
        }
        else if (eventType == 2)
        {
            if (loadedObviousEvents.ContainsKey(eventID))
            {
                curEvent = loadedObviousEvents[eventID];
                curEvent?.Trigger();
            }
            else
            {
                LoadEvent(obviousRefs[eventID], eventID, loadedObviousEvents, false);
            }
        }
    }

    private void LoadEvent(AssetReferenceGameEvent eventReference, int eventID, Dictionary<int, GameEvent> loadedEvents, bool isSubtle)
    {
        eventReference.LoadAssetAsync<GameEvent>().Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameEvent loadedEvent = handle.Result;
                loadedEvents[eventID] = loadedEvent;
                curEvent = loadedEvent;
                curEvent?.Trigger();
            }
            else
            {
                Debug.LogError($"Failed to load event {eventReference}");
            }
        };
    }
}
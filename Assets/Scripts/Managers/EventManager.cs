using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject noEvent;
    [SerializeField] private GameObject endEvent;
    [SerializeField] private List<GameObject> subtleEvent;
    [SerializeField] private List<GameObject> obviousEvent;
    //[SerializeField] private Transform parent;
    private bool currentIsNone = false;

    private GameObject currentEventObject = null;
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CallEvent(0, -1);
    }

    public void CallEvent(int eventType, int eventID)
    {
        if (currentEventObject != null)
        {
            if (currentIsNone && eventType == 0)
            {
                return;
            }
            KillEvent();
        }

        GameObject prefabToSpawn = null;
        switch (eventType)
        {
            case 0:
                prefabToSpawn = noEvent;
                break;
            case 1:
                if (eventID >= 0 && eventID < subtleEvent.Count)
                    prefabToSpawn = subtleEvent[eventID];
                break;
            case 2:
                if (eventID >= 0 && eventID < obviousEvent.Count)
                    prefabToSpawn = obviousEvent[eventID];
                break;
            case 8:
                prefabToSpawn = endEvent;
                break;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError("Invalid eventType or eventID");
            return;
        }

        currentEventObject = Instantiate(prefabToSpawn, this.gameObject.transform);
        //currentEventObject.name = prefabToSpawn.name;

        // 자동으로 이름 기준으로 상태 설정
        string prefabStateName = prefabToSpawn.name;  // 예: "ScreenHand"

        // 설정할 State 이름 (예외 프리팹만 따로 처리)
        switch (prefabStateName)
        {
            case "BlinkLight":
            case "ScreenHand":
            case "ScreenVideo":
            case "Stoker":
            case "WeirdSpeaker":
                AkSoundEngine.SetState("RoomState", prefabStateName);
                Debug.Log($"[State] RoomState set to: {prefabStateName}");
                break;

            default:
                AkSoundEngine.SetState("RoomState", "Normal");
                Debug.Log("[State] RoomState set to: Normal (default)");
                break;
            }
        Debug.Log($"[State] RoomState set to {prefabStateName}");
    }

    public void KillEvent()
    {
        if (currentEventObject != null)
        {
            Destroy(currentEventObject);
            currentEventObject = null;
        }
    }

    public int GetSubtleEventCount() => subtleEvent.Count;
    public int GetObviousEventCount() => obviousEvent.Count;
}

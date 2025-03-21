using UnityEngine;
using System.Collections.Generic;

public class EventProbabilityManager : MonoBehaviour
{
    [Header("에러확률")]
    [SerializeField, Range(0, 100)] private float initialErrorProbability = 40f;
    [Header("에러 발생시 subtle이 발생할 확률")]
    [SerializeField, Range(0, 100)] private float subtleErrorProbability = 70f;

    [Header("Current Probabilities")]
    [SerializeField, Range(0, 100)] private float currentSubtleErrorProbability;
    [SerializeField, Range(0, 100)] private float currentObviousErrorProbability;
    [SerializeField, Range(0, 100)] private float currentNormalProbability;

    [Header("Event Options")]
    [SerializeField] private List<int> subtleErrorEventIDs;
    [SerializeField] private List<int> obviousErrorEventIDs;

    private HashSet<int> usedSubtleErrorEventIDs;
    private HashSet<int> usedObviousErrorEventIDs;

    private int _lastEventType = -1;
    private int _lastEventID = -1;
    private int _consecutiveNormalCount = 0;
    [SerializeField] private int _consecutiveNormalLimit = 2;

    private void Awake()
    {
        ResetProbabilities();
    }

    public void ResetProbabilities()
    {
        currentNormalProbability = (100f - initialErrorProbability);
        currentSubtleErrorProbability = initialErrorProbability * (subtleErrorProbability / 100f);
        currentObviousErrorProbability = initialErrorProbability * ((100f - subtleErrorProbability) / 100f);

        usedSubtleErrorEventIDs = new HashSet<int>();
        usedObviousErrorEventIDs = new HashSet<int>();
    }

    public void GetRandomEvent(int forceEvent)
    {
        if (forceEvent == -1)
        {
            _lastEventType = 0;
            _lastEventID = -1;
            return;
        }

        int eventType = RandomType();
        int selectedEventID = RandomEventFromList(eventType);
        _lastEventType = eventType;
        _lastEventID = selectedEventID;
    }

    private int RandomType()
    {
        float totalProbability = currentNormalProbability + currentSubtleErrorProbability + currentObviousErrorProbability;
        float rand = Random.value * totalProbability;

        // No error event
        if (rand < currentNormalProbability && _consecutiveNormalCount < _consecutiveNormalLimit)
        {
            _consecutiveNormalCount++;
            return 0;
        }

        rand -= currentNormalProbability;

        // Subtle error
        if (rand < currentSubtleErrorProbability && subtleErrorEventIDs.Count - usedSubtleErrorEventIDs.Count > 0)
        {
            _consecutiveNormalCount = 0;
            return 1;
        }

        rand -= currentSubtleErrorProbability;

        // Obvious error
        if (rand < currentObviousErrorProbability && obviousErrorEventIDs.Count - usedObviousErrorEventIDs.Count > 0)
        {
            _consecutiveNormalCount = 0;
            return 2;
        }

        // No error due to lack of available events
        Debug.Log("Normal is over limit, but there are no more events available.");
        return 0;
    }

    private int RandomEventFromList(int eventType)
    {
        List<int> selectedList = eventType switch
        {
            2 => obviousErrorEventIDs,
            1 => subtleErrorEventIDs,
            _ => null
        };

        HashSet<int> usedSet = eventType switch
        {
            2 => usedObviousErrorEventIDs,
            1 => usedSubtleErrorEventIDs,
            _ => null
        };

        if (selectedList != null && selectedList.Count > 0)
        {
            List<int> availableEvents = new List<int>();

            foreach (var eventID in selectedList)
            {
                if (!usedSet.Contains(eventID))
                {
                    availableEvents.Add(eventID);
                }
            }

            if (availableEvents.Count > 0)
            {
                int selectedEventID = availableEvents[Random.Range(0, availableEvents.Count)];
                usedSet.Add(selectedEventID);
                return selectedEventID;
            }
        }
        return -1;
    }

    public int GetLastEventType() => _lastEventType;
    public int GetLastEventID() => _lastEventID;
}
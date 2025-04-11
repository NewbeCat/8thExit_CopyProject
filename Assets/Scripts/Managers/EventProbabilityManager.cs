using UnityEngine;
using System.Collections.Generic;

public class EventProbabilityManager : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private EventManager eventManager;

    [Header("Variables")]
    [SerializeField] private int _lastEventType = -1;
    [SerializeField] private int _lastEventID = -1;
    private int _consecutiveNormalCount = 0;
    [SerializeField] private int _consecutiveNormalLimit = 2;

    [Header("에러확률")]
    [SerializeField, Range(0, 100)] private float initialErrorProbability = 40f;
    [Header("에러 발생시 subtle이 발생할 확률")]
    [SerializeField, Range(0, 100)] private float subtleErrorProbability = 70f;

    [Header("Current Probabilities")]
    [SerializeField, Range(0, 100)] private float currentSubtleErrorProbability;
    [SerializeField, Range(0, 100)] private float currentObviousErrorProbability;
    [SerializeField, Range(0, 100)] private float currentNormalProbability;

    private HashSet<int> subtleErrorEventIDs;
    private HashSet<int> obviousErrorEventIDs;

    private HashSet<int> usedSubtleErrorEventIDs;
    private HashSet<int> usedObviousErrorEventIDs;

    public void GetRandomEvent(bool forceEvent = false)
    {
        if (forceEvent)
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
    public int GetEventType() => _lastEventType;
    public int GetEventID() => _lastEventID;

    private void Awake()
    {
        Reset();
    }

    private void Reset()
    {

        subtleErrorEventIDs = new HashSet<int>();
        obviousErrorEventIDs = new HashSet<int>();

        int subtleCount = eventManager.GetSubtleEventCount();
        int obviousCount = eventManager.GetObviousEventCount();

        for (int i = 0; i < subtleCount; i++)
        {
            subtleErrorEventIDs.Add(i);
        }

        for (int i = 0; i < obviousCount; i++)
        {
            obviousErrorEventIDs.Add(i);
        }

        usedSubtleErrorEventIDs = new HashSet<int>();
        usedObviousErrorEventIDs = new HashSet<int>();
        _consecutiveNormalCount = 0;

        ResetProbabilities();
    }

    private void ResetProbabilities()
    {
        currentNormalProbability = (100f - initialErrorProbability);
        currentSubtleErrorProbability = initialErrorProbability * (subtleErrorProbability / 100f);
        currentObviousErrorProbability = initialErrorProbability * ((100f - subtleErrorProbability) / 100f);
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
        if (subtleErrorEventIDs.Count == usedSubtleErrorEventIDs.Count && obviousErrorEventIDs.Count == usedObviousErrorEventIDs.Count)
        {
            usedSubtleErrorEventIDs = new HashSet<int>();
            usedObviousErrorEventIDs = new HashSet<int>();
        }
        return 0;
    }

    private int RandomEventFromList(int eventType)
    {
        HashSet<int> selectedList = eventType switch
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
}
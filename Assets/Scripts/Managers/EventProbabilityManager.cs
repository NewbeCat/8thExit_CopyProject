using UnityEngine;
using System.Collections.Generic;

public class EventProbabilityManager : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private float subtleErrorProbability = 20f;
    [SerializeField, Range(0, 100)] private float obviousErrorProbability = 10f;
    private float normalProbability => 100f - (obviousErrorProbability + subtleErrorProbability);

    [SerializeField] private List<int> subtleErrorEventIDs;
    [SerializeField] private List<int> obviousErrorEventIDs;

    private int _lastEventType = -1; // 마지막 이벤트 타입 (-1: 없음)
    private int _lastEventID = -1; // 마지막 선택된 이벤트 ID

    private void OnValidate()
    {
        // 이벤트 확률 합이 100을 초과하지 않도록 처리
        if (obviousErrorProbability + subtleErrorProbability > 100f)
        {
            obviousErrorProbability = 100f - subtleErrorProbability;
        }
    }

    public int GetRandomEvent()
    {
        int eventType = RandomType();
        int selectedEventID = -1;

        do
        {
            selectedEventID = RandomEventFromList(eventType);
        } while (selectedEventID == _lastEventID && selectedEventID != -1); // 동일한 이벤트 연속 선택 방지

        _lastEventID = selectedEventID;
        return selectedEventID;
    }

    private int RandomType()
    {
        int eventType;
        float rand = Random.value * 100f;
        if (rand < normalProbability) eventType = 0;
        else if (rand < normalProbability + subtleErrorProbability) eventType = 1;
        else eventType = 2;

        _lastEventType = eventType;
        return eventType;
    }

    private int RandomEventFromList(int eventType)
    {
        List<int> selectedList = null;
        switch (eventType)
        {
            case 2:
                selectedList = obviousErrorEventIDs;
                break;
            case 1:
                selectedList = subtleErrorEventIDs;
                break;
            default:
                selectedList = null;
                break;
        }

        if (selectedList != null && selectedList.Count > 0)
        {
            return selectedList[Random.Range(0, selectedList.Count)];
        }
        return -1;
    }

    public int GetLastEventType() => _lastEventType;

    public int GetLastEventID() => _lastEventID;
}

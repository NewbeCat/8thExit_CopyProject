using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, GameEvent> events = new Dictionary<string, GameEvent>();

    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterEvent(string eventName, GameEvent gameEvent)
    {
        if (!events.ContainsKey(eventName))
            events.Add(eventName, gameEvent);
    }

    public void CallEvent(string eventName)
    {
        if (events.ContainsKey(eventName))
            events[eventName].Call();
    }

    public void KillEvent(string eventName)
    {
        if (events.ContainsKey(eventName))
            events[eventName].Kill();
    }
}

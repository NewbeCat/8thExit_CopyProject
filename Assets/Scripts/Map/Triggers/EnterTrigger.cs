using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    [SerializeField] private RoomLoop _roomLoop;
    [SerializeField] private bool _isEntry;
    [SerializeField] private bool active;

    public void Initialize(RoomLoop room, bool entry)
    {
        _roomLoop = room;
        _isEntry = entry;
        active = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.CompareTag("Player"))
        {
            _roomLoop.EnterRoom(_isEntry);
        }
    }

    public void controlTrigger(bool change)
    {
        active = change;
    }
}

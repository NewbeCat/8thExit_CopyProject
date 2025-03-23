using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    [SerializeField] private RoomLoop _roomLoop;
    [SerializeField] private bool _isEntry;

    public void Initialize(RoomLoop room, bool entry)
    {
        _roomLoop = room;
        _isEntry = entry;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _roomLoop.EnterRoom(_isEntry);
        }
    }
}

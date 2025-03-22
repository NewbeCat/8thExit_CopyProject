using UnityEngine;

public class WarpTrigger : MonoBehaviour
{
    [SerializeField] private RoomLoop _roomLoop;
    [SerializeField] private bool _yesAnswer;

    public void Initialize(RoomLoop room, bool yes)
    {
        _roomLoop = room;
        _yesAnswer = yes;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _roomLoop.WarpConnection(_yesAnswer);
        }
    }
}

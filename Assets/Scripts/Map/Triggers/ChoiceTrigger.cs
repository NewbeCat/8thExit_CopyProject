using UnityEngine;

public class ChoiceTrigger : MonoBehaviour
{
    [SerializeField] private RoomLoop _roomLoop;
    [SerializeField] private bool _choice; //false=noSelect, true=yesSelect

    public void Initialize(RoomLoop room, bool choice)
    {
        _roomLoop = room;
        _choice = choice;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _roomLoop.ChoiceEvent(_choice);
        }
    }
}

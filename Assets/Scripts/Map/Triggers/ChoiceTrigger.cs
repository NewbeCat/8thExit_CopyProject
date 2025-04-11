using UnityEngine;

public class ChoiceTrigger : MonoBehaviour
{
    [SerializeField] private RoomLoop _roomLoop;
    [SerializeField] private bool _yesAnswer; //false=noSelect, true=yesSelect

    [SerializeField] private bool active;

    public void Initialize(RoomLoop room, bool yesAnswer)
    {
        _roomLoop = room;
        _yesAnswer = yesAnswer;
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.CompareTag("Player"))
        {
            _roomLoop.ChoiceEvent(_yesAnswer);
        }
    }

    public void controlTrigger(bool change)
    {
        active = change;
    }
}

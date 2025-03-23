using UnityEngine;

public class RoomLoop : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private WarpZone _warpZone;
    [SerializeField] private EventProbabilityManager _eventPicker;
    [SerializeField] private RoomLoop otherRoom;
    [SerializeField] private GameObject _posterNormal;
    [SerializeField] private GameObject _posterCorrect;


    [Header("Triggers")]
    [SerializeField] private GameObject entryRoom;
    [SerializeField] private GameObject exitRoom;
    [SerializeField] private GameObject choiceNo;
    [SerializeField] private GameObject choiceYes;

    [Header("Variables")]
    [SerializeField] private bool isRoom2;
    [SerializeField] private int roomEvent;
    private bool notInRoom;

    //trigger functions
    public void EnterRoom(bool isEntry)
    {
        toggleChoiceAndEntry();
        otherRoom.Reset();
        if (isEntry)
        {
            _eventPicker.GetRandomEvent();
            roomEvent = _eventPicker.GetEventType();
            callEvent(roomEvent);
            if (roomEvent == 0)
            {
                _warpZone.setWarpFlip(false);
            }
            else
            {
                _warpZone.setWarpFlip(true);
                togglePosters(false);
            }
        }
        else
        {
            _warpZone.resetRoom();
        }
    }

    public void ChoiceEvent(bool choseYes)
    {
        toggleChoiceAndEntry();
        if ((roomEvent == 0) == (choseYes))
        { //맞음
            _warpZone.addRoom();
        }
        else // 틀림
        {
            _warpZone.resetRoom();
            togglePosters();
        }
    }

    public void WarpConnection(bool _yesAnswer)
    {
        Debug.Log("entered to warp, you said yes? " + _yesAnswer);
        _warpZone.WarpPlayer(isRoom2, _yesAnswer);
    }


    //private
    private void Awake()
    {
        Reset();
    }

    private void Reset()
    {
        toggleChoiceAndEntry(true);
        togglePosters();
        killEvent();
    }

    private void callEvent(int eventCode)
    {
        if (eventCode == 0) { return; }
        else
        {
            Debug.Log("current Event called is" + eventCode);
        }
    }
    private void killEvent() { }


    //finished private
    private void toggleChoiceAndEntry(bool force = false)
    {
        notInRoom = (force) ? true : !notInRoom;
        choiceYes.SetActive(!notInRoom);
        choiceNo.SetActive(!notInRoom);
        entryRoom.SetActive(notInRoom);
        exitRoom.SetActive(notInRoom);
    }

    private void togglePosters(bool normal = true)
    {
        _posterNormal.SetActive(normal);
        _posterCorrect.SetActive(!normal);
    }
}

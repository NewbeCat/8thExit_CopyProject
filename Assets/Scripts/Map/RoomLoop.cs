using UnityEngine;

public class RoomLoop : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private WarpZone _warpZone;
    [SerializeField] private EventProbabilityManager _eventPicker;
    [SerializeField] private EventManager eventManager;
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
    [SerializeField] private int eventType;
    [SerializeField] private int eventID;
    private bool notInRoom;

    //trigger functions
    public void EnterRoom(bool isEntry)
    {
        toggleChoiceAndEntry();
        otherRoom.Reset();
        if (isEntry)
        {
            _eventPicker.GetRandomEvent();
            eventType = _eventPicker.GetEventType();
            eventID = _eventPicker.GetEventID();
            callEvent();
            if (eventType == 0)
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
        if ((eventType == 0) == (choseYes))
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

    private void callEvent()
    {
        if (eventType == 0) { return; }
        else
        {
            Debug.Log("currently event is " + eventType + " - " + eventID);
            eventManager.CallEvent(eventType, eventID, isRoom2);
        }
    }
    private void killEvent()
    {
        eventManager.KillEvent();
    }


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

using UnityEngine;

public class RoomLoop : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private EventProbabilityManager _eventPicker;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private WarpZone warpZone;

    [Header("Triggers")]
    [SerializeField] private EnterTrigger entryRoom;
    [SerializeField] private EnterTrigger exitRoom;
    [SerializeField] private ChoiceTrigger choiceNo;
    [SerializeField] private ChoiceTrigger choiceYes;

    [Header("Posters")]
    [SerializeField] private GameObject _posterNormal1;
    [SerializeField] private GameObject _posterChanged1;
    [SerializeField] private GameObject _posterNormal2;
    [SerializeField] private GameObject _posterChanged2;
    [SerializeField] private GameObject startPoster;
    [SerializeField] private GameObject startWall;

    [Header("Variables")]
    [SerializeField] private int eventType;
    [SerializeField] private int eventID;
    private bool notInRoom;
    private bool start = true;

    //trigger functions
    public void EnterRoom(bool _isEntry)
    {
        toggleChoiceAndEntry();

        if (!_isEntry)
        {
            warpZone.resetRoom();
            if (eventType != 0)
            {
                eventType = 0;
                eventID = -1;
                callEvent();
            }
        }
        else
        {
            //다음 번호 선택 & preload?
            _eventPicker.GetRandomEvent(start);
            if (start) start = false;
            eventType = _eventPicker.GetEventType();
            eventID = _eventPicker.GetEventID();
            callEvent();
        }

    }

    public void ChoiceEvent(bool _yesAnswer)
    {
        toggleChoiceAndEntry();

        //답 따라 워프 조정 & 포스터 조정
        if (!_yesAnswer && eventType != 0 && eventType != 8)
        { // no 답이 맞았을 경우
            warpZone.setWarpFlip(true);
            togglePosters(false);
        }
        else
        {
            warpZone.setWarpFlip(false);
            togglePosters(true);
        }

        //답 따라 숫자 조정
        if (_yesAnswer == (eventType == 0 || eventType == -1 || eventType == 8))
        {
            warpZone.addRoom();
        }
        else
        {
            warpZone.resetRoom();
        }
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
    }

    private void callEvent()
    {
        eventManager.CallEvent(eventType, eventID);
    }

    //finished private
    private void toggleChoiceAndEntry(bool force = false)
    {
        notInRoom = (force) ? true : !notInRoom;
        choiceYes.controlTrigger(!notInRoom);
        choiceNo.controlTrigger(!notInRoom && !start);
        entryRoom.controlTrigger(notInRoom);
        exitRoom.controlTrigger(notInRoom);
    }

    private void togglePosters(bool normal = true)
    {
        _posterNormal1.SetActive(normal && !start);
        _posterChanged1.SetActive(!normal);
        _posterNormal2.SetActive(normal && !start);
        _posterChanged2.SetActive(!normal);
        startPoster.SetActive(start);
        startWall.SetActive(start);
    }
}

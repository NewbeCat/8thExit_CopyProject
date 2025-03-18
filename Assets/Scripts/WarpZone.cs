using UnityEngine;
using System.Collections.Generic;

public class WarpZone : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private CharacterController _playerController;


    [Header("Loop Settings")]
    public int _curRoomNum = 0;
    [SerializeField] int _maxRoomNum = 8; //시작방과 끝 방 제외
    [SerializeField] private List<Transform> _transformA = new List<Transform>();
    [SerializeField] private List<Transform> _transformB = new List<Transform>();
    [SerializeField] private int _lastEntryType = -1; // start = 0, end = 1, AorB = 2
    [SerializeField] private bool _lastEntryGroup = false; // A=false, B=true

    [Header("Event Settings")]
    [SerializeField] private EventProbabilityManager eventManager;
    [SerializeField] private int _curEvent = -1;

    private void Start()
    {
        _playerController = _player.GetComponent<CharacterController>();
    }

    public void WarpPlayer(int currentEntryType, bool currentEntryGroup)
    {
        bool correct = (_lastEntryGroup == currentEntryGroup) ? (_curEvent == 0) : (_curEvent != 0);

        Debug.Log($"you chose {(_lastEntryGroup == currentEntryGroup ? "YES" : "NO")}");
        Debug.Log(correct ? "you were right" : "you were wrong, resetting");

        if (correct)
        {
            if (_curRoomNum > _maxRoomNum)
            {
                WarpAmount(currentEntryType, currentEntryGroup, 1);
                _curEvent = 0;
            }
            else
            {
                WarpAmount(currentEntryType, currentEntryGroup, 2);
                int eventManagerCheck = eventManager.GetRandomEvent();
                _curEvent = eventManager.GetLastEventType();
                Debug.Log("Next Event Type is" + _curEvent);
                Debug.Log($"Event triggered: {eventManagerCheck}");
                _curRoomNum++;
            }
        }
        else
        {
            WarpAmount(currentEntryType, currentEntryGroup, 0);
            _curEvent = -1;
            _curRoomNum = 0;
        }
        _lastEntryType = currentEntryType;
        _lastEntryGroup = currentEntryGroup;
        Debug.Log($"Updating last entry: Type={currentEntryType}, Group={currentEntryGroup}");

    }


    private void WarpAmount(int no1, bool group, int no2)
    {
        Debug.Log("warping from " + no1 + " to " + no2);
        Transform start = (!group) ? _transformA[no1] : _transformB[no1];
        Transform end = (group) ? _transformA[no2] : _transformB[no2];
        _playerController.enabled = false;
        _player.transform.position += new Vector3(end.position.x - start.position.x, end.position.y - start.position.y, end.position.z - start.position.z);
        _playerController.enabled = true;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class WarpZone : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private CharacterController _playerController;


    [Header("Loop Settings")]
    public int _curRoomNum = 0;
    [SerializeField] int _maxRoomNum = 8; //시작방과 끝 방 제외
    private int _lastEntryType = -1; // start = 0, end = 1, normalIn = 2, normalOut = 3
    private bool _lastEntryGroup = false; // A=false, B=true

    [Header("Event Settings")]
    [SerializeField] private int _curEvent = -1;

    [Header("Connections")]
    [SerializeField] private EventProbabilityManager eventManager;
    [SerializeField] private List<Transform> _transformA = new List<Transform>();
    [SerializeField] private List<Transform> _transformB = new List<Transform>();

    private void Start()
    {
        _playerController = _player.GetComponent<CharacterController>();
    }

    public void WarpPlayer(int currentEntryType, bool currentEntryGroup)
    {
        bool correct = (_lastEntryGroup == currentEntryGroup) ? (_curEvent == 0) : (_curEvent != 0);

        if (correct)
        {
            if (_curRoomNum >= _maxRoomNum)
            {
                WarpAmount(currentEntryType, currentEntryGroup, 1);
                _curEvent = 0;
            }
            else
            {
                WarpAmount(currentEntryType, currentEntryGroup, 2);
                eventManager.GetRandomEvent(_curEvent);
                _curEvent = eventManager.GetLastEventType();
                _curRoomNum++;
                Debug.Log($"This Event triggered: {eventManager.GetLastEventID()}");
                Debug.Log("This Event Type is" + _curEvent);
            }
        }
        else
        {
            WarpAmount(currentEntryType, currentEntryGroup, 0);
            _curEvent = -1;
            _curRoomNum = 0;
            eventManager.ResetProbabilities();
        }

        _lastEntryType = currentEntryType;
        _lastEntryGroup = currentEntryGroup;
    }


    private void WarpAmount(int no1, bool group, int no2)
    {
        Transform start = (!group) ? _transformA[no1] : _transformB[no1];
        Transform end = (group) ? _transformA[no2] : _transformB[no2];
        _playerController.enabled = false;
        _player.transform.position += new Vector3(end.position.x - start.position.x, end.position.y - start.position.y, end.position.z - start.position.z);
        _playerController.enabled = true;
    }
}

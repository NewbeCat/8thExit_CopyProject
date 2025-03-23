using UnityEngine;
using System.Collections.Generic;

public class WarpZone : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private EventProbabilityManager eventManager;
    [SerializeField] private GameObject _player;
    private CharacterController _playerController;

    [Header("Loop Settings")]
    public int _curRoomNum = 0; // 포스터 전부 해당 번호대로 변경됨
    [SerializeField] private int _maxRoomNum = 8; // 시작방과 끝 방 제외
    private bool _flip = false;

    [Header("Transforms")]
    [SerializeField] private GameObject pointNo;
    [SerializeField] private GameObject pointYes;
    [SerializeField] private Transform room1;
    [SerializeField] private Transform room2;
    [SerializeField] private Transform turnCorridor;

    [Header("Travel length - 거리 확정될 경우 Transforms 삭제!!")]
    [SerializeField] private Vector3 roomWarp;
    [SerializeField] private Vector3 colsWarp;
    [SerializeField] private Vector3 turnAround;

    public void WarpPlayer(bool whichRoom, bool yesAnswer) //room1, yes - false true
    {
        if (_curRoomNum >= _maxRoomNum)
        {
            //whichRoom와 yesAnswer로 위치 파악해 endRoom 부르기 & 이동은 안하기!
            //or
            //warp하되 위치 설정 달라야함
            Debug.Log("You reached the endRoom!! As long as you don't go back to prev room - you won");
            return;
        }

        Vector3 offset;
        bool twerl = false;
        if (yesAnswer || !_flip)
        {
            offset = ((!whichRoom) ? roomWarp : -roomWarp) + ((yesAnswer) ? colsWarp : -colsWarp);
        }
        else
        {
            //_player.transform.Rotate(0, 180, 0);
            offset = 2 * (turnAround + -_player.transform.position);
            offset = roomWarp + new Vector3(offset.x, 0f, offset.z);
            twerl = true;
        }

        Debug.Log("playing warpplayer" + offset.x + "/" + offset.y + "/" + offset.z);
        Debug.Log("twerl : " + twerl);
        WarpAmount(offset, twerl);
    }


    //finished private
    private void Start()
    {
        _playerController = _player.GetComponent<CharacterController>();

        roomWarp = room2.position - room1.position; // 룸1 → 룸2 이동 벡터
        colsWarp = pointNo.transform.position - pointYes.transform.position; // 나가는 길 → 들어오는 길 이동 벡터
        turnAround = turnCorridor.position; // 회전 기준 위치
    }
    private void WarpAmount(Vector3 amount, bool twerl)
    {
        _playerController.enabled = false;
        _player.transform.position += amount;
        if (twerl) _player.transform.Rotate(0, 180, 0);
        _playerController.enabled = true;
    }


    //simple variable public
    public void addRoom()
    {
        _curRoomNum++;
    }
    public void resetRoom()
    {
        _curRoomNum = 0;
        //deactivate all endrooms
    }
    public void setWarpFlip(bool flip)
    {
        _flip = flip;
    }

}
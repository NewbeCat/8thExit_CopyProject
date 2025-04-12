using UnityEngine;
using System.Collections.Generic;

public class WarpZone : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private EventProbabilityManager eventManager;
    [SerializeField] private HallPoster hallPoster;
    [SerializeField] private GameObject _player;
    private CharacterController _playerController;

    [Header("Loop Settings")]
    public int _curRoomNum = 0; // 포스터 전부 해당 번호대로 변경됨
    [SerializeField] private int _maxRoomNum = 8; // 시작방과 끝 방 제외
    private bool _flip = false;

    [Header("Transforms")]
    [SerializeField] private Transform inRoomOuter;
    [SerializeField] private Transform inRoomInner;
    [SerializeField] private Transform outRoomOuter;
    [SerializeField] private Transform outRoomInner;


    public void WarpPlayer(bool outCoridoor) //직행 맞음 - outRoomOuter -> inRoomInner, 직행 틀림 - , 돌아감 맞음 inRoomOuter -> inRoomInner , 돌아감 틀림 inRoomOuter -> outRoomInner
    {
        if (_curRoomNum >= _maxRoomNum)
        {
            //whichRoom와 yesAnswer로 위치 파악해 endRoom 부르기 & 이동은 안하기!
            //or
            //warp하되 위치 설정 달라야함
            Debug.Log("You reached the endRoom!! As long as you don't go back to prev room - you won");
            return;
        }

        Vector3 offset; //끝 - 시작
        bool twerl = false;
        if (_flip)
        {
            offset = (!outCoridoor) ? 2 * (inRoomOuter.position - _player.transform.position) : 2 * (outRoomOuter.position - _player.transform.position);
            offset = -(outRoomInner.position - outRoomOuter.position) + new Vector3(offset.x, 0f, offset.z);
            twerl = true;
        }
        else
        {
            offset = (!outCoridoor) ? (outRoomInner.position - inRoomOuter.position) : (inRoomInner.position - outRoomOuter.position);
        }

        Debug.Log("playing warpplayer" + offset.x + "/" + offset.y + "/" + offset.z);
        Debug.Log("twerl : " + twerl);
        WarpAmount(offset, twerl);
    }


    //finished private
    private void Start()
    {
        _player = Managers.Instance.Player.gameObject;
        _playerController = _player.GetComponent<CharacterController>();
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
        hallPoster.UpdatePosters(_curRoomNum);
    }
    public void resetRoom()
    {
        if (_curRoomNum > 0) _curRoomNum = 0;
        hallPoster.UpdatePosters(_curRoomNum);
        //deactivate all endrooms
    }
    public void setWarpFlip(bool flip)
    {
        _flip = flip;
    }

}
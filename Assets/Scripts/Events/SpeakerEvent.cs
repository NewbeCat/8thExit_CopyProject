using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "GameEvents/SpeakerEvent")]
public class SpeakerEvent : GameEvent
{
    [SerializeField] private List<GameObject> speakerList = new List<GameObject>();
    private GameObject speaker;
    private int randomEvent;

    [Header("위치 / 존재여부 / 그외 변화사항 추가시 문의 부탁")]
    [SerializeField] private List<Vector3> movedList = new List<Vector3>();

    private Vector3 originalPosition;

    protected override void Execute()
    {
        if (speakerList.Count == 0) return;

        speaker = speakerList[Random.Range(0, speakerList.Count)];
        randomEvent = Random.Range(0, 2);

        if (randomEvent == 0 && movedList.Count > 0)
        {
            originalPosition = speaker.transform.position;
            Vector3 moveOffset = movedList[Random.Range(0, movedList.Count)];
            speaker.transform.position += moveOffset;
        }
        else
        {
            speaker.SetActive(false);
        }
    }

    protected override void ResetEvent()
    {
        if (speaker == null) return;

        if (randomEvent == 0)
        {
            speaker.transform.position = originalPosition;
        }
        else
        {
            speaker.SetActive(true);
        }
    }
}
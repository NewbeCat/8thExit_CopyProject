using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "GameEvents/SpeakerEvent")] //ScriptableObject 생성 후 Addressible 켜기
public class SpeakerEvent : GameEvent
{
    private string speakerTag = "Speakers"; // 태그를 통해 찾기!!
    [SerializeField] private List<Vector3> movedList = new List<Vector3>();

    private GameObject speaker;
    private Vector3 originalPosition;
    private int randomEvent;

    protected override void Execute()
    {
        GameObject[] speakers = ObjectsByTag(speakerTag); //태그를 통해 필요한 오브젝트 찾기. 룸 지정은 자동으로 됨
        if (speakers.Length == 0) return;

        //speaker = speakers[Random.Range(0, speakers.Length)];
        //randomEvent = Random.Range(0, 2);
        speaker = speakers[0];
        randomEvent = 1;

        if (randomEvent == 0 && movedList.Count > 0)
        {
            originalPosition = speaker.transform.position;
            Vector3 moveOffset = movedList[Random.Range(0, movedList.Count)];
            speaker.transform.position += moveOffset;
        }
        else
        {
            randomEvent = 1; // movedList가 비엇을시 대비책, 수정 예정
            Debug.Log("Speaker before deactivation: " + speaker.name + ", Active: " + speaker.activeSelf + ", Heirarchy: " + speaker.activeInHierarchy);
            speaker.SetActive(false);
            Debug.Log("Speaker after deactivation: " + speaker.name + ", Active: " + speaker.activeSelf + ", Heirarchy: " + speaker.activeInHierarchy);

        }
    }

    protected override void ResetEvent()
    {
        Debug.Log("resetting speaker event");
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

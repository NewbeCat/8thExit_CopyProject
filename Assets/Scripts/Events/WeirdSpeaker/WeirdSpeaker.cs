using UnityEngine;
using AK.Wwise;
using System.Collections;

public class WeirdSpeaker : MonoBehaviour
{
    [SerializeField] private GameObject loopSpeakerObject; // 루프 사운드 재생 오브젝트 (AkAmbient 붙임)
    [SerializeField] private AK.Wwise.Event speakerOneShotEvent; // 1회 효과음 이벤트 (드래그 앤 드랍)

    [SerializeField] private BoxEventTrigger BoxEventTrigger;
    [SerializeField] private BoxEventTrigger[] BoxEventEndTriggers;

    private bool isPlayed;

    private void Awake()
    {
        BoxEventTrigger.OnTrigger += PlayTrigger;

        foreach (var item in BoxEventEndTriggers)
        {
            item.OnTrigger += StopTrigger;
        }

        if (loopSpeakerObject != null)
            loopSpeakerObject.SetActive(false); // 시작 시 루프 사운드 비활성화
    }

    private void PlayTrigger()
    {
        if (isPlayed)
        {
            return;
        }

        isPlayed = true;

        // 루프 사운드 오브젝트 활성화 → AkAmbient의 "Trigger on Enable"로 자동 재생
        loopSpeakerObject.SetActive(true);

        // One-shot 효과음 재생
        speakerOneShotEvent.Post(loopSpeakerObject); // 또는 gameObject
    }

    private void StopTrigger()
    {
        loopSpeakerObject.SetActive(false);
    }
}

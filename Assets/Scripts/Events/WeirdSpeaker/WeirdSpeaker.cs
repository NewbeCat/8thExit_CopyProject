using UnityEngine;
using AK.Wwise;
using System.Collections;

public class WeirdSpeaker : MonoBehaviour
{
    [SerializeField] private GameObject loopSpeakerObject; // ���� ���� ��� ������Ʈ (AkAmbient ����)
    [SerializeField] private AK.Wwise.Event speakerOneShotEvent; // 1ȸ ȿ���� �̺�Ʈ (�巡�� �� ���)

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
            loopSpeakerObject.SetActive(false); // ���� �� ���� ���� ��Ȱ��ȭ
    }

    private void PlayTrigger()
    {
        if (isPlayed)
        {
            return;
        }

        isPlayed = true;

        // ���� ���� ������Ʈ Ȱ��ȭ �� AkAmbient�� "Trigger on Enable"�� �ڵ� ���
        loopSpeakerObject.SetActive(true);

        // One-shot ȿ���� ���
        speakerOneShotEvent.Post(loopSpeakerObject); // �Ǵ� gameObject
    }

    private void StopTrigger()
    {
        loopSpeakerObject.SetActive(false);
    }
}

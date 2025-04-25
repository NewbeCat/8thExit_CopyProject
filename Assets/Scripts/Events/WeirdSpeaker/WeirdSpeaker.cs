using UnityEngine;

public class WeirdSpeaker : MonoBehaviour
{
    [SerializeField] private AudioSource loopSpeaker;
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
    }

    private void PlayTrigger()
    {
        if (isPlayed)
        {
            return;
        }

        isPlayed = true;
        loopSpeaker.clip = Managers.Instance.Sound.GetAudioClip(ESoundClip.SpeakerLoop);
        loopSpeaker.Play();
        Managers.Instance.Sound.PlaySFX(ESoundClip.SpeakerOneShot, loopSpeaker.transform.position);
    }

    private void StopTrigger()
    {
        loopSpeaker.Stop();
    }
}

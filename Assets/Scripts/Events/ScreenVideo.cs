using UnityEngine;
using UnityEngine.Video;

public class ScreenVideo : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Material originMaterial;
    [SerializeField] private Material afterMeterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxEventTrigger boxEventTrigger;
    // 임시 추가
    [SerializeField] private AudioSource source;

    private bool isFirstOn;

    private void Awake()
    {
        meshRenderer.sharedMaterial = originMaterial;
        boxEventTrigger.OnTrigger += OnTrigger;
    }

    private void OnTrigger()
    {
        if (isFirstOn)
        {
            return;
        }
        meshRenderer.sharedMaterial = afterMeterial;
        videoPlayer.Play();
        isFirstOn = true;
        source.clip = Managers.Instance.Sound.GetAudioClip(ESoundClip.ScreenVideo);
        source.Play();
    }

}

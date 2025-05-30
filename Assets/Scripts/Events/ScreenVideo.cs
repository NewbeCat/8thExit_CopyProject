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
    [SerializeField] private GameObject soundEmitterObject;

    private bool isFirstOn;

    private void Awake()
    {
        meshRenderer.sharedMaterial = originMaterial;
        boxEventTrigger.OnTrigger += OnTrigger;

        // 처음엔 사운드 오브젝트 비활성화 (자동 재생 방지)
        if (soundEmitterObject != null)
        {
            soundEmitterObject.SetActive(false);
        }
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

        // 3D 사운드 오브젝트 활성화 → AkAmbient 사운드 재생
        if (soundEmitterObject != null)
        {
            soundEmitterObject.SetActive(true);
        }
    }
}

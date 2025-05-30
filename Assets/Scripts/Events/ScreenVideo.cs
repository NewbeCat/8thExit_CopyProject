using UnityEngine;
using UnityEngine.Video;

public class ScreenVideo : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Material originMaterial;
    [SerializeField] private Material afterMeterial;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxEventTrigger boxEventTrigger;
    // �ӽ� �߰�
    [SerializeField] private GameObject soundEmitterObject;

    private bool isFirstOn;

    private void Awake()
    {
        meshRenderer.sharedMaterial = originMaterial;
        boxEventTrigger.OnTrigger += OnTrigger;

        // ó���� ���� ������Ʈ ��Ȱ��ȭ (�ڵ� ��� ����)
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

        // 3D ���� ������Ʈ Ȱ��ȭ �� AkAmbient ���� ���
        if (soundEmitterObject != null)
        {
            soundEmitterObject.SetActive(true);
        }
    }
}

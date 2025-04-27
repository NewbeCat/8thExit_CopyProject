using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class ScreenHand : MonoBehaviour
{
    [SerializeField] private Vector3 originPos;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveDuration;
    [SerializeField] private Transform handTransform;

    [SerializeField] private BoxEventTrigger boxEventTrigger;

    [SerializeField] private AudioSource audioSource; 

    private bool isShowed;

    private void Awake()
    {
        boxEventTrigger.OnTrigger += Show;
        moveDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        handTransform.gameObject.SetActive(false);
    }

    private void Show()
    {
        if (isShowed)
        {
            return;
        }

        // »ç¿îµå
        isShowed = true;
        handTransform.gameObject.SetActive(true);
        audioSource.clip = Managers.Instance.Sound.GetAudioClip(ESoundClip.ScreenHand);
        audioSource.Play();
        handTransform.DOLocalMove(targetPos, moveDuration).SetEase(Ease.OutQuad);
    }
}

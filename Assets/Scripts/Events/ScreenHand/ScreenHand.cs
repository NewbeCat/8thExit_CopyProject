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

    private bool isShowed;

    private AkAmbient handAkAmbient;

    private void Awake()
    {
        boxEventTrigger.OnTrigger += Show;
        moveDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        handAkAmbient = handTransform.GetComponent<AkAmbient>();
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

        if (handAkAmbient != null)
        {
            handAkAmbient.HandleEvent(handTransform.gameObject);
        }

        handTransform.DOLocalMove(targetPos, moveDuration).SetEase(Ease.OutQuad);
    }
}

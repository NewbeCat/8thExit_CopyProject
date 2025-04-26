using DG.Tweening;
using UnityEngine;

public class ScreenHand : MonoBehaviour
{
    [SerializeField] private Vector3 originPos;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveDuration;
    [SerializeField] private Transform handTransform;

    [SerializeField] private BoxEventTrigger boxEventTrigger;

    private bool isShowed;

    private void Awake()
    {
        boxEventTrigger.OnTrigger += Show;
        moveDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        originPos = handTransform.position;
        targetPos = handTransform.position + new Vector3(0, 0, -5);
        handTransform.gameObject.SetActive(false);
    }

    private void Show()
    {
        if (isShowed)
        {
            return;
        }

        // »ç¿îµå
        handTransform.gameObject.SetActive(true);
        isShowed = true;
        handTransform.DOMove(targetPos, moveDuration).SetEase(Ease.OutQuad);
    }
}

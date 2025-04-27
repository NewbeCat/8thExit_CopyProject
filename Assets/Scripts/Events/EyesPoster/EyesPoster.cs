using UnityEngine;

public class EyesPoster : MonoBehaviour
{
    [SerializeField] private float clampX = 0.2f;
    [SerializeField] private Transform pupilTrans;

    [SerializeField] private bool isReverse;

    private float diff;

    private void Update()
    {
        float currentDiff = Managers.Instance.Player.transform.position.x - transform.position.x;
        if (diff != currentDiff)
        {
            diff = isReverse ? -currentDiff : currentDiff;
            Vector3 pupilLocalPos = pupilTrans.localPosition;
            pupilLocalPos.x = Mathf.Clamp(-diff, -clampX, clampX);
            pupilTrans.localPosition = pupilLocalPos;
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

// First Person View and using only One Camera in Player -> Controller
public class CameraController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform playerTransform;
    [Header("Reversal Option")]
    [SerializeField] private bool horizontalReversal = false;
    [SerializeField] private bool verticalReversal = false;
    [Header("Sensitivity")]
    [SerializeField] private float horizontalSensitivity = 1.0f;
    [SerializeField] private float verticalSensitivity = 1.0f;
    [Header("RotationX Limitation")]
    [SerializeField] private float upAngleLimitation = -90f;
    [SerializeField] private float downAngleLimitation = 90f;
    #endregion

    #region Unity Methods
    private void LateUpdate()
    {

    }
    #endregion

    #region Init Methods
    public void Init()
    {
        Managers.Instance.Input.look += UpdateCameraRotation;
    }
    #endregion

    private void UpdateCameraRotation(Vector2 delta)
    {
        // reversal option
        delta.x = horizontalReversal ? -delta.x : delta.x;
        delta.y = verticalReversal ? -delta.y : delta.y;

        // in unity, rotation order Z -> Y -> X
        Quaternion rotationY = Quaternion.Euler(new Vector3(0, delta.x, 0));
        playerTransform.rotation *= rotationY;

        // limit rotationX to read easy
        // localRotation.x is Quaternion value (0~1)
        float rotationX = transform.localEulerAngles.x - delta.y;
        // unity eulerAngle's scope is 0~360
        // transform rotation is -180~180
        if (rotationX > 180f)
        {
            rotationX -= 360f;
        }

        rotationX = Mathf.Clamp(rotationX, upAngleLimitation, downAngleLimitation);

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}

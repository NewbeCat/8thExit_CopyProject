using System;
using UnityEngine;

// First Person View and using only One Camera in Player -> Controller
public class CameraController : MonoBehaviour
{
    #region Fields
    [SerializeField] private Transform _playerTransform;
    [Header("Reversal Option")]
    [SerializeField] private bool _horizontalReversal = false;
    [SerializeField] private bool _verticalReversal = false;
    [Header("Sensitivity")]
    [SerializeField][Range(0, 3f)] private float _horizontalSensitivity = 1.0f;
    [SerializeField][Range(0, 3f)] private float _verticalSensitivity = 1.0f;
    [Header("RotationX Limitation")]
    [SerializeField] private float _upAngleLimitation = -90f;
    [SerializeField] private float _downAngleLimitation = 90f;
    #endregion

    #region Unity Methods
    public float shakeIntensity = 0.5f; // 흔들림 강도
    public float shakeDuration = 1.5f; // 흔들림 지속 시간
    public float fallSpeed = 2.0f; // 카메라 떨어지는 속도

    public Vector3 originalPosition;

    public bool isPlayerable = true;

    #endregion

    private void OnDestroy()
    {
        Managers.Instance.Input.look -= UpdateCameraRotation;
    }

    #region Init Methods
    public void Init()
    {
        Managers.Instance.Input.look += UpdateCameraRotation;
    }
    #endregion

    private void UpdateCameraRotation(Vector2 delta)
    {
        if (!isPlayerable && _playerTransform == null)
        {
            return;
        }

        // reversal option
        delta.x = _horizontalReversal ? -delta.x : delta.x;
        delta.y = _verticalReversal ? -delta.y : delta.y;

        // in unity, rotation order Z -> Y -> X
        Quaternion rotationY = Quaternion.Euler(new Vector3(0, delta.x * _horizontalSensitivity, 0));
        if (_playerTransform != null)
        {
            _playerTransform.rotation *= rotationY;
        }

        // limit rotationX to read easy
        // localRotation.x is Quaternion value (0~1)
        float rotationX = transform.localEulerAngles.x - delta.y * _verticalSensitivity;
        // unity eulerAngle's scope is 0~360
        // transform rotation is -180~180
        if (rotationX > 180f)
        {
            rotationX -= 360f;
        }

        rotationX = Mathf.Clamp(rotationX, _upAngleLimitation, _downAngleLimitation);

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    public void DoDeadMotion()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(_playerTransform.position.x, _playerTransform.position.y + 1, _playerTransform.position.z), fallSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(Vector3.up);  // 하늘을 향하도록 회전
    }

    public void DoShakeMotion()
    {
        transform.position = originalPosition + UnityEngine.Random.insideUnitSphere * shakeIntensity;
    }
}

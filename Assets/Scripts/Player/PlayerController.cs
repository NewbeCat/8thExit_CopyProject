using UnityEngine;

public class PlayerController : MonoBehaviour, IManager
{
    #region Fields
    [field: Header("Movement Fields")]
    [field: SerializeField] public PlayerMovementStateModule playerMovementModule { get; private set; }
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _sprintSpeed = 4f;
    [SerializeField] private float _footStepUnitTime = 0.5f;
    #endregion

    #region Unity Methods
    private void FixedUpdate()
    {
        playerMovementModule.FixedUpdate();
    }

    private void Update()
    {
        playerMovementModule.Update();
    }
    #endregion

    #region Init Methods
    public void Init()
    {
        _cameraController.Init();
        playerMovementModule = new PlayerMovementStateModule(_characterController, EPlayerMovement.Idle, new IdleState(this), _walkSpeed, _sprintSpeed, _footStepUnitTime);
        playerMovementModule.TryAddState(EPlayerMovement.Walk, new WalkState(this));
        playerMovementModule.TryAddState(EPlayerMovement.Sprint, new SprintState(this));
    }
    #endregion
}

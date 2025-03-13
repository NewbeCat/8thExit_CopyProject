using UnityEngine;

public class PlayerController : MonoBehaviour, IManager
{
    #region Fields
    [field: Header("Movement Fields")]
    [field: SerializeField] public PlayerMovementStateModule playerMovementModule { get; private set; }
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 4f;
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
        cameraController.Init();
        playerMovementModule = new PlayerMovementStateModule(characterController, EPlayerMovement.Idle, new IdleState(this), walkSpeed, sprintSpeed);
        playerMovementModule.TryAddState(EPlayerMovement.Walk, new WalkState(this));
        playerMovementModule.TryAddState(EPlayerMovement.Sprint, new SprintState(this));
    }
    #endregion
}

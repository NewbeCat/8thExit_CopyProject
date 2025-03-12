using UnityEngine;

public class PlayerController : MonoBehaviour, IManager
{
    [field: SerializeField] public PlayerMovementStateModule playerMovementModule { get; private set; }
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float sprintSpeed = 4f;

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

    public void Init()
    {
        playerMovementModule = new PlayerMovementStateModule(characterController, EPlayerMovement.Idle, new IdleState(this), walkSpeed, sprintSpeed);
        playerMovementModule.TryAddState(EPlayerMovement.Walk, new WalkState(this));
        playerMovementModule.TryAddState(EPlayerMovement.Sprint, new SprintState(this));
    }


}

using System;
using UnityEngine;

[Serializable]
public class PlayerMovementStateModule : StateMachineBase<EPlayerMovement>
{
    #region Fields
    [Header("Movement Fields")]
    [field: Header("For InGame")]
    [field: SerializeField] public bool IsSprint { get; private set; }
    [field: SerializeField] public float InGameWalkSpeed { get; private set; }
    [field: SerializeField] public float InGameSprintSpeed { get; private set; }
    private Vector2 _moveDirection;
    private CharacterController _characterController;
    #endregion

    public PlayerMovementStateModule(CharacterController characterController, EPlayerMovement ePlayerMovement, IState<EPlayerMovement> newState, float walkSpeed, float sprintSpeed)
    {
        _characterController = characterController;
        InGameWalkSpeed = walkSpeed;
        InGameSprintSpeed = sprintSpeed;
        TryAddState(ePlayerMovement, newState);
        ChangeState(newState);
    }

    #region State Machine
    public override void ChangeState(IState<EPlayerMovement> newState)
    {
        base.ChangeState(newState);
    }

    public override void FixedUpdate()
    {
        currentState.FixedUpdate();
        ApplyGravity();
    }

    public override void Update()
    {
        currentState.Update();
    }

    public override void ExitState()
    {
        currentState.Exit();
    }

    public override bool TryAddState(EPlayerMovement movementType, IState<EPlayerMovement> newState)
    {
        bool isAdded = base.TryAddState(movementType, newState);
        if (isAdded)
        {
            switch (movementType)
            {
                case EPlayerMovement.Idle:
                    Managers.Instance.Input.movement += UpdateMoveDirection;
                    break;
                case EPlayerMovement.Walk:
                    // case EPlayerMovement.Walk is empty as subscribe duplication
                    break;
                case EPlayerMovement.Sprint:
                    Managers.Instance.Input.sprint += UpdateSprint;
                    break;
                default:
                    Debug.Assert(false, "case doesn't exist");
                    break;
            }
        }

        return isAdded;
    }
    #endregion

    #region Move Methods
    public void Move()
    {
        if (_moveDirection == Vector2.zero)
        {
            _characterController.Move(Vector2.zero);
            return;
        }

        float speed = IsSprint ? InGameSprintSpeed * Time.deltaTime : InGameWalkSpeed * Time.deltaTime;

        Vector3 finalDirection = _characterController.transform.right * _moveDirection.x + _characterController.transform.forward * _moveDirection.y;
        _characterController.Move(finalDirection.normalized * speed);
    }

    private void UpdateMoveDirection(Vector2 direction)
    {
        _moveDirection = direction;

        EPlayerMovement targetState = (direction == Vector2.zero) ? EPlayerMovement.Idle : 
            (IsSprint ? EPlayerMovement.Sprint : EPlayerMovement.Walk);

        if (TryGetState(targetState, out IState<EPlayerMovement> newState))
        {
            ChangeState(newState);
        }
    }

    private void UpdateSprint(bool isSprint)
    {
        IsSprint = isSprint;
    }

    private void ApplyGravity()
    {
        Vector3 velocity = _characterController.velocity;

        if (_characterController.isGrounded)
        {
            velocity.y = -1f;
        }
        else
        {
            velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        }

        _characterController.Move(new Vector3(0, velocity.y, 0));
    }
    #endregion
}

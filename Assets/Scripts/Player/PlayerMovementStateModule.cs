using System;
using UnityEngine;

[Serializable]
public class PlayerMovementStateModule : StateMachineBase<EPlayerMovement>
{
    public bool IsSprint { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; } = 0.25f;
    [field: SerializeField] public float SprintSpeed { get; private set; } = 0.5f;

    private Vector2 moveDirection;

    private CharacterController characterController;

    public PlayerMovementStateModule(CharacterController characterController, EPlayerMovement ePlayerMovement, IState<EPlayerMovement> newState)
    {
        this.characterController = characterController;
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
    private void UpdateMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
        if (direction == Vector2.zero)
        {
            if (TryGetState(EPlayerMovement.Idle, out IState<EPlayerMovement> newState))
            {
                ChangeState(newState);
            }
        }
        else
        {
            if (TryGetState(EPlayerMovement.Walk, out IState<EPlayerMovement> newState))
            {
                ChangeState(newState);
            }
        }
    }

    public void Move()
    {
        float speed = IsSprint ? SprintSpeed * Time.deltaTime : MoveSpeed * Time.deltaTime;
        characterController.Move(new Vector3(moveDirection.x, 0, moveDirection.y) * speed);
    }

    private void UpdateSprint(bool isSprint)
    {
        IsSprint = isSprint;
    }

    private void ApplyGravity()
    {
        Vector3 velocity = characterController.velocity;

        if (characterController.isGrounded)
        {
            velocity.y = -1f;
        }
        else
        {
            velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
        }

        characterController.Move(velocity);
    }
    #endregion
}

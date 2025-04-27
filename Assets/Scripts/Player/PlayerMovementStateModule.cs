using System;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;

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

    [field: Header("FootStep Fields")]
    private float _movingElapsedTime = 0f;
    private float _movingDifference = 2f;
    private float _footStepUnitTime = 0f;
    private bool _isLeftStep = true;
    private float _footStepUnitPosX;

    private ObjectPooler<BloodFootStep> pooler;
    private Material[] footStepMaterials;

    private bool isBloodStepActive;

    #endregion

    public PlayerMovementStateModule(CharacterController characterController, EPlayerMovement ePlayerMovement, 
        IState<EPlayerMovement> newState, BloodFootStep footStep, Material[] footStepMaterials, float walkSpeed, 
        float sprintSpeed, float footStepUnitTime)
    {
        _characterController = characterController;
        InGameWalkSpeed = walkSpeed;
        InGameSprintSpeed = sprintSpeed;
        TryAddState(ePlayerMovement, newState);
        ChangeState(newState);
        _movingDifference = sprintSpeed / walkSpeed;
        _footStepUnitTime = footStepUnitTime;
        _footStepUnitPosX = characterController.radius;
        pooler = new ObjectPooler<BloodFootStep>(footStep, null, 10, 100);

        this.footStepMaterials = new Material[footStepMaterials.Length];

        for (int i = 0; i < footStepMaterials.Length; i++)
        {
            this.footStepMaterials[i] = footStepMaterials[i];
        }
    }

    #region State Machine
    public override void ChangeState(IState<EPlayerMovement> newState)
    {
        base.ChangeState(newState);
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if (currentState == null)
        {
            return;
        }

        currentState.FixedUpdate();
        currentState.Update();
        ApplyGravity();
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

        float speed = IsSprint ? InGameSprintSpeed : InGameWalkSpeed;

        Vector3 finalDirection = _characterController.transform.right * _moveDirection.x + _characterController.transform.forward * _moveDirection.y;
        _characterController.Move(finalDirection.normalized * speed * Time.deltaTime);
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

    public void ResetMovingTime()
    {
        _movingElapsedTime = 0f;
    }

    public void IncreaseMovingTime()
    {
        _movingElapsedTime += IsSprint ? _movingDifference * Time.deltaTime : Time.deltaTime;
        if (_movingElapsedTime >= _footStepUnitTime)
        {
            _movingElapsedTime -= _footStepUnitTime;
            ESoundClip eSoundClip = IsSprint ? ESoundClip.Run : ESoundClip.Walk;
            Vector3 footStepPos = _characterController.transform.position;

            Debug.DrawRay(footStepPos, Vector3.down * 10f);

            if (Physics.Raycast(footStepPos, Vector3.down * 10f, out RaycastHit hit, 1 << LayerMask.NameToLayer("Floor")))
            {
                footStepPos = hit.point + new Vector3(0, 0.1f, 0);

                //footStepPos.x = _isLeftStep ? -_footStepUnitPosX : _footStepUnitPosX;

                if (isBloodStepActive)
                {
                    eSoundClip = IsSprint ? ESoundClip.RunBlood : ESoundClip.WalkBlood;
                    BloodFootStep bloodFootStep = pooler.Pool();
                    bloodFootStep.transform.position = footStepPos;
                    bloodFootStep.transform.rotation = Quaternion.LookRotation(_characterController.transform.forward);
                    bloodFootStep.MeshRenderer.sharedMaterial = _isLeftStep ? footStepMaterials[0] : footStepMaterials[1];
                }
            }

            Managers.Instance.Sound.PlaySFX(eSoundClip, footStepPos);
        }
    }

    #endregion

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

    public void OnUpdateBloodFootStepActiveState(bool isActive)
    {
        isBloodStepActive = isActive;
    }
}

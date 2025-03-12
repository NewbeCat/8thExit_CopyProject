using UnityEngine;

public abstract class PlayerMovementStateBase : IState<EPlayerMovement>
{
    protected PlayerController playerController;

    public PlayerMovementStateBase(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    #region State Methods
    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
    public EPlayerMovement GetStateType()
    {
        return playerController.playerMovementModule.GetStateType();
    }
    #endregion
}

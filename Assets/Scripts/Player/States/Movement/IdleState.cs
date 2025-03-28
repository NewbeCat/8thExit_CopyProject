using UnityEngine;

public class IdleState : PlayerMovementStateBase
{
    public IdleState(PlayerController playerController) : base(playerController)
    {
        
    }

    #region State Methods
    public override void Enter()
    {
        //playerController.playerMovementModule.ResetMovingTime();
        // TODO: 카메라 천천히 흔들리기 시작
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        playerController.playerMovementModule.Move();
    }

    public override void Exit()
    {
        // TODO: 카메라 흔들림 멈춤
    }
    #endregion
}

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
        // TODO: ī�޶� õõ�� ��鸮�� ����
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
        // TODO: ī�޶� ��鸲 ����
    }
    #endregion
}

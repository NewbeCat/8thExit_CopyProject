using UnityEngine;

public class IdleState : PlayerMovementStateBase
{
    public IdleState(PlayerController playerController) : base(playerController)
    {
        
    }

    public override void Enter()
    {
        // ī�޶� õõ�� ��鸮�� ����
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
        // ī�޶� ��鸲 ����
    }
}

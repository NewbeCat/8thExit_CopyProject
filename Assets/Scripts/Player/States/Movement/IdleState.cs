using UnityEngine;

public class IdleState : PlayerMovementStateBase
{
    public IdleState(PlayerController playerController) : base(playerController)
    {
        
    }

    public override void Enter()
    {
        // 카메라 천천히 흔들리기 시작
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
        // 카메라 흔들림 멈춤
    }
}

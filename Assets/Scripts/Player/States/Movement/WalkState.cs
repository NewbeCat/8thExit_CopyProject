using UnityEngine;

public class WalkState : PlayerMovementStateBase
{
    public WalkState(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {
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
    }
}

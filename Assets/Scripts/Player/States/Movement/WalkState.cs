using UnityEngine;

public class WalkState : PlayerMovementStateBase
{
    public WalkState(PlayerController playerController) : base(playerController)
    {
    }

    #region State Methods
    public override void Enter()
    {
       // TODO: 카메라 걷는 속도로 흔들림
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        playerController.playerMovementModule.Move();
        playerController.playerMovementModule.IncreaseMovingTime();
    }

    public override void Exit()
    {
        // TODO: 카메라 흔들림 초기화
    }

    #endregion
}

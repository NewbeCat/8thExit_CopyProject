using UnityEngine;

public class SprintState : PlayerMovementStateBase
{
    public SprintState(PlayerController playerController) : base(playerController)
    {
    }

    #region State Methods
    public override void Enter()
    {
        // TODO: 카메라 강하게 흔들림
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
        // 카메라 흔들림 초기화
    }
    #endregion
}

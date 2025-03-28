using UnityEngine;

public class WalkState : PlayerMovementStateBase
{
    public WalkState(PlayerController playerController) : base(playerController)
    {
    }

    #region State Methods
    public override void Enter()
    {
       // TODO: ī�޶� �ȴ� �ӵ��� ��鸲
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
        // TODO: ī�޶� ��鸲 �ʱ�ȭ
    }

    #endregion
}

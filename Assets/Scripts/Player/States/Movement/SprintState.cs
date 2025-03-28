using UnityEngine;

public class SprintState : PlayerMovementStateBase
{
    public SprintState(PlayerController playerController) : base(playerController)
    {
    }

    #region State Methods
    public override void Enter()
    {
        // TODO: ī�޶� ���ϰ� ��鸲
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
        // ī�޶� ��鸲 �ʱ�ȭ
    }
    #endregion
}

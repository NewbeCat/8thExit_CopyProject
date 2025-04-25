using UnityEngine;

public class BloodFootStepEventHandler : MonoBehaviour
{
    [SerializeField] private BoxEventTrigger[] triggerOn;
    [SerializeField] private BoxEventTrigger[] triggerOff;

    private void Start()
    {
        PlayerController playerController = Managers.Instance.Player;

        foreach (var item in triggerOn)
        {
            item.OnTrigger += () => playerController.playerMovementModule.OnUpdateBloodFootStepActiveState(true);
        }

        foreach (var item in triggerOff)
        {
            item.OnTrigger += () => playerController.playerMovementModule.OnUpdateBloodFootStepActiveState(false);
        }

    }
}

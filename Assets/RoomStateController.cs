using UnityEngine;

public class RoomStateController : MonoBehaviour
{
    [SerializeField] private string stateName = ""; // �⺻�� Normal

    private void OnEnable()
    {
        if (string.IsNullOrWhiteSpace(stateName))
        {
            Debug.LogWarning($"{gameObject.name} has no stateName set!");
            return;
        }

        AkSoundEngine.SetState("RoomState", stateName);
        Debug.Log("State changed to: " + stateName);
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "GameEvents/ChangingRotationEvent")]
public class ChangingRotationEvent : GameEvent
{
    [SerializeField] private Transform _myTransform;
    [SerializeField] private Vector3 _originEulerVec = Vector3.zero;
    [SerializeField] private Vector3 _targetEulderVec = Vector3.zero;

    protected override void Execute()
    {
        _myTransform.rotation = Quaternion.Euler(_targetEulderVec);
    }

    protected override void ResetEvent()
    {
        _myTransform.rotation = Quaternion.Euler(_originEulerVec);
    }
}

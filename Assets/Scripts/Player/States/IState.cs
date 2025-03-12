using UnityEngine;

public interface IState<T>
{
    void Enter();
    void FixedUpdate();
    void Update();
    void Exit();
    T GetStateType();
}

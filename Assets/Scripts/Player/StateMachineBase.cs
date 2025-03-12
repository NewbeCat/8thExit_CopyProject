using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StateMachineBase<T> where T : Enum
{
    #region Fields
    [Header("State Fields")]
    private Dictionary<T, IState<T>> stateDict = new Dictionary<T, IState<T>>();
    protected IState<T> currentState;
    #endregion

    #region State Methods
    public virtual void ChangeState(IState<T> newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public abstract void FixedUpdate();

    public abstract void Update();

    public abstract void ExitState();

    public T GetStateType()
    {
        return currentState.GetStateType();
    }

    public bool TryGetState(T t, out IState<T> state)
    {
        return stateDict.TryGetValue(t, out state);
    }

    public virtual bool TryAddState(T t, IState<T> newState)
    {
        if (stateDict.ContainsKey(t))
        {
            return false;
        }

        stateDict.Add(t, newState);
        return true;
    }
    #endregion
}

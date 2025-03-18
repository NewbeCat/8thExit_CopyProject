using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StateMachineBase<TEnum> where TEnum : Enum
{
    #region Fields
    [Header("State Fields")]
    protected IState<TEnum> currentState;
    private Dictionary<TEnum, IState<TEnum>> _stateDict = new Dictionary<TEnum, IState<TEnum>>();
    #endregion

    #region State Methods
    public virtual void ChangeState(IState<TEnum> newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public abstract void FixedUpdate();

    public abstract void Update();

    public abstract void ExitState();

    public TEnum GetStateType()
    {
        return currentState.GetStateType();
    }

    public bool TryGetState(TEnum t, out IState<TEnum> state)
    {
        return _stateDict.TryGetValue(t, out state);
    }

    public virtual bool TryAddState(TEnum t, IState<TEnum> newState)
    {
        if (_stateDict.ContainsKey(t))
        {
            return false;
        }

        _stateDict.Add(t, newState);
        return true;
    }
    #endregion
}

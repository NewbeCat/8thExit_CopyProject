using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    public bool isPersistent = false;
    protected bool isActive = false;

    public virtual void Call()
    {
        if (!isActive)
        {
            isActive = true;
            Execute();
        }
    }

    public virtual void Kill()
    {
        if (isActive)
        {
            ResetEvent();
            if (!isPersistent)
                isActive = false;
        }
    }

    protected abstract void Execute();
    protected abstract void ResetEvent();
}


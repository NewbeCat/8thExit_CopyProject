using System;
using UnityEngine;

public interface IPoolable<T>
{
    // Return Event : Use when you want return to PoolManager
    public event Action<T> returnAction;
}

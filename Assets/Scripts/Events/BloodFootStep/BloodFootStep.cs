using System;
using System.Collections;
using UnityEngine;

public class BloodFootStep : MonoBehaviour, IPoolable<BloodFootStep>
{
    public event Action<BloodFootStep> returnAction;

    [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }

    private bool isFirstSpawned;

    private void OnEnable()
    {
        if (!isFirstSpawned)
        {
            isFirstSpawned = true;
        }
        else
        {
            Managers.Instance.StartCoroutine(CoReturnToPool());
        }
    }

    private IEnumerator CoReturnToPool()
    {
        yield return Managers.Instance.Coroutine.GetWaitForSeconds(3f);
        returnAction?.Invoke(this);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T : MonoBehaviour, IPoolable<T>
{
    private T _prefab;
    private Queue<T> _pool = new Queue<T>();
    private List<T> _activeObjects = new List<T>();
    private Transform _parent;
    private int _initCount;
    private int _totalCount;
    private int _maxCount;

    public ObjectPooler(T prefab, Transform parent, int initCount, int maxCount)
    {
        _prefab = prefab;
        _parent = parent;
        _initCount = initCount;
        _maxCount = maxCount;

        for (int i = 0; i < initCount && _totalCount <= maxCount; i++)
        {
            InstantitateNewObject();
        }
    }

    #region Pool Methods
    public T Pool()
    {
        T t;

        if (_totalCount < _maxCount && _pool.Count <= _totalCount / 2)
        {
            for (int i = 0; i < _initCount && _totalCount <= _maxCount; i++)
            {
                InstantitateNewObject();
            }
        }

        if (_pool.Count > 0)
        {
            t = _pool.Dequeue();
        }
        else
        {
            t = _activeObjects[0];
            _activeObjects.RemoveAt(0);
        }

        _activeObjects.Add(t);
        t.gameObject.SetActive(true);

        return t;
    }

    private void ReturnToPooler(T t)
    {
        _activeObjects.Remove(t);
        _pool.Enqueue(t);
        t.gameObject.SetActive(false);
    }

    private void InstantitateNewObject()
    {
        T t = Object.Instantiate(_prefab);
        t.transform.SetParent(_parent);
        t.gameObject.SetActive(false);
        t.returnAction += ReturnToPooler;
        _pool.Enqueue(t);
        _totalCount++;
    }
    #endregion
}

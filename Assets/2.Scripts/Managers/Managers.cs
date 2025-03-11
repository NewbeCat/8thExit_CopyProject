using System;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    public static Managers Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject(typeof(Managers).Name);
                instance = go.AddComponent<Managers>();
            }

            return instance;
        }
    }
    
    public static InputManager Input { get => input; }
    private static InputManager input;

    private void Awake()
    {
        GameObject go = new GameObject(typeof(Managers).Name);
        instance = go.AddComponent<Managers>();
        InitMonoBehaviourSigleton(ref input);
    }

    // Not inherit MonoBehaviour
    private void InitSingleton<T>(ref T singletonInstance) where T : class, new()
    {
        singletonInstance = new T();
    }

    private void InitMonoBehaviourSigleton<T>(ref T singletonInstance) where T : MonoBehaviour
    {
        if (singletonInstance == null)
        {
            // if instance is in hierachy
            singletonInstance = FindFirstObjectByType<T>();
            // if instance is not in hierachy
            if (singletonInstance == null)
            {
                GameObject go = new GameObject(typeof(T).Name);
                singletonInstance = go.AddComponent<T>();
            }
        }
    }
}

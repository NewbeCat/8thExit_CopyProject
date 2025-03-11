using System;
using Unity.VisualScripting;
using UnityEngine;

// Manage Sigleton Managers
public class Managers : MonoBehaviour
{
    public static Managers Instance { get { Init(); return instance; } }
    private static Managers instance;
    
    public InputManager Input { get { InitMonoBehaviourSigleton(ref input); return Instance.input; } }
    private InputManager input;

    private void Awake()
    {
        Init();
    }

    private static void Init()
    {
        if (instance == null)
        {
            string className = typeof(Managers).Name;
            GameObject go = GameObject.Find(className);
            if (go == null)
            {
                go = new GameObject(className);
                go.AddComponent<Managers>();
            }

            instance = go.GetComponent<Managers>();
            DontDestroyOnLoad(go);
        }
    }

    private static void InitMonoBehaviourSigleton<T>(ref T singletonInstance) where T : MonoBehaviour, IManager
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

            singletonInstance.Init();
        }
    }

    // Init Singleton, but doesn't inherit MonoBehaviour
    private void InitSingleton<T>(ref T singletonInstance) where T : class, IManager, new()
    {
        singletonInstance = new T();
        singletonInstance.Init();
    }
}

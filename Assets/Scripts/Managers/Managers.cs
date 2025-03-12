using System;
using System.Collections.Generic;
using UnityEngine;

// Manage Sigleton Managers Initiation order 
// not static for using Instance Managers
public class Managers : MonoBehaviour
{
    #region Fields
    [Header("Singleton")]
    public static Managers Instance
    { 
        get 
        { 
            Init(); 
            return instance; 
        } 
    }
    private static Managers instance;
    
    public InputManager Input 
    { 
        get 
        { 
            InitMonoBehaviourSigleton(ref input); 
            return Instance.input; 
        } 
    }
    private InputManager input;

    public PlayerController Player
    {
        get
        {
            InitMonoBehaviourSigleton(ref input);
            return Instance.player;
        }
    }
    private PlayerController player;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Init();
    }
    #endregion

    #region Init Methods
    private static void Init()
    {
        InitManagers();
    }

    private static void InitManagers()
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

            // Seperate Core or Content 
            InitMonoBehaviourSigleton(ref instance.input);
            InitMonoBehaviourSigleton(ref instance.player);
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }

    // Init Singleton, inherit MonoBehaviour
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
    #endregion
}

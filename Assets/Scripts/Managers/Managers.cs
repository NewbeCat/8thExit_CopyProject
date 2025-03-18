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
            return _instance; 
        } 
    }
    private static Managers _instance;
    
    public InputManager Input 
    { 
        get 
        { 
            InitMonoBehaviourSigleton(ref _input); 
            return Instance._input; 
        } 
    }
    private InputManager _input;

    public SoundManager Sound
    {
        get
        {
            InitMonoBehaviourSigleton(ref _sound);
            return Instance._sound;
        }
    }
    private SoundManager _sound;

    public PlayerController Player
    {
        get
        {
            InitMonoBehaviourSigleton(ref _input);
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
        if (_instance == null)
        {
            string className = typeof(Managers).Name;
            GameObject go = GameObject.Find(className);
            if (go == null)
            {
                go = new GameObject(className);
                go.AddComponent<Managers>();
            }

            _instance = go.GetComponent<Managers>();
            DontDestroyOnLoad(go);

            // Seperate Core or Content 
            InitMonoBehaviourSigleton(ref _instance._input);
            InitMonoBehaviourSigleton(ref _instance.player);
        }
        else
        {
            Destroy(_instance.gameObject);
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

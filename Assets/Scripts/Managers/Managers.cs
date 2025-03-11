using System;
using System.Collections.Generic;
using UnityEngine;

// Manage Sigleton Managers Initiation order 
// not static for using Instance Managers
public class Managers : MonoBehaviour
{
    #region Singleton
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
        RemoveDuplicates();
        InitManagers();
    }

    // Remove Managers Duplicates
    private static void RemoveDuplicates()
    {
        List<Managers> managers = new List<Managers>(FindObjectsByType<Managers>(sortMode: FindObjectsSortMode.InstanceID));

        while (managers.Count > 0 && managers.Count != 1)
        {
            try
            {
                Destroy(managers[1]);
                managers.Remove(managers[1]);
            }
            catch (NullReferenceException e)
            {
                Debug.Assert(false, e.Message);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Assert(false, e.Message);
            }
        }
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

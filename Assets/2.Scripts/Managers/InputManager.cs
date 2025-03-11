using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    #region Fields
    /*[Header("Singleton Pattern")]
    // Dont
    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject(typeof(InputManager).Name);
                InputManager inputManager = go.AddComponent<InputManager>();
                instance = inputManager;
            }

            return instance;
        }
    }*/

    [field : Header("PlayerInput")]
    // for not allowing to call input methods
    public Action<Vector2> movement;
    public Action<Vector2> look;
    public Action<bool> sprint;
    private PlayerInput playerInput;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Init();
    }
    #endregion

    #region Init Methods

    private void Init()
    {
        playerInput = GetComponent<PlayerInput>();
        InitActions();
    }

    private void InitActions()
    {
        InputAction moveAction = playerInput.actions.FindAction(EInputAction.Move.ToString());
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        InputAction lookAction = playerInput.actions.FindAction(EInputAction.Look.ToString());
        lookAction.performed += OnLook;
        lookAction.canceled += OnLook;
        InputAction sprintAction = playerInput.actions.FindAction(EInputAction.Sprint.ToString());
        sprintAction.started += OnSprint;
        sprintAction.canceled += OnSprint;
    }
    #endregion

    #region Input Event Methods
    private void OnMove(CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        movement?.Invoke(direction);
    }

    private void OnLook(CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        look?.Invoke(direction);
    }

    private void OnSprint(CallbackContext context)
    {
        if (context.started)
        {
            sprint?.Invoke(true);
        }
        else if (context.canceled)
        {
            sprint?.Invoke(false);
        }
        else
        {
            Debug.Assert(false, "Sprint button event is wrong");
        }
    }
    #endregion
}

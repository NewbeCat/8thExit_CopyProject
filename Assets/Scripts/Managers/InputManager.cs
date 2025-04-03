using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour, IManager
{
    #region Fields
    [field : Header("PlayerInput")]
    // for not allowing to call input methods -> methods are private
    public event Action<Vector2> movement;
    public event Action<Vector2> look;
    public event Action<bool> sprint;
    private PlayerInput _playerInput;
    #endregion

    #region Unity Methods
    #endregion

    #region Init Methods
    public void Init()
    {
        _playerInput = GetComponent<PlayerInput>();
        InitActions();
    }

    // Init Input actions
    private void InitActions()
    {
        InputAction moveAction = _playerInput.actions.FindAction(EInputAction.Move.ToString());
        //moveAction.started += OnMove;
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        InputAction lookAction = _playerInput.actions.FindAction(EInputAction.Look.ToString());
        lookAction.performed += OnLook;
        lookAction.canceled += OnLook;
        InputAction sprintAction = _playerInput.actions.FindAction(EInputAction.Sprint.ToString());
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

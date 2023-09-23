using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Input Reader code sourced from 
/// https://blog.logrocket.com/building-third-person-controller-unity-new-input-system/
/// </summary>
public class InputReader : MonoBehaviour, InputActions.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;

    public Action OnJumpPerformed;

    private InputActions inputActions;

    private void OnEnable()
    {
        if (inputActions != null)
            return;

        inputActions = new InputActions();
        inputActions.Player.SetCallbacks(this);
        inputActions.Player.Enable();
    }

    public void OnDisable()
    {
        inputActions.Player.Disable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnJumpPerformed?.Invoke();
    }
}

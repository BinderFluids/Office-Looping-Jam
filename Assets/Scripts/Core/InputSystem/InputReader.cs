using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem; 


[CreateAssetMenu(menuName = "Create InputReader", fileName = "InputReader", order = 0)]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> onMove = delegate { };
    public event Action<bool> onInteract = delegate { };
    
    public InputSystem inputActions;
    
    public Vector2 Direction => inputActions.Player.Move.ReadValue<Vector2>();

    public void EnableActions()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem();
            inputActions.Player.Enable();
        }
        inputActions.Enable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        onMove.Invoke(context.ReadValue<Vector2>());
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                onInteract.Invoke(true);
                break;
            case InputActionPhase.Performed:
                onInteract.Invoke(false);
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }


    public void OnCrouch(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        
    }
}

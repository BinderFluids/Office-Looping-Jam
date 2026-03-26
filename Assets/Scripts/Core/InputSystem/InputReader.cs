using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem; 


[CreateAssetMenu(menuName = "Create InputReader", fileName = "InputReader", order = 0)]
public class InputReader : ScriptableObject, IPlayerActions, IDialogueActions, IDebugActions
{
    private InputActionType activeActionType; 
    public BoolInputData Interact;
    public BoolInputData Progress;
    public Vector2InputData PlayerMove;
    public Vector2InputData DialogueMove;
    public BoolInputData DebugA;
    
    private InputSystem inputActions;
    
    public void EnableInput(InputActionType inputActionType)
    {
        activeActionType = inputActionType;
        
        if (inputActionType != InputActionType.None)
        {
            if (inputActions == null)
            {
                OnFirstEnable();
            }
            inputActions.Enable();
        }

        switch (inputActionType)
        {
            case InputActionType.Player:
                inputActions.Player.Enable();
                inputActions.Dialogue.Disable();
                break;
            case InputActionType.Dialogue:
                inputActions.Dialogue.Enable();
                inputActions.Player.Disable();
                break;
            default:
                inputActions.Disable();
                break;
        }
    }
    void OnFirstEnable()
    {
        inputActions = new InputSystem();
        inputActions.Player.SetCallbacks(this); 
        inputActions.Dialogue.SetCallbacks(this);
        
        inputActions.Debug.Enable();
        inputActions.Debug.SetCallbacks(this); 
        
        PlayerMove = new Vector2InputData(inputActions.Player.Move);
        Interact = new BoolInputData(inputActions.Player.Interact);
        
        DialogueMove = new Vector2InputData(inputActions.Dialogue.Move);
        Progress = new BoolInputData(inputActions.Dialogue.Progress);
        
        DebugA = new BoolInputData(inputActions.Debug.A);
    }
    
    public void OnA(InputAction.CallbackContext context)
    {
        DebugA.Trigger(context);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        PlayerMove.Trigger(context);
    }
    public void OnProgress(InputAction.CallbackContext context)
    {
        Progress.Trigger(context);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Interact.Trigger(context);
    }
}
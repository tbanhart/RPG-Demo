using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    #region General Properties

    InputReceiver controller {get => this.GetComponent<InputReceiver>();}

    public Vector2 CursorPosition {get; set;}

    #endregion

    #region Input System Properties

    PlayerInput inputActions;
    InputAction select;
    InputAction contextMenu;
    InputAction cursorPosition;
    InputAction sheathe;
    InputAction drop;
    InputAction rotateCamera;
    InputAction zoomCamera;
    InputAction resetCamera;
    InputAction exit;
    InputAction amWalk;
    InputAction amAttack;
    InputAction amExamine;
    #endregion

    #region Input System Wiring - EACH NEW INPUT NEEDS TO BE DECLARED HERE
    
    // *** This is a terrible way of passing the data, needs rework like bad but idk how yet
    private void Update() {
        controller.UpdateCursorPosition(cursorPosition.ReadValue<Vector2>());
    }   

    private void Awake() {
        inputActions = new PlayerInput();
    }

    private void OnEnable() {
        // Select
        select = inputActions.Player.Select;
        select.Enable();

        inputActions.Player.Select.performed += GetSelection;
        inputActions.Player.Select.Enable();

        // ContextMenu
        contextMenu = inputActions.Player.ContextMenu;
        contextMenu.Enable();

        inputActions.Player.ContextMenu.performed += OpenContextMenu;
        inputActions.Player.ContextMenu.Enable();

        // Sheathe
        sheathe = inputActions.Player.Sheathe;
        sheathe.Enable();

        inputActions.Player.Sheathe.performed += Sheathe;
        inputActions.Player.Sheathe.Enable();

        // Drop
        drop = inputActions.Player.Drop;
        drop.Enable();

        inputActions.Player.Drop.performed += Drop;
        inputActions.Player.Drop.Enable();

        // CursorPosition
        cursorPosition = inputActions.Player.CursorPosition;
        cursorPosition.Enable();

        //RotateCamera
        rotateCamera = inputActions.Player.RotateCamera;
        rotateCamera.Enable();

        inputActions.Player.RotateCamera.performed += Rotate;
        inputActions.Player.RotateCamera.canceled += Rotate;
        inputActions.Player.RotateCamera.Enable();

        //ZoomCamera
        zoomCamera = inputActions.Player.ZoomCamera;
        zoomCamera.Enable();

        inputActions.Player.ZoomCamera.performed += Zoom;
        inputActions.Player.ZoomCamera.canceled += Zoom;
        inputActions.Player.ZoomCamera.Enable();

        //ResetCamera
        resetCamera = inputActions.Player.ResetCamera;
        resetCamera.Enable();

        inputActions.Player.ResetCamera.performed += ResetCamera;
        inputActions.Player.ResetCamera.canceled += ResetCamera;
        inputActions.Player.ResetCamera.Enable();

        //Exit
        exit = inputActions.Player.Exit;
        exit.Enable();

        inputActions.Player.Exit.performed += Exit;
        inputActions.Player.Exit.canceled += Exit;
        inputActions.Player.Exit.Enable();

        //ActionMasks
        amAttack = inputActions.Player.AMAttack;
        amAttack.Enable();

        inputActions.Player.AMAttack.performed += ApplyActionMask;
        inputActions.Player.AMAttack.Enable();

        amWalk = inputActions.Player.AMWalk;
        amWalk.Enable();

        inputActions.Player.AMWalk.performed += ApplyActionMask;
        inputActions.Player.AMWalk.Enable();

        amExamine = inputActions.Player.AMExamine;
        amExamine.Enable();

        inputActions.Player.AMExamine.performed += ApplyActionMask;
        inputActions.Player.AMExamine.Enable();
    }

    private void OnDisable() {
        select.Disable();
        inputActions.Player.Select.Disable();
    }

    #endregion

    #region Input Handlers

    void GetSelection(InputAction.CallbackContext obj){
        controller.HandleSelect();
    }

    void OpenContextMenu(InputAction.CallbackContext obj){
        controller.HandleOpenContext();
    }

    void Drop(InputAction.CallbackContext obj){
        controller.Drop();
    }

    void Sheathe(InputAction.CallbackContext obj){
        controller.Sheathe();
    }

    void Rotate(InputAction.CallbackContext obj){
        controller.RotateCamera(obj.ReadValue<float>());
    }

    void Zoom(InputAction.CallbackContext obj){
        controller.ZoomCamera(obj.ReadValue<float>());
    }

    void ResetCamera(InputAction.CallbackContext obj){
        controller.ResetCamera();
    }

    void Exit(InputAction.CallbackContext obj){
        controller.Exit();
    }

    void ApplyActionMask(InputAction.CallbackContext obj){
        ActionType action;
        
        switch(obj.action.name){
            case "AMAttack":
                action = ActionType.Attack;
                break;

            case "AMWalk":
                action = ActionType.Walk;
                break;

            case "AMExamine":
                action = ActionType.Examine;
                break;
            
            default:
                action = ActionType.Default;
                break;
        }

        controller.SetActionMask(action);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    #region General Properties

    PlayerController controller {get => this.GetComponent<PlayerController>();}

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

    #endregion

    #region Input System Wiring - EACH NEW INPUT NEEDS TO BE DECLARED HERE
    
    // *** This is a terrible way of passing the data, needs rework like bad but idk how yet
    private void Update() {
        controller.CursorPosition = cursorPosition.ReadValue<Vector2>();
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

    #endregion
}

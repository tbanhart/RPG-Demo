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
    InputAction cursorPosition;

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
        //Select
        select = inputActions.Player.Select;
        select.Enable();

        inputActions.Player.Select.performed += GetSelection;
        inputActions.Player.Select.Enable();

        //CursorPosition
        cursorPosition = inputActions.Player.CursorPosition;
        cursorPosition.Enable();
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

    #endregion
}

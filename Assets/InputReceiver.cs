using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    InputType currentInput;

    Vector2 cursorPosition;

    Vector3 cameraInput;

    ActionType actionMask;

    private void Awake() {
        ResetInputs();
    }

    internal void HandleSelect()
    {
        currentInput = InputType.Select;
    }

    internal void HandleOpenContext()
    {
        currentInput = InputType.OpenContextMenu;
    }

    internal void Drop()
    {
        currentInput = InputType.Drop;
    }

    internal void Sheathe()
    {
        currentInput = InputType.Sheathe;
    }

    internal void RotateCamera(float v)
    {
        cameraInput.x = v;
    }

    internal void ZoomCamera(float v)
    {
        cameraInput.z = v;
    }

    internal void ResetCamera()
    {
        cameraInput.y = 1;
    }

    internal void Exit()
    {
        currentInput = InputType.Exit;
    }

    internal void SetActionMask(ActionType action)
    {
        actionMask = action;
    }

    internal void UpdateCursorPosition(Vector2 position){
        cursorPosition = position;
    }

    public InputData GetInput(){
        var data = new InputData();

        data.CurrentInput = currentInput;
        data.CameraInput = cameraInput;
        data.ActionMask = actionMask;

        ResetInputs();

        return data;
    }

    void ResetInputs(){
        currentInput = InputType.None;
        cameraInput = Vector3.zero;
        cursorPosition = Vector2.zero;
        actionMask = ActionType.Default;
    }
}

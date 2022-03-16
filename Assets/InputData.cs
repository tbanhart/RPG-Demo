using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputData : MonoBehaviour
{
    public InputType CurrentInput = InputType.None;

    public Vector3 CameraInput = Vector3.zero;

    public Vector2 CursorPosition = Vector2.zero;

    public ActionType ActionMask = ActionType.Default;
}
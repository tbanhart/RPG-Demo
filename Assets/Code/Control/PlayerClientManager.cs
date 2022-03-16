using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClientManager : MonoBehaviour
{
    public Dictionary<GameObject,GameObject> ConnectedPlayers;

    public Dictionary<GameObject, PlayerController> PlayerControllers;

    [SerializeField] GameObject SPInput;

    [SerializeField] GameObject SPPlayer;

    private void Awake() {
        // *** Just for single player ***
        ConnectedPlayers = new Dictionary<GameObject, GameObject>();
        PlayerControllers = new Dictionary<GameObject, PlayerController>();
        ConnectedPlayers.Add(SPPlayer, SPInput);
        PlayerControllers.Add(SPPlayer, SPPlayer.GetComponent<PlayerController>());
        //*** ***
    }

    private void Update() {
        foreach(var playerPair in ConnectedPlayers){
            var objPlayer = playerPair.Key;
            var objInput = playerPair.Value;
            var controller = PlayerControllers[playerPair.Key];
            var input = objInput.GetComponent<InputReceiver>().GetInput();

            controller.CursorPosition = input.CursorPosition;
            controller.CameraInput = input.CameraInput;
            controller.MaskedAction = input.ActionMask;
            controller.CurrentInput = input.CurrentInput;

            // *** Just for single player
            if(input.CurrentInput == InputType.Exit) Application.Quit();
            // ***
        }
    }
}

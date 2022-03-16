using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonText : MonoBehaviour
{
    [SerializeField] GameObject buttonText;

    [SerializeField] public int ID;

    ActionType _action {get;set;}

    GameObject _callingPlayer {get;set;}

    GameObject _target {get; set;}

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(delegate { SelectAction(); });
    }

    public void SetupButton(GameObject callingPlayer, string text, ActionType action, GameObject target){
        _callingPlayer = callingPlayer;
        buttonText.GetComponent<TextMeshProUGUI>().text = text;
        _action = action;
        _target = target;
    }

    void SelectAction(){
        _callingPlayer.GetComponent<PlayerController>().MaskedAction = _action;
        _callingPlayer.GetComponent<PlayerController>().HoveredObject = _target;
        //_callingPlayer.GetComponent<ActorController>().AddInteraction(new Interaction(_action, _target), true);
    }

}

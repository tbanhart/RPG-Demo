using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public List<ActionType> AvailableActions;

    public Interaction GetInteraction(ActionType action){
        return new Interaction(action, this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public List<ActionType> AvailableActions;
    [SerializeField] public bool Grabbable;
    [SerializeField] public bool Equippable;
    [SerializeField] public float Weight;

    public Interaction GetInteraction(ActionType action){
        return new Interaction(action, this.gameObject);
    }
}

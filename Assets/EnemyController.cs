using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Movement movement;

    Combat combat;

    Inventory inventory;

    PlayerAnimator animator;

    Interaction CurrentInteraction;

    State CurrentState;

    Detection detection;

    private void Awake() {
        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<PlayerAnimator>();
        detection = GetComponent<Detection>();
        CurrentInteraction = null;
        SetState(State.IDLE);
    }

    private void Update() {
        var 
        if(detection.Targets.Length != 0){
            if()
        }
    }

    void SetState(State state){
        CurrentState = state;
    }

}
